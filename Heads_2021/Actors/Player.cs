using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_2021
{
    class Player : Actor
    {
        // References
        protected Controller controller;
        protected ProgressBar nrgBar;
        protected TextObject playerName;
        // Variables
        protected int playerId;
        private bool isFirePressed;
        // Properties
        public override int Energy { get => base.Energy; set { base.Energy = value; nrgBar.Scale((float)value / (float)maxEnergy); } }
        public bool IsGrounded { get { return RigidBody.Velocity.Y == 0; } }

        public Player(Controller ctrl, int id = 0) : base("player_1")
        {
            // Misc
            controller = ctrl;
            maxSpeed = 6;
            bulletType = BulletType.PlayerBullet;
            IsActive = true;
            // RB
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.Tile);
            RigidBody.Friction = 40;
            // Nrg Bar
            nrgBar = new ProgressBar("barFrame", "blueBar", new Vector2(Game.PixelsToUnits(4.0f), Game.PixelsToUnits(4.0f)));
            nrgBar.Position = new Vector2(1.0f, 1.5f);
            // Player ID and Name
            playerId = id;
            //Vector2 playerNamePos = nrgBar.Position + new Vector2(0, -1);
            //playerName = new TextObject(playerNamePos, $"Player {playerId + 1}", FontMngr.GetFont(), 5);
            //playerName.IsActive = true;

            Reset();
        }

        public void Input()
        {
            Vector2 direction = new Vector2(controller.GetHorizontal(), controller.GetVertical());

            if(direction != Vector2.Zero)
            {
                RigidBody.Velocity = direction.Normalized() * maxSpeed;
            }

            // SHOOT
            if (controller.IsFirePressed())
            {
                if (!isFirePressed)
                {
                    isFirePressed = true;
                    Shoot(Forward);
                }
            }
            else if (isFirePressed)
            {
                isFirePressed = false;
            }
        }

        public override void OnDie()
        {
            IsActive = false;
            ((PlayScene)Game.CurrentScene).OnPlayerDies();
        }
    }
}
