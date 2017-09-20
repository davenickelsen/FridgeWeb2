using System.Collections.Generic;
using FridgeData.Models;
using FridgeCoreWeb.Models;

namespace FridgeCoreWeb.Repositories
{
    public interface ILinesRepository
    {
        List<Game> GetGames(int week, int season);
        void SaveLines(List<LineUpdateModel> updates);
    }
}
