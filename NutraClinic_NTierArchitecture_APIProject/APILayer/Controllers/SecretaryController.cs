using BusinessLayer.IServices;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace APILayer.Controllers
{
    [Authorize(Roles = "Secretary")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecretaryController : ControllerBase
    {


        private readonly ISecretaryService _secretaryService;
        private readonly IDietitianService _dietitianService;
        private readonly IUserService _userService;
        private readonly ISpecialtyService _specialtyService;
        private readonly IAppointmentService _appointmentService;

        public SecretaryController(
            ISecretaryService secretaryService,
            IDietitianService dietitianService,
            IUserService userService,
            ISpecialtyService specialtyService,
            IAppointmentService appointmentService)
        {
            _secretaryService = secretaryService;
            _dietitianService = dietitianService;
            _userService = userService;
            _specialtyService = specialtyService;
            _appointmentService = appointmentService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllSecretaries() => Ok(await _secretaryService.GetAllAsync());

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sec = await _secretaryService.GetByIdAsync(id);
            return sec == null ? NotFound(new { message = "Sekreter bulunamadı." }) : Ok(sec);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Secretary sec)
        {
            await _secretaryService.AddAsync(sec);
            return Ok(new { message = "Sekreter eklendi." });
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Secretary sec)
        {
            if (id != sec.SecretaryId) return BadRequest();
            await _secretaryService.UpdateAsync(sec);
            return Ok(new { message = "Sekreter güncellendi." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _secretaryService.DeleteAsync(id);
            return Ok(new { message = "Sekreter silindi." });
        }

        [HttpPost("AddDietitian")]
        public async Task<IActionResult> AddDietitian([FromBody] DietitianCreateDto dto)
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
                CreatedAt = DateTime.Now
            };

            await _dietitianService.AddAsync(dietitian);
            return Ok(new { message = "Diyetisyen başarıyla eklendi." });
        }

        [HttpDelete("DeleteDietitian/{id}")]
        public async Task<IActionResult> DeleteDietitian(int id)
        {
            var dietitian = await _dietitianService.GetByIdAsync(id);
            if (dietitian == null)
                return NotFound(new { message = "Diyetisyen bulunamadı." });

            await _dietitianService.DeleteAsync(id);
            return Ok(new { message = "Diyetisyen silindi." });
        }

        [HttpPut("ApproveUser/{id}")]
        public async Task<IActionResult> ApproveUser(int id, [FromBody] ApproveUserDto dto)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });

            user.IsApproved = dto.IsApproved;
            await _userService.UpdateAsync(user);

            return Ok(new { message = $"Kullanıcı durumu güncellendi: {(dto.IsApproved ? "Onaylandı" : "Reddedildi")}" });
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers() => Ok(await _userService.GetAllAsync());

        [HttpPost("AddSpecialty")]
        public async Task<IActionResult> AddSpecialty([FromBody] SpecialtyCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var specialty = new Specialty { Name = dto.Name };
            await _specialtyService.AddAsync(specialty);

            return Ok(new { message = "Uzmanlık eklendi." });
        }

        [HttpDelete("DeleteSpecialty/{id}")]
        public async Task<IActionResult> DeleteSpecialty(int id)
        {
            await _specialtyService.DeleteAsync(id);
            return Ok(new { message = "Uzmanlık silindi." });
        }

        [HttpGet("GetAllSpecialties")]
        public async Task<IActionResult> GetAllSpecialties() => Ok(await _specialtyService.GetAllAsync());

        [HttpPut("ApproveAppointment/{id}")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(new { message = "Randevu bulunamadı." });

            appointment.Status = "Approved";
            await _appointmentService.UpdateAsync(appointment);

            return Ok(new { message = "Randevu onaylandı." });
        }

        [HttpPut("RejectAppointment/{id}")]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(new { message = "Randevu bulunamadı." });

            appointment.Status = "Rejected";
            await _appointmentService.UpdateAsync(appointment);

            return Ok(new { message = "Randevu reddedildi." });
        }

        [HttpGet("GetPendingAppointments")]
        public async Task<IActionResult> GetPendingAppointments()
        {
            var appointments = await _appointmentService.GetAllAsync();
            var pending = appointments.Where(a => a.Status == "Pending");
            return Ok(pending);
        }
    }
}
