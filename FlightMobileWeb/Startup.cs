using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FlightMobileWeb.Data;
using FlightMobileWeb.Model;

namespace FlightMobileWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the 
        //container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            string ip = Configuration.GetValue<string>("Logging:DataOfServer:Ip");
            int port = Configuration.GetValue<int>("Logging:DataOfServer:Port");
            FlightMobile.simulatorModel = new MySimulatorModel();
            FlightMobile.simulatorModel.Run(ip, port);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP 
        //request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
