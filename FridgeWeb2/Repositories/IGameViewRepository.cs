using System;
using System.Collections.Generic;
using FridgeCoreWeb.Models;

namespace FridgeCoreWeb.Repositories
{
    public interface IGameViewRepository
    {
        List<GameViewGroupModel> GetGameViews(int week, int season, int userId, DateTime currentTime, bool restricted = false);
    }
}
