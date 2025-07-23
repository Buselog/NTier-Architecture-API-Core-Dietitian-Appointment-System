using BusinessLayer.IServices;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDietitianService _dietitianService;
        private readonly ISpecialtyService _specialtyService;

        public AppointmentController(IAppointmentService appointmentService, IDietitianService dietitianService, ISpecialtyService specialtyService)
        {
            _appointmentService = appointmentService;
            _dietitianService = dietitianService;
            _specialtyService = specialtyService;
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> GetAll()
        {
            var appointmentList = await _appointmentService.GetAllAsync();
            return Ok(appointmentList);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = "Secretary,Dietitian")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appointment = new Appointment
            {
                UserId = dto.UserId,
                DietitianId = dto.DietitianId,
                AppointmentDate = dto.AppointmentDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            await _appointmentService.AddAsync(appointment);
            return Ok(new { message = "Randevu talebi oluşturuldu, onay bekleniyor." });
        }

        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Update(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return BadRequest();
            }

            await _appointmentService.UpdateAsync(appointment);
            return Ok();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return Ok();
        }


        [HttpGet("GetCalendar/{dietitianId}")]
        [Authorize(Roles = "User,Dietitian,Secretary")]
        public async Task<IActionResult> GetCalendar(int dietitianId)
        {
            var calendar = await _appointmentService.GetCalendarAsync(dietitianId);
            return Ok(calendar);
        }

        [HttpGet("GetAvailableSlots")]
        [Authorize(Roles = "User,Dietitian,Secretary")]
        public async Task<IActionResult> GetAvailableSlots(int dietitianId, DateTime date)
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(dietitianId, date);
            return Ok(slots);
        }

        [HttpGet("User/{userId}")]
        [Authorize(Roles = "User,Dietitian,Secretary")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var appts = await _appointmentService.GetByUserIdAsync(userId);
            var dtos = new List<UserAppointmentDto>();
            foreach (var a in appts.OrderByDescending(x => x.AppointmentDate).ThenBy(x => x.StartTime))
            {
                var d = await _dietitianService.GetByIdAsync(a.DietitianId); 
                string specialtyName = "";
                if (d?.SpecialtyId != null && d.SpecialtyId > 0)
                {
                    var sp = await _specialtyService.GetByIdAsync(d.SpecialtyId);
                    specialtyName = sp?.Name ?? "";
                }

                dtos.Add(new UserAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    DietitianId = a.DietitianId,
                    DietitianName = d?.FullName ?? "",
                    DietitianPhoto = d?.ProfilePhotoUrl ?? "/img/default-user.png",
                    SpecialtyName = specialtyName
                });
            }

            return Ok(dtos);
        }

        [HttpPut("Cancel/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appt = await _appointmentService.GetByIdAsync(id);
            if (appt == null) return NotFound();

            if (appt.AppointmentDate.Date < DateTime.Today)
                return BadRequest(new { message = "Geçmiş bir randevu iptal edilemez." });

            if (appt.Status == "Cancelled" || appt.Status == "Rejected")
                return BadRequest(new { message = "Bu randevu zaten iptal/reddedilmiş." });

            appt.Status = "Cancelled";
            await _appointmentService.UpdateAsync(appt);
            return Ok(new { message = "Randevu iptal edildi." });
        }

        [HttpGet("Pending")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _appointmentService.GetPendingWithDetailsAsync(null);
            return Ok(list);
        }

        [HttpGet("PendingByDietitian/{dietitianId}")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> GetPendingByDietitian(int dietitianId)
        {
            var list = await _appointmentService.GetPendingWithDetailsAsync(dietitianId);
            return Ok(list);
        }

        [HttpPut("Status/{id}")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] AppointmentStatusDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest(new { message = "Invalid status." });

            var ok = await _appointmentService.ChangeStatusAsync(id, dto.Status);
            if (!ok) return NotFound();

            return Ok(new { message = "Status updated.", status = dto.Status });
        }

        [HttpGet("DietitianApproved/{dietitianId}")]
        [Authorize(Roles = "Dietitian,Secretary")]
        public async Task<IActionResult> GetApprovedForDietitian(int dietitianId)
        {
            var appts = await _appointmentService.GetByDietitianIdAsync(dietitianId); 
            return Ok(appts);
        }



    }
}
