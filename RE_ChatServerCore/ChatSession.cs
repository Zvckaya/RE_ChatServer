using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ChatSession : Session
    {
        public override void OnConnected(EndPoint endpoint )
        {
            byte[] sendbuff = Encoding.UTF8.GetBytes($"{SessionId}님 어서오세요 ");
            Send(sendbuff);
        }

        public override void OnDisconnected(EndPoint endpoint)
        {
            Console.WriteLine($"{SessionId}님이 퇴장했습니다 ");

        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string message = Encoding.UTF8.GetString(buffer.Array,buffer.Offset,buffer.Count);
            Console.WriteLine($"[받음]:{message}");

            
        }

        public override void OnSend(ArraySegment<byte> buffer)
        {
            string message = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[메세지가 정상적으로 전송됨]:{message}");

        }
    }
}
