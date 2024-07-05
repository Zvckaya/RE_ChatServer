using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Room
    {
        public List<Session> _session = new List<Session>();
        private object _lock = new object();

        public void Enter(Session sesssion)
        {
            lock (_lock)
            {
                _session.Add(sesssion);
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
