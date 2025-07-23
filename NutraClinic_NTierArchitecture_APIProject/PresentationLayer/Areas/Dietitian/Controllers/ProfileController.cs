using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayer.Areas.Dietitian.ViewModels;
using PresentationLayer.Helpers;
using System.Text;

namespace PresentationLayer.Areas.Dietitian.Controllers
{
    [Area("Dietitian")]
    public class ProfileController : Controller
    {
        private readonly ApiHelper _api;
        public ProfileController(ApiHelper api) => _api = api;

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _api.CreateClientWithToken();
            var resp = await client.GetAsync($"api/Dietitian/GetById/{id}");
            if (!resp.IsSuccessStatusCode) return NotFound();

            var json = await resp.Content.ReadAsStringAsync();
            var diet = JsonConvert.DeserializeObject<ModelLayer.Dietitian>(json);

            var vm = new DietitianEditVM
            {
                DietitianId = diet.DietitianId,
                FullName = diet.FullName,
                Email = diet.Email,
                Biography = diet.Biography,
                WorkingDays = diet.WorkingDays,
                WorkingHours = diet.WorkingHours
            };


            return PartialView("_EditPartial", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DietitianEditVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, error = "Form is invalid.", details = errors });
            }

            var client = _api.CreateClientWithToken();
            var payload = JsonConvert.SerializeObject(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var resp = await client.PutAsync($"api/Dietitian/Update/{model.DietitianId}", content);

            if (!resp.IsSuccessStatusCode)
            {
                return Json(new { success = false, error = "Update failed!" });
            }

            return Json(new { success = true });
        }

    }

}
