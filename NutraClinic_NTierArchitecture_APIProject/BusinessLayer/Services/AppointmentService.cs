using BusinessLayer.Helpers;
using BusinessLayer.IServices;
using DataLayer.Repository;
using DataLayer.Repository.IRepository;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _uow;

        public AppointmentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _uow.Appointment.GetAllAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _uow.Appointment.GetByIdAsync(id);
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _uow.Appointment.AddAsync(appointment);
            await _uow.SaveAsync();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _uow.Appointment.Update(appointment);
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.Appointment.GetByIdAsync(id);
            if (entity != null)
            {
                _uow.Appointment.Remove(entity);
                await _uow.SaveAsync();
            }
        }

        public async Task<IEnumerable<Appointment>> GetByUserIdAsync(int userId)
        {
            return await _uow.Appointment.GetWhereAsync(a => a.UserId == userId);
        }

        public async Task<IEnumerable<Appointment>> GetByDietitianIdAsync(int dietitianId)
        {
            return await _uow.Appointment.GetWhereAsync(a =>
                a.DietitianId == dietitianId && a.Status == "Approved");
        }

        public async Task<List<CalendarDayDto>> GetCalendarAsync(int dietitianId)
        {
            var result = new List<CalendarDayDto>();
            var today = DateTime.Today;
            var endDate = today.AddDays(30);

            var dietitian = await _uow.Dietitian.GetByIdAsync(dietitianId);
            if (dietitian == null) return result;

            var workingDays = ScheduleParser.ParseWorkingDays(dietitian.WorkingDays);
            if (workingDays.Count == 0)
            {
                workingDays = new HashSet<DayOfWeek>(
                    Enum.GetValues<DayOfWeek>()
                );
            }

            var (startHour, endHour) = ScheduleParser.ParseWorkingHours(dietitian.WorkingHours);
            var slotSpan = TimeSpan.FromHours(1); 
            if (startHour == null || endHour == null)
            {
                startHour = new TimeSpan(9, 0, 0);
                endHour = new TimeSpan(17, 0, 0);
            }
            var totalSlotsPerDay = (int)((endHour.Value - startHour.Value).TotalHours / slotSpan.TotalHours);

            var appts = await _uow.Appointment.GetWhereAsync(a =>
                a.DietitianId == dietitianId &&
                a.AppointmentDate >= today &&
                a.AppointmentDate < endDate &&
                a.Status != "Cancelled" &&
                a.Status != "Rejected"
            );

            var grouped = appts
                .GroupBy(a => a.AppointmentDate.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            for (var date = today; date < endDate; date = date.AddDays(1))
            {
                if (!workingDays.Contains(date.DayOfWeek))
                    continue;

                grouped.TryGetValue(date.Date, out var dayAppts);
                var takenCount = dayAppts?.Count ?? 0;

                result.Add(new CalendarDayDto
                {
                    Date = date,
                    IsFull = takenCount >= totalSlotsPerDay
                });
            }

            return result;
        }

        public async Task<List<TimeSlotDto>> GetAvailableSlotsAsync(int dietitianId, DateTime date)
        {
            var result = new List<TimeSlotDto>();

            var dietitian = await _uow.Dietitian.GetByIdAsync(dietitianId);
            if (dietitian == null) return result;

            var workingDays = ScheduleParser.ParseWorkingDays(dietitian.WorkingDays);
            if (workingDays.Count > 0 && !workingDays.Contains(date.DayOfWeek))
            {
                return result;
            }

            var (startHour, endHour) = ScheduleParser.ParseWorkingHours(dietitian.WorkingHours);
            if (startHour == null || endHour == null)
            {
                startHour = new TimeSpan(9, 0, 0);
                endHour = new TimeSpan(17, 0, 0);
            }
            var slotSpan = TimeSpan.FromHours(1);

            var takenAppts = await _uow.Appointment.GetWhereAsync(a =>
                a.DietitianId == dietitianId &&
                a.AppointmentDate == date.Date &&
                a.Status != "Cancelled" &&
                a.Status != "Rejected"
            );

            var takenStarts = new HashSet<TimeSpan>(takenAppts.Select(a => a.StartTime));

            for (var t = startHour.Value; t < endHour.Value; t = t.Add(slotSpan))
            {
                var slotEnd = t + slotSpan;
                bool isTaken = takenStarts.Contains(t);

                result.Add(new TimeSlotDto
                {
                    Time = $"{t:hh\\:mm}-{slotEnd:hh\\:mm}",
                    Available = !isTaken
                });
            }

            return result;
        }

        public async Task<IEnumerable<PendingApptDetailDto>> GetPendingWithDetailsAsync(int? dietitianId)
        {
            var pending = await _uow.Appointment.GetWhereAsync(a =>
                a.Status == "Pending" &&
                (!dietitianId.HasValue || a.DietitianId == dietitianId.Value));

            if (pending == null) return Enumerable.Empty<PendingApptDetailDto>();

            var userIds = pending.Select(a => a.UserId).Distinct().ToList();
            var dietIds = pending.Select(a => a.DietitianId).Distinct().ToList();

            var users = await _uow.User.GetWhereAsync(u => userIds.Contains(u.UserId));
            var diets = await _uow.Dietitian.GetWhereAsync(d => dietIds.Contains(d.DietitianId));
            var specIds = diets.Select(d => d.SpecialtyId).Distinct().ToList();
            var specs = await _uow.Specialty.GetWhereAsync(s => specIds.Contains(s.SpecialtyId));

            string NameOr(int id, IEnumerable<ModelLayer.User> coll) =>
                coll.FirstOrDefault(x => x.UserId == id)?.FullName ?? $"User#{id}";
            string DietNameOr(int id, IEnumerable<ModelLayer.Dietitian> coll) =>
                coll.FirstOrDefault(x => x.DietitianId == id)?.FullName ?? $"Dietitian#{id}";
            string SpecNameOr(int? specId) =>
                specId.HasValue
                    ? (specs.FirstOrDefault(x => x.SpecialtyId == specId.Value)?.Name ?? "—")
                    : "—";

            var dictDiet = diets.ToDictionary(d => d.DietitianId);
            var list = pending.Select(a =>
            {
                dictDiet.TryGetValue(a.DietitianId, out var dObj);
                var specName = dObj != null ? SpecNameOr(dObj.SpecialtyId) : "—";
                return new PendingApptDetailDto
                {
                    AppointmentId = a.AppointmentId,
                    UserId = a.UserId,
                    UserName = NameOr(a.UserId, users),
                    DietitianId = a.DietitianId,
                    DietitianName = DietNameOr(a.DietitianId, diets),
                    SpecialtyName = specName,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt
                };
            }).OrderBy(x => x.AppointmentDate).ThenBy(x => x.StartTime);

            return list;
        }

        public async Task<bool> ChangeStatusAsync(int appointmentId, string status)
        {
            var appt = await _uow.Appointment.GetByIdAsync(appointmentId);
            if (appt == null) return false;

            switch (status)
            {
                case "Approved":
                case "Rejected":
                case "Cancelled":
                    appt.Status = status;
                    break;
                default:
                    appt.Status = "Pending"; // fallback
                    break;
            }

            _uow.Appointment.Update(appt);
            await _uow.SaveAsync();
            return true;
        }




    }
}
