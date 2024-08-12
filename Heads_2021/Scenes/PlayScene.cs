using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_2021
{
    class PlayScene : Scene
    {
        protected List<Player> players;
        protected GameObject bg;
        protected int alivePlayers;

        public List<Player> Players { get { return players; } }
        public Enemy Enemy;

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();

            FontMngr.Init();
            Font stdFont = FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            Font comic = FontMngr.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);

            bg = new GameObject("bg", DrawLayer.Background);
            bg.Position = Game.ScreenCenter;
            bg.IsActive = true;

            players = new List<Player>();

            Player player = new Player(Game.GetController(0), 0);
            player.Position = new Vector2(4, 4);

            Controller controller = Game.GetController(1);

            if(controller is KeyboardController)
            {
                List<KeyCode> keys = new List<KeyCode>();
                keys.Add(KeyCode.Up);
                keys.Add(KeyCode.Down);
                keys.Add(KeyCode.Right);
                keys.Add(KeyCode.Left);
                keys.Add(KeyCode.CtrlRight);

                KeysList keyList = new KeysList(keys);
                controller = new KeyboardController(1, keyList);
            }

            Player player2 = new Player(controller, 1);
            player2.Position = new Vector2(6, 4);



            players.Add(player);
            players.Add(player2);

            alivePlayers = players.Count;

            BulletMngr.Init();

            Enemy = new Enemy();
            Enemy.Position = new Vector2(15, 4);

            PowerUpsMngr.Init();

            base.Start();
        }

        private static void LoadAssets()
        {
            GfxMngr.AddTexture("bg", "Assets/hex_grid_green.png");

            GfxMngr.AddTexture("player_1", "Assets/player_1.png");

            GfxMngr.AddTexture("enemy_0", "Assets/enemy_0.png");
            GfxMngr.AddTexture("enemy_1", "Assets/enemy_1.png");

            GfxMngr.AddTexture("bullet", "Assets/fireball.png");

            GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");

            GfxMngr.AddTexture("energyPowerUp", "Assets/heart.png");
        }

        public override void Input()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].IsAlive)
                {
                    players[i].Input();
                }
            }
        }

        public override void Update()
        {
            PhysicsMngr.Update();
            UpdateMngr.Update();
            PowerUpsMngr.Update();

            PhysicsMngr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            GfxMngr.ClearAll();
            FontMngr.ClearAll();

            DebugMngr.ClearAll();

            return base.OnExit();
        }

        public override void Draw()
        {
            DrawMngr.Draw();
        }

        public virtual void OnPlayerDies()
        {
            alivePlayers--;
            if(alivePlayers <= 0)
            {
                IsPlaying = false;
            }
        }
    }
}
