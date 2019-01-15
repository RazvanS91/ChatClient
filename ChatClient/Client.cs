using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatClient
{
    public class Client
    {
        private TcpClient client = new TcpClient();
        private IPAddress address = IPAddress.Parse("192.168.1.105");
        private StreamReader sReader;
        private string username;
        private int consoleLine;

        public Client()
        {
            client.Connect(address, 12345);
            Stream stream = client.GetStream();
            sReader = new StreamReader(stream);
            BeginChat();
        }

        public void BeginChat()
        {
            new Thread(ReceiveFromServer).Start();
            SetUsername();
            while (true)
            {
                var message = Console.ReadLine();
                SendToServer(message);
            }
        }

        private void SetUsername()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Please select a username : ");
            username = Console.ReadLine();
            Console.SetCursorPosition(0, consoleLine + 1);
            Console.ResetColor();
            SendToServer(username);
            Console.Title = $"Welcome {username} !";
        }

        private void ReceiveFromServer()
        {
            consoleLine = 0;
            while (true)
            {
                var msg = sReader.GetData(sReader.ReadShort());
                if (String.IsNullOrEmpty(username))
                {
                    consoleLine++;
                    Console.WriteLine($"{GenerateNewLines(consoleLine)}{Encoding.ASCII.GetString(msg)}");
                    Console.SetCursorPosition(27, 0);
                }
                else
                    Console.WriteLine(Encoding.ASCII.GetString(msg));
            }
        }

        private string GenerateNewLines(int lines)
        {
            string result = null;
            for (int i = 0; i < lines; i++)
                result += "\n";
            return result;
        }

        private void SendToServer(string message)
        {
            sReader.WriteShort((short)message.Length);
            sReader.WriteData(Encoding.ASCII.GetBytes(message));
        }
    }
}
