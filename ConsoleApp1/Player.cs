using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public delegate string WeaponSelector(IJob job);

    public abstract class Character
    {
        public Position Pos;
        public int Hp;
        public int Atk;
        public abstract string OnDie();
    }

    class Warrior : IJob
    {
        public int GetHp() => 120;
        public int GetAtk() => 5;
        public string GetWeapon() => "검";
    }

    class Archer : IJob
    {
        public int GetHp() => 100;
        public int GetAtk() => 4;
        public string GetWeapon() => "활";
    }

    class Mage : IJob
    {
        public int GetHp() => 80;
        public int GetAtk() => 6;
        public string GetWeapon() => "지팡이";
    }

    class Thief : IJob
    {
        public int GetHp() => 70;
        public int GetAtk() => 12;
        public string GetWeapon() => "단검";
    }

    public class Player : Character
    {
        public string job;
        public string Weapon;

        private IJob currentJob;

        public Player(int x, int y)
        {
            Pos = new Position(x, y);
        }

        public void SetJob(int choice)
        {
            switch (choice)
            {
                case 1: currentJob = new Warrior(); job = "전사"; break;
                case 2: currentJob = new Archer(); job = "궁수"; break;
                case 3: currentJob = new Mage(); job = "마법사"; break;
                case 4: currentJob = new Thief(); job = "도적"; break;
            }

            Hp = currentJob.GetHp();
            Atk = currentJob.GetAtk();

            WeaponSelector selector = (j) => j.GetWeapon();
            Weapon = selector(currentJob);
        }
        public override string OnDie()
        {
            return "플레이어가 쓰러졌습니다... 게임 오버!";
        }
    }
}
