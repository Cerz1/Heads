using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    class Background : IDrawable
    {
        protected Sprite sky;
        protected Sprite sky2;
        protected Texture skyTexture;

        protected Texture[] textures;
        protected Sprite[] sprites;

        public DrawLayer Layer { get; protected set; }

        public Background(int numTextures)
        {
            Layer = DrawLayer.Background;

            skyTexture = new Texture("Assets/sky.png");
            sky = new Sprite(Game.PixelsToUnits(skyTexture.Width), Game.PixelsToUnits(skyTexture.Height));
            sky.Camera = CameraMngr.GetCamera("Sky");

            sky2 = new Sprite(sky.Width, sky.Height);
            sky2.Camera = sky.Camera;
            sky2.position.Y = sky.position.Y;
            sky2.position.X = sky.Width;

            textures = new Texture[numTextures];
            sprites = new Sprite[numTextures*2];

            float[] positions = new float[] { 2.5f, 0, 4.1f, 4.1f };

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = new Texture($"Assets/bg_{i}.png");

                sprites[i] = new Sprite(Game.PixelsToUnits(textures[i].Width), Game.PixelsToUnits(textures[i].Height));
                sprites[i].position.Y = positions[i];

                int cloneIndex = i + numTextures;

                sprites[cloneIndex] = new Sprite(sprites[i].Width, sprites[i].Height);
                sprites[cloneIndex].position.Y = sprites[i].position.Y;
                sprites[cloneIndex].position.X = sprites[i].Width;

                //setting cameras
                if(i< numTextures - 1)
                {
                    sprites[i].Camera = CameraMngr.GetCamera($"Bg_{i}");
                    sprites[cloneIndex].Camera = sprites[i].Camera;
                }

            }

            DrawMngr.AddItem(this);
        }


        public void Draw()
        {
            sky.DrawTexture(skyTexture);
            sky2.DrawTexture(skyTexture);

            for (int i = 0; i < textures.Length; i++)
            {
                sprites[i].DrawTexture(textures[i]);
                sprites[i+textures.Length].DrawTexture(textures[i]);
            }
        }
    }
}
