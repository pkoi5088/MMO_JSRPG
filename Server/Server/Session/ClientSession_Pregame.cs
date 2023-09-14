using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DB;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class ClientSession : PacketSession
    {
        public int AccountDbId { get; private set; }
        public List<LobbyPlayerInfo> LobbyPlayers { get; set; } = new List<LobbyPlayerInfo>();

        public void HandleLogin(C_Login loginPacket)
        {
            // TODO: 보안체크
            if (ServerState != PlayerServerState.ServerStateLogin)
                return;

            // TODO: 문제가 있긴 있다.
            // 1. 동시에 다른 사람이 같은 UniqueId를 보낸다면?
            // 2. 악의적으로 여러번 보낸다면
            // 3. 뜬금없는 타이밍에 그냥 이 패킷을 보낸다면?

            LobbyPlayers.Clear();

            using (AppDbContext db = new AppDbContext())
            {
                AccountDb findAccount = db.Accounts
                    .Include(a=>a.Players)
                    .Where(a => a.AccountName == loginPacket.UniqueId).FirstOrDefault();

                if (findAccount != null)
                {
                    // AccountDbId 메모리에 기억
                    AccountDbId = findAccount.AccountDbId;
                    S_Login loginOk = new S_Login() { LoginOk = 1 };
                    foreach(PlayerDb playerDb in findAccount.Players)
                    {
                        LobbyPlayerInfo lobbyPlayer = new LobbyPlayerInfo()
                        {
                            PlayerDbId = playerDb.AccountDbId,
                            Name = playerDb.PlayerName,
                            StatInfo = new StatInfo()
                            {
                                Level = playerDb.Level,
                                Hp = playerDb.Hp,
                                MaxHp = playerDb.MaxHp,
                                Attack = playerDb.Attack,
                                Speed = playerDb.Speed,
                                TotalExp = playerDb.TotalExp
                            }
                        };

                        // 메모리에도 들고 있다.
                        LobbyPlayers.Add(lobbyPlayer);

                        // 패킷에 넣어준다.
                        loginOk.Players.Add(lobbyPlayer);
                    }
                    Send(loginOk);

                    // 로비로 이동
                    ServerState = PlayerServerState.ServerStateLobby;
                }
                else
                {
                    AccountDb newAccount = new AccountDb { AccountName = loginPacket.UniqueId };
                    db.Accounts.Add(newAccount);
                    bool succcess = db.SaveChangesEx();
                    if (succcess == false)
                        return;

                    // AccountDbId 메모리에 기억
                    AccountDbId = newAccount.AccountDbId;

                    S_Login loginOk = new S_Login() { LoginOk = 1 };
                    Send(loginOk);

                    // 로비로 이동
                    ServerState = PlayerServerState.ServerStateLobby;
                }
            }
        }

        public void HandleEnterPlayer(C_EnterGame enterGamePacket)
        {
            if (ServerState != PlayerServerState.ServerStateLobby)
                return;

            LobbyPlayerInfo playerInfo = LobbyPlayers.Find(p => p.Name == enterGamePacket.Name);
            if (playerInfo == null)
                return;

            MyPlayer = ObjectManager.Instance.Add<Player>();
            {
                MyPlayer.PlayerDbId = playerInfo.PlayerDbId;
                MyPlayer.Info.Name = playerInfo.Name;
                MyPlayer.Info.PosInfo.State = CreatureState.Idle;
                MyPlayer.Info.PosInfo.MoveDir = MoveDir.Down;
                MyPlayer.Info.PosInfo.PosX = 0;
                MyPlayer.Info.PosInfo.PosY = 0;
                MyPlayer.Stat.MergeFrom(playerInfo.StatInfo);
                MyPlayer.Session = this;

                S_ItemList itemListPacket = new S_ItemList();

                // 아이템 목록을 가지고 있다.
                using (AppDbContext db = new AppDbContext())
                {
                    List<ItemDb> items = db.Items
                        .Where(i => i.OwnerDbId == playerInfo.PlayerDbId)
                        .ToList();

                    foreach(ItemDb itemDb in items)
                    {
                        Item item = Item.MakeItem(itemDb);
                        if(item!= null)
                        {
                            MyPlayer.Inven.Add(item);

                            ItemInfo info = new ItemInfo();
                            info.MergeFrom(item.Info);
                            itemListPacket.Items.Add(info);
                        }
                    }
                }
                Send(itemListPacket);
            }

            ServerState = PlayerServerState.ServerStateGame;

            GameLogic.Instance.Push(() =>
            {
                GameRoom insertRoom = GameLogic.Instance.Find(1);
                insertRoom.Push(insertRoom.EnterGame, MyPlayer);
            });

        }

        public void HandleCreatePlayer(C_CreatePlayer createPacket)
        {
            // TODO: 보안체크
            if (ServerState != PlayerServerState.ServerStateLobby)
                return;

            using(AppDbContext db = new AppDbContext())
            {
                PlayerDb findPlayer = db.Players
                    .Where(p => p.PlayerName == createPacket.Name).FirstOrDefault();

                if (findPlayer != null)
                {
                    Send(new S_CreatePlayer());
                }
                else
                {
                    // 1레벨 스텟 정보 추출
                    StatInfo stat = null;
                    DataManager.StatDict.TryGetValue(1, out stat);

                    // DB에 플레이어 만들어줘야 함
                    PlayerDb newPlayerDb = new PlayerDb()
                    {
                        PlayerName = createPacket.Name,
                        Level = stat.Level,
                        Hp = stat.Hp,
                        MaxHp = stat.MaxHp,
                        Attack = stat.Attack,
                        Speed = stat.Speed,
                        TotalExp = 0,
                        AccountDbId = AccountDbId
                    };

                    db.Players.Add(newPlayerDb);
                    bool success = db.SaveChangesEx();
                    if (success == false)
                        return;

                    //메모리에 추가
                    LobbyPlayerInfo lobbyPlayer = new LobbyPlayerInfo()
                    {
                        PlayerDbId = newPlayerDb.PlayerDbId,
                        Name = createPacket.Name,
                        StatInfo = new StatInfo()
                        {
                            Level = stat.Level,
                            Hp = stat.Hp,
                            MaxHp = stat.MaxHp,
                            Attack = stat.Attack,
                            Speed = stat.Speed,
                            TotalExp = 0
                        }
                    };

                    // 메모리에도 들고 있다.
                    LobbyPlayers.Add(lobbyPlayer);

                    // 쿨라에 전송
                    S_CreatePlayer newPlayer = new S_CreatePlayer() { Player = new LobbyPlayerInfo() };
                    newPlayer.Player.MergeFrom(lobbyPlayer);

                    Send(newPlayer);
                }
            }
        }
    }
}
