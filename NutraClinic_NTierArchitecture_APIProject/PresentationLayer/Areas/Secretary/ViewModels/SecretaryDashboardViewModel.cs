namespace PresentationLayer.Areas.Secretary.ViewModels
{
    public class SecretaryDashboardViewModel
    {
        public string SecretaryName { get; set; }
        public int TotalDietitians { get; set; }
        public int TotalUsers { get; set; }
        public int PendingAppointments { get; set; }
        public int TotalSpecialties { get; set; } 
        public List<DietitianListItemVM> Dietitians { get; set; }
        public List<SpecialtyListItemVM> Specialties { get; set; }
    }

}
