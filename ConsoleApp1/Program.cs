using System;
using System.Collections.Generic;
//using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Xml.Linq;

namespace ConsoleApp1
{
    public interface IJob
    {
        int GetHp();
        int GetAtk();      
        string GetWeapon();
    }

    public delegate string WeaponSelector(IJob job);
 
    public abstract class Weapon
    {
        public abstract string WeaponBase(IJob job);
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

    public class DungeonGame
    {
        Random randMove = new Random();
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
            int jobNum = 0;
            Console.WriteLine("플레이어 직업을 고르세요 1.전사 2.궁수 3.마법사 4.도적");
            jobNum = int.Parse(Console.ReadLine());

            for (int s = 0; s < stageCount; s++)
            {
                int rows = 10 + (s * 3);
                int cols = 15 + (s * 8);
                int maxMonsters = 5 + (s * 2);
                int bossMonster = 1;
                int monsterCount = 0;
                int bossCount = 0;

                bool isGameOver = false;
               
                Console.Clear();

                Battle battle = new Battle();
                Monster monster = new Monster();
                Map map = new Map(rows, cols);
                Player player = new Player(1, 1);

                Monster[] enemies = new Monster[maxMonsters + bossMonster];

                map.InitializeMap(s);
                map.Maps[player.Pos.Y, player.Pos.X] = 'P';
                player.SetJob(jobNum);
                player.InitWeapon();

                //SaveLoadJson mapSave = new SaveLoadJson();
                //mapSave.SaveMapJsonToTxt(map.Maps);            

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
                    string job = player.job;                 
                    string weapon = player.Weapon;

                    Console.WriteLine($"플레이어 직업 : {job}\n플레이어 무기 : {weapon}\n플레이어 HP: {player.Hp} 플레이어 ATK: {player.Atk} ");
                    Console.WriteLine();
                    Console.WriteLine($"[스테이지 {s + 1}] 몬스터 처치: {monsterCount}/{maxMonsters} 보스몬스터 처치: {bossCount}/{bossMonster}");
                    Console.WriteLine("A(왼쪽),D(오른쪽),W(위),S(아래) 를 눌러 플레이어를 이동시키세요");

                    map.PrintMap();

                    Position nextPos = GetNextPosition(player.Pos);
                    if (nextPos == null)
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