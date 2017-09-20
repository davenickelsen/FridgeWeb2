using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FridgeData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FridgeData.Authorization
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> _manager;
        private IHttpContextAccessor _accessor;
        private IFridgeContext _fridgeContext;
        private RoleManager<IdentityRole> _roleManager;

        public UserService(IFridgeContext context, UserManager<AppUser> manager, IHttpContextAccessor accessor, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _fridgeContext = context;
            _accessor = accessor;
            _manager = manager;         
            
        }

        public Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return _manager.CreateAsync(user, password);
        }

        public Task<AppUser> FindByIdAsync(string userId)
        {
            return _manager.FindByIdAsync(userId);
        }

        public User GetUser(int id)
        {
            return _fridgeContext.Users.Single(u => u.Id == id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _fridgeContext.Users.Where(u => u.Active == true).ToList();
        }
        public User GetCurrentUser()
        {
            var id =_manager.GetUserId(_accessor.HttpContext.User);
            if (id == null)
                return null;
            return _fridgeContext.Users.Single(u => u.Id.ToString() == id);

        }

        //public async Task<bool> Test()
        //{
        //    var roleName = "Administrator";
        //    var role =new IdentityRole(roleName);
        //    if (!_roleManager.RoleExistsAsync(roleName).Result)
        //    {
        //        var done = _roleManager.CreateAsync(role).Result;
        //    }
        //    var appUser = _manager.FindByIdAsync("7").Result;
        //    var result = _manager.AddToRoleAsync(appUser, roleName).Result;
            
        //    return true;
        //}
    }
}