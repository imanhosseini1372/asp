using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Dto.Users;
using WebShop.Application.Repositories.Framework.Interfaces.Contexts;
using WebShop.Application.Repositories.Users.Interfaces.Queries;
using WebShop.Framework.Paginations;

namespace WebShop.Application.Repositories.Users.Services.Queries
{
    public class GetUsers : IGetUsers
    {
        private readonly IWebShopDbContext _db;

        public GetUsers(IWebShopDbContext db)
        {
            _db = db;
        }



        public ResultGetUserDto GetAllUsers(RequestGetUserDto request)
        {
            int rowsCount = 0;
            var users = _db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                request.SearchKey = request.SearchKey.ToLower();
                users.Where(
                    e => e.UserName.ToLower().Contains(request.SearchKey) ||
                    e.Email.ToLower().Contains(request.SearchKey) ||
                    e.UserName.ToLower().Contains(request.SearchKey)
                    );

            }

            return new ResultGetUserDto()
            {
                List = users
                            .ToPaged(request.PageNum, request.PageSize, out rowsCount)
                            .Select(e => new UserDto()
                            {
                                Id = e.Id,
                                UserName = e.UserName,
                                FullName = e.FullName,
                                Email = e.Email,
                                RoleId = e.RoleId
                            }).ToList()
                            ,
                rowsCount = rowsCount
            };
        }
    }
}
