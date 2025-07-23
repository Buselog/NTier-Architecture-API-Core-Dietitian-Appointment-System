using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class UserAppointmentDto
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }

        public int DietitianId { get; set; }
        public string DietitianName { get; set; }
        public string DietitianPhoto { get; set; }
        public string SpecialtyName { get; set; }

        public bool IsPast => AppointmentDate.Date < DateTime.Today
                              || (AppointmentDate.Date == DateTime.Today && StartTime < DateTime.Now.TimeOfDay);
    }
}
