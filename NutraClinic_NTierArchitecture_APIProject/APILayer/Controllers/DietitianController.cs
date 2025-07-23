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
    public class DietitianController : ControllerBase
    {
        private readonly IDietitianService _dietitianService;
        private readonly IAppointmentService _appointmentService;
        public DietitianController(IDietitianService dietitianService, IAppointmentService appointmentService)
        {
            _dietitianService = dietitianService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Route("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var dietitianList = await _dietitianService.GetAllAsync();
            return Ok(dietitianList);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var dietitian = await _dietitianService.GetByIdAsync(id);
            if(dietitian == null)
            {
                return NotFound();
            }
            return Ok(dietitian);
        }
        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Create(DietitianCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dietitian = new Dietitian
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                Biography = dto.Biography,
                SpecialtyId = dto.SpecialtyId,  
                WorkingDays = dto.WorkingDays,
                WorkingHours = dto.WorkingHours,
                CreatedAt = dto.CreatedAt
            };

            await _dietitianService.AddAsync(dietitian);
            return Ok(new { message = "Dietitian added successfully", dietitianId = dietitian.DietitianId });
        }

        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = "Dietitian")]
        public async Task<IActionResult> Update(int id, [FromBody] DietitianUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingDietitian = await _dietitianService.GetByIdAsync(id);
            if (existingDietitian == null)
                return NotFound();

            existingDietitian.FullName = dto.FullName;
            existingDietitian.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                existingDietitian.Password = dto.Password; // Şifre hash'i sonra yapılacak
            existingDietitian.WorkingHours = dto.WorkingHours;
            existingDietitian.WorkingDays = dto.WorkingDays;
            existingDietitian.Biography = dto.Biography;
 

            await _dietitianService.UpdateAsync(existingDietitian);
            return Ok(new { message = "Profil güncellendi." });
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dietitianService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("Appointment/{id}")]
        public async Task<IActionResult> GetAppointments(int dietitianId)
        {
            var appointments = await _appointmentService.GetByDietitianIdAsync(dietitianId);
            return Ok(appointments);
        }

        [HttpGet]
        [Route("GetCalendar/{id}")]
        public async Task<IActionResult> GetCalendar(int dietitianId)
        {
            var calendar = await _appointmentService.GetCalendarAsync(dietitianId);
            return Ok(calendar);
        }




    }
}
