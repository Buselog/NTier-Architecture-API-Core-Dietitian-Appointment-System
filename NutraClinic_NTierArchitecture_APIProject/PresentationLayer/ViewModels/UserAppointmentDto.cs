namespace PresentationLayer.ViewModels
{
    public class UserAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string DietitianName { get; set; }
        public string SpecialtyName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
