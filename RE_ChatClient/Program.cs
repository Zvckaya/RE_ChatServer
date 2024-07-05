
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
            Console.WriteLine("접속");
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

        public void SendMessage(string message)
        {
            byte[] sendBuff = Encoding.UTF8.GetBytes(message);
            Send(new ArraySegment<byte>(sendBuff,0, sendBuff.Length));
            
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

            ClientSession session = new ClientSession();
            

            Connector connector = new Connector();
            connector.Connect(endPoint, () => {
                session = new ClientSession();
                return session;
            });
            

            while (true)
            {
                string input = Console.ReadLine();
                session.SendMessage(input);
                
            }
        }
    }
}