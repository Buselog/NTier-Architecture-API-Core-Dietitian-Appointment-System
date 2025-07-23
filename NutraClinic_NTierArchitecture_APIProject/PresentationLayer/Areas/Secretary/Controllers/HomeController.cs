using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using Newtonsoft.Json;
using PresentationLayer.Areas.Secretary.ViewModels;
using PresentationLayer.Helpers;

namespace PresentationLayer.Areas.Secretary.Controllers
{
    [Area("Secretary")]
    public class HomeController : Controller
    {
        private readonly ApiHelper _api;

        public HomeController(ApiHelper api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var secName = HttpContext.Session.GetString("FullName") ?? "Secretary";

            var client = _api.CreateClientWithToken();

            var dietitianResp = await client.GetAsync("api/Dietitian/GetAll");
            var dietitians = new List<ModelLayer.Dietitian>();
            if (dietitianResp.IsSuccessStatusCode)
            {
                var json = await dietitianResp.Content.ReadAsStringAsync();
                dietitians = JsonConvert.DeserializeObject<List<ModelLayer.Dietitian>>(json) ?? new();
            }

            var userResp = await client.GetAsync("api/User/GetAll");
            var users = new List<ModelLayer.User>();
            if (userResp.IsSuccessStatusCode)
            {
                var json = await userResp.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<ModelLayer.User>>(json) ?? new();
            }

            var appResp = await client.GetAsync("api/Appointment/GetAll");
            var appointments = new List<Appointment>();
            if (appResp.IsSuccessStatusCode)
            {
                var json = await appResp.Content.ReadAsStringAsync();
                appointments = JsonConvert.DeserializeObject<List<Appointment>>(json) ?? new();
            }

            var specResp = await client.GetAsync("api/Specialty/GetAll");
            var specialties = new List<Specialty>();
            if (specResp.IsSuccessStatusCode)
            {
                var json = await specResp.Content.ReadAsStringAsync();
                specialties = JsonConvert.DeserializeObject<List<Specialty>>(json) ?? new();
            }

            var vm = new SecretaryDashboardViewModel
            {
                SecretaryName = secName,
                TotalDietitians = dietitians.Count,
                TotalUsers = users.Count,
                PendingAppointments = appointments.Count(a => a.Status == "Pending"),
                TotalSpecialties = specialties.Count,  

                Dietitians = dietitians.Select(d => new DietitianListItemVM
                {
                    DietitianId = d.DietitianId,
                    FullName = d.FullName,
                    SpecialtyName = specialties.FirstOrDefault(s => s.SpecialtyId == d.SpecialtyId)?.Name,
                    ProfilePhotoUrl = string.IsNullOrWhiteSpace(d.ProfilePhotoUrl) ? "/img/default-user.png" : d.ProfilePhotoUrl,
                    WorkingDays = d.WorkingDays,
                    WorkingHours = d.WorkingHours
                }).ToList(),
                Specialties = specialties.Select(s => new SpecialtyListItemVM
                {
                    SpecialtyId = s.SpecialtyId,
                    Name = s.Name
                }).ToList()
            };

            return View(vm);
        }
    }
}
