using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Monster
    {
        public int Hp =10;
        public int Atk = 4;
     
        public Position Pos;
        public Monster() { }

        public Monster(int x)
        {
            Hp = x;
        }

        public Monster(int x, int y)
        {
            Pos = new Position(x, y);
        }
    }

    public class BossMonster : Monster
    {
        public int BossHp = 30;
        public int BossAtk = 8;

        public BossMonster(int s)
        {
            BossHp = 30 + (s * 5);
            BossAtk = 8 + (s * 2);
        }

        public BossMonster(int x, int y)
        {
            Pos = new Position(x, y);
        }
    }
}
