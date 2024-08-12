using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_2021
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet() : base("bullet")
        {
            Type = BulletType.EnemyBullet;
            RigidBody.Type = RigidBodyType.EnemyBullet;
            RigidBody.AddCollisionType(RigidBodyType.PlayerBullet | RigidBodyType.Player | RigidBodyType.Tile);
            sprite.SetAdditiveTint(255, 0, 255, 0);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Player)
            {
                ((Player)collisionInfo.Collider).AddDamage(damage);
            }

            BulletMngr.RestoreBullet(this);
        }
    }
}
