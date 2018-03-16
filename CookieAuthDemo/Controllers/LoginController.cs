using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CookieAuthDemo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CookieAuthDemo.Controllers
{
    public class LoginController : Controller
    {
        UserDataAccessLayer userDataAccessLayer = new UserDataAccessLayer();

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser([Bind] UserDetails user)
        {
            if (ModelState.IsValid)
            {
                var RegistrationStatus = userDataAccessLayer.RegisterUser(user);
                if (RegistrationStatus == "Success")
                {
                    ModelState.Clear();
                    TempData["Success"] = "RegistrationStatus successful";
                    return View();
                }
                else
                {
                    TempData["Fail"] = "This User ID already exists. Registration Failed.";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult UserLogin(string returnUrl)
        {
            return View(new UserDetails(returnUrl));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin([Bind] UserDetails user)
        {
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");

            if (ModelState.IsValid)
            {
                var loginStatus = userDataAccessLayer.ValidateLogin(user);
                if (loginStatus == "Success")
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserId)
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);

                    if (string.IsNullOrEmpty(user.ReturnUrl))
                        return RedirectToAction("UserHome", "User");

                    return Redirect(user.ReturnUrl);
                }
                else
                {
                    TempData["UserLoginFailed"] = "Login Failed.Please enter correct credentials";
                    return View();
                }
            }
            else
                return View();
        }
    }
}