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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAppointmentService _appointmentService;
        public UserController(IUserService userService, IAppointmentService appointmentService)
        {
            _userService = userService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> GetAll()
        {
            var userList = await _userService.GetAllAsync();
            return Ok(userList);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = "Secretary, Dietitian")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUserAppointments/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserAppointments(int id)
        {
            var appointments = await _appointmentService.GetByUserIdAsync(id);
            return Ok(appointments);
        }

        [HttpPost]
        [Route("BookAppointment")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookAppointment(Appointment appointment)
        {
            appointment.Status = "Pending";
            appointment.CreatedAt = DateTime.Now;

            await _appointmentService.AddAsync(appointment);
            return Ok("Randevunuz oluşturuldu. Onay bekliyor.");
        }

        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userService.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            existingUser.FullName = dto.FullName;
            existingUser.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                existingUser.Password = dto.Password; 
            existingUser.DateOfBirth = dto.DateOfBirth;
            existingUser.ProfilePhotoUrl = dto.ProfilePhotoUrl;

            await _userService.UpdateAsync(existingUser);
            return Ok(new { message = "Kullanıcı bilgileri güncellendi." });
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
    }
}
