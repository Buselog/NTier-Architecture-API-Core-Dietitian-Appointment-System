namespace PresentationLayer.Areas.Secretary.ViewModels
{
    public class DietitianCreateVM
    {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string? ProfilePhotoUrl { get; set; }
            public string? Biography { get; set; }
            public int SpecialtyId { get; set; }
            public string? WorkingDays { get; set; }
            public string? WorkingHours { get; set; }
        }
}
