using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Map
    {
        public char[,] Maps;

        public int Rows;
        public int Cols;

        SaveLoadJson maps = new SaveLoadJson();
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
            maps.ConvertMap2(Maps);
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
}
