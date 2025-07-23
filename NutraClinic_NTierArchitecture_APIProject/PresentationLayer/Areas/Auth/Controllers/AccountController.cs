using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayer.Helpers;
using PresentationLayer.ViewModels;
using System.Text;

namespace PresentationLayer.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class AccountController : Controller
    {
        private readonly ApiHelper _api;

        public AccountController(ApiHelper api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model); 
            var payload = new
            {
                Email = model.Email,
                Password = model.Password
            };

            var client = _api.CreateClientWithToken();
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync("api/Auth/Login", content);

            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = "Geçersiz email veya şifre.";
                return View(model);
            }

            var json = await resp.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            string token = data.token;
            string role = data.role;
            string fullName = data.fullName;
            int subjectId = data.subjectId;

            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            HttpContext.Session.SetString("UserRole", role);
            HttpContext.Session.SetString("FullName", fullName);
            HttpContext.Session.SetInt32("SubjectId", (int)subjectId);

            return role switch
            {
                "Secretary" => RedirectToAction("Index", "Home", new { area = "Secretary" }),
                "Dietitian" => RedirectToAction("Index", "Home", new { area = "Dietitian" }),
                "User" =>  RedirectToAction("Index", "User", new { area = "User" }),

            _ => RedirectToAction("Login")
            };
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
