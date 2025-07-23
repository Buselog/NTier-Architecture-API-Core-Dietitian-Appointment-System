using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using Newtonsoft.Json;
using PresentationLayer.Areas.Secretary.ViewModels;
using PresentationLayer.Helpers;
using System.Text;

namespace PresentationLayer.Areas.Secretary.Controllers
{
    [Area("Secretary")]
    public class DietitianController : Controller
    {
        private readonly ApiHelper _api;

        public DietitianController(ApiHelper api)
        {
            _api = api;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new DietitianCreateVM();
            await LoadSpecialtiesToViewBag();
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Create(DietitianCreateVM vm)
        {
            var client = _api.CreateClientWithToken();

            var dto = new DietitianCreateDto
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Password = vm.Password,
                ProfilePhotoUrl = vm.ProfilePhotoUrl,
                Biography = vm.Biography,
                SpecialtyId = vm.SpecialtyId,
                WorkingDays = vm.WorkingDays,
                WorkingHours = vm.WorkingHours,
                CreatedAt = DateTime.Now
            };

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var resp = await client.PostAsync("api/Dietitian/Create", content);

            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = "Create failed.";
                ViewBag.ErrorDetails = await resp.Content.ReadAsStringAsync();
                await LoadSpecialtiesToViewBag(); 
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.DeleteAsync($"api/Dietitian/Delete/{id}");
            if (!resp.IsSuccessStatusCode)
                return Json(new { success = false, message = "Delete failed" });

            return Json(new { success = true, message = "Deleted" });
        }

        private async Task LoadSpecialtiesToViewBag()
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.GetAsync("api/Specialty/GetAll");
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Specialties = new List<dynamic>();
                return;
            }
            var json = await resp.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<dynamic>>(json) ?? new();
            ViewBag.Specialties = list;
        }
    }
}
