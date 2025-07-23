using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class Dietitian
    {
        public int DietitianId { get; set; }          
        public string FullName { get; set; }         
        public string Email { get; set; }            
        public string Password { get; set; }      
        public DateTime? DateOfBirth { get; set; }   
        public string? ProfilePhotoUrl { get; set; }  
        public DateTime CreatedAt { get; set; }       
        public string Biography { get; set; }         
        public string WorkingDays { get; set; }       
        public string WorkingHours { get; set; }     
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
    }
}
