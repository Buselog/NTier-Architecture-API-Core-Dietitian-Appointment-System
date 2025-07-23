using BusinessLayer.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Secretary")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        [Route("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var specialtyList = await _specialtyService.GetAllAsync();
            return Ok(specialtyList);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = "Secretary, Dietitian")]
        public async Task<IActionResult> GetById(int id)
        {
            var specialty = await _specialtyService.GetByIdAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return Ok(specialty);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Specialty specialty)
        {
            await _specialtyService.AddAsync(specialty);
            return Ok();
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, Specialty specialty)
        {
            if (id != specialty.SpecialtyId)
            {
                return BadRequest();
            }

            await _specialtyService.UpdateAsync(specialty);
            return Ok();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _specialtyService.DeleteAsync(id);
            return Ok();
        }
    }
}
