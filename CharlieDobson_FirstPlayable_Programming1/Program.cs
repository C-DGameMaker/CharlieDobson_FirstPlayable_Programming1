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

        


        static void Main(string[] args)
        {
            Intro();
            while(isAlive == true)
            {

            }
            Ending();
        }

        static void Intro()
        {
            
        }

        static void Ending()
        {

        }

        static void MakeMap(int scale)
        {

        }
    }
}
