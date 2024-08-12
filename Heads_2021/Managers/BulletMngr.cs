using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    static class BulletMngr
    {
        private static Queue<Bullet>[] bullets;
        private static int queueSize = 16;

        public static void Init()
        {
            bullets = new Queue<Bullet>[(int)BulletType.LAST];

            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Queue<Bullet>(queueSize);

                switch ((BulletType)i)
                {
                    case BulletType.PlayerBullet:

                        for (int q = 0; q < queueSize; q++)
                        {
                            bullets[i].Enqueue(new PlayerBullet());
                        }

                        break;

                    case BulletType.EnemyBullet:

                        for (int q = 0; q < queueSize; q++)
                        {
                            bullets[i].Enqueue(new EnemyBullet());
                        }

                        break;
                }
            }
        }

        public static Bullet GetBullet(BulletType type)
        {
            int index = (int)type;

            if (bullets[index].Count > 0)
            {
                Bullet bullet = bullets[index].Dequeue();
                bullet.IsActive = true;

                return bullet;
            }

            return null;
        }

        public static void RestoreBullet(Bullet bullet)
        {
            bullet.Reset();
            bullets[(int)bullet.Type].Enqueue(bullet);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Clear();
            }
        }
    }
}
