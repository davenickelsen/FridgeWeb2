using System;
using System.Collections.Generic;
using FridgeCoreWeb.Models;
using FridgeCoreWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FridgeWeb2.Api
{
    [Authorize(Policy = "Administrators")]
    [Route("api/Lines")]
    public class LinesController : Controller
    {
        private ILinesRepository _repository;

        public LinesController(ILinesRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public string Post([FromBody] List<LineUpdateModel> updates)
        {
            try
            {
                _repository.SaveLines(updates);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}