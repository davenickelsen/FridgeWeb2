using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FridgeWeb2.Models
{
    public class WeekHeaderViewModel
    {
        public IEnumerable<SelectListItem> Users { get; set; }
        public string SelectedUserName { get; set; }
        public string Rankings { get; set; }
        public int SelectedWeek { get; set; }
        public IEnumerable<SelectListItem> Weeks { get; set; }
        public string WeekTotals { get; set; }
    }
}
