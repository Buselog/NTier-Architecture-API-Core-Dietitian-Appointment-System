using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var fullName = HttpContext.Session.GetString("FullName");
            return View("Default", (role, fullName));
        }
    }
}
