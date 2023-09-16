using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class Zone
    {
        public int IndexY { get; private set; }
        public int IndexX { get; private set; }

        public HashSet<Player> Players { get; set; } = new HashSet<Player>();

        public Zone(int y, int x)
        {
            IndexY = y;
            IndexX = x;
        }

        public Player FindOne(Func<Player, bool> condition)
        {
            foreach (Player player in Players)
            {
                if (condition.Invoke(player))
                    return player;
            }

            return null;
        }

        public List<Player> FindAll(Func<Player, bool> condition)
        {
            List<Player> findList = new List<Player>();

            foreach (Player player in Players)
            {
                if (condition.Invoke(player))
                    findList.Add(player);
            }

            return findList;
        }
    }
}
