using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class Appointment
    {
        public int AppointmentId { get; set; }     
        public int UserId { get; set; }              
        public User User { get; set; }                
        public int DietitianId { get; set; }         
        public Dietitian Dietitian { get; set; }      
        public DateTime AppointmentDate { get; set; } 
        public TimeSpan StartTime { get; set; }       
        public TimeSpan EndTime { get; set; }         
        public string Status { get; set; }             
        public DateTime CreatedAt { get; set; }       
    }
}
