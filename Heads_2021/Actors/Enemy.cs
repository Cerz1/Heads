using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_2021
{
    class Enemy : Actor
    {
        protected float visionRadius;
        protected float shootDistance;

        protected float halfConeAngle = MathHelper.DegreesToRadians(40);
        protected Vector2 pointToReach;

        protected StateMachine fsm;

        protected ProgressBar nrgBar1;
        public override int Energy { get => base.Energy; set { base.Energy = value; nrgBar1.Scale((float)value / (float)maxEnergy); } }

        public float followSpeed;
        public float walkSpeed;
        public Player Rival;

        public PowerUp BestPowerUp;

        private int bestChoice;
        private float checkForVisiblePowerUps;


        public Enemy() : base("enemy_0")
        {
            // Set RB
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Enemy;
            RigidBody.AddCollisionType(RigidBodyType.PowerUp);
            bulletType = BulletType.EnemyBullet;


            nrgBar1 = new ProgressBar("barFrame", "blueBar", new Vector2(Game.PixelsToUnits(4.0f), Game.PixelsToUnits(4.0f)));
            nrgBar1.Position = new Vector2(12.0f, 1.5f);
            //Vector2 enemyNamePos = nrgBar.Position + new Vector2(0, -1);
            //playerName = new TextObject(enemyNamePos, $"Enemy", FontMngr.GetFont(), 5);
            //playerName.IsActive = true;

            // FSM Set
            visionRadius = 5.0f;
            walkSpeed = 1.5f;
            followSpeed = walkSpeed * 2.0f;
            shootDistance = 3.0f;

            fsm = new StateMachine();
            fsm.AddState(StateEnum.WALK, new WalkState(this));
            fsm.AddState(StateEnum.FOLLOW, new FollowState(this));
            fsm.AddState(StateEnum.SHOOT, new ShootState(this));
            fsm.GoTo(StateEnum.WALK);

            IsActive = true;
            bestChoice = 0;
            Reset();
        }

        public void ComputeRandomPoint()
        {
            float randX = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoWidth - 2) + 1;
            float randY = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoHeight - 2) + 1;

            pointToReach = new Vector2(randX, randY);
        }

        public void HeadToPoint()
        {
            Vector2 distVect = pointToReach - Position;

            if (distVect.LengthSquared <= 0.01f)
            {
                ComputeRandomPoint();
            }

            RigidBody.Velocity = distVect.Normalized() * walkSpeed;
        }

        public bool CanAttackPlayer()
        {
            if (Rival == null)
            {
                return false;
            }

            Vector2 distVect = Rival.Position - Position;

            return distVect.LengthSquared < shootDistance * shootDistance;
        }

        public void HeadToPlayer()
        {
            if (Rival != null)
            {
                Vector2 distVect = Rival.Position - Position;
                RigidBody.Velocity = distVect.Normalized() * followSpeed;
            }
        }

        public void HeadToPowerUp()
        {
            if (BestPowerUp != null)
            {
                Vector2 distVect = BestPowerUp.Position - Position;
                RigidBody.Velocity = distVect.Normalized() * followSpeed;
            }
        }

        public List<Player> GetVisiblePlayers()
        {
            List<Player> players = ((PlayScene)Game.CurrentScene).Players;
            List<Player> visiblePlayers = new List<Player>();

            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].IsAlive)
                {
                    continue;
                }

                Vector2 distVect = players[i].Position - Position;

                if (distVect.LengthSquared < visionRadius * visionRadius)
                {
                    // Player is inside vision radius
                    // Check for cone angle
                    double angleCos = MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1, 1);
                    float playerAngle = (float)Math.Acos(angleCos);

                    if (playerAngle < halfConeAngle)
                    {
                        visiblePlayers.Add(players[i]);
                    }
                }

            }

            return visiblePlayers;
        }

        public List<PowerUp> GetVisiblePowerUps()
        {
            List<PowerUp> powerUps = PowerUpsMngr.ActivePowerUps;
            List<PowerUp> visiblePowerUps = new List<PowerUp>();
            for (int i = 0; i < powerUps.Count; i++)
            {
                //if (!powerUps[i].IsActive)
                //{
                //    continue;
                //}
                //Vector2 distVect = powerUps[i].Position - Position;
                //if (distVect.LengthSquared < visionRadius * visionRadius)
                //{
                //    visiblePowerUps.Add(powerUps[i]);
                //}
                Vector2 distVect = powerUps[i].Position - Position;

                if (distVect.LengthSquared < visionRadius * visionRadius)
                {
                    // Player is inside vision radius
                    // Check for cone angle
                    double angleCos = MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1, 1);
                    float playerAngle = (float)Math.Acos(angleCos);

                    if (playerAngle < halfConeAngle)
                    {
                        visiblePowerUps.Add(powerUps[i]);
                    }
                }

            }

            return visiblePowerUps;
        }


        public Player GetBestPlayerToFight()
        {
            List<Player> visiblePlayers = GetVisiblePlayers();

            Player bestPlayer = null;

            if (visiblePlayers.Count > 0)
            {
                if (visiblePlayers.Count > 1)
                {
                    // Fuzzy logic
                    float fuzzyMax = -1.0f;
                    float fuzzySum = fuzzyMax;

                    for (int i = 0; i < visiblePlayers.Count; i++)
                    {
                        // Distance
                        Vector2 distFromPlayer = Position - visiblePlayers[i].Position;
                        float fuzzyDistance = 1 - distFromPlayer.LengthSquared / (visionRadius * visionRadius);

                        // Energy
                        float fuzzyEnergy = 1 - visiblePlayers[i].Energy / visiblePlayers[i].MaxEnergy;

                        // Angle
                        double playerAngle = MathHelper.Clamp(Vector2.Dot(visiblePlayers[i].Forward, distFromPlayer.Normalized()), -1, 1);
                        float fuzzyAngle = 1 - (float)(Math.Acos(playerAngle) / Math.PI);

                        // Sum
                        fuzzySum = fuzzyDistance + fuzzyEnergy + fuzzyAngle;

                        if (fuzzySum > fuzzyMax)
                        {
                            fuzzyMax = fuzzySum;
                            bestPlayer = visiblePlayers[i];
                        }
                    }
                }
                else
                {
                    bestPlayer = visiblePlayers[0];
                }
            }

            return bestPlayer;
        }

        public PowerUp GetBestPowerUps()
        {
            List<PowerUp> visiblePowerUps = GetVisiblePowerUps();

            float fuzzyMax = -1.0f;
            float fuzzySum = fuzzyMax;

            PowerUp bestPowerUp = null;

            if (visiblePowerUps.Count > 0)
            {
                for (int i = 0; i < visiblePowerUps.Count; i++)
                {
                    Vector2 distFromPlayer = Position - visiblePowerUps[i].Position;
                    float fuzzyDistance = 1 - distFromPlayer.LengthSquared / (visionRadius * visionRadius);

                    double playerAngle = MathHelper.Clamp(Vector2.Dot(visiblePowerUps[i].Forward, distFromPlayer.Normalized()), -1, 1);
                    float fuzzyAngle = 1 - (float)(Math.Acos(playerAngle) / Math.PI);

                    fuzzySum = fuzzyDistance + fuzzyAngle;

                    if (fuzzySum > fuzzyMax)
                    {
                        fuzzyMax = fuzzySum;
                        bestPowerUp = visiblePowerUps[i];
                    }
                }
            }


            return bestPowerUp;
        }


        //public bool CanSeePlayer()
        //{
        //    List<Player> players2 = ((PlayScene)Game.CurrentScene).Players;

        //    //Vector2 distVect = player.Position - Position;

        //    //if(distVect.LengthSquared < visionRadius * visionRadius)
        //    //{
        //    //    // Player is inside vision radius
        //    //    // Check for cone angle
        //    //    double angleCos = MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1, 1);
        //    //    float playerAngle = (float)Math.Acos(angleCos);

        //    //    return playerAngle < halfConeAngle;
        //    //}

        //    return false;
        //}

        public void ShootPlayer()
        {
            //Player player = ((PlayScene)Game.CurrentScene).Player;
            //Vector2 direction = player.Position - Position;
            Shoot(Forward);
        }

        public void LookAtPlayer()
        {
            if (Rival != null)
            {
                Vector2 direction = Rival.Position - Position;
                Forward = direction;
            }
        }

        public void GetBestChoice()
        {
            if (BestPowerUp == null)
            {
                return;
            }
            else if (Rival == null && BestPowerUp == null)
            {
                return;
            }
            else if (Rival == null)
            {
                if (BestPowerUp != null && this.energy < 100)
                {
                    HeadToPowerUp();
                }
            }
            else
            {
                float fuzzySum1 = -1.0f;
                float fuzzySum = -1.0f;

                // Distance
                Vector2 distFromPlayer = Position - Rival.Position;
                float fuzzyDistance = 1 - distFromPlayer.LengthSquared / (visionRadius * visionRadius);

                // Energy
                float fuzzyEnergy = 1 - Rival.Energy / Rival.MaxEnergy;

                //// Angle
                //double playerAngle = MathHelper.Clamp(Vector2.Dot(Rival.Forward, distFromPlayer.Normalized()), -1, 1);
                //float fuzzyAngle = 1 - (float)(Math.Acos(playerAngle) / Math.PI);

                // Sum
                fuzzySum = fuzzyDistance + fuzzyEnergy;

                // Distance
                Vector2 distFromPowerUp = Position - BestPowerUp.Position;
                float fuzzyDistance1 = 1 - distFromPowerUp.LengthSquared / (visionRadius * visionRadius);

                // Energy
                float fuzzyEnergy1 = 1 - this.Energy / this.MaxEnergy;

                //// Angle
                //double playerAngle1 = MathHelper.Clamp(Vector2.Dot(BestPowerUp.Forward, distFromPowerUp.Normalized()), -1, 1);
                //float fuzzyAngle1 = 1 - (float)(Math.Acos(playerAngle) / Math.PI);

                // Sum
                fuzzySum1 = fuzzyDistance1 + fuzzyEnergy1;

                if (fuzzySum > fuzzySum1)
                {
                    return;
                }
                else
                {
                    HeadToPowerUp();
                }
            }
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (RigidBody.Velocity != Vector2.Zero)
                {
                    Forward = RigidBody.Velocity;
                }

                fsm.Update();
                checkForVisiblePowerUps -= Game.DeltaTime;
                if (checkForVisiblePowerUps <= 0)
                {
                    BestPowerUp = GetBestPowerUps();
                    checkForVisiblePowerUps = RandomGenerator.GetRandomFloat() + 0.2f;
                }
                GetBestChoice();

            }
        }

        public override void OnDie()
        {
            IsActive = false;
        }
    }
}
