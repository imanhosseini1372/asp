using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebShop.Domain.Entities.Framework;

namespace WebShop.Domain.Entities.Users
{
    public class User:BaseEntity
    {
        public string UserName { get; set; }
       
        public string FullName { get; set; }
      
        public string Password { get; set; }
       
        public string Email { get; set; }
        public string Mobile { get; set; }
        
        public int RoleId { get; set; }
       
        public Role Role { get; set; }
       
    }
}
