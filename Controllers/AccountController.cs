using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

using Client.Models;
using Client.Services;
using Client.Extensions;
using Client.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        [BindProperty]
        public LoginModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> Login(LoginModel model, string ReturnUrl = "/")
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            model.ReturnUrl = ReturnUrl;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, UserServices Svc, CrytoUtilsExtensions Cryto){
            if (ModelState.IsValid)
            {
                Users user = Svc.GetUser(Input.User.Username); //AuthenticateUser(Input.Email, Input.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    model.Message = "Invalid login attempt.";
                    return View(model);
                }

                if (!user.Enabled){
                    ModelState.AddModelError(string.Empty, "Login account Disabled.");
                    model.Message = "Login account Disabled.";
                    return View(model);
                }

                if(user.ToChange){
                    //Redirect Page to Change Password, Before Contininuing.
                }

                if (!Cryto.Decrypt(user.Password).Equals(Input.User.Password)){
                    ModelState.AddModelError(string.Empty, "Login Failed. Invalid password.");
                    model.Message = "Login Failed. Invalid password.";
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.UserData, user.Username),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (!string.IsNullOrEmpty(Input.ReturnUrl.Trim()))
                    return LocalRedirect(Input.ReturnUrl.Trim());
                return LocalRedirect("/");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
