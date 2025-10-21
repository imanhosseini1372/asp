using Humanizer;
using Microsoft.AspNetCore.Mvc;
using WebShop.Application.Dto.Framework;
using WebShop.Application.Dto.Users;
using WebShop.Application.Repositories.Users.Interfaces;

namespace WebShop.WebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        #region Inject
        private readonly IUserService _UsersService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService UsersService,IRoleService roleService)
        {
            _UsersService = UsersService;
            _roleService = roleService;
        }
        #endregion

        #region Methods

        #region UserList

        public IActionResult UserList(int pageNum = 1, int pageSize = 10 ,bool isDelete=false)
        {
            ViewBag.DeleteList = isDelete;
            ResultGetListDto<UserDto> res;
            if (!isDelete)
            {
              
                res = _UsersService.GetAllUsers(pageNum, pageSize);
            }
            else 
            {
                
                res = _UsersService.GetAllUsersDeleted(pageNum, pageSize);
            }
                return View(res);
        }
        #endregion

      

        #region CreateUser
        
        
        public IActionResult CreateUser()
        {
            ViewBag.Role = _roleService.GetRoles().List;
            return View();
        }
        [HttpPost]
        public IActionResult CreateUser(UserDto user)
        {
            ViewBag.Role = _roleService.GetRoles().List;
            if (ModelState.IsValid)
            {
               var res= _UsersService.CreateUser(user);
                if (res.IsSuccess) 
                {
                    return RedirectToAction("UserList");
                }
                ModelState.AddModelError("", res.Message);
            }
            return View(user);
        }
        #endregion

        #region DeleteUser
        public IActionResult DeleteUser(int userId)
        {
            _UsersService.DeleteUserOrUndoDelete(userId);
            return RedirectToAction("UserList");
        }
        #endregion

        #region EditUser


        public IActionResult EditUser(int Id)
        {
            //ViewBag.Role = _userService.GetRoles().Where(e => e.Id != dto.RoleId);
            return View();
        }
        [HttpPost]
        public IActionResult EditUser(UserDto user)
        {
            //ViewBag.Role = _userService.GetRoles().Where(e => e.Id != dto.RoleId);
            return View();
        }
        #endregion

        #region ResetPassword

        public IActionResult ResetPassword(int userId) 
        {
            _UsersService.ResetPassword(userId);
            return RedirectToAction("UserList");
        }

        #endregion
        #endregion
    }
}
