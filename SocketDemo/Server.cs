using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerDemo
{
    class Server
    {
        public static void Main(string[] args)
        {
            StartServer();
        }
        public static void StartServer()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress address = host.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(address, 11000);

            try
            {
                Socket listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(endPoint);
                listener.Listen(5);
                Console.WriteLine("Server waiting...");
                Socket handler = listener.Accept();

                string data = null;
                byte[] buffer = null;

                while(true)
                {
                    buffer = new byte[512];
                    int bytesRec = handler.Receive(buffer);
                    data += Encoding.ASCII.GetString(buffer, 0, bytesRec);
                    if (data.Contains("<EOF>"))
                        break;
                }

                Console.WriteLine("Text recieved: {0}", data);

                //Echo message back to client then close remote socket
                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
