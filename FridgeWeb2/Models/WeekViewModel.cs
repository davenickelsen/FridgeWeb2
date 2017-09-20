using System.Collections.Generic;
using System.Linq;

namespace FridgeCoreWeb.Models
{
    public class WeekViewModel
    {
        public int Week { get; set; }
        public int UserId { get; set; }
        public List<GameViewGroupModel> GameGroups { get; set; }

        public int GameCount => Enumerable.SelectMany(GameGroups, g => g.GameViewModels).Count();

        public bool IsEditable => Enumerable.SelectMany(GameGroups, g => g.GameViewModels).Any(gvm => gvm.Editable);

        public int[] AvailableConfidences
        {
            get
            {
                {
                    var takenConfidences = Enumerable.SelectMany(GameGroups, gg => gg.GameViewModels)
                        .Select(gvm => gvm.Confidence ?? 0).Distinct();
                    var availableConfidences = Enumerable.Range(1, GameCount);
                    return availableConfidences.Where(c => !takenConfidences.Contains(c)).ToArray();
                }
            }
        }

        public string Rankings { get; set; }
        public string SelectedUserName { get; set; }

        public IEnumerable<string> UserNames { get; set; }
    }
}
