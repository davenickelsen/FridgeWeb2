using System.Collections.Generic;
using System.Threading.Tasks;
using FridgeData.Models;
using Microsoft.AspNetCore.Identity;

namespace FridgeData.Authorization
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(AppUser user, string password);

        Task<AppUser> FindByIdAsync(string userId);

        IEnumerable<User> GetAllUsers();

        User GetUser(int id);
        User GetCurrentUser();

        //Task<bool> Test();

    }
}
