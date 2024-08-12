using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    class WalkState : State
    {
        private Enemy enemy;
        private float checkForVisiblePlayers;

        public WalkState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void OnEnter()
        {
            enemy.ComputeRandomPoint();
            checkForVisiblePlayers = 0.0f;
        }

        public override void Update()
        {
            //Timer!
            checkForVisiblePlayers -= Game.DeltaTime;

            if (checkForVisiblePlayers <= 0.0f)
            {
                Player p_rival = enemy.GetBestPlayerToFight();
                checkForVisiblePlayers = RandomGenerator.GetRandomFloat() + 0.2f;

                if (p_rival != null)
                {
                    enemy.Rival = p_rival;
                    stateMachine.GoTo(StateEnum.FOLLOW);
                    return;
                }
            }

            enemy.HeadToPoint();
        }
    }
}
