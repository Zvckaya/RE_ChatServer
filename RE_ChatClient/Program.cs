
using Core;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class ClientSession : Session
    {
        public override void OnConnected(EndPoint endpoint)
        {
            byte[] sendbuff = Encoding.UTF8.GetBytes($"{SessionId}님이 접속했습니다. ");
            Send(sendbuff);
        }

        public override void OnDisconnected(EndPoint endpoint)
        {
            
        }
        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string message = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[받음]:{message}");
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