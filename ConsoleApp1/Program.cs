using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml.Linq;

namespace ConsoleApp1
{
    public delegate string AddWeapon(String wp);

    interface IJob
    {
        string playerJob(int c);
    }

    public abstract class Weapon
    {
        public abstract string WeaponBase(string job);
    }

    public class Position
    {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
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

            if (monster is BossMonster)
            {
                Console.WriteLine("                              / \\__ / \\");
                Console.WriteLine("                              ( @ @ )");
                Console.WriteLine("                               ) = ( ");
                Console.WriteLine("                              ( ___ )");
                Console.WriteLine("                              / \\   ");
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

            DrawBoard(player, monster, "앗! 야생의 몬스터가 나타났다!");
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
                        DrawBoard(player, monster, "앗! 공격이 빗나갔다!");
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
                    DrawBoard(player, monster, "몬스터를 물리쳤습니다!");
                    Console.ReadKey(true);
                    return 1;
                }


                player.Hp -= monster.Atk;
                DrawBoard(player, monster, $"몬스터의 반격! 플레이어는 {monster.Atk}의 피해를 입었다!");
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
            Random rand = new Random();

            DrawBoard(player, boss, "앗! 야생의 몬스터가 나타났다!");
            Console.ReadKey(true);

            while (player.Hp > 0 && boss.Hp > 0)
            {
                DrawBoard(player, boss, "당신의 턴입니다. 무엇을 할까요?");

                char input = Console.ReadKey().KeyChar;

                if (input == '1')
                {
                    if (rand.Next(0, 100) < 90)
                    {
                        boss.Hp -= player.Atk;
                        DrawBoard(player, boss, $"플레이어의 공격! 몬스터에게 {player.Atk}의 피해!");
                    }
                    else
                    {
                        DrawBoard(player, boss, "앗! 공격이 빗나갔다!");
                    }
                    Console.ReadKey(true);
                }
                else if (input == '2')
                {
                    if (rand.Next(0, 100) < 50)
                    {
                        DrawBoard(player, boss, "무사히 도망쳤다!");
                        Console.ReadKey(true);
                        return 2;
                    }
                    else
                    {
                        DrawBoard(player, boss, "도망치지 못했다!");
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    continue;
                }

                if (boss.Hp <= 0)
                {
                    DrawBoard(player, boss, "몬스터를 물리쳤습니다!");
                    Console.ReadKey(true);
                    return 1;
                }


                player.Hp -= boss.Atk;
                DrawBoard(player, boss, $"몬스터의 반격! 플레이어는 {boss.Atk}의 피해를 입었다!");
                Console.ReadKey(true);

                if (player.Hp <= 0)
                {
                    DrawBoard(player, boss, "플레이어가 눈앞이 깜깜해졌다...");
                    Console.ReadKey(true);
                    return 0;
                }
            }
            return 1;
        }
    }

    public class Player : Weapon, IJob
    {
        AddWeapon addWeapon = new AddWeapon(add);
        public Position Pos;

        public int Hp; 
        public int Atk;
        public int range;
        public int count;

        static string add(string wp) { return wp; }
        public string job;
        public string Weapon;


        public Player(int x, int y)
        {
            Pos = new Position(x, y);
        }

        public Player() { }

        public override string WeaponBase(string job)
        {
            if (job == "전사")
                Weapon = addWeapon("검");
            else if (job == "궁수")
                Weapon = addWeapon("활");
            else if (job == "마법사")
                Weapon = addWeapon("지팡이");
            else if (job == "도적")
                Weapon = addWeapon("단검");

            return Weapon;
        }

        public string playerJob(int count)
        {
            if (count == 1)
                job = "전사";
            else if (count == 2)
                job = "궁수";
            else if (count == 3)
                job = "마법사";
            else if (count == 4)
                job = "도적";

            return job;
        }

        public void Playerstat(int c)
        {
            if (c == 1)
            {
                Hp = 200;
                Atk = 2;
                range = 1;
                
            }
            else if (c == 2)
            {
                Hp = 100;
                Atk = 3;
                range = 3;
            }
            else if (c == 3)
            {
                Hp = 80;
                Atk = 4;
                range = 2;
            }
            else if (c == 4)
            {
                Hp = 50;
                Atk = 8;
                range = 1;
            }
        }
    }

    public class Monster
    {
        public int Hp = 4;
        public int Atk = 2;

