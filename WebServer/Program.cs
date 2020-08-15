using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, 7777);

                Console.WriteLine("TCP Start:");
                server.Start();
                while (true)
                {
                Console.WriteLine("TCP Wait for a client:");
                TcpClient client = server.AcceptTcpClient();

                NetworkStream stream= client.GetStream();

                string response = "Hello from the server";
                byte [] data = Encoding.UTF8.GetBytes(response);

                stream.Write(data, 0, data.Length);
                Console.WriteLine("Send: {0}", response);

                stream.Close();
                client.Close();
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (server != null)
                server.Stop();
        }
    }
}
}
