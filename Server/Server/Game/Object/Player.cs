using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using Server.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class Player : GameObject
    {
        public int PlayerDbId { get; set; }
        public ClientSession Session { get; set; }
        public Inventory Inven { get; private set; } = new Inventory();

        public Player()
        {
            ObjectType = GameObjectType.Player;
        }

        public override void OnDamaged(GameObject attacker, int damage)
        {
            base.OnDamaged(attacker, damage);
        }

        public override void OnDead(GameObject attacker)
        {
            base.OnDead(attacker);
        }

        public void OnLeaveGame()
        {
            // DB 연동?
            // -- 피가 깎일 때마다 DB 접근할 필요가 있을까?
            // 1) 서버가 다운되면 아직 저장되지 않은 정보가 날아간다.
            // 2) 코드 흐름을 다 막아버린다.
            // - 비동기(Async) 방법 사용?
            // - 다른 쓰레드로 DB 일감을 던져버린다.
            // -- 결과를 받아서 이어서 처리해야하는 경우
            // -- 아이템 생성

            DbTransaction.SavePlayerStatus_AllInOne(this, Room);
        }
    }
}
