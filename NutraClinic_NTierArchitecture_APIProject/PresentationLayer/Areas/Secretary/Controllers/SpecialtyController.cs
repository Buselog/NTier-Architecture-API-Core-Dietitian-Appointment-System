using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayer.Areas.Secretary.ViewModels;
using PresentationLayer.Helpers;
using System.Text;

namespace PresentationLayer.Areas.Secretary.Controllers
{
    [Area("Secretary")]
    public class SpecialtyController : Controller
    {
        private readonly ApiHelper _api;
        public SpecialtyController(ApiHelper api) { _api = api; }

        [HttpGet]
        public IActionResult Create() => View(new SpecialtyCreateEditVM());

        [HttpPost]
        public async Task<IActionResult> Create(SpecialtyCreateEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var payload = new { Name = vm.Name };
            var client = _api.CreateClientWithToken();
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync("api/Specialty/Create", content);
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = "Create failed.";
                return View(vm);
            }
            TempData["ToastMessage"] = "Specialty created.";
            return RedirectToAction("Index", "Home", new { area = "Secretary" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.GetAsync($"api/Specialty/GetById/{id}");
            if (!resp.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home", new { area = "Secretary" });

            var json = await resp.Content.ReadAsStringAsync();
            dynamic dto = JsonConvert.DeserializeObject(json);

            var vm = new SpecialtyCreateEditVM
            {
                SpecialtyId = (int)dto.specialtyId,
                Name = (string)dto.name
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SpecialtyCreateEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var payload = new { SpecialtyId = vm.SpecialtyId, Name = vm.Name };
            var client = _api.CreateClientWithToken();
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var resp = await client.PutAsync($"api/Specialty/Update/{vm.SpecialtyId}", content);

            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = "Update failed.";
                return View(vm);
            }
            TempData["ToastMessage"] = "Specialty updated.";
            return RedirectToAction("Index", "Home", new { area = "Secretary" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.DeleteAsync($"api/Specialty/Delete/{id}");
            if (!resp.IsSuccessStatusCode)
                return Json(new { success = false, message = "Delete failed" });

            return Json(new { success = true, message = "Deleted" });
        }
    }
}
