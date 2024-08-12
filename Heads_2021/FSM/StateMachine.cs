using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    enum StateEnum
    {
        WALK, FOLLOW, SHOOT
    };
    class StateMachine
    {
        private Dictionary<StateEnum, State> states;
        private State current;

        public StateMachine()
        {
            states = new Dictionary<StateEnum, State>();
            current = null;
        }

        public void AddState(StateEnum key, State state)
        {
            states[key] = state;
            state.SetStateMachine(this);
        }

        public void GoTo(StateEnum key)
        {
            if(current != null)
            {
                current.OnExit();
            }

            current = states[key];
            current.OnEnter();
        }

        public void Update()
        {
            current.Update();
        }
    }
}
