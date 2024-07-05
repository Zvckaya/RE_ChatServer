using Core;
using System.Net;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Listener listener = new Listener(); 
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
            listener.Init(endPoint, () => new ChatSession());

            Console.WriteLine("서버시작....");
            while (true)
            {

            }
        }
    }
}