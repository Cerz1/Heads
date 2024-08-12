using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads_2021
{
    class ShootState : State
    {
        private Enemy enemy;

        private float shootTimeLimit = 0.25f;
        private float shootCoolDown = 0.0f;

        private float checkForNewPlayers;

        public ShootState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void OnEnter()
        {
            enemy.RigidBody.Velocity = Vector2.Zero;
        }

        public override void Update()
        {
            shootCoolDown -= Game.Window.DeltaTime;
            checkForNewPlayers -= Game.DeltaTime;

            if (checkForNewPlayers <= 0)
            {
                enemy.Rival = enemy.GetBestPlayerToFight();

                checkForNewPlayers = RandomGenerator.GetRandomFloat() + 0.2f;
            }

            if (enemy.Rival==null || !enemy.CanAttackPlayer())
            {
                stateMachine.GoTo(StateEnum.WALK);
            }
            else
            {
                enemy.LookAtPlayer();

                if(shootCoolDown <= 0.0f)
                {
                    shootCoolDown = shootTimeLimit;
                    enemy.ShootPlayer();
                }
            }
        }
    }
}
