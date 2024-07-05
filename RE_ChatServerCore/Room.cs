using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Room
    {
        private static Room roomMaster;
        public static Room instance
        {
            get
            {
                if (roomMaster == null)
                {
                    roomMaster = new Room();
                }
                return roomMaster;
            }
        }

        public List<Session> _session = new List<Session>();
        private object _lock = new object();

        public void Enter(Session sesssion)
        {
            lock (_lock)
            {
                _session.Add(sesssion);
                Console.WriteLine($"{sesssion.SessionId} 가 룸에 추가됨 ");
            }
        }

        public void Leave(Session session)
        {
            lock (_lock)
            {
                _session.Remove(session);
            }
        }

        public void Broadcast(string message)
        {

            byte[] sendbuff = Encoding.UTF8.GetBytes(message);
            lock (_lock)
            {
                foreach(var session in _session)
                {
                    session.Send(new ArraySegment<byte>(sendbuff));
                }
            }
        }

    }
}
