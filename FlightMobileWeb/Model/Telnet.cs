﻿using System;
using System.Net.Sockets;
using System.Text;

namespace FlightMobileWeb.Model
{
    public class Telnet
    {
        // For connecting to the simulator.
        TcpClient tcpclnt;
        // For reading and writing data.
        NetworkStream stm;
        private string ip;
        private int port;
        public Telnet(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
        public void Connect()
        {
            this.tcpclnt = new TcpClient();
            try
            {
                // Try connect the server.
                tcpclnt.Connect(ip, port);
                stm = tcpclnt.GetStream();
            }
            catch (Exception e)
            {
                // Can't connect.
                if (e.Message.Contains("No connection"))
                {
                    Exception e1 = new Exception("not connected");
                    throw e1;
                }
                Console.WriteLine("dfsfsdfsdfsdfs");
            }
        }
        public void Disconnect()
        {
            tcpclnt.Close();
        }
        public string Read()
        {
            byte[] byteArray = new byte[100];
            int k = this.stm.Read(byteArray, 0, 100);
            string massage = "";
            // Convert from bytes to string.
            for (int i = 0; i < k; i++)
                massage += (Convert.ToChar(byteArray[i]));
            return massage;
        }
        public void Write(string command)
        {
            this.stm = this.tcpclnt.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] byteArray = asen.GetBytes(command);
            stm.Write(byteArray, 0, byteArray.Length);
        }
        public void SetTimeOutRead(int time)
        {
            this.tcpclnt.ReceiveTimeout = time;
        }

        public TcpClient getTcpClient()
        {
            return this.tcpclnt;
        }

        public bool ISConnect()
        {
            return this.tcpclnt.Connected;
        }
    }
}