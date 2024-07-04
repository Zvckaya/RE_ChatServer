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
        public override void OnConnected(EndPoint endpoint)
        {
            Console.WriteLine("서버에 접속했습니다.");
        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string message = Encoding.UTF8.GetString(buffer.Array,buffer.Offset,buffer.Count);
            Console.WriteLine($"[받음]:{message}");

            _room.Broadcast(message);
        }

        public override void OnSend(ArraySegment<byte> buffer)
        {
            string message = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[메세지가 정상적으로 전송됨]:{message}");

        }
    }
}
