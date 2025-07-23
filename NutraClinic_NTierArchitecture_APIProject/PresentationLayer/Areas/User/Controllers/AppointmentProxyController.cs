using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayer.Helpers;
using System.Text;

namespace PresentationLayer.Areas.User.Controllers
{
    [Area("User")]
    [Route("User/[controller]/[action]")]
    public class AppointmentProxyController : Controller
    {
        private readonly ApiHelper _api;
        public AppointmentProxyController(ApiHelper api) => _api = api;

        [HttpGet]
        public async Task<IActionResult> Calendar(int dietitianId)
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.GetAsync($"api/Appointment/GetCalendar/{dietitianId}");
            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode);

            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> Slots(int dietitianId, DateTime date)
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.GetAsync($"api/Appointment/GetAvailableSlots?dietitianId={dietitianId}&date={date:O}");
            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode);

            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateProxyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TimeSpan.TryParse(dto.StartTime, out var start))
                return BadRequest("Invalid StartTime format.");
            if (!TimeSpan.TryParse(dto.EndTime, out var end))
                return BadRequest("Invalid EndTime format.");

            var apiDto = new
            {
                UserId = dto.UserId,
                DietitianId = dto.DietitianId,
                AppointmentDate = dto.AppointmentDate, 
                StartTime = start, 
                EndTime = end
            };

            var client = _api.CreateClientWithToken();
            var content = new StringContent(
                JsonConvert.SerializeObject(apiDto),
                Encoding.UTF8,
                "application/json");

            var resp = await client.PostAsync("api/Appointment/Create", content);
            var body = await resp.Content.ReadAsStringAsync();
            return StatusCode((int)resp.StatusCode, body);
        }

        public class AppointmentCreateProxyDto
        {
            public int UserId { get; set; }
            public int DietitianId { get; set; }
            public DateTime AppointmentDate { get; set; }
            public string StartTime { get; set; } = ""; 
            public string EndTime { get; set; } = "";
        }

        [HttpGet]
        public async Task<IActionResult> UserAppointments()
        {
            var userId = HttpContext.Session.GetInt32("SubjectId");
            if (userId == null) return Unauthorized();

            var client = _api.CreateClientWithToken();
            var resp = await client.GetAsync($"api/Appointment/User/{userId}");
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode);

            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpPut]
        public async Task<IActionResult> Cancel(int id)
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.PutAsync($"api/Appointment/Cancel/{id}", null);
            var body = await resp.Content.ReadAsStringAsync();
            return StatusCode((int)resp.StatusCode, body);
        }

    }
}