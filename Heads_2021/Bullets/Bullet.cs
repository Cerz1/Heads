using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    enum BulletType { PlayerBullet, EnemyBullet, LAST }

    abstract class Bullet : GameObject
    {
        protected int damage = 25;
        protected float maxSpeed = 10;

        public BulletType Type { get; protected set; }

        public Bullet(string texturePath, int spriteWidth=0, int spriteHeight=0) : base(texturePath)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
        }

        public void Shoot(Vector2 shootPos, Vector2 shootDir)
        {
            sprite.position = shootPos;
            RigidBody.Velocity = shootDir * maxSpeed;
            Forward = shootDir;
        }

        public virtual void Reset()
        {
            IsActive = false;
        }

        public override void Update()
        {
            if (IsActive)
            {
                Vector2 cameraDist = Position - Game.ScreenCenter;

                if (cameraDist.LengthSquared > Game.HalfDiagonalSquared)
                {
                    BulletMngr.RestoreBullet(this);
                }
            }
        }
    }
}
