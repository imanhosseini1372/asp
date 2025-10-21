using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Dto.Framework;
using WebShop.Application.Dto.Users;

namespace WebShop.Application.Repositories.Users.Interfaces
{
    public interface IRoleService
    {
        #region Queries
        ResultGetListDto<RoleDto> GetRoles();
        ResultDto<RoleDto> GetRoleById(int roleId);
        #endregion

        #region Commands

        #endregion
    }
}
