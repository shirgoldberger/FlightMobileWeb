using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileServer.Data;
using FlightMobileServer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileServer.Controllers
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
            string result;
            result = FlightMobile.s.send("/controls/flight/elevator", value.Elevator);
            if (String.Compare(result, "OK") != 0) {
                return NotFound(result);
            }
            result =FlightMobile.s.send("/controls/flight/rudder", value.Rudder);
            if (String.Compare(result, "OK") != 0)
            {
                return NotFound(result);
            }
            result = FlightMobile.s.send("/controls/flight/aileron", value.Aileron);
            if (String.Compare(result, "OK") != 0)
            {
                return NotFound(result);
            }
            result = FlightMobile.s.send("/controls/engines/current-engine/throttle", value.Throttle);
            if (String.Compare(result, "OK") != 0)
            {
                return NotFound(result);
            }
            return Ok();
        }
    }
}
