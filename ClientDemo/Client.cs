using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientDemo
{
    class Client
    {
        public static void Main(string[] args)
        {
            StartClient();
        }
        public static void StartClient()
        {
            //Message to send and recieve buffer
            byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
            byte[] bytes = new byte[512];

            //Configure endpoint for server
            IPHostEntry remoteHost = Dns.GetHostEntry("localhost");
            IPAddress address = remoteHost.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(address, 11000);

            try
            {
                //Create socket to connect to server
                Socket sender = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(remoteEP);
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                //Send message to server through sender socket
                int bytesSent = sender.Send(msg);

                //Recieve response from server
                string data = null;
                while (true)
                {
                    int bytesRec = sender.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.Contains("<EOF>"))
                        break;
                }

                //Print response
                Console.WriteLine("Echo test: {0}", data);

                //Release sender socket
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
