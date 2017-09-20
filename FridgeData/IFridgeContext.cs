using FridgeData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FridgeData
{
    public interface IFridgeContext
    {
        DbSet<Game> Games { get; set; }
        DbSet<Pick> Picks { get; set; }
        DbSet<User> Users { get; set; }

        DbSet<SeasonTotal> SeasonTotals { get; set; }

        int SaveChanges();
        EntityEntry Attach(object entity);
        EntityEntry Add(object entity);
    }
}
