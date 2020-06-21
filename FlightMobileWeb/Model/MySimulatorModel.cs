using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net.Sockets;
using FlightMobileWeb.Data;

namespace FlightMobileWeb.Model
{
    public class MySimulatorModel: IModel
    {
        Telnet telnetClient;
        private readonly BlockingCollection<AsyncCommand> queueCommand;
        public string elevatorCommand = "/controls/flight/elevator";
        public string rudderCommand = "/controls/flight/rudder";
        public string aileronCommand = "/controls/flight/aileron";
        public string throttleCommand = "/controls/engines/current-engine/throttle";


        public MySimulatorModel() {
            queueCommand = new BlockingCollection<AsyncCommand>();

        }
        public void Run(string ip, int port)
        {
            // Set ip and port.
            this.telnetClient = new Telnet(ip, port);
            // Connect to the simulator.
            this.Connect();
            // Set time out to 10 seconds.
            this.telnetClient.SetTimeOutRead(10000);
            Start();
        }
        public void Connect()
        {
            try
            {
                // Try connect.
                telnetClient.Connect();
                this.telnetClient.Write("data\n");
            }
            catch (Exception e)
            {
                // We couldn't connect.
                if (e.Message == "not connected")
                {
                    Console.WriteLine("not connected");
                }

            }
        }
        public void Disconnect()
        {
            // Stop the connection with the simulator.
            telnetClient.Disconnect();
        }

        public Task<Result> Execute(Command cmd) {

            var asyncCommand = new AsyncCommand(cmd);
            queueCommand.Add(asyncCommand);
            return asyncCommand.Task;
        }

        public void Start() {
            Task.Factory.StartNew(ProcessCommand);
        }

       
        private void ProcessCommand() {
            if (!FlightMobile.simulatorModel.ISConnect()) {
                return;
            }
            NetworkStream stream = telnetClient.getTcpClient().GetStream();
            foreach (AsyncCommand command in queueCommand.GetConsumingEnumerable()) {
                Result result;
                result = FlightMobile.simulatorModel.Send
                    (elevatorCommand, command.Command.Elevator);
                if (result !=Result.Ok) {
                    command.Completion.SetResult(result);
                    continue;
                }
                result = FlightMobile.simulatorModel.Send(rudderCommand, command.Command.Rudder);
                if (result != Result.Ok) {
                    command.Completion.SetResult(result);
                    continue;
                }
                result = FlightMobile.simulatorModel
                    .Send(aileronCommand, command.Command.Aileron);
                if (result != Result.Ok) {
                    command.Completion.SetResult(result);
                    continue;
                }
                result = FlightMobile.simulatorModel
                    .Send(throttleCommand, command.Command.Throttle);
                if (result != Result.Ok) {
                    command.Completion.SetResult(result);
                    continue;
                }
                command.Completion.SetResult(Result.Ok);
            }
        }

        //execptions:
        //0 = "OK".
        //1 = "Timeout of getting a result from the FlightGear"
        //2 = "The fightgear has been disconnected"
        //3 = "The connection with the flightgear has been lost"
        //4 =
        public Result Send(string data, double value) {
            try {
                telnetClient.Write("set " + data + " "  + value + "\r\n");
                telnetClient.Write("get " + data + "\r\n");
                double get = Double.Parse(telnetClient.Read());
                if (Math.Abs(get - value) > 0.1) {
                    return Result.FailUpdate;
                }
            }
            catch (IOException e) {
                if (e.ToString().Contains("A connection attempt failed because the " +
                    "connected party did not properly respond after a period of time, or " +
                    "established connection failed because connected host has failed to respond."))
                {
                    return Result.Timeout;
                }
                else {
                    return Result.Disconnected;
                }
            }
            catch (Exception) {
                if (!telnetClient.ISConnect()) {
                    return Result.ConnectionLost;
                }
                return Result.NotOk;
            }
            return Result.Ok;
        }
        public bool ISConnect()
        {
            return this.telnetClient.ISConnect();
        }
    }
}