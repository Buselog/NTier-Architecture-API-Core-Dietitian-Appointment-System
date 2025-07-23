using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using Newtonsoft.Json;
using PresentationLayer.Helpers;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Areas.User.Controllers
{
    [Area("User")]
    public class UserController : Controller
    {
        private readonly ApiHelper _api;

        public UserController(ApiHelper api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var client = _api.CreateClientWithToken();
            var subjectId = HttpContext.Session.GetInt32("SubjectId") ?? 0;
            ViewBag.SubjectId = subjectId;

            var specialties = new List<Specialty>();
            var sResp = await client.GetAsync("api/Specialty/GetAll");
            if (sResp.IsSuccessStatusCode)
            {
                var sJson = await sResp.Content.ReadAsStringAsync();
                specialties = JsonConvert.DeserializeObject<List<Specialty>>(sJson) ?? new();
            }

            var spLookup = specialties.ToDictionary(s => s.SpecialtyId, s => s.Name);

            var dietitiansEntity = new List<ModelLayer.Dietitian>();
            var dResp = await client.GetAsync("api/Dietitian/GetAll");
            if (dResp.IsSuccessStatusCode)
            {
                var dJson = await dResp.Content.ReadAsStringAsync();
                dietitiansEntity = JsonConvert.DeserializeObject<List<ModelLayer.Dietitian>>(dJson) ?? new();
            }

            var dietitianCards = dietitiansEntity.Select(d => new DietitianCardViewModel
            {
                DietitianId = d.DietitianId,
                FullName = d.FullName,
                SpecialtyName = spLookup.TryGetValue(d.SpecialtyId, out var spName) ? spName : "—",
                ProfilePhotoUrl = string.IsNullOrWhiteSpace(d.ProfilePhotoUrl) ? "/img/default-user.png" : d.ProfilePhotoUrl,
                Biography = d.Biography ?? "",
                WorkingDays = d.WorkingDays,
                WorkingHours = d.WorkingHours
            }).ToList();

            var vm = new UserDashboardViewModel
            {
                Specialties = specialties,
                Dietitians = dietitianCards
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> MyAppointmentsPartial()
        {
            var userId = HttpContext.Session.GetInt32("SubjectId");
            if (userId == null) return Unauthorized();

            var client = _api.CreateClientWithToken();
            // Bu endpoint var mı? yoksa yazacağız:
            var resp = await client.GetAsync($"api/Appointment/User/{userId}");
            if (!resp.IsSuccessStatusCode)
                return PartialView("_MyAppointments", new List<ModelLayer.UserAppointmentDto>()); // boş döndür

            var json = await resp.Content.ReadAsStringAsync();
            var appts = JsonConvert.DeserializeObject<List<ModelLayer.UserAppointmentDto>>(json) ?? new();
            return PartialView("_MyAppointments", appts);
        }

        [HttpPost]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.PostAsync($"api/Appointment/Update/{id}", null);
            if (!resp.IsSuccessStatusCode)
                return BadRequest();
            return Ok();
        }
    }
}
