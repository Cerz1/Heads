using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    abstract class Actor : GameObject
    {
        // Variables
        protected BulletType bulletType;
        protected int energy;
        protected int maxEnergy;
        protected float maxSpeed;

        // Propertiex
        public bool IsAlive { get { return energy > 0; } }

        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }

        public int MaxEnergy { get { return maxEnergy; } }

        public Actor(string texturePath, float w = 0, float h = 0) : base(texturePath, w:w ,h:h)
        {
            maxEnergy = 100;
        }

        protected virtual void Shoot(Vector2 direction)
        {
            Bullet b = BulletMngr.GetBullet(bulletType);

            if (b != null)
            {
                b.IsActive = true;
                b.Shoot(sprite.position, direction);
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            //SpawnMngr.RestoreEnemy(((Enemy)other));
            //AddDamage(30);
            OnWallCollides(collisionInfo);
        }

        protected virtual void OnWallCollides(Collision collisionInfo)
        {
            if(collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                // Horizontal Collision
                if(X < collisionInfo.Collider.X)
                {
                    // Collision from Left (inverse horizontal delta)
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                X += collisionInfo.Delta.X;
                RigidBody.Velocity.X = 0.0f;
            }
            else
            {
                // Vertical Collision
                if (Y < collisionInfo.Collider.Y)
                {
                    // Collision from Top
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    RigidBody.Velocity.Y = 0.0f;
                }
                else
                {
                    // Collision from Bottom
                    RigidBody.Velocity.Y = -RigidBody.Velocity.Y * 0.8f;
                }

                Y += collisionInfo.Delta.Y;
            }
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                OnDie();
            }
        }

        public abstract void OnDie();

        public virtual void Reset()
        {
            Energy = maxEnergy;
        }

        public override void Update()
        {
            if (IsActive && RigidBody.Velocity != Vector2.Zero)
            {
                Forward = RigidBody.Velocity;
            }
        }
    }
}

