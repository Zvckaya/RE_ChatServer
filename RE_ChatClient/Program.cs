
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Client
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(endPoint);

            byte[] sendBuff = Encoding.UTF8.GetBytes("Hello, World!");
            socket.Send(sendBuff);

            byte[] recvBuff = new byte[1024];
            int bytesReceived = socket.Receive(recvBuff);
            string recvData = Encoding.UTF8.GetString(recvBuff, 0, bytesReceived);

            Console.WriteLine($"[From Server] {recvData}");

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();


        }
    }
}