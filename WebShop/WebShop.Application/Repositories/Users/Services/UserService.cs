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
using WebShop.Domain.Entities.Users;
using WebShop.Framework.Paginations;
using WebShop.Framework.Security;

namespace WebShop.Application.Repositories.Users.Services
{
    public class UserService : IUserService
    {
        #region Inject
        private readonly IWebShopDbContext _db;

        public UserService(IWebShopDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Queries

        public ResultGetListDto<UserDto> GetAllUsers(int pageNum, int pageSize, string searchKey = "")
        {
            int rowsCount = 0;
            var users = _db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                searchKey = searchKey.ToLower();
                users.Where(
                    e => e.UserName.ToLower().Contains(searchKey) ||
                    e.Email.ToLower().Contains(searchKey) ||
                    e.UserName.ToLower().Contains(searchKey)
                    );

            }

            return new ResultGetListDto<UserDto>()
            {
                List = users.Include(e => e.Role)
                            .AsNoTracking()
                            .ToPaged(pageNum, pageSize, out rowsCount)
                            .Select(e => new UserDto()
                            {
                                Id = e.Id,
                                UserName = e.UserName,
                                FullName = e.FullName,
                                Email = e.Email,
                                Mobile = e.Mobile,
                                RoleId = e.RoleId,
                                RoleTitle = e.Role.RoleTitle,
                                IsDelete = e.IsDeleted


                            })
                            .OrderByDescending(e => e.Id)
                            .ToList()
                            ,
                rowsCount = rowsCount,
                PageCount = Pagination.PageCount(rowsCount, pageSize)
            };
        }
        public ResultGetListDto<UserDto> GetAllUsersDeleted(int pageNum, int pageSize, string searchKey = "")
        {
            int rowsCount = 0;
            var users = _db.Users.AsQueryable().IgnoreQueryFilters();
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                searchKey = searchKey.ToLower();
                users.Where(
                    e => e.UserName.ToLower().Contains(searchKey) ||
                    e.Email.ToLower().Contains(searchKey) ||
                    e.UserName.ToLower().Contains(searchKey)
                    );

            }

            return new ResultGetListDto<UserDto>()
            {
                List = users.Include(e => e.Role)
                            .Where(e => e.IsDeleted)
                            .AsNoTracking()
                            .ToPaged(pageNum, pageSize, out rowsCount)
                            .Select(e => new UserDto()
                            {
                                Id = e.Id,
                                UserName = e.UserName,
                                FullName = e.FullName,
                                Mobile = e.Mobile,
                                Email = e.Email,
                                RoleId = e.RoleId,
                                RoleTitle = e.Role.RoleTitle,
                                IsDelete = e.IsDeleted,
                            })
                            .OrderByDescending(e => e.Id)
                            .ToList()
                            ,
                rowsCount = rowsCount,
                PageCount = Pagination.PageCount(rowsCount, pageSize)
            };
        }
        public ResultDto<UserDto> GetUserById(int userId)
        {
            var getUser = _db.Users.SingleOrDefault(e => e.Id == userId);
            if (getUser != null)
            {
                var userDto = new UserDto()
                {
                    Id = getUser.Id,
                    UserName = getUser.UserName,
                    FullName = getUser.FullName,
                    Email = getUser.Email,
                    Mobile = getUser.Mobile,
                    RoleId = getUser.RoleId,
                    RoleTitle = getUser.Role.RoleTitle

                };
                return new ResultDto<UserDto>() { IsSuccess = true, Message = "Success", Result = userDto };
            }
            return new ResultDto<UserDto>() { IsSuccess = false, Message = "Failed" };
        }


        #endregion

        #region Commands
        public ResultDto<int> CreateUser(UserDto addUser)
        {

            if (_db.Users.IgnoreQueryFilters().Any(e => e.Email.ToLower() == addUser.Email.ToLower()))
            {
                return new ResultDto<int>() { IsSuccess = false, Message = "با این ایمیل قبلا ثبت نام شده است", Result = -1 };
            }
            if (_db.Users.Any(e => e.UserName == addUser.UserName))
            {
                return new ResultDto<int>() { IsSuccess = false, Message = "نام کاربری تکراری است", Result = -1 };
            }
            User user = new User()
            {
                Id = 0,
                UserName = addUser.UserName,
                FullName = addUser.FullName,
                Email = addUser.Email,
                Mobile = addUser.Mobile,
                RoleId = addUser.RoleId,
                Password = PasswordHasher.HashPassword(addUser.Password),

            };
            _db.Users.Add(user);
            _db.SaveChanges();
            return new ResultDto<int>() { IsSuccess = true, Message = "ثبت نام با موفقیت انجام شد", Result = user.Id };
        }

        public ResultDto DeleteUserOrUndoDelete(int userId)
        {
            var deleteUser = _db.Users.IgnoreQueryFilters().SingleOrDefault(e => e.Id == userId);
            if (deleteUser != null)
            {
                try
                {
                    deleteUser.IsDeleted = !deleteUser.IsDeleted;
                    _db.SaveChanges();
                    return new ResultDto() { IsSuccess = true, Message = "Success" };
                }
                catch (Exception e)
                {

                    return new ResultDto() { IsSuccess = true, Message = e.Message };
                }
            }
            return new ResultDto() { IsSuccess = true, Message = "Failed" };
        }
        public ResultDto ResetPassword(int userId)
        {
            var User = _db.Users.SingleOrDefault(e => e.Id == userId);
            if (User != null)
            {
                try
                {
                    User.Password = PasswordHasher.HashPassword("123");
                    _db.SaveChanges();
                    return new ResultDto() { IsSuccess = true, Message = "Success" };
                }
                catch (Exception e)
                {

                    return new ResultDto() { IsSuccess = true, Message = e.Message };
                }
            }
            return new ResultDto() { IsSuccess = true, Message = "Failed" };
        }

        public ResultDto UpdateUser(UserDto editUser)
        {
            var user = _db.Users.SingleOrDefault(e => e.Id == editUser.Id);
            if (user != null)
            {
                try
                {
                    user.FullName = editUser.FullName;
                    user.UserName = editUser.UserName;
                    user.Email = editUser.Email;
                    user.Password = PasswordHasher.HashPassword(editUser.Password);
                    user.Mobile = editUser.Mobile;
                    user.RoleId = editUser.RoleId;

                    _db.SaveChanges();
                    return new ResultDto() { IsSuccess = true, Message = "Success" };
                }
                catch (Exception e)
                {

                    return new ResultDto() { IsSuccess = true, Message = e.Message };
                }
            }
            return new ResultDto() { IsSuccess = true, Message = "Failed" };
        }
        #endregion
    }
}
