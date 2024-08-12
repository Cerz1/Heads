using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_2021
{
    class PlayerBullet : Bullet
    {
        public PlayerBullet() : base("bullet")
        {
            Type = BulletType.PlayerBullet;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.EnemyBullet | RigidBodyType.Enemy | RigidBodyType.Tile );
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Enemy)
            {
                ((Enemy)collisionInfo.Collider).AddDamage(damage);
            }

            BulletMngr.RestoreBullet(this);
        }
    }
}
