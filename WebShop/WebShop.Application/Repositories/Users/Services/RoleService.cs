using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Dto.Framework;
using WebShop.Application.Dto.Users;
using WebShop.Application.Repositories.Framework.Interfaces.Contexts;
using WebShop.Application.Repositories.Users.Interfaces;

namespace WebShop.Application.Repositories.Users.Services
{
    public class RoleService : IRoleService
    {
        private readonly IWebShopDbContext _db;
        #region Inject

        public RoleService(IWebShopDbContext db)
        {
            _db = db;
        }
        #endregion
        #region Queries
        public ResultDto<RoleDto> GetRoleById(int roleId)
        {
            return new ResultDto<RoleDto>() { };
        }

        public ResultGetListDto<RoleDto> GetRoles()
        {
           return new ResultGetListDto<RoleDto>() 
           { rowsCount=_db.Roles.Count(),
           List= _db.Roles
           .AsNoTracking()
           .Select(e => new RoleDto() { RoleId = e.Id, RoleTitle = e.RoleTitle })
           .ToList()
           };
        }
        #endregion
    }
}
