using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Battle
    {
        public void DrawBoard(Player player, Monster monster, string message)
        {
            int mHp = monster is BossMonster ? ((BossMonster)monster).BossHp : monster.Hp;
            string mName = monster is BossMonster ? "보스 몬스터" : "야생의 몬스터";
            Console.Clear();
            Console.WriteLine("======================================================================");
            Console.WriteLine();
            Console.WriteLine($"                           [ {mName} ]");
            Console.WriteLine($"                             HP: {Math.Max(0, mHp)}");
            Console.WriteLine();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (monster is BossMonster)
            {
                Console.WriteLine("                  ⠀ ⠀⠀⠀ ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢻⢷⣷⣣⠀⠀⠀⠀⠀⠀⠀⠀⠀");
                Console.WriteLine("                   ⠀⠀⠀ ⠀⠀⠀⠀⣀⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀⢏⣿⣿⡵⡀⠀⠀⠀⠀⠀⠀⠀");
                Console.WriteLine("                   ⠀⠀ ⠀⠀⣠⣮⣷⣿⣿⣿⣿⣷⣄⣄⠀⠀⠀⠀⠈⢞⣿⣿⡵⡀⠀⠀⠀⠀⠀");
                Console.WriteLine("                     ⠀⠀⡠⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣏⢦⣤⡀⠀⠀⠀⠫⣻⣿⣾⢄⠀⠀⠀");
                Console.WriteLine("                     ⠀⣔⣿⣿⣿⣿⣿⣿⠿⣿⠻⢟⣿⣿⣿⣿⣿⡆⠀⠀⠀⠑⡿⣿⣯⢆⠀⠀");
                Console.WriteLine("                     ⢰⣸⢿⣻⢟⠃⠉⠉⠀⡠⠤⠸⣸⣿⣿⣿⡳⠁⠀⠀⠀⠀⡨⠺⠿⠇⢓⡄");
                Console.WriteLine("                     ⠧⠊⠁⠘⣖⣳⠠⣶⣋⡹⠁⠀⠛⣩⢻⠋⠀⠀⠀⠀⠀⢀⠇⠀⠀⠀⠀⢾⠀");
                Console.WriteLine("                    ⠀⠀⢠⠂⠁⠓⠒⠊⠀⡠⠤⡀⢠⠀⠚⠀⠀⠀⠀⠀⡠⠊⢀⠤⡤⣔⠩⠼⡀");
                Console.WriteLine("                    ⠀⠀⢇⠀⠀⢀⡠⢔⣪⠠⠖⠇⡘⠀⠀⠀⢀⠄⠒⠉⢀⠔⠁⠀⣧⢞⠮⠭⠵⡀");
                Console.WriteLine("                    ⠀⠀⠘⠒⠉⣾⣀⣀⠀⣀⣀⠦⠗⠹⠙⠃⠁⠀⡠⠔⡡⠔⠒⠉⡨⢴⢹⣿⣏⡆");
                Console.WriteLine("                     ⠀⠀⠀⠀⡸⠉⠀⠀⠁⠀⠀⠀⠀⣇⡠⡄⡶⠯⠔⠈⠀⠀⡠⠊⠀⠀⡿⣿⣿⡇");
                Console.WriteLine("                    ⠀⠀⠀⢀⠇⠀⠀⠀⠀⢀⣀⠤⡤⠵⠊⢸⠀⡠⠤⠤⠐⠉⠀⠀⠀⠀⣷⣿⢿⡇");
                Console.WriteLine("                    ⠀⠀⢀⠃⠀⢀⣀⣀⣀⣠⣀⣀⣿⠉⠉⠉⠉⠀⠀");
            }
            else
            {
                Console.WriteLine("                               ( `_´)");
                Console.WriteLine("                               />  />");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"  [ 플레이어 ] 직업: {player.job} | 무기: {player.Weapon}");
            Console.WriteLine($"  ▶ HP : {Math.Max(0, player.Hp),-4} | ATK : {player.Atk,-4}");
            Console.WriteLine("======================================================================");
            Console.WriteLine($"  {message}");
            Console.WriteLine("======================================================================");
            Console.WriteLine("  1.공격 (Attack)              2.도망가기 (Run)");
            Console.WriteLine("======================================================================");
            Console.Write("  행동을 선택하세요: ");
        }

        private int GetHp(Monster m) => m is BossMonster b ? b.BossHp : m.Hp;     
        private int GetAtk(Monster m) => m is BossMonster b ? b.BossAtk : m.Atk;
        private void ApplyDamage(Monster m, int damage)
        {
            if (m is BossMonster b) b.BossHp -= damage;
            else m.Hp -= damage;
        }
       
        public int DoBattle(Player player, Monster monster)
        {
            Random rand = new Random();
            bool isBoss = monster is BossMonster;
            string encounterMsg = isBoss ? "앗! 보스 몬스터가 나타났다!" : "앗! 야생의 몬스터가 나타났다!";
            DrawBoard(player, monster, encounterMsg);
            Console.ReadKey(true);

            while (player.Hp > 0 && GetHp(monster) > 0)
            {
                DrawBoard(player, monster, "당신의 턴입니다. 무엇을 할까요?");

                char input = Console.ReadKey().KeyChar;

                if (input == '1')
                {
                    if (rand.Next(0, 100) < 90)
                    {
                        ApplyDamage(monster, player.Atk);
                        DrawBoard(player, monster, $"플레이어의 공격! 몬스터에게 {player.Atk}의 피해!");
                    }
                    else
                    {
                        DrawBoard(player, monster, "공격이 빗나갔다!");
                    }
                    Console.ReadKey(true);
                }
                else if (input == '2')
                {
                    if (rand.Next(0, 100) < 50)
                    {
                        DrawBoard(player, monster, "무사히 도망쳤다!");
                        Console.ReadKey(true);
                        return 2;
                    }
                    else
                    {
                        DrawBoard(player, monster, "도망치지 못했다!");
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    continue;
                }

                if (GetHp(monster) <= 0)
                {
                    DrawBoard(player, monster, "몬스터를 물리쳤습니다!");
                    Console.ReadKey(true);
                    return 1;
                }

                int mAtk = GetAtk(monster);
                player.Hp -= mAtk;
                DrawBoard(player, monster, $"몬스터의 반격! 플레이어는 {mAtk}의 피해를 입었다!");
                Console.ReadKey(true);

                if (player.Hp <= 0)
                {
                    DrawBoard(player, monster, "플레이어가 눈앞이 깜깜해졌다...");
                    Console.ReadKey(true);
                    return 0;
                }
            }
            return 1;
        }
        public int DoBossBattle(Player player, BossMonster boss)
        {
            return DoBattle(player, boss);
        }
    }
}
