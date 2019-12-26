using System;
using System.Collections.Generic;

namespace FridgeData.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string Login { get; set; }
        public string Email { get; set; }
        public string ScrambledPassword { get; set; }
        public bool? Admin { get; set; }
        public DateTime? LastViewedMessages { get; set; }
        public bool? Active { get; set; }
        public bool? NonPaying { get; set; }
        public ICollection<Pick> Picks { get; set; }
    }
}