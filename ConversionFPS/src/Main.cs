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

        public static Camera Camera;

        ConversionManager conversionManager;
        
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

            conversionManager = new ConversionManager();
            EventManager.Instance.AddListener<OnGameOverEvent>(HandleGameOverEvent);

            SoundManager.AddEffect("Win", "YouWin");
            SoundManager.AddEffect("GameOver", "YouLose");

            Camera = new Camera(new Vector3(0, 0.8f, 0), Vector3.Zero, 5f);
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
                    Camera.Update(gameTime);
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
                maze.Draw(Camera, effect);
                enemy1.Draw(Camera, effect);
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
