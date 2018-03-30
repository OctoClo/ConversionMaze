using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ConversionFPS
{
    enum GameState { Level1, Level2, Win, GameOver };

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

        public static Vector2 Center;
        float speed, rotationSpeed;
        HUD hud;
        ConversionManager conversionManager;

        Enemy enemy1, enemy2;
        Door door;

        public Main(Game1 game, GraphicsDeviceManager graphics)
        {
            Instance = game;
            Graphics = graphics;
            Device = Graphics.GraphicsDevice;
            Batch = new SpriteBatch(Device);
            Rand = new Random();

            Height = 800;
            Width = 1280;
            Center = new Vector2(Width / 2, Height / 2);

            LevelNumber = 1;
            PlayerPosition = Vector3.Zero;
            Rotation = 0;

            GameState = GameState.Level1;

            speed = 2f;
            rotationSpeed = 3f;
            hud = new HUD();
            conversionManager = new ConversionManager();

            enemy1 = new Enemy(new Vector3(100, 100, 0));
            enemy2 = new Enemy(new Vector3(100, 200, 0));
            door = new Door(new Vector3(100, 300, 0));
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
            if (GameState != GameState.GameOver && GameState != GameState.Win)
            {
                Input.Update();
                hud.Update(gameTime);

                if (Input.KeyPressed(Keys.E, true) && !Convertible.IsConversionOn)
                {
                    EventManager.Instance.Raise(new OnConversionStartEvent() { convertible = enemy1 });
                    conversionManager.Initialize(enemy1);
                }
                else if (Input.KeyPressed(Keys.Escape, true) && Convertible.IsConversionOn)
                    EventManager.Instance.Raise(new OnConversionStopEvent() { convertible = enemy1 });
                else if (Input.KeyPressed(Keys.Escape, true))
                    Instance.Exit();

                // Move only if the player is not currently converting
                if (!Convertible.IsConversionOn)
                {
                    // Lock mouse position at center of screen
                    if (Instance.IsActive)
                    {
                        if (Input.MousePos.X != Center.X)
                            Rotation += (Input.MousePos.X - Center.X) / rotationSpeed;

                        Mouse.SetPosition((int)Center.X, (int)Center.Y);
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
                else
                    conversionManager.Update(gameTime);
            }
        }

        public void Draw()
        {
            Device.Clear(Color.Black);
            Batch.Begin();

            if (GameState == GameState.GameOver)
                Batch.DrawString(HUD.Font, "YOU LOSE.", Center - (HUD.Font.MeasureString("YOU LOSE") / 2), Color.DarkRed);
            else if (GameState == GameState.Win)
                Batch.DrawString(HUD.Font, "YOU WIN !", Center - (HUD.Font.MeasureString("YOU WIN !") / 2), Color.White);
            else
            {
                hud.Draw();
                enemy1.Draw();
                if (Convertible.IsConversionOn)
                    conversionManager.Draw();
                //enemy2.Draw();
                //door.Draw();
            }
            
            Batch.End();
        }
    }
}
