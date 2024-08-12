using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_2021
{
    static class Game
    {
        public static Window Window;
        public static Scene CurrentScene { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }

        private static KeyboardController keyboardCtrl;
        private static List<Controller> controllers;

        public static float UnitSize { get; private set; }
        public static float OptimalScreenHeight { get; private set; }
        public static float OptimalUnitSize { get; private set; }
        public static Vector2 ScreenCenter { get; private set; }
        public static float HalfDiagonalSquared { get { return ScreenCenter.LengthSquared; } }

        public static void Init()
        {
            Window = new Window(1280, 720, "Heads");
            Window.SetVSync(false);
            Window.SetDefaultViewportOrthographicSize(10);

            OptimalScreenHeight = 1080;//best resolution
            UnitSize = Window.Height / Window.OrthoHeight;
            OptimalUnitSize = OptimalScreenHeight / Window.OrthoHeight;

            ScreenCenter = new Vector2(Window.OrthoWidth * 0.5f, Window.OrthoHeight * 0.5f);

            PlayScene playScene = new PlayScene();
            playScene.NextScene = null;

            CurrentScene = playScene;

            // CONTROLLERS

            //create default keys for keyboard controller
            List<KeyCode> keys = new List<KeyCode>();
            keys.Add(KeyCode.W);
            keys.Add(KeyCode.S);
            keys.Add(KeyCode.D);
            keys.Add(KeyCode.A);
            keys.Add(KeyCode.Space);

            KeysList keyList = new KeysList(keys);

            // Always create a keyboard controller (init at 0 cause we only have 1 keyboard)
            keyboardCtrl = new KeyboardController(0, keyList);

            // Check how many joypads are currently connected
            string[] joysticks = Window.Joysticks;
            controllers = new List<Controller>();

            for (int i = 0; i < joysticks.Length; i++)
            {
                Console.WriteLine(Window.JoystickDebug(i));
                Console.WriteLine(joysticks[i]);

                if(joysticks[i] != null && joysticks[i] != "Unmapped Controller")
                {
                    controllers.Add(new JoypadController(i));
                }
            }
        }

        public static float PixelsToUnits(float pixelsSize)
        {
            return pixelsSize / OptimalUnitSize;
        }

        public static Controller GetController(int index)
        {
            Controller ctrl = keyboardCtrl;

            if(index < controllers.Count)
            {
                ctrl = controllers[index];
            }

            return ctrl;
        }

        public static void Play()
        {
            CurrentScene.Start();

            while (Window.IsOpened)
            {
                // Show FPS on Window Title Bar
                Window.SetTitle($"FPS: {1f / Window.DeltaTime}");

                // Exit when ESC is pressed
                if (Window.GetKey(KeyCode.Esc))
                {
                    break;
                }

                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.OnExit();

                    if(nextScene != null)
                    {
                        CurrentScene = nextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                // INPUT
                CurrentScene.Input();

                // UPDATE
                CurrentScene.Update();

                // DRAW
                CurrentScene.Draw();

                Window.Update();
            }
        }
    }
}
