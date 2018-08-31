using System;
using System.Linq;
using FridgeData;
using FridgeData.Authorization;
using FridgeData.Models;
using FridgeWeb2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FridgeWeb2.Api
{
    [Route("api/Users")]
    public class UsersController
    {
        private IUserService _userService;
        private IFridgeContext _context;

        public UsersController(IUserService userService, IFridgeContext context)
        {
            _context = context;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public bool Post([FromBody] CreateUserModel model)
        {
            if (model.Token != Environment.GetEnvironmentVariable("FRIDGE_API_TOKEN"))
            {
                throw new UnauthorizedAccessException("Bad key");
            }

            User user = _context.Users.SingleOrDefault(u => u.Login.ToLower() == model.Login.ToLower());
            if (user != null)
            {
                throw new Exception("User already exists");
            }

            user = new User {FirstName = model.FirstName, Active = true, Admin = false, LastName = model.LastName, Email = model.Email, Login = model.Login, ScrambledPassword = "Default"};
            _context.Users.Add(user);
            _context.SaveChanges();
            
            return _userService.CreateAsync(new AppUser {Id = user.Id.ToString(), UserName = model.Login}, model.Password).GetAwaiter().GetResult() == IdentityResult.Success;
        }
        
        
    }
}