using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Application.Dto.Users;
using WebShop.Application.Repositories.Users.Interfaces;
using WebShop.Framework.Security;

namespace WebShop.WebSite.Controllers
{
    public class AccountController : Controller
    {

        #region Inject
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Register

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public IActionResult Register(RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            if (register.Password!=register.RePassword)
            {
                ModelState.AddModelError("","رمز عبور با تکرار رمز عبور یکسان نیست");
                return View(register);
            }
            var user = new UserDto() 
            {
                FullName=register.FullName,
                UserName= register.UserName,
                Email=register.Email,
                Password=register.Password,
                Mobile=register.Mobile,
                RoleId=3,
                IsDelete=false
            };
            var res=_userService.CreateUser(user);
            if (res.IsSuccess)
            {
                return RedirectToAction("Login", new LoginDto() { UserNameOrEmail = user.Email });
            }
            else
            {
                ModelState.AddModelError("",res.Message);
                return View(register);
            }
            

        }

        #endregion

        #region Login

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var res=_userService.GetUserByUserNameOrEmail(login.UserNameOrEmail);
            if (!res.IsSuccess||!PasswordHasher.VerifyHashedPassword(res.Result.Password,login.Password))
            {
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                return View(login);
            }
            List<Claim> claims = new List<Claim>()
            {
            new Claim(ClaimTypes.Name,res.Result.UserName),
            new Claim(ClaimTypes.NameIdentifier,res.Result.Id.ToString()),
            new Claim(ClaimTypes.GivenName,res.Result.FullName),
            new Claim(ClaimTypes.Role,res.Result.RoleTitle),
            };
            var identity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var principal=new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties()
            {
                IsPersistent = login.RememberMe
            };
            HttpContext.SignInAsync(principal, properties);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logout
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
        public IActionResult AccessDenied()
        {
            return View("Login");
        }
        #endregion
    }
}
