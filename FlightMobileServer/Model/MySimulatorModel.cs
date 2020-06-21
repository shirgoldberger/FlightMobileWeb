using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.IO;
using FlightMobileServer.Data;
using FlightMobileServer.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlightSimulatorApp.Model
{
    public class MySimulatorModel
    {
        Telnet telnetClient;

        public MySimulatorModel() { }
        public void Run(string ip, int port)
        {
            // Set ip and port.
            this.telnetClient = new Telnet(ip, port);
            // Connect to the simulator.
            this.Connect();
            // Set time out to 10 seconds.
            this.telnetClient.SetTimeOutRead(10000);
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
                    //this.ConnectError = true;
                }
            }
        }
        public void Disconnect()
        {
            // Stop the connection with the simulator.
            telnetClient.Disconnect();
        }
        public string send(string data, double value)
        {
            try
            {
                telnetClient.Write("set " + data + " "  + value + "\r\n");
                telnetClient.Write("get " + data + "\r\n");
                
                if (Double.Parse(telnetClient.Read()) != value)
                {
                    return "Failed to update "+ data;
                }
            }
            catch (IOException e)
            {
                if (e.ToString().Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond."))
                {
                    return "Timeout of getting a result from the FlightGear";
                }
                else
                {
                    return "The fightgear has been disconnected";

                }
            }
            catch (Exception)
            {
                if (!telnetClient.IsConnect())
                {
                    return "The connection with the flightgear has been lost";
                }
                return "problems";
            }
            return "OK";
        }
    }
}