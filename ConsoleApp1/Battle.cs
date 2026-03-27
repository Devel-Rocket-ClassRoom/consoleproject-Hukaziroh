using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Battle
    {
        public void DrawBoard(Player player, Monster monster, string message)
        {
            string mName = monster is BossMonster ? "보스 몬스터" : "야생의 몬스터";
            Console.Clear();
            Console.WriteLine("======================================================================");
            Console.WriteLine();
            Console.WriteLine($"                           [ {mName} ]");
            Console.WriteLine($"                             HP: {Math.Max(0, monster.Hp)}");
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


        public int DoBattle(Player player, Monster monster)
        {
            Random rand = new Random();

            bool isBoss = monster is BossMonster;
            string encounterMsg = isBoss ? "앗! 보스 몬스터가 나타났다!" : "앗! 야생의 몬스터가 나타났다!";

            DrawBoard(player, monster, encounterMsg);
            Console.ReadKey(true);

            while (player.Hp > 0 && monster.Hp > 0)
            {
                DrawBoard(player, monster, "당신의 턴입니다. 무엇을 할까요?");

                char input = Console.ReadKey().KeyChar;

                if (input == '1')
                {
                    if (rand.Next(0, 100) < 90)
                    {
                        monster.Hp -= player.Atk;
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

                if (monster.Hp <= 0)
                {
                    DrawBoard(player, monster, monster.OnDie());
                    Console.ReadKey(true);
                    return 1;
                }
                player.Hp -= monster.Atk;
                DrawBoard(player, monster, $"몬스터의 반격! 플레이어는 {monster.Atk}의 피해를 입었다!");
                Console.ReadKey(true);

                if (player.Hp <= 0)
                {
                    DrawBoard(player, monster, player.OnDie());
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
