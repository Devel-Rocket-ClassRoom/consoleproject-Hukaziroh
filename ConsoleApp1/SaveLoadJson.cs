using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class JobStat
    {
        private int hp;
        private int atk;
        public int Hp { get { return hp; } set { hp = value; } }
        public int Atk { get { return atk; } set { atk = value; } }
    }

    public class SaveLoadJson
    {
        public string[] ConvertMap2(char[,] map)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            string[] result = new string[rows];

            for (int i = 0; i < rows; i++)
            {
                char[] rowChars = new char[cols];
                for (int j = 0; j < cols; j++)
                {
                    rowChars[j] = map[i, j];
                }
                result[i] = new string(rowChars);
            }
            return result;
        }

        public void SaveMapJsonToTxt(char[,] mapData)
        {
            string[] convertedMap = ConvertMap2(mapData);

            string folderPath = "./GameData";
            string filePath = Path.Combine(folderPath, "MapData.json");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string result = JsonSerializer.Serialize(convertedMap, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, result);

            Console.WriteLine("Json 으로 변환된 문자열 : \n" + result);
        }

        public void SaveJobData()
        {  
            string folderPath = "./GameData";
            string filePath = Path.Combine(folderPath, "Job.json");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            JobStat stat = new JobStat { Hp = 100, Atk = 10 };
            string result = JsonSerializer.Serialize(stat, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, result);

            Console.WriteLine("Json 으로 변환된 문자열 : \n" + result);
            Console.ReadLine();

        }

        public JobStat LoadJson()
        {
            string folderPath = "./GameData";
            string filePath = Path.Combine(folderPath, "Thief.json");


            string s = File.ReadAllText(filePath);
            JobStat jj = JsonSerializer.Deserialize<JobStat>(s);

            if (jj != null)
            {
                return jj;
            }
            else
            {
                Console.WriteLine("정상적인 데이터가 아닙니다.");
                return null;
            }
        }
    }
}
