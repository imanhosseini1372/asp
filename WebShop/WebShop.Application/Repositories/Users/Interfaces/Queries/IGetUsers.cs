using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Dto.Users;

namespace WebShop.Application.Repositories.Users.Interfaces.Queries
{
    public interface IGetUsers
    {

        ResultGetUserDto GetAllUsers(RequestGetUserDto request);
    }
}
