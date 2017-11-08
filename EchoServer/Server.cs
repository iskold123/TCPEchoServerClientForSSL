using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace EchoServer
{
    internal class Server
    {
        private int PORT;

        public Server(int port)
        {
            this.PORT = port;
        }

        public void Start()
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Loopback, PORT);
            serverSocket.Start();
            Console.WriteLine("Server started");

            using (TcpClient connectionSocket = serverSocket.AcceptTcpClient())
            using (Stream ns = connectionSocket.GetStream())
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                Console.WriteLine("Server activated");
                sw.AutoFlush = true; // enable automatic flushing

                string message = sr.ReadLine(); // read string from client
                string answer = "";
                while (!string.IsNullOrEmpty(message))
                {

                    Console.WriteLine("Client: " + message);
                    answer = message.ToUpper(); // convert string to upper case
                    sw.WriteLine(answer); // send back upper case string
                    message = sr.ReadLine();

                }
            }
        }
    }
}