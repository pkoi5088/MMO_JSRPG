using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessionFactory;

        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory, int register = 10, int backlog = 100)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory = sessionFactory;

            _listenSocket.Bind(endPoint);

            // backlog = 최대 개수
            _listenSocket.Listen(backlog);

            for (int i = 0; i < register; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                RegisterAccept(args);
            }
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            try
            {
                bool pending = _listenSocket.AcceptAsync(args);
                if (pending == false)
                    OnAcceptCompleted(null, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError == SocketError.Success)
                {
                    Session session = _sessionFactory.Invoke();
                    session.Start(args.AcceptSocket);
                    session.OnConnected(args.AcceptSocket.RemoteEndPoint);
                }
                else
                    Console.WriteLine(args.SocketError.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            RegisterAccept(args);
        }
    }
}
