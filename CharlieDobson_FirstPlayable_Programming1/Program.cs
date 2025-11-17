using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CharlieDobson_FirstPlayable_Programming1
{
    internal class Program
    {
        //Player
        static int playerHealth = 100;
        static string playerStatus = "";
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

        static int scaleMap = 2;
        //Turn based stuff
        static int turn = -1;

        static int xMovement = 0;
        static int yMovement = 0;

        static ConsoleKey input;
        //Movement

        static int xAxisPlayer = 20;
        static int yAxisPlayer = 17;

        static int xAxisEnemy;
        static int yAxisEnemy;




        static void Main(string[] args)
        {
            //Console.CursorVisible = false;
            //Intro();
            //Console.ReadKey(true);
            //Console.Clear();

            //StartGame();
            //Console.ReadKey(true);
            //Console.Clear();

            while (isAlive == true)
            {
                ProcessInput();
                Update();
                Draw();
                
            }
            Ending();
        }

        static void StartGame()
        {
            Console.Write("Loading map");
            Thread.Sleep(1000);
            Console.Clear();

            MakeMapThread(scaleMap);
            Console.WriteLine();
            Console.WriteLine("Press any key to start");
        }
        static void Intro()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Welcome to my firs playable program.");
            Console.ResetColor();

            Console.WriteLine("Use WSAD to walk.");
            Console.WriteLine("If you walk into an enemy, you will damage it.");
            Console.WriteLine("If an enemy walks into you, it will damage you.");

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

        static void MakeMapThread(int scale)
        {
            for (int b = 0; b < map.GetLength(1) * scale + 2; b++)
            {
                if (b == 0)
                {
                    Console.Write("╔");
                }

                else if (b == map.GetLength(1) * scale + 1)
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
            Thread.Sleep(100);
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
                    Thread.Sleep(100);
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
                else if (b > map.GetLength(1) * scale + 1)
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

            Console.SetCursorPosition(62, 1);
            Console.Write("~~~~~~~~~~");
            Console.SetCursorPosition(60, 2);
            Console.Write($"Turn: {turn}");
            Console.SetCursorPosition(62, 3);
            Console.Write("~~~~~~~~~~");
            Console.SetCursorPosition(60, 4);
            Console.Write($"Player Health: {playerHealth}");
            Console.SetCursorPosition(60, 5);
            Console.Write($"Staus: {playerStatus}");
            Console.SetCursorPosition(60, 6);
            Console.Write($"Total Gold: {curGold}");
            Console.SetCursorPosition(60, 7);
            Console.Write($"Total enemies killed: {totalEnemyKilled}");

            Console.SetCursorPosition(62, 8);
            Console.Write("~~~~~~~~~~");
            Console.SetCursorPosition(60, 9);
            Console.Write($"Total Enemies on map: {Enemies.Count}");


        }

        static void HealthStatus()
        {
            if (playerHealth == 100)
            {
                playerStatus = "Perfectly healthy";
            }
            else if (playerHealth >= 90 && playerHealth < 100)
            {
                playerStatus = "Very Healthy";
            }
            else if (playerHealth >= 75 && playerHealth < 100)
            {
                playerStatus = "Healthy";
            }
            else if (playerHealth >= 50 && playerHealth < 100)
            {
                playerStatus = "Hurt";
            }
            else if (playerHealth >= 25 && playerHealth < 100)
            {
                playerStatus = "Badly hurt";
            }
            else if (playerHealth >= 10 && playerHealth < 100)
            {
                playerStatus = "Danger";
            }
            else if (playerHealth > 0 && playerHealth < 100)
            {
                playerStatus = "Immedate Danger";
            }
            else if(playerHealth == 0 && playerHealth < 100)
            {
                playerStatus = "dead";
            }
            else
            {
                playerStatus = "not valid health status";
            }
        }
        //Actual Game loop logic
        static void ProcessInput()
        {
            if (turn < 0) return;

            yMovement = 0;
            xMovement = 0;

            input = ConsoleKey.NoName;
            while (input == ConsoleKey.NoName)
            {
                input = Console.ReadKey(true).Key;

                if(input != ConsoleKey.W || input != ConsoleKey.S || input != ConsoleKey.A || input != ConsoleKey.D)
                {
                    input = ConsoleKey.NoName;
                }
            }

            if(input == ConsoleKey.W)
            {
                yMovement--;
            }
            if(input == ConsoleKey.S)
            {
                yMovement++;
            }
            if(input == ConsoleKey.A)
            {
                xMovement--;
            }
            if(input == ConsoleKey.D)
            {
                xMovement++;
            }

           

            
        }
        static void Update()
        {
            HealthStatus();

            if (xAxisPlayer + xMovement > 0 && xAxisPlayer + xMovement < map.GetLength(1) * scaleMap + 1)
            {
                xAxisPlayer += xMovement;
            }

            if (yAxisPlayer + yMovement > 0 && yAxisPlayer + yMovement < map.GetLength(0) * scaleMap + 1)
            {
                yAxisPlayer += yMovement;
            }
            turn++;
        }
        static void Draw()
        {
            MakeMap(scaleMap);
            HUD();

            Console.SetCursorPosition(xAxisPlayer, yAxisPlayer);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("@");
            Console.ResetColor();
        }

        

        
    }
}
