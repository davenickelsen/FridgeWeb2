using System;
using FridgeData;
using FridgeData.Models;
using FridgeCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using FridgeData.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FridgeCoreWeb.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<AppUser> _signInManager;
        private IFridgeContext _context;
        private IUserService _userService;

        public AccountController(IFridgeContext context, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userService = userService;
            _context = context;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        // GET: /<controller>/
        public IActionResult LogIn()
        {
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            var user = _context.Users.SingleOrDefault(u => string.Compare(u.Login, model.UserName, StringComparison.OrdinalIgnoreCase) == 0);
            if (user != null)
            {
                var appUser = await _userService.FindByIdAsync(user.Id.ToString());
                if (appUser == null)
                {
                    //appUser = new AppUser { Id = user.Id.ToString(), SelectedUserName = model.SelectedUserName.ToLower() };
                    //appUser.Claims.Add(new IdentityUserClaim<string>());
                    //var result = await _userService.CreateAsync(appUser, model.Password);
                    //if (!result.Succeeded)
                    //{
                        return ReportLoginError(model, "Could not authenticate user");
                    //}
                }
                try
                {
                    await _signInManager.PasswordSignInAsync(appUser, model.Password, true, false);
                    
                }
                catch //(Exception ex)
                {
                    return ReportLoginError(model);
                }
                return RedirectToAction("Index", "Home");

            }
            return ReportLoginError(model);

        }

        private IActionResult ReportLoginError(LoginViewModel model, string errorMessage = "Invalid login attempt")
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }
    }
}
