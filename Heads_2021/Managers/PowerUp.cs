using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    abstract class PowerUp : GameObject
    {
        public PowerUp(string textureName) : base(textureName)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.PowerUp;
            RigidBody.IsCollisionAffected = true;

            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.Player);

            //RigidBody.Velocity = new OpenTK.Vector2(-300.0f, 0.0f);
        }

        //public virtual void OnAttach(Enemy p)
        //{
        //    attachedEnemy = p;
        //    IsActive = false;
        //}

        //public virtual void OnDetach()
        //{
        //    attachedEnemy = null;
        //    PowerUpsMngr.RestorePowerUp(this);
        //}

        //public override void Update()
        //{
        //    if (IsActive)
        //    {
        //        if (Position.X + HalfWidth < 0)
        //        {
        //            PowerUpsMngr.RestorePowerUp(this);
        //        }
        //    }
        //}

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Enemy)
            {
                ((Enemy)collisionInfo.Collider).Reset();
                PowerUpsMngr.RestorePowerUp(this);
            }
            if (collisionInfo.Collider is Player)
            {
                ((Player)collisionInfo.Collider).Reset();
                PowerUpsMngr.RestorePowerUp(this);
            }
        }
    }
}

