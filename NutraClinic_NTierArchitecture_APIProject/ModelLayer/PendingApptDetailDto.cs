using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class PendingApptDetailDto
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int DietitianId { get; set; }
        public string DietitianName { get; set; }
        public string SpecialtyName { get; set; } 
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AppointmentStatusDto
    {
        public string Status { get; set; } 
    }
}
