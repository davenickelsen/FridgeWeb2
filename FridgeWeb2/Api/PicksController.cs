using System;
using System.Collections.Generic;
using System.Linq;
using FridgeCoreWeb.Models;
using FridgeData;
using FridgeData.Authorization;
using FridgeData.Helpers;
using FridgeData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FridgeWeb2.Api
{
    [Route("api/Picks")]
    public class PicksController : Controller
    {
        private IUserService _service;
        private IFridgeContext _context;
        private ITimeHelper _timeHelper;

        public PicksController(IUserService service, IFridgeContext context, ITimeHelper helper)
        {
            _timeHelper = helper;
            _context = context;
            _service = service;
        }

        
        // POST api/values
        [HttpPost]
        public object Post([FromBody] List<UpdatedPickModel> updatedPicks)
        {
            int updateCount = 0;
            User user = _service.GetCurrentUser();
            if (updatedPicks.Any(p => p.UserId != user.Id))
                throw new NotSupportedException();
        
            foreach (var update in updatedPicks)
            {
                var game = _context.Games.Find(update.GameId);
                if (game.GameTime >= _timeHelper.GetCurrentTime())
                {
                    var pickEntity = FindOrCreatePick(update, user.Id);
                    UpdatePick(pickEntity, update);
                    updateCount++;
                }

            }
            _context.SaveChanges();
            return new {updated = updateCount, sent = updatedPicks.Count};
        }

        private void UpdatePick(Pick pickEntity, UpdatedPickModel update)
        {
            pickEntity.GameId = update.GameId;
            pickEntity.UserId = update.UserId;
            pickEntity.VersusSpread = update.VersusSpread;
            pickEntity.EvenUp = update.EvenUp;
            pickEntity.Confidence = update.Confidence;
        }

        private Pick FindOrCreatePick(UpdatedPickModel update, int userId)
        {
            var existingPick = _context.Picks.SingleOrDefault(p => p.GameId == update.GameId && p.UserId == userId);
            if (existingPick != null)
            {
                return existingPick;
            }
            
            var pick = new Pick();
            pick.UserId = userId;
            _context.Add(pick);
            return pick;
        }
    }
}
