using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieDobson_FirstPlayable_Programming1
{
    internal class Program
    {
        //Player
        static int playerHealth = 100;
        static int curGold;
        static bool isAlive = true;

        //Enemy
        static int enemyHealth;
        static List<int> Enemies = new List<int>();
        static Random enemyHealthRan = new Random();
        static List<int> EnemyHealth = new List<int>();
        static int totalEnemyKilled;

        //Map
        // ░ = grass
        // ▒ = water
        // ▓ = mountain
        // █ = trees
        // ═ = border top
        // ║ = border line
        // ╔ ╚ ╗ ╝ = Corner

        static string[,] map =
        {
            {"▓", "▓", "▓", "░", "░", "█", "█", "█", "█", "░", "░", "░", "█", "█", "█", "█", "█"},
            {"░", "▓", "░", "░", "█", "█", "▒", "█", "░", "░", "░", "░", "░", "█", "█", "█", "░"},
            {"█", "░", "░", "░", "█", "▒", "▒", "█", "░", "░", "░", "░", "░", "█", "█", "░", "░"},
            {"█", "█", "░", "░", "█", "▒", "▒", "█", "█", "█", "░", "░", "░", "░", "░", "░", "░"},
            {"█", "█", "█", "░", "█", "█", "▒", "▒", "▒", "█", "░", "░", "░", "░", "░", "░", "░"},
            {"▒", "▒", "▒", "░", "░", "█", "█", "█", "▒", "█", "░", "░", "░", "░", "░", "░", "░"},
            {"▒", "▒", "▒", "▒", "░", "░", "█", "█", "█", "█", "░", "░", "░", "░", "▒", "▒", "▒"},
            {"█", "█", "█", "░", "░", "░", "░", "█", "█", "░", "░", "░", "░", "░", "▒", "▒", "▒"},
            {"█", "█", "░", "░", "░", "░", "░", "░", "█", "░", "░", "░", "░", "░", "▒", "▒", "▓"},
            {"▓", "█", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "▓", "▓", "▓"},
            {"▓", "▓", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "▓", "▓", "▓", "▓"},
            {"▓", "▓", "▓", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "▓", "▓", "▓"},
            {"▓", "▓", "▓", "▓", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "░", "▓", "▓"},
        };




        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            //Intro();
            while(isAlive == true)
            {
                Draw();
                Console.ReadKey(true);
            }
            Ending();
        }

        static void Intro()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Welcome to my firs playable program.");
            Console.ResetColor();

            Console.WriteLine("Use WSAD to walk.");
            Console.WriteLine("If you walk into an enemy, you will damage it.");
            Console.WriteLine("If an enemy walks into you, it will damage you.");

            Console.ReadKey(true);
            Console.Clear();

        }


        static void Ending()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("YOU DIED");
            Console.ResetColor();

            Console.WriteLine($"You killed {totalEnemyKilled} number of enemies.");
            Console.WriteLine("RESET PRORGAM TO TRY AGAIN");
            Console.ReadKey(true);
            Console.Clear();
        }
        //Outside of game loop stuff
        static void MakeMap(int scale)
        {
            for (int b = 0; b < map.GetLength(1) * scale + 2; b++)
            {
                if(b == 0)
                {
                    Console.Write("╔");
                }

                else if(b == map.GetLength(1) * scale + 1)
                {
                    Console.Write("╗");
                }

                else if (b > map.GetLength(1) * scale + 1)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("═");
                }
            }
            Console.Write("\n");

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int s = 0; s < scale; s++)
                {
                    Console.Write("║");
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        for (int i = 0; i < scale; i++)
                        {
                            if (map[y, x] == "▒")
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                            }
                            else if (map[y, x] == "░")
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                            }
                            else if (map[y, x] == "▓")
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            else if (map[y, x] == "█")
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            }

                            Console.Write(map[y, x]);
                            Console.ResetColor();
                        }

                    }
                    Console.Write("║");
                    Console.Write("\n");
                }

            }

            for (int b = 0; b < map.GetLength(1) * scale + 2; b++)
            {
                if (b == 0)
                {
                    Console.Write("╚");
                }
                else if (b == map.GetLength(1) * scale + 1)
                {
                    Console.Write("╝");
                }
                else if(b > map.GetLength(1) * scale + 1)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("═");
                }

                    
            }
        }

        static void HUD()
        {
            Console.SetCursorPosition(65, 0);
            Console.Write("HUD");

            Console.SetCursorPosition(60, 1);
            Console.Write("  ~~~~~~~~~~");
        }
        //Actual Game loop logic
        static void Draw()
        {
            MakeMap(2);
            HUD();
        }

        
    }
}
