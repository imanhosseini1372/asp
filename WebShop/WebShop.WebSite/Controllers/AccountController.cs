using Microsoft.AspNetCore.Mvc;

namespace WebShop.WebSite.Controllers
{
    public class AccountController : Controller
    {

        #region Register

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        #endregion

        #region Login
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region Logout
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            return RedirectToAction();
        }
        #endregion
    }
}
