using System.Collections.Generic;
using FridgeData;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FridgeData.Authorization;
using FridgeData.Helpers;
using FridgeCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using FridgeCoreWeb.Repositories;
using FridgeData.Models;

namespace FridgeCoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private IGameViewRepository _repository;
        private ITimeHelper _timeHelper;
        private IUserService _userService;

        public HomeController(IGameViewRepository repository, ITimeHelper helper, IUserService userService)
        {
            _userService = userService;
            _repository = repository;
            _timeHelper = helper;
        }


        [Authorize]
        public IActionResult Index(int? userId)
        {
            var currentWeek = _timeHelper.GetCurrentWeek();
            var currentUser = _userService.GetCurrentUser();
            var user = currentUser;
            if (userId.HasValue && userId.GetValueOrDefault(0) != user.Id)
            {
                user = _userService.GetUser(userId.Value);
            }

            var gameGroups = GetGameGroups(currentWeek, user.Id, currentUser);
            var weekViewModel = new WeekViewModel { Week = currentWeek, GameGroups = gameGroups, UserId = user.Id };
            return View(new IndexViewModel { WeekViewModel = weekViewModel,
                                             CurrentWeek = currentWeek,
                                             UserId = user.Id,
                                             UserName = user.FullName });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Week(int id, int? userId)
        {
            var user = _userService.GetCurrentUser();
            userId = userId ?? user.Id;
            var gameGroups = GetGameGroups(id, userId, user);
            var vm = new WeekViewModel { Week = id, GameGroups = gameGroups };
            return PartialView(vm);
        }

        private List<GameViewGroupModel> GetGameGroups(int id, int? requestedUserId, User currentUser)
        {
            var gameGroups = _repository.GetGameViews(id, _timeHelper.GetCurrentSeason(), requestedUserId.Value,_timeHelper.GetCurrentTime(), requestedUserId.Value != currentUser.Id);
            gameGroups.OrderBy(g => g.GameTime);
            return gameGroups;
        }
    }
}
