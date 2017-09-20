namespace FridgeCoreWeb.Models
{
    public class IndexViewModel
    {
        public WeekViewModel WeekViewModel {get; set; } 

        public int CurrentWeek { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
}
}
