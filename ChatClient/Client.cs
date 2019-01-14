using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
    public class Client
    {
        private TcpClient client = new TcpClient();
        private IPAddress address = IPAddress.Parse("192.168.1.105");
        private StreamReader sReader;

        public Client()
        {
            client.Connect(address, 12345);
            Stream stream = client.GetStream();
            sReader = new StreamReader(stream);
            BeginChat();
        }

        public void BeginChat()
        {
            Console.Write("Please select a username : ");
            var username = Console.ReadLine();
            SendToServer(username);
            while (true)
            {
                var message = Console.ReadLine();
                SendToServer(message);
                ReceiveFromServer();
            }
        }

        private void ReceiveFromServer()
        {
            var msg = sReader.GetData(sReader.ReadShort());
            Console.WriteLine(Encoding.ASCII.GetString(msg));
        }

        private void SendToServer(string message)
        {
            sReader.WriteShort((short)message.Length);
            sReader.WriteData(Encoding.ASCII.GetBytes(message));
        }
    }
}
