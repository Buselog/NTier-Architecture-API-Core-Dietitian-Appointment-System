using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayer.Helpers;
using System.Text;

namespace PresentationLayer.Areas.Secretary.Controllers
{
    [Area("Secretary")]
    public class AppointmentAdminController : Controller
    {
        private readonly ApiHelper _api;
        public AppointmentAdminController(ApiHelper api) => _api = api;

        [HttpGet]
        public async Task<IActionResult> Pending(int? dietitianId)
        {
            var client = _api.CreateClientWithToken();
            string url = dietitianId.HasValue
                ? $"api/Appointment/PendingByDietitian/{dietitianId.Value}"
                : "api/Appointment/Pending";

            var resp = await client.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode);

            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpPut]
        public async Task<IActionResult> Approve(int id)
        {
            return await ChangeStatus(id, "Approved");
        }
        [HttpPut]
        public async Task<IActionResult> Reject(int id)
        {
            return await ChangeStatus(id, "Rejected");
        }

        private async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var client = _api.CreateClientWithToken();
            var payload = new { Status = status };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var resp = await client.PutAsync($"api/Appointment/Status/{id}", content);
            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode);

            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}
