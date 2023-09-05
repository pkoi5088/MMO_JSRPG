using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Data;
using Server.Game;
using ServerCore;
using System;
using System.Net;
using System.Text;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public Player MyPlayer { get; set; }
        public int SessionId { get; set; }

        public void Send(IMessage packet)
        {
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
            Send(new ArraySegment<byte>(sendBuffer));
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected: {endPoint}");

            {
                S_Connected connectedPacket = new S_Connected();
                Send(connectedPacket);
            }

            // TODO: 로비에서 캐릭터 선택
            MyPlayer = ObjectManager.Instance.Add<Player>();
            {
                MyPlayer.Info.Name = $"Player_{MyPlayer.Info.ObjectId}";
                MyPlayer.Info.PosInfo.State = CreatureState.Idle;
                MyPlayer.Info.PosInfo.MoveDir = MoveDir.Down;


                MyPlayer.Info.PosInfo.PosX = 0;
                MyPlayer.Info.PosInfo.PosY = 0;

                StatInfo stat = null;
                DataManager.StatDict.TryGetValue(1, out stat);
                MyPlayer.Stat.MergeFrom(stat);

                MyPlayer.Session = this;
            }

            // TODO: 입장요청 들어오면 입장
            GameRoom insertRoom = RoomManager.Instance.Find(1);
            //Vector2Int startPos = insertRoom.GetSafeRandomPos();
            //MyPlayer.Info.PosInfo.PosX = startPos.x;
            //MyPlayer.Info.PosInfo.PosY = startPos.y;

            insertRoom.Push(insertRoom.EnterGame, MyPlayer);
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            GameRoom room = RoomManager.Instance.Find(1);
            room.Push(room.LeaveGame, MyPlayer.Info.ObjectId);

            SessionManager.Instance.Remove(this);

            Console.WriteLine($"OnDisconnected: {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
