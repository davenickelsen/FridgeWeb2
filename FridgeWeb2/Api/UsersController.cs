using System;
using FridgeData.Authorization;
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

        public UsersController(IUserService userService)
        {
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

            var user = _userService.FindByIdAsync(model.UserName).GetAwaiter().GetResult();
            if (user != null)
            {
                throw new Exception("User already exists");
            }

            return _userService.CreateAsync(new AppUser {Id = model.UserName}, model.Password).GetAwaiter().GetResult() == IdentityResult.Success;
        }
        
        
    }
}