using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Connector
    {

        Func<Session> _sessionFac;


        public void Connect(IPEndPoint endPoint , Func<Session> sessionFactorty)
        {
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFac = sessionFactorty;

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectedCompleted);
            e.RemoteEndPoint = endPoint;
            e.UserToken = socket;

            RegisterConnect(e,socket);

        }

        private void RegisterConnect(SocketAsyncEventArgs e,Socket s)
        {
            if (s == null)
                return;

            bool pending = s.ConnectAsync(e);
            if (!pending)
                OnConnectedCompleted(null, e);
        }

        private void OnConnectedCompleted(object? sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Session session = _sessionFac.Invoke();
                session.Start(e.ConnectSocket);
                session.OnConnected(e.RemoteEndPoint);
            }
        }
    }
}
