using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    class FollowState : State
    {
        private Enemy enemy;
        private float checkForNewPlayers;

        public FollowState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void Update()
        {
            checkForNewPlayers -= Game.DeltaTime;

            if (checkForNewPlayers <= 0)
            {
                enemy.Rival = enemy.GetBestPlayerToFight();

                checkForNewPlayers = RandomGenerator.GetRandomFloat() + 0.2f;
            }

            if (enemy.Rival == null || !enemy.Rival.IsAlive)
            {
                stateMachine.GoTo(StateEnum.WALK);
            }
            else if (enemy.CanAttackPlayer())
            {
                stateMachine.GoTo(StateEnum.SHOOT);
            }
            else
            {
                enemy.HeadToPlayer();
            }
        }
    }
}
