using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class User
    {
        public int UserId { get; set; }             
        public string FullName { get; set; }         
        public string Email { get; set; }           
        public string Password { get; set; }    
        public DateTime? DateOfBirth { get; set; }  
        public string? ProfilePhotoUrl { get; set; } 
        public bool IsEmailConfirmed { get; set; }  
        public bool IsApproved { get; set; }         
        public DateTime CreatedAt { get; set; }      
    }
}
