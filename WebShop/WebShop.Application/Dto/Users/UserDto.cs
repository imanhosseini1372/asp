using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Application.Dto.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }
    }
}
