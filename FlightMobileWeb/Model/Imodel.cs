using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileWeb.Model
{
    interface IModel
    {
        public Task<Result> Execute(Command cmd);
        public void Run(string ip, int port);
        public void Connect();
        public void Disconnect();

        public void Start();
        public Result Send(string data, double value);
        public bool ISConnect();
    }
}
