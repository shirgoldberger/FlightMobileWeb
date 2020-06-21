using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FlightMobileWeb.Data;

namespace FlightMobileWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        string httpAddress;
        public ScreenshotController(IConfiguration config) {
            httpAddress = config.GetValue<string>("Logging:DataOfServer:HttpAddress");
        }

        // GET: api/Screenshot
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var httpClient = new HttpClient {
                Timeout = TimeSpan.FromSeconds(10000)
            };
            try {
                HttpResponseMessage response = await 
                    httpClient.GetAsync(httpAddress +"/screenshot");
                var image = await response.Content.ReadAsStreamAsync();
                var stImg = File(image, "image/jpg");
                return stImg;
            }
            catch (Exception) {
                Console.WriteLine("http problem connection -Flight Gear");
            }
            return NotFound("http problem connection -Flight Gear");
        }
    }
}
