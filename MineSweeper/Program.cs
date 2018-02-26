using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class Program
    {
        static bool flag = false;
        static bool win = false;
        static void Main(string[] args)
        {
            int size = 10;
            
            string input = "y";
            while (input == "y")
            {
                Console.WriteLine("=====Mine Sweeper=====");
                Console.WriteLine();
                flag = false;
                int[,] map = new int[size, size];
                string[,] temp = new string[size, size];
                string[,] result = new string[size, size];

                generateMineMap(map, size, generateMineCoordinate(size), result, temp);
                printMineMap(result, size);
                while (!flag)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.Write("input x (space) y: ");
                    string[] xy = Console.ReadLine().Split(' ');
                    int y = Convert.ToInt32(xy[0]);
                    int x = Convert.ToInt32(xy[1]);
                    cekMap(map, temp, size, x, y, out result);
                    Console.WriteLine("======================");
                    printMineMap(result, size);
                    if (win)
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("======================");
                        Console.WriteLine("Menang");
                        Console.WriteLine("======================");
                        break;
                    }
                }
                if (flag)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("======================");
                    Console.WriteLine("KALAH!!!!");
                    Console.WriteLine("======================");
                }
                Console.WriteLine();
                Console.WriteLine("Try Again (y/n): ");
                input = Console.ReadLine().ToLower();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.Read();
        }

        static HashSet<string> generateMineCoordinate(int size)
        {
            Random rnd = new Random();
            HashSet<string> set = new HashSet<string>();
            while(set.Count < size)
            {
                int tempX = rnd.Next(0, size - 1);
                int tempY = rnd.Next(0, size - 1);
                set.Add(tempX + " " + tempY);
            }
            return set;
        }

        static void generateMineMap(int[,] map, int size, HashSet<string> set, string[,] result, string[,] temp)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = " ";
                    temp[i, j] = " ";
                    if (set.Contains(i + " " + j))
                    {
                        map[i, j] = 99;
                        giveMineMapNumberBesideBom(map, size, i, j);
                    }
                }
            }
        }

        static void giveMineMapNumberBesideBom(int[,] map, int size, int row, int col)
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= size || i < 0 || j >= size || j < 0 || map[i,j] == 99)
                    {
                        continue;
                    } else
                    {
                        map[i, j] += 1;
                    }
                }
            }
        }

        static void printMineMap(string[,] result, int size)
        {
            int count = 0;
            Console.Write(" ");
            for (int i = 0; i < size; i++)
            {
                Console.Write(" " + i);
            }
            Console.WriteLine();
            string border = "+" + "-";
            for (int i = 0; i < size; i++)
            {
                Console.Write(" ");
                for (int j = 0; j < size; j++)
                {
                    Console.Write(border);
                }
                Console.Write('+');
                Console.WriteLine();
                Console.Write(i + "");
                for (int j = 0; j < size; j++)
                {
                    if (result[i, j] == " ") count++;
                    Console.Write('|' + result[i, j]);
                }
                Console.Write('|');
                Console.WriteLine();
            }
            Console.Write(" ");
            for (int i = 0; i < size; i++)
            {
                Console.Write(border);
            }
            Console.Write('+');
            if (count == 10) win = true;
        }

        static void cekMap(int[,] map, string[,] temp, int size, int x, int y, out string[,] result)
        {
            if (temp[x, y] == "X")
            {
                result = temp;
            }
            else if (map[x, y] == 99)
            {
                temp[x, y] = "B";
                flag = true;
            }
            else if (map[x, y] > 0)
            {
                temp[x, y] = map[x, y].ToString();
            } else
            {
                temp[x, y] = "X";
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if ((i == x && j == y) || i >= size || i < 0 || j >= size || j < 0 || map[i, j] == 99)
                        {
                            continue;
                        } else
                        {
                            cekMap(map, temp, size, i, j, out result);
                        }
                    }
                }
            }
            result = temp;
        }
    }
}
