using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class DietitianUpdateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public string WorkingDays { get; set; }
        public string WorkingHours { get; set; }
        public string Biography { get; set; }  
    }
}
