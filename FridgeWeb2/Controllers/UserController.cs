using FridgeCoreWeb.Models;
using FridgeData.Authorization;
using FridgeData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FridgeWeb2.Controllers
{
    public class UserController : Controller
    {
        private IUserService _service;
        private UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> manager, IUserService service)
        {
            _userManager = manager;
            _service = service;
        }

        [Authorize]
        public IActionResult Index()
        {
            var currentUser = _service.GetCurrentUser();
            var model = GetBlankUserModel(currentUser);
            return View("Index", model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(UserViewModel model)
        {
            var currentUser = _service.GetCurrentUser();
            var appUser =_userManager.FindByIdAsync(currentUser.Id.ToString()).GetAwaiter().GetResult();
            var result = _userManager.CheckPasswordAsync(appUser, model.OldPassword).GetAwaiter().GetResult();
            var returnModel = GetBlankUserModel(currentUser);
            if (result)
            {
                var changeResult =_userManager.ChangePasswordAsync(appUser, model.OldPassword, model.NewPassword).GetAwaiter().GetResult();
                if (changeResult != IdentityResult.Success)
                    result = false;
                else
                {
                    result = UpdateUser(currentUser, model);
                }
            }
            returnModel.Message = result ? "Password changed successfully." : "Password change unsuccessful. Check input and try again.";
            return View("Index", returnModel);
        }

        private bool UpdateUser(User currentUser, UserViewModel model)
        {
            if (_service.LoginExists(model.Login, currentUser.Id))
            {
                return false;
            }
            currentUser.Login = model.Login;
            currentUser.Email = model.Email;
            _service.UpdateUser(currentUser);
            return true;
        }

        private static UserViewModel GetBlankUserModel(User currentUser)
        {
            return new UserViewModel
            {
                Login = currentUser.Login,
                FullName = currentUser.FullName,
                OldPassword = "",
                NewPassword = "",
                Message = ""
            };
        }
    }
}