        public Monster() { }
        public Position Pos;

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
        public int BossHp = 10;
        public int BossAtk = 4;

        public BossMonster(int s)
        {
            BossHp = 10 + (s * 5);
        }

        public BossMonster(int x, int y)
        {
            Pos = new Position(x, y);
        }
    }


    public class Map
    {
        public char[,] Maps;

        public int Rows;
        public int Cols;

        public Map(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Maps = new char[rows, cols];
        }
        public void InitializeMap(int s)
        {
            Console.CursorVisible = false;
            Random rand = new Random();
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    if (y == 0 || y == Rows - 1 || x == 0 || x == Cols - 1)
                        Maps[y, x] = '#';
                    else if (rand.Next(0, 100) < 5 + (2 * s))
                        Maps[y, x] = '#';
                    else
                        Maps[y, x] = ' ';
                }
            }
        }

        public void PrintMap()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    Console.Write(Maps[y, x]);
                }
                Console.WriteLine();
            }
        }

        public Position GetRandomEmptyPosition()
        {
            Random rand = new Random();
            while (true)
            {
                int r = rand.Next(1, Rows - 1);
                int c = rand.Next(1, Cols - 1);

                if (Maps[r, c] == ' ')
                {
                    return new Position(c, r);
                }
            }
        }
    }

    public class DungeonGame
    {
        Position GetNextPosition(Position currentPos)
        {

            char input = char.ToUpper(Console.ReadKey(true).KeyChar);
            Console.WriteLine();

            int nextX = currentPos.X;
            int nextY = currentPos.Y;

            switch (input)
            {
                case 'W': nextY--; break;
                case 'S': nextY++; break;
                case 'A': nextX--; break;
                case 'D': nextX++; break;
                default:                  
                    return null;
            }
            return new Position(nextX, nextY);
        }

        void MovePlayer(Player player, Map map, int nextX, int nextY)
        {
            map.Maps[player.Pos.Y, player.Pos.X] = ' ';
            player.Pos.X = nextX;
            player.Pos.Y = nextY;
            map.Maps[player.Pos.Y, player.Pos.X] = 'P';
        }

        void MoveMonster(Monster m, Map map, Random randMove)
        {
            if (m == null || map.Maps[m.Pos.Y, m.Pos.X] != 'M' && map.Maps[m.Pos.Y, m.Pos.X] != 'B')
                return;
            int dir = randMove.Next(0, 4);
            int mNextX = m.Pos.X;
            int mNextY = m.Pos.Y;

            if (dir == 0) mNextX--;
            else if (dir == 1) mNextX++;
            else if (dir == 2) mNextY--;
            else if (dir == 3) mNextY++;

            if (map.Maps[mNextY, mNextX] == ' ')
            {
                map.Maps[m.Pos.Y, m.Pos.X] = ' ';

                m.Pos.Y = mNextY;
                m.Pos.X = mNextX;

                if (m is BossMonster)
                    map.Maps[m.Pos.Y, m.Pos.X] = 'B';
                else
                    map.Maps[m.Pos.Y, m.Pos.X] = 'M';
            }
        }
        public void PlayGame(int stageCount)
        {
            Console.CursorVisible = false;
            int c = 0;
            Console.WriteLine("플레이어 직업을 고르세요 1.전사 2.궁수 3.마법사 4.도적");
            c = int.Parse(Console.ReadLine());

            for (int s = 0; s < stageCount; s++)
            {
                int rows = 10 + (s * 3);
                int cols = 15 + (s * 8);
                int maxMonsters = 5 + (s * 2);
                int bossMonster = 1;
                int monsterCount = 0;
                int bossCount = 0;

                bool isGameOver = false;

                string weapon;

                Console.Clear();

                Player p = new Player();
                Battle battle = new Battle();
                Monster monster = new Monster();
                Map map = new Map(rows, cols);
                Player player = new Player(1, 1);
                Monster[] enemies = new Monster[maxMonsters + bossMonster];

                map.InitializeMap(s);
                map.Maps[player.Pos.Y, player.Pos.X] = 'P';
                player.Playerstat(c);

                for (int i = 0; i < maxMonsters; i++)
                {
                    Position spawnPos = map.GetRandomEmptyPosition();
                    enemies[i] = new Monster(spawnPos.X, spawnPos.Y);
                    map.Maps[spawnPos.Y, spawnPos.X] = 'M';
                }

                for (int i = 0; i < bossMonster; i++)
                {
                    Position spawnPos = map.GetRandomEmptyPosition();
                    enemies[maxMonsters + i] = new BossMonster(spawnPos.X, spawnPos.Y);
                    map.Maps[spawnPos.Y, spawnPos.X] = 'B';
                }

                Position doorPos = map.GetRandomEmptyPosition();
                map.Maps[doorPos.Y, doorPos.X] = 'D';

                while (!isGameOver)
                {
                    string job = p.playerJob(c);
                    p.WeaponBase(job);
                    weapon = p.Weapon;

                    Console.WriteLine($"플레이어 직업 : {job}\n플레이어 무기 : {weapon}\n플레이어 HP: {player.Hp} 플레이어 ATK: {player.Atk} ");
                    Console.WriteLine();
                    Console.WriteLine($"[스테이지 {s + 1}] 몬스터 처치: {monsterCount}/{maxMonsters} 보스몬스터 처치: {bossCount}/{bossMonster}");
                    Console.WriteLine("A(왼쪽),D(오른쪽),W(위),S(아래) 를 눌러 플레이어를 이동시키세요");
                   
                    map.PrintMap();
                  
                    Position nextPos = GetNextPosition(player.Pos);
                    if(nextPos == null)
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }

                    int nextX = nextPos.X;
                    int nextY = nextPos.Y;
                    Console.Clear();

                    if (map.Maps[nextY, nextX] == '#')
                    {
                        Console.WriteLine("벽입니다. 다른 방향으로 이동하세요.");
                    }
                    else if (map.Maps[nextY, nextX] == 'M')
                    {

                        Monster battleMonster = new Monster(4);
                        int battleResult = battle.DoBattle(player, battleMonster);
                        if (battleResult == 1)
                        {
                            monsterCount++;
                            MovePlayer(player,map,nextX,nextY);

                            for (int i = 0; i < enemies.Length; i++)
                            {
                                if (enemies[i] != null && enemies[i].Pos.X == nextX && enemies[i].Pos.Y == nextY)
                                {
                                    enemies[i] = null;
                                    break;
                                }
                            }

                            Console.WriteLine("계속하려면 아무키를 눌러주세요");
                            Console.ReadKey(true);
                            Console.Clear();
                        }
                        else if(battleResult == 2)
                        {
                            Console.Clear();
                        }
                        else
                        {
                            isGameOver = true;
                            return;
                        }

                    }
                    else if (map.Maps[nextY, nextX] == 'B')
                    {
                        BossMonster battleBossMonster = new BossMonster(s);
                        if (monsterCount < maxMonsters)
                        {
                            Console.WriteLine($"일반 몬스터가 아직 {maxMonsters - monsterCount}마리 남았습니다");
                        }
                        else
                        {
                            int battleResult = battle.DoBossBattle(player, battleBossMonster);
                            if (battleResult == 1)
                            {
                                bossCount++;
                                MovePlayer(player, map, nextX, nextY);

                                for (int i = 0; i < enemies.Length; i++)
                                {
                                    if (enemies[i] != null && enemies[i].Pos.X == nextX && enemies[i].Pos.Y == nextY)
                                    {
                                        enemies[i] = null;
                                        break;
                                    }
                                }

                                Console.WriteLine("계속하려면 아무키를 눌러주세요");
                                Console.ReadKey(true);
                                Console.Clear();
                            }
                            else if (battleResult == 2)
                            {
                                Console.Clear();
                            }
                            else
                            {
                                isGameOver = true;
                                return;
                            }
                        }

                    }
                    else if (map.Maps[nextY, nextX] == 'D')
                    {
                        if (monsterCount < maxMonsters)
                        {
                            Console.WriteLine("몬스터가 남아있습니다.");
                        }
                        else if (bossCount < 1)
                        {
                            Console.WriteLine("보스몹을 잡으세요.");
                        }
                        else
                        {
                            if (s < stageCount - 1)
                                Console.WriteLine("다음 단계로 넘어갑니다!");
                            else
                                Console.WriteLine("게임 클리어!");
                            isGameOver = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("이곳은 안전합니다.");
                        MovePlayer(player, map, nextX, nextY);
                    }

                    Random randMove = new Random();

                    for (int i = 0; i < enemies.Length; i++)
                    {
                        MoveMonster(enemies[i], map, randMove);
                        
                    }
                }
            }
        }

       
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            DungeonGame game = new DungeonGame();
            game.PlayGame(5);
        }
    }
}