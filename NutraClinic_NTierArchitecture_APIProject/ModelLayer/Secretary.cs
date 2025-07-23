using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class Secretary
    {
        public int SecretaryId { get; set; }  
        public string FullName { get; set; }         
        public string Email { get; set; }           
        public string Password { get; set; }    
        public DateTime CreatedAt { get; set; }     
    }
}