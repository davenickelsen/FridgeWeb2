using System.Threading.Tasks;
using FridgeData.Authorization;
using FridgeCoreWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace FridgeCoreWeb.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
     {
            private IUserService _userService;

            public NavigationViewComponent(IUserService service)
            {
                _userService = service;
            }

            public async Task<IViewComponentResult> InvokeAsync()
            {
                var user = _userService.GetCurrentUser();
                return View("Default", new NavigationViewModel{IsAdmin = user.Admin.GetValueOrDefault(false)});
            }
    }
}
