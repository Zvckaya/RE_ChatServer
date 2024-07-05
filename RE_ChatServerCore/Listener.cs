using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessonFactory;
        private int _sessionIdCounter = 0;
        private object _lock = new object();
        private Room _room;

        public void Init(IPEndPoint endPoint,Func<Session> sessonFactory)
        {
            _listenSocket = new Socket(endPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            _sessonFactory = sessonFactory;

            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(10);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);

            RegisterAccept(args);

        }

        private void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;
           
            bool pending = _listenSocket.AcceptAsync(args);
            if (!pending)
                OnAcceptCompleted(null, args);
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {
                int sessionId;
                lock (_lock)
                {
                    sessionId = ++_sessionIdCounter;
                }
                Session session = _sessonFactory.Invoke();
                session.Start(e.AcceptSocket);
            }
            else
            {
                Console.WriteLine("Failed to accept client ");
            }

            RegisterAccept(e);
        }
    }
}
