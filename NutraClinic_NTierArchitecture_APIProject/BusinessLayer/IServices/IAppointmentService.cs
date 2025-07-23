using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(int id);
        Task<List<CalendarDayDto>> GetCalendarAsync(int dietitianId);
        Task<List<TimeSlotDto>> GetAvailableSlotsAsync(int dietitianId, DateTime date);

        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int id);
        Task<IEnumerable<Appointment>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Appointment>> GetByDietitianIdAsync(int dietitianId);
        Task<IEnumerable<PendingApptDetailDto>> GetPendingWithDetailsAsync(int? dietitianId);
        Task<bool> ChangeStatusAsync(int appointmentId, string status);

    }
}
