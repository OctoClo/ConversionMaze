using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ConversionFPS
{
    enum GameState { Playing, Win, GameOver };

    class Main
    {
        public static Game1 Instance;
        public static GraphicsDeviceManager Graphics;
        public static GraphicsDevice Device;
        public static SpriteBatch Batch;
        public static Random Rand;

        public static int Height, Width;

        public static int LevelNumber;
        public static Vector3 PlayerPosition;
        public static float Rotation;

        public static GameState GameState;

        Vector2 center;
        float speed, rotationSpeed;
        HUD hud;

        public Main(Game1 game, GraphicsDeviceManager graphics)
        {
            Instance = game;
            Graphics = graphics;
            Device = Graphics.GraphicsDevice;
            Batch = new SpriteBatch(Device);
            Rand = new Random();

            Height = 800;
            Width = 1280;
            center = new Vector2(Width / 2, Height / 2);

            LevelNumber = 1;
            PlayerPosition = Vector3.Zero;
            Rotation = 0;

            GameState = GameState.Playing;

            speed = 2f;
            rotationSpeed = 3f;
            hud = new HUD();
        }

        public void Initialize()
        {
            Graphics.PreferredBackBufferHeight = Height;
            Graphics.PreferredBackBufferWidth = Width;
            Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.ApplyChanges();

            Instance.Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (Width / 2),
                                                 (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (Height / 2) - 40);
        }

        public void Update(GameTime gameTime)
        {
            if (GameState != GameState.Playing)
                Instance.Exit();

            Input.Update();
            hud.Update(gameTime);

            //Lock or unlock mouse movement
            if (Instance.IsActive)
            {
                if (Input.MousePos.X != center.X)
                    Rotation += (Input.MousePos.X - center.X) / rotationSpeed;

                Mouse.SetPosition((int)center.X, (int)center.Y);
                Instance.IsMouseVisible = false;
            }

            if (Input.KeyPressed(Keys.Z, false))
            {
                PlayerPosition.X += speed * (float)Math.Sin(MathHelper.ToRadians(Rotation));
                PlayerPosition.Y -= speed * (float)Math.Cos(MathHelper.ToRadians(Rotation));
            }
            if (Input.KeyPressed(Keys.S, false))
            {
                PlayerPosition.X += speed * (float)Math.Sin(MathHelper.ToRadians(Rotation + 180));
                PlayerPosition.Y -= speed * (float)Math.Cos(MathHelper.ToRadians(Rotation + 180));
            }
            if (Input.KeyPressed(Keys.Q, false))
            {
                PlayerPosition.X += speed * (float)Math.Sin(MathHelper.ToRadians(Rotation - 90));
                PlayerPosition.Y -= speed * (float)Math.Cos(MathHelper.ToRadians(Rotation - 90));
            }
            if (Input.KeyPressed(Keys.D, false))
            {
                PlayerPosition.X += speed * (float)Math.Sin(MathHelper.ToRadians(Rotation + 90));
                PlayerPosition.Y -= speed * (float)Math.Cos(MathHelper.ToRadians(Rotation + 90));
            }

            PlayerPosition.X = MathHelper.Clamp(PlayerPosition.X, 0f, 780f);
            PlayerPosition.Y = MathHelper.Clamp(PlayerPosition.Y, 0f, 580f);
        }

        public void Draw()
        {
            Device.Clear(Color.Black);
            Batch.Begin();
            hud.Draw();
            Batch.End();
        }
    }
}
