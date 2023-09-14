﻿using System.Net;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Data;
using Server.DB;
using Server.Game;
using ServerCore;

namespace Server
{
    // 1. GameRoom 방식의 간단한 동기화 <- OK
    // 2. 더 넓은 영역 관리

    // 1. Recv (N개)
    // 2. GameLogic (1)
    // 3. Send(1)
    // 4. DB (1)

    class Program
    {
        static Listener _listener = new Listener();

        static void GameLogicTask()
        {
            while (true)
            {
                GameLogic.Instance.Update();
                Thread.Sleep(0);
            }
        }

        static void DbTask()
        {
            while (true)
            {
                DbTransaction.Instance.Flush();
                Thread.Sleep(0);
            }
        }

        static void NetworkTask()
        {
            while (true)
            {
                List<ClientSession> sessions = SessionManager.Instance.GetSessions();
                foreach (ClientSession session in sessions)
                {
                    session.FlushSend();
                }

                Thread.Sleep(0);
            }
        }

        static void Main(string[] args)
        {
            ConfigManager.LoadConfig();
            DataManager.LoadData();

            GameLogic.Instance.Push(() =>
            {
                GameLogic.Instance.Add(1);
            });

            // DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            // GameLogicTask
            {
                Task gameLogicTask = new Task(GameLogicTask, TaskCreationOptions.LongRunning);
                gameLogicTask.Start();
            }

            // NetworkTask
            {
                Task networkTask = new Task(NetworkTask, TaskCreationOptions.LongRunning);
                networkTask.Start();
            }

            DbTask();
        }
    }
}