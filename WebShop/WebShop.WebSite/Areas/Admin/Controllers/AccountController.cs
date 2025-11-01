using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Application.Dto.Framework;
using WebShop.Application.Dto.Users;
using WebShop.Application.Repositories.Users.Interfaces;

namespace WebShop.WebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            ViewBag.Previous = pageNum - 1;
            ViewBag.Next = pageNum + 1;
            ViewBag.PageNum = pageNum;

            ResultGetListDto<UserDto> res;
            if (!isDelete)
            {
              
                res = _UsersService.GetAllUsers(pageNum, pageSize);
            
            }
            else 
            {
                
                res = _UsersService.GetAllUsersDeleted(pageNum, pageSize);
            }
            //اگر صفحه آخر باشد باقی مانده ردیف ها را نشان میدهد و نباید به تعداد
            //pagesize ردیف برگردانده شود
            if (res.PageCount==pageNum)
            {
                int takeLast = res.rowsCount % pageSize;
               res.List= res.List.TakeLast(takeLast).ToList();
            }

            ViewBag.Count = res.PageCount;
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


        public IActionResult EditUser(int userId)
        {
            var userDto = _UsersService.GetUserById(userId).Result;
            var roles= _roleService.GetRoles().List;
            ViewBag.Role = roles.Where(e => e.RoleId != userDto.RoleId);
            return View(userDto);
        }
        [HttpPost]
        public IActionResult EditUser(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                var roles = _roleService.GetRoles().List;
                ViewBag.Role = roles;
                return View(user);
            }
            var res= _UsersService.UpdateUser(user);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
                var roles = _roleService.GetRoles().List;
                ViewBag.Role = roles;
                return View(user);
            }
            return RedirectToAction("UserList");
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
