using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class Session
    {
        private Socket _socket;
        private int _disconnect;
        private object _lock = new object();

        private SocketAsyncEventArgs _recvArgs;
        private SocketAsyncEventArgs _sendArgs;

        public abstract void OnConnected(EndPoint endpoint);
        public abstract void OnRecv(ArraySegment<byte> buffer);
        public abstract void OnSend(ArraySegment<byte> buffer);
        public abstract void OnDisconnected(EndPoint endpoint);

        public int SessionId { get; private set; }

        public void Start(Socket socket)
        {
            _socket = socket;
        

            _recvArgs = new SocketAsyncEventArgs();
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _recvArgs.SetBuffer(new byte[1024], 0, 1024);
            SessionId = GenerateSessionId();

            _sendArgs = new SocketAsyncEventArgs();
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            OnConnected(_socket.RemoteEndPoint);

            RegisterRecv();

        }

        private int GenerateSessionId()
        {
            return new Random().Next(1000, 9999);
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _disconnect, 1) == 1)
                return;
            OnDisconnected(_recvArgs.RemoteEndPoint);

            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();

        }

        public void Send(ArraySegment<byte> sendBuff)
        {
            lock (_lock)
            {
                _sendArgs.SetBuffer(sendBuff.Array, sendBuff.Offset, sendBuff.Count);
                bool pending = _socket.SendAsync(_sendArgs);
                if (!pending)
                    OnSendCompleted(null, _sendArgs);
            }
        }

        private void RegisterRecv()
        {
            bool pending = _socket.ReceiveAsync(_recvArgs);
            if (!pending)
                OnRecvCompleted(null, _recvArgs);
        }

        private void OnRecvCompleted(object? sender, SocketAsyncEventArgs e)
        {
            if(_recvArgs.BytesTransferred >0 && _recvArgs.SocketError == SocketError.Success)
            {
                try
                {
                    ArraySegment<byte> buff = new ArraySegment<byte>(e.Buffer, e.Offset, e.Count);
                    OnRecv(buff);
                    RegisterRecv();
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Disconnect();
            }
        }

        private void OnSendCompleted(object? sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                OnSend(new ArraySegment<byte>(e.Buffer, e.Offset, e.Count));
            }
            else
            {
                Disconnect();
            }
        }
    }
}
