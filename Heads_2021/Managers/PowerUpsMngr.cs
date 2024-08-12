using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads_2021
{
    static class PowerUpsMngr
    {
        public static List<PowerUp> items;
        public static List<PowerUp> ActivePowerUps;

        static float nextSpawn;

        public static void Init()
        {
            items = new List<PowerUp>();
            items.Add(new EnergyPowerUp());
            items.Add(new EnergyPowerUp());
            items.Add(new EnergyPowerUp());
            ActivePowerUps = new List<PowerUp>();
        }

        public static void Update()
        {
            nextSpawn -= Game.Window.DeltaTime;

            if (nextSpawn <= 0)
            {
                SpawnPowerUp();
                nextSpawn = RandomGenerator.GetRandomFloat() * 8 + 2;
            }
        }

        public static void SpawnPowerUp()
        {
            if (items.Count > 0)
            {
                int randomIndex = RandomGenerator.GetRandomInt(0, items.Count);
                PowerUp randPowerUp = items[randomIndex];
                items.RemoveAt(randomIndex);

                randPowerUp.Position = new Vector2(RandomGenerator.GetRandomInt(1,17), RandomGenerator.GetRandomInt(1, 9));

                randPowerUp.IsActive = true;
                ActivePowerUps.Add(randPowerUp);
            }
        }

        public static void RestorePowerUp(PowerUp p)
        {
            p.IsActive = false;
            items.Add(p);
            ActivePowerUps.Remove(p);
        }

        public static void Clear()
        {
            items.Clear();
        }
    }
}
