using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileWeb.Data;
using FlightMobileWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {

        // GET: api/Commands
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Commands/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Commands
        [HttpPost]
        public async Task<ActionResult<Command>> Post(Command value)
        {
            if (!FlightMobile.simulatorModel.ISConnect()) {
                return NotFound("The fightgear is not connected to the server");
            }
            var res = await FlightMobile.simulatorModel.Execute(value);
            if (res == Result.Ok) {
                return Ok();
            }
            if (res == Result.Timeout) {
                return NotFound("Timeout of getting a result from the FlightGear");
            }
            if (res == Result.Disconnected) {
                return NotFound("The fightgear has been disconnected");
            }
            if (res == Result.ConnectionLost) {
                return NotFound("The connection with the flightgear has been lost");
            }
            if (res == Result.NotOk) {
                return NotFound("other problem");
            }
            if (res == Result.FailUpdate) {
                return NotFound("Failed to update command");
            }
            return NotFound();
        }
    }
}