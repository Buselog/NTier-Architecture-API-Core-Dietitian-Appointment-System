using ModelLayer;

namespace PresentationLayer.ViewModels
{
    public class UserDashboardViewModel
    {
        public List<Specialty> Specialties { get; set; } = new();
        public List<DietitianCardViewModel> Dietitians { get; set; } = new();
    }
}
