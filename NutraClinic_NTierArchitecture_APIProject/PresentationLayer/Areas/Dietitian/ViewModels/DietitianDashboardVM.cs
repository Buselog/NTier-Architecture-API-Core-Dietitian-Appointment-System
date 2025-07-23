namespace PresentationLayer.Areas.Dietitian.ViewModels
{
    public class DietitianDashboardVM
    {
        public int DietitianId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string SpecialtyName { get; set; } = "";
        public string Biography { get; set; } = "";
        public string WorkingDays { get; set; }     
        public string WorkingHours { get; set; } 
        public string? ProfilePhotoUrl { get; set; }
        public string? Password { get; set; }

        public List<ApprovedApptVM> ApprovedAppointments { get; set; } = new();
    }

    public class ApprovedApptVM
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string UserFullName { get; set; } = "";
    }

    public class DietitianEditVM
    {
        public int DietitianId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Password { get; set; }  
        public string Biography { get; set; } = "";
        public string WorkingDays { get; set; } = "";
        public string WorkingHours { get; set; } = "";
    }
  
}
