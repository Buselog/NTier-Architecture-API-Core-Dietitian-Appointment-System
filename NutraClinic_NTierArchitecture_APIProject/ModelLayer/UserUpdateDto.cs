using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class UserUpdateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public string ProfilePhotoUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
