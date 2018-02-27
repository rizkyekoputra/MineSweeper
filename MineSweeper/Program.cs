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
                Console.Clear();
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
                    Console.Write("assign a flag(y/n): ");
                    string flagBoom = Console.ReadLine().ToLower();
                    Console.Write("input x (space) y: ");
                    string[] xy = Console.ReadLine().Split(' ');
                    int y = Convert.ToInt32(xy[0]);
                    int x = Convert.ToInt32(xy[1]);
                    if (temp[x, y] != " " && temp[x, y] != "X" && temp[x, y] != "F")
                    {
                        int countFlagBoom = checkFlagBoomAround(temp, map, x, y, size);
                        if (countFlagBoom > 9) flag = true;
                        else if (countFlagBoom == map[x, y] && countFlagBoom < 10) cekFlagMap(map, temp, size, x, y, result);
                    } else cekMap(map, temp, size, x, y, flagBoom, out result);
                    Console.WriteLine("======================");
                    Console.Clear();
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
                    if (result[i, j] == " " || result[i, j] == "F") count++;
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

        static void cekMap(int[,] map, string[,] temp, int size, int x, int y, string flagBoom, out string[,] result)
        {
            if (flagBoom == "y")
            {
                if (temp[x, y] != " " && temp[x, y] != "F") { }
                else if (temp[x, y] == " ") temp[x, y] = "F";
                else temp[x, y] = " ";
                result = temp;
            } else
            {
                if (temp[x, y] == "F") { }
                else if (temp[x, y] == "X") result = temp;
                else if (map[x, y] == 99)
                {
                    temp[x, y] = "B";
                    flag = true;
                }
                else if (map[x, y] > 0)
                {
                    temp[x, y] = map[x, y].ToString();
                }
                else
                {
                    temp[x, y] = "X";
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if ((i == x && j == y) || i >= size || i < 0 || j >= size || j < 0 || map[i, j] == 99)
                            {
                                continue;
                            }
                            else
                            {
                                cekMap(map, temp, size, i, j, flagBoom, out result);
                            }
                        }
                    }
                }
                result = temp;
            }
        }

        static int checkFlagBoomAround(string[,] temp, int[,] map, int x, int y, int size)
        {
            int count = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if ((i == x && j == y) || i >= size || i < 0 || j >= size || j < 0) continue;
                    else if (temp[i, j] == "F")
                    {
                        if (map[i, j] == 99) count++;
                        else count += 100;
                    }
                }
            }
            return count;
        }

        static void cekFlagMap(int[,] map, string[,] temp, int size, int x, int y, string[,] result)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if ((i == x && j == y) || i >= size || i < 0 || j >= size || j < 0) continue;
                    else if (temp[i, j] == " ") cekMap(map, temp, size, i, j, "n", out result);
                }
            }
        }
    }
}
