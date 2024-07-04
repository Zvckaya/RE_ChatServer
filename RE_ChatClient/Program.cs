
using Core;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class ClientSession : Session
    {
        public override void OnConnected(EndPoint endpoint)
        {
            Console.WriteLine (endpoint.ToString());
        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
        }

        public override void OnSend(ArraySegment<byte> buffer)
        {

        }
    }

    class Client
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new ClientSession(); });

            while (true)
            {

            }
            


        }
    }
}