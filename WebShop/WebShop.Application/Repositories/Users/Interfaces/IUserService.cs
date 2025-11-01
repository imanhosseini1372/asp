using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Dto.Framework;
using WebShop.Application.Dto.Users;

namespace WebShop.Application.Repositories.Users.Interfaces
{
    public interface IUserService
    {

        #region Queries
        ResultGetListDto<UserDto> GetAllUsers(int pageNum, int pageSize, string searchKey = "");
        ResultGetListDto<UserDto> GetAllUsersDeleted(int pageNum, int pageSize, string searchKey = "");
        ResultDto<UserDto> GetUserById(int userId);
        ResultDto<UserDto> GetUserByUserNameOrEmail(string searchKey);
        #endregion
        #region Commands
        ResultDto<int> CreateUser(UserDto addUser);
        ResultDto DeleteUserOrUndoDelete(int userId);
        ResultDto ResetPassword(int userId);
        ResultDto UpdateUser(UserDto editUser);
        #endregion
    }
}
