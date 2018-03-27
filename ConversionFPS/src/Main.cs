using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ConversionFPS
{
    class Main
    {
        public static Game1 Instance;
        public static GraphicsDeviceManager Graphics;
        public static Random Rand;

        public static int Width, Height;

        public Main(Game1 game, GraphicsDeviceManager graphics)
        {
            Instance = game;
            Graphics = graphics;
            Rand = new Random();

            Width = 800;
            Height = 600;
        }

        public void Initialize()
        {
            Graphics.PreferredBackBufferWidth = Width;
            Graphics.PreferredBackBufferHeight = Height;
            Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.ApplyChanges();
        }

        public void Update(GameTime time)
        {
            Input.Update();
        }

        public void Draw()
        {
            Graphics.GraphicsDevice.Clear(Color.Black);
        }
    }
}
