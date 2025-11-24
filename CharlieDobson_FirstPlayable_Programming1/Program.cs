using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CharlieDobson_FirstPlayable_Programming1
{
    internal class Program
    {
        //Player
        static int playerHealth = 100;
        static string playerStatus = "";
        static int curGold;
        static bool isAlive = true;
        static int totalEnemyKilled;

        //Enemy
        static int enemyAHealth = 0;
        static int enemyBHealth = 0;
        static ConsoleColor[] EnemyColors = { ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Cyan};
        static ConsoleColor enemyColorA;
        static ConsoleColor enemyColorB;
        static bool sameAsPlayerA = true;
        static bool sameAsPlayerB = true;

        //Map
        static string map = "Map.txt";
        static string[] inGameMap = System.IO.File.ReadAllLines(map);
        static ConsoleColor[] mapColors = { ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Blue, ConsoleColor.Gray};

        //Turn based stuff
        static int turn = 0;
        static int xMovement = 0;
        static int yMovement = 0;
        static ConsoleKey input;

        static Random rand = new Random();

        //Movement
        static int xAxisPlayer = 20;
        static int yAxisPlayer = 17;

        static int xAxisEnemyA;
        static int yAxisEnemyA;

        static int xAxisEnemyB;
        static int yAxisEnemyB;

        static int xAxisA = 0;
        static int yAxisA = 0;

        static int xAxisB = 0;
        static int yAxisB = 0;

        static bool attack;

        //Health
        static int healthPickUPX;
        static int healthPickUPY;

        static int healthOnBoard;

        //Ending
        static string scorePath = "Score.txt";
        static bool length = false;
        static string initials = "";



        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Intro();
            Console.ReadKey(true);
            Console.Clear();

            StartGame();
            Console.ReadKey(true);
            Console.Clear();

            while (isAlive == true)
            {
                input = ConsoleKey.NoName;
                ProcessInput();
                Update();
                Draw();
                Thread.Sleep(100);

            }
            Console.Clear();
            Ending();
            Console.ReadKey(true);
        }

        static void StartGame()
        {
            Console.Write("Loading map");
            Thread.Sleep(1000);
            Console.Clear();

            MakeMapThread(1);
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
            Console.WriteLine($"You collected {curGold} amount of Gold.");
            List<string> allScores = new List<string>(System.IO.File.ReadAllLines(scorePath));

            Console.WriteLine("Input your intitals here:");

            while (length == false)
            {
                Console.WriteLine("Your initals should be three letters");
                initials = Console.ReadLine();

                if (initials.Length == 3)
                {
                    length = true;
                }
            }

            string data = Convert.ToString(curGold + totalEnemyKilled) + ": " + initials + ": Gold earned: " + curGold 
                + " and enemies killed: " + totalEnemyKilled;
            allScores.Add(data);

            allScores.Sort();
            allScores.Reverse();

            Console.WriteLine("ALL HIGH SCORE");
            foreach (string score in allScores)
            {
                Console.WriteLine(score);
            }

            System.IO.File.WriteAllLines(scorePath, allScores);

            Console.WriteLine("RESET PRORGAM TO TRY AGAIN");
            Console.ReadKey(true);
            Console.Clear();
        }

        //Outside of game loop stuff
        static void MakeMap(int scale)
        {
            int length = inGameMap.Length;
            int height = inGameMap[0].Length;

            for (int b = 0; b < height * scale + 2; b++)
            {
                if (b == 0)
                {
                    Console.Write("╔");
                }

                else if (b == height * scale + 1)
                {
                    Console.Write("╗");
                }
                else
                {
                    Console.Write("═");
                }
            }
            Console.Write("\n");



            for (int y = 0; y < length; y++)
            {

                for (int s = 0; s < scale; s++)
                {
                    Console.Write("║");

                    for (int x = 0; x < height; x++)
                    {
                        char tile = inGameMap[y][x];

                        if (tile == '▒')
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                        }
                        else if (tile == '░')
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                        }
                        else if (tile == '▓')
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else if (tile == '█')
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }

                        for (int c = 0; c < scale; c++)
                        {
                            Console.Write(tile);
                        }
                    }
                    Console.ResetColor();
                    Console.Write("║");
                    Console.Write("\n");
                }
            }



            for (int b = 0; b < height * scale + 2; b++)
            {
                if (b == 0)
                {
                    Console.Write("╚");
                }

                else if (b == height * scale + 1)
                {
                    Console.Write("╝");
                }
                else
                {
                    Console.Write("═");
                }
            }


        }

        static void MakeMapThread(int scale)
        {
            int length = inGameMap.Length;
            int height = inGameMap[0].Length;

            for (int b = 0; b < height * scale + 2; b++)
            {
                if (b == 0)
                {
                    Console.Write("╔");
                }

                else if (b == height * scale + 1)
                {
                    Console.Write("╗");
                }
                else
                {
                    Console.Write("═");
                }
            }
            Thread.Sleep(100);
            Console.Write("\n");



            for (int y = 0; y < length; y++)
            {

                for (int s = 0; s < scale; s++)
                {
                    Console.Write("║");

                    for (int x = 0; x < height; x++)
                    {
                        if (inGameMap[y][x] == '▒')
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                        }
                        else if (inGameMap[y][x] == '░')
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                        }
                        else if (inGameMap[y][x] == '▓')
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else if (inGameMap[y][x] == '█')
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }

                        for (int c = 0; c < scale; c++)
                        {
                            Console.Write(inGameMap[y][x]);
                        }
                    }
                    Console.ResetColor();
                    Console.Write("║");
                    Thread.Sleep(100);
                    Console.Write("\n");
                }
            }




            for (int b = 0; b < height * scale + 2; b++)
            {
                if (b == 0)
                {
                    Console.Write("╚");
                }

                else if (b == height * scale + 1)
                {
                    Console.Write("╝");
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
            Console.ForegroundColor = enemyColorA;
            Console.Write($"Enemy A");
            Console.ResetColor();
            Console.SetCursorPosition(60, 10);
            Console.Write($"Enemy A Health: {enemyAHealth}");
            Console.SetCursorPosition(60, 11);
            Console.ForegroundColor = enemyColorB;
            Console.Write($"Enemy B");
            Console.ResetColor();
            Console.SetCursorPosition(60, 12);
            Console.Write($"Enemy B Health: {enemyBHealth}");



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
            if (turn == 0) return;

            yMovement = 0;
            xMovement = 0;

            input = ConsoleKey.NoName;
            while (input == ConsoleKey.NoName)
            {
                input = Console.ReadKey(true).Key;

                if (input != ConsoleKey.W && input != ConsoleKey.S && input != ConsoleKey.A && input != ConsoleKey.D)
                {
                    input = ConsoleKey.NoName;
                }
            }

            if (input == ConsoleKey.W)
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
            Damage();

            if (attack == false)
            {
                if (xAxisPlayer + xMovement > 0 && xAxisPlayer + xMovement < inGameMap[0].Length * 1 + 1)
                {
                    xAxisPlayer += xMovement;

                }

                if (yAxisPlayer + yMovement > 0 && yAxisPlayer + yMovement < inGameMap.Length * 1 + 1)
                {
                    yAxisPlayer += yMovement;

                }

                EnemyMovement();
            }


            if (playerHealth <= 0)
            {
                isAlive = false;
            }
            
            if (enemyAHealth < 1)
            { 
                Enemy();
            }
            if(enemyBHealth < 1)
            {
                Enemy();
            }

            if(healthOnBoard != 1 && playerHealth < 100)
            {
                HealthPickup();
                healthOnBoard++;
            }

            Heal();

            turn++;
        }
        static void Draw()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            MakeMap(1);
            HUD();

            Console.SetCursorPosition(xAxisPlayer, yAxisPlayer);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("@");
            Console.ResetColor();

            Console.SetCursorPosition(60, 18);
            Console.Write("Enemy Positions. . .");
            Thread.Sleep(1000);
            Console.SetCursorPosition(60, 18);
            Console.Write("                      ");

            Console.SetCursorPosition(xAxisEnemyA, yAxisEnemyA);
            Console.ForegroundColor = enemyColorA;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("#");
            Console.ResetColor();

            Console.SetCursorPosition(xAxisEnemyB, yAxisEnemyB);
            Console.ForegroundColor = enemyColorB;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("#");
            Console.ResetColor();

            if(healthOnBoard == 1)
            {
                Console.SetCursorPosition(healthPickUPX, healthPickUPY);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write("+");
                Console.ResetColor();
            }

            

            
        }

        static void Enemy()
        {
            if (enemyAHealth < 1)
            {
                sameAsPlayerA = true;
                enemyAHealth = rand.Next(2, 11) * 5;
                enemyColorA = EnemyColors[rand.Next(0, 3)];

                if(sameAsPlayerA == true)
                {
                    xAxisEnemyA = rand.Next(1, inGameMap[0].Length + 1);
                    yAxisEnemyA = rand.Next(1, inGameMap.Length + 1);

                    if (xAxisEnemyA != xAxisPlayer && yAxisEnemyA != yAxisPlayer)
                    {
                        sameAsPlayerA = false;
                    }
                }
                
            }

            if (enemyBHealth < 1)
            {
                sameAsPlayerB = true;
                enemyBHealth = rand.Next(2, 11) * 5;
                enemyColorB = EnemyColors[rand.Next(0, 3)];

                if (sameAsPlayerB == true)
                {
                    xAxisEnemyB = rand.Next(1, inGameMap[0].Length + 1);
                    yAxisEnemyB = rand.Next(1, inGameMap.Length + 1);

                    if (xAxisEnemyB != xAxisPlayer && yAxisEnemyB != yAxisPlayer)
                    {
                        sameAsPlayerB = false;
                    }
                }
            }
        }

        static void EnemyMovement()
        {
            xAxisA = 0;
            yAxisA = 0;
            xAxisB = 0;
            yAxisB = 0;
            if(xAxisEnemyA <= xAxisPlayer)
            {
                xAxisA++;
            }
            else
            {
                xAxisA--;
            }

            if (yAxisEnemyA <= yAxisPlayer)
            {
                yAxisA++;
            }
            else
            {
                yAxisA--;
            }

            if(xAxisEnemyA + xAxisA > 0 && xAxisEnemyA + xAxisA < inGameMap[0].Length + 1)
            {
                xAxisEnemyA += xAxisA;
            }
            if (yAxisEnemyA + yAxisA > 0 && yAxisEnemyA + yAxisA < inGameMap.Length + 1)
            {
                yAxisEnemyA += yAxisA;
            }

            
            if (xAxisEnemyB < xAxisPlayer)
            {
                xAxisB++;
            }
            else
            {
                xAxisB--;
            }

            if (yAxisEnemyB < yAxisPlayer)
            {
                yAxisB++;
            }
            else
            {
                yAxisB--;
            }

            if (xAxisEnemyB + xAxisB > 0 && xAxisEnemyB + xAxisB < inGameMap[0].Length + 1)
            {
                xAxisEnemyB += xAxisB;
            }
            if (yAxisEnemyB + yAxisB > 0 && yAxisEnemyB + yAxisB < inGameMap.Length + 1)
            {
                yAxisEnemyB += yAxisB;
            }

        }
        
        static void Damage()
        {
            if(xAxisPlayer + xMovement  == xAxisEnemyA)
            {
                if (yAxisPlayer + yMovement == yAxisEnemyA)
                {
                    attack = true;
                    playerHealth = playerHealth - 5;
                    enemyAHealth = enemyAHealth - 5;

                    if (enemyAHealth < 1)
                    {
                        CoinsGained();
                        totalEnemyKilled++;
                    }
                    
                }
            }

            if (xAxisPlayer + xMovement == xAxisEnemyB)
            {
                if (yAxisPlayer + yMovement == yAxisEnemyB)
                {
                    attack = true;
                    playerHealth = playerHealth - 5;
                    enemyBHealth = enemyBHealth - 5;
                }

                if (enemyBHealth < 1)
                {
                    CoinsGained();
                    totalEnemyKilled++;
                }
            }

            attack = false;
        }
        static void CoinsGained()
        {
            int coin = rand.Next(1, 11);
            curGold += coin;
        }

        static void HealthPickup()
        {
            healthPickUPX = rand.Next(1, inGameMap[0].Length + 1);
            healthPickUPY = rand.Next(1, inGameMap.Length + 1);

        }

        static void Heal()
        {
            if(xAxisPlayer == healthPickUPX)
            {
                if(yAxisPlayer == healthPickUPY)
                {
                    healthOnBoard--;
                    playerHealth += 5;
                }
            }
        }




    }
}
