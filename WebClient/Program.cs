using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebClient
{
    class Program
    {
        static void Main(string[] args)
        {
           try
           {
               string host = "www.google.com";
               TcpClient client = new TcpClient();
               client.Connect(host, 443);

               SslStream sslStream = new SslStream(
                   client.GetStream(),
                   false,
                   new RemoteCertificateValidationCallback(ValidateServerCertificate),
                   null
               );

               sslStream.AuthenticateAsClient("");
               sslStream.ReadTimeout = 2000;

               StringBuilder dataCompiler = new StringBuilder();
               dataCompiler.AppendLine("GET / HTTP/1.1");
               dataCompiler.AppendLine($"Host: {host}");
               dataCompiler.AppendLine("Accept: text/html");
               dataCompiler.AppendLine("Connection: close");
               dataCompiler.AppendLine($"User-Agent: {Assembly.GetExecutingAssembly().GetName().Name}");
               dataCompiler.AppendLine("");

               string requestData = dataCompiler.ToString();

               sslStream.Write(Encoding.UTF8.GetBytes(requestData));
               Console.WriteLine(requestData);

               var reader = new StreamReader(sslStream, Encoding.UTF8);

               Console.WriteLine("TCP Received:");
               Console.WriteLine(reader.ReadToEnd());

               reader.Close();
               client.Close();
           }
           catch (Exception e)
           {
               Console.WriteLine(e.ToString());
           }
        }
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
