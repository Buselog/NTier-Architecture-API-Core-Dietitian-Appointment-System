using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using Newtonsoft.Json;
using PresentationLayer.Areas.Dietitian.ViewModels;
using PresentationLayer.Helpers;

namespace PresentationLayer.Areas.Dietitian.Controllers
{
    [Area("Dietitian")]
    public class HomeController : Controller
    {
        private readonly ApiHelper _api;
        public HomeController(ApiHelper api) => _api = api;

        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("SubjectId");
            if (id == null)
                return RedirectToAction("Login", "Account", new { area = "Auth" });

            var client = _api.CreateClientWithToken();

            var dietResp = await client.GetAsync($"api/Dietitian/GetById/{id}");
            if (!dietResp.IsSuccessStatusCode)
            {
                TempData["Error"] = "Dietitian info load failed.";
                return View(new DietitianDashboardVM());
            }

            var dietJson = await dietResp.Content.ReadAsStringAsync();
            var diet = JsonConvert.DeserializeObject<ModelLayer.Dietitian>(dietJson)!;
           
            var approved = new List<ApprovedApptVM>();
            var apptResp = await client.GetAsync($"api/Appointment/DietitianApproved/{id}");
            if (apptResp.IsSuccessStatusCode)
            {
                var apptJson = await apptResp.Content.ReadAsStringAsync();
                var appts = JsonConvert.DeserializeObject<List<ModelLayer.Appointment>>(apptJson) ?? new();
                approved = await MapApprovedAppointmentsAsync(appts, client);
            }
            else
            {
                var fallbackResp = await client.GetAsync($"api/Appointment/dietitian/{id}");
                if (fallbackResp.IsSuccessStatusCode)
                {
                    var fbJson = await fallbackResp.Content.ReadAsStringAsync();
                    var appts = JsonConvert.DeserializeObject<List<ModelLayer.Appointment>>(fbJson) ?? new();
                    appts = appts.Where(a => a.Status == "Approved").ToList();
                    approved = await MapApprovedAppointmentsAsync(appts, client);
                }
            }

            var vm = new DietitianDashboardVM
            {
                DietitianId = diet.DietitianId,
                FullName = diet.FullName,
                Email = diet.Email,
                Biography = diet.Biography,
                WorkingDays = diet.WorkingDays,
                WorkingHours = diet.WorkingHours,
                ProfilePhotoUrl = string.IsNullOrWhiteSpace(diet.ProfilePhotoUrl) ? "/img/default-user.png" : diet.ProfilePhotoUrl,
                ApprovedAppointments = approved
            };

            return View(vm);
        }
        private async Task<List<ApprovedApptVM>> MapApprovedAppointmentsAsync(
            List<ModelLayer.Appointment> appts,
            HttpClient client)
        {
            var result = new List<ApprovedApptVM>();
            foreach (var a in appts)
            {
                string userName = "";
                if (a.UserId > 0)
                {
                    var userResp = await client.GetAsync($"api/User/GetById/{a.UserId}");
                    if (userResp.IsSuccessStatusCode)
                    {
                        var userJson = await userResp.Content.ReadAsStringAsync();
                        var usr = JsonConvert.DeserializeObject<ModelLayer.User>(userJson);
                        userName = usr?.FullName ?? "";
                    }
                }

                result.Add(new ApprovedApptVM
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate.Date,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    UserFullName = userName
                });
            }
            return result;
        }
    }
}
