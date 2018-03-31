using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ConversionFPS
{
    enum GameState { Level1, Level2, Win, GameOver };

    class OnGameOverEvent : GameEvent { }

    class Main
    {
        public static Game1 Instance;
        public static GraphicsDeviceManager Graphics;
        public static GraphicsDevice Device;
        public static SpriteBatch Batch;
        public static Random Rand;

        public static int Height, Width;
        public static Vector2 Center;

        public static GameState GameState;

        public static int LevelNumber;
        public static Vector2 PlayerPosition;
        public static float Rotation;

        float speed, rotationSpeed;

        ConversionManager conversionManager;

        Camera camera;
        HUD hud;

        BasicEffect effect;
        Maze maze;

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

            GameState = GameState.Level1;

            LevelNumber = 1;
            PlayerPosition = Vector2.Zero;
            Rotation = 0;

            speed = 0.1f;
            rotationSpeed = 0.05f;

            conversionManager = new ConversionManager();
            EventManager.Instance.AddListener<OnGameOverEvent>(HandleGameOverEvent);

            SoundManager.AddEffect("Win", "YouWin");
            SoundManager.AddEffect("GameOver", "YouLose");

            camera = new Camera(new Vector3(0, 0.1f, 0), 0, Device.Viewport.AspectRatio, 0.05f, 100f);
            hud = new HUD();

            effect = new BasicEffect(Device);
            maze = new Maze();

            enemy1 = new Enemy(new Vector3(3, 0, 3));
            enemy2 = new Enemy(new Vector3(4, 0, 4));
            door = new Door(new Vector3(10, 0, 10));            
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
            Input.Update();

            if (GameState == GameState.GameOver || GameState == GameState.Win)
            {
                if (Input.KeyPressed(Keys.Escape, true))
                    Instance.Exit();
            }
            else
            {
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
                            Rotation += (Input.MousePos.X - Center.X) * gameTime.ElapsedGameTime.Milliseconds * rotationSpeed;

                        Mouse.SetPosition((int)Center.X, (int)Center.Y);
                        Instance.IsMouseVisible = false;
                    }

                    if (Input.KeyPressed(Keys.Z, false))
                    {
                        PlayerPosition.X += (float)Math.Sin(MathHelper.ToRadians(Rotation)) * gameTime.ElapsedGameTime.Milliseconds * speed;
                        PlayerPosition.Y -= (float)Math.Cos(MathHelper.ToRadians(Rotation)) * gameTime.ElapsedGameTime.Milliseconds * speed;
                    }
                    if (Input.KeyPressed(Keys.S, false))
                    {
                        PlayerPosition.X += (float)Math.Sin(MathHelper.ToRadians(Rotation + 180)) * gameTime.ElapsedGameTime.Milliseconds * speed;
                        PlayerPosition.Y -= (float)Math.Cos(MathHelper.ToRadians(Rotation + 180)) * gameTime.ElapsedGameTime.Milliseconds * speed;
                    }
                    if (Input.KeyPressed(Keys.Q, false))
                    {
                        PlayerPosition.X += (float)Math.Sin(MathHelper.ToRadians(Rotation - 90)) * gameTime.ElapsedGameTime.Milliseconds * speed;
                        PlayerPosition.Y -= (float)Math.Cos(MathHelper.ToRadians(Rotation - 90)) * gameTime.ElapsedGameTime.Milliseconds * speed;
                    }
                    if (Input.KeyPressed(Keys.D, false))
                    {
                        PlayerPosition.X += (float)Math.Sin(MathHelper.ToRadians(Rotation + 90)) * gameTime.ElapsedGameTime.Milliseconds * speed;
                        PlayerPosition.Y -= (float)Math.Cos(MathHelper.ToRadians(Rotation + 90)) * gameTime.ElapsedGameTime.Milliseconds * speed;
                    }

                    PlayerPosition = new Vector2(MathHelper.Clamp(PlayerPosition.X, 0f, 780f), MathHelper.Clamp(PlayerPosition.Y, 0f, 580f));
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
                maze.Draw(camera, effect);
                enemy1.Draw(camera, effect);
                //enemy2.Draw();
                //door.Draw();
                hud.Draw();
                if (Convertible.IsConversionOn)
                    conversionManager.Draw();
            }
            
            Batch.End();
        }

        void HandleGameOverEvent(OnGameOverEvent e)
        {
            GameState = GameState.GameOver;
            SoundManager.Play("GameOver");
        }
    }
}
