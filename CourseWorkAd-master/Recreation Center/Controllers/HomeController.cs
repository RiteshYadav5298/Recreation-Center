using CourseWorkAd.DBContext;
using CourseWorkAd.Models;
using CourseWorkAd.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CourseWorkAd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext dbContext;
        private readonly ValidFavour _userService;

        public HomeController(ApplicationDBContext db, ValidFavour userService)
        {
            dbContext = db;
            _userService = userService;
        }

        public IActionResult Index(bool Islogout = false)
        {
            ViewBag.islogout = Islogout;
            return View();
        }

        //need to remove register from home
        public IActionResult RegisterUser(User users)
        {
            users.UserName = "admin";
            users.password = "admin";
            users.UserType = "admin";
            dbContext.Users.Add(users);
            dbContext.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            //login functionality
            ViewData["ReturnUrl"] = ReturnUrl;

            if (_userService.TryValidateUser(username, password, out List<Claim> claims))
            {
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);
                if (ReturnUrl != null)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Dashboard", "User", new { IsLogin = true });
                }
            }
            else
            {
                TempData["Error"] = "Invalid username or password";
                return Redirect("/");
            }
        }

        //logout of the appliation 

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}