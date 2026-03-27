using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Monster : Character
    {    
        public Monster() { }

        public Monster(int hp, int atk, int x, int y)
        {
            Hp = hp;
            Atk = atk;
            Pos = new Position(x, y);
        }

        public override string OnDie()
        {
            return "야생의 몬스터가 쓰러졌습니다.";
        }
    }

    public class BossMonster : Monster
    {
        public BossMonster(int s, int x, int y)
        {
            Hp = 30 + (s * 5);
            Atk = 8 + (s * 2);
            Pos = new Position(x, y);
        }

        public override string OnDie()
        {
            return "보스 몬스터가 쓰러졌습니다.";
        }
    }
}
