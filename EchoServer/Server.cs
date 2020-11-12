using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

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

            string serverCertificateFile = @"C:\Users\Bruger\Certificates\ServerSSL.pfx";
            bool clientCertificateRequired = false;
            bool checkCertificateRevocation = true;
            SslProtocols enabledSSLProtocols = SslProtocols.Tls;

            X509Certificate serverCertificate =
                new X509Certificate2(serverCertificateFile, "madsersej");

            TcpListener serverSocket = new TcpListener(IPAddress.Any, PORT);
            serverSocket.Start();
            Console.WriteLine("Server started");

            //Stream unsecureStream = connectionSocket.GetStream();
            //bool leaveInnerStreamOpen = false;
            //SslStream sslStream = new SslStream(unsecureStream, leaveInnerStreamOpen);
            //sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired,
            //    enabledSSLProtocols, checkCertificateRevocation);

            TcpClient connectionSocket = serverSocket.AcceptTcpClient();
            Stream uns = connectionSocket.GetStream();
            bool leaveInnerStreamOpen = false;
            SslStream SslStream = new SslStream(uns,leaveInnerStreamOpen);
            SslStream.AuthenticateAsServer(serverCertificate);

            using (StreamReader sr = new StreamReader(SslStream))
            using (StreamWriter sw = new StreamWriter(SslStream))
                
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