using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    class GameOverScene : TitleScene
    {
        public GameOverScene() : base("Assets/gameOverBg.png", Aiv.Fast2D.KeyCode.Y)
        {

        }

        public override void Input()
        {
            base.Input();

            if(IsPlaying && Game.Window.GetKey(Aiv.Fast2D.KeyCode.N))
            {
                IsPlaying = false;
                NextScene = null;
            }
        }
    }
}
