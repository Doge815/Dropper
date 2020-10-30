using System.Net.NetworkInformation;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Dropper.Extensions;
using System.Text;

namespace Dropper.Backend
{
    public class Connector
    {
        public static int port = 50;
        public static IPAddress localAddr = IPAddress.Parse("127.0.0.1");

        TcpListener Server;
        TcpClient Client;

        List<IPAddress> Devices;

        public Connector()
        {
            Server = new TcpListener(localAddr, port);
            Server.Start();
            ThreadPool.QueueUserWorkItem(HandleRequests);

            Client = new TcpClient();
            Client.Connect(localAddr, port);
            var ns = Client.GetStream();
            Debug.Print("Send MSG");
            ns.WriteOnNetworkStream("MSG");
        }

        public void HandleRequests(object stateInfo)
        {
            ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
            TcpClient client = null;

            void DoAcceptTcpClientCallback(IAsyncResult ar)
            {
                TcpListener listener = (TcpListener)ar.AsyncState;
                client = listener.EndAcceptTcpClient(ar);
                Debug.Print("Client connected");
                tcpClientConnected.Set();
            }

            Debug.Print("Server started");
            while (true)
            {
                tcpClientConnected.Reset();
                Server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), Server);
                tcpClientConnected.WaitOne();
                var stream = client.GetStream();

                string text = stream.ReadNetworkStream();
                Debug.Print("msg is: " + text);

                stream.WriteOnNetworkStream("ANSWER");
            }
        }

        public void FindNewClients()
        {

        }

        public void ResetClients()
        {

        }
    }
}
