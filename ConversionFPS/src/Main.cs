using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ConversionFPS
{
    enum GameState { Playing, Transition, Win, GameOver };

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
        public static Maze Maze;

        ConversionManager conversionManager;
        HUD hud;
        BasicEffect effect;

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

            GameState = GameState.Playing;

            conversionManager = new ConversionManager();
            effect = new BasicEffect(Device);

            SoundManager.AddEffect("Win", "YouWin");
            SoundManager.AddEffect("GameOver", "YouLose");

            EventManager.Instance.AddListener<OnGameOverEvent>(HandleGameOverEvent);
            EventManager.Instance.AddListener<OnLevelEndEvent>(HandleOnLevelEndEvent);

            LoadLevel(1);
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

        void LoadLevel(int level)
        {
            LevelNumber = level;
            MazeBuilder.Initialize(LevelNumber);
            Camera = new Camera(new Vector3(1.2f, 0.8f, 1.2f), Vector3.Zero, 5f);
            Maze = new Maze();
            hud = new HUD();
            GameState = GameState.Transition;
        }

        public void Update(GameTime gameTime)
        {
            if (Instance.IsActive)
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
                    Maze.Update(gameTime);

                    /*if (Input.KeyPressed(Keys.E, true) && !Convertible.IsConversionOn)
                    {
                        EventManager.Instance.Raise(new OnConversionStartEvent() { convertible = enemy1 });
                        conversionManager.Initialize(enemy1);
                    }
                    else if (Input.KeyPressed(Keys.Escape, true) && Convertible.IsConversionOn)
                        EventManager.Instance.Raise(new OnConversionStopEvent() { convertible = enemy1 });
                    else */
                    if (Input.KeyPressed(Keys.Escape, true))
                        Instance.Exit();

                    // Move only if the player is not currently converting
                    if (!Convertible.IsConversionOn)
                        Camera.Update(gameTime);
                    else
                        conversionManager.Update(gameTime);

                    if (GameState == GameState.Transition)
                        GameState = GameState.Playing;
                }
            }
        }

        public void Draw()
        {
            if (GameState != GameState.Transition)
            {
                if (GameState == GameState.GameOver)
                {
                    Device.Clear(Color.Black);
                    Batch.Begin();
                    Batch.DrawString(HUD.Font, "YOU LOSE.", Center - (HUD.Font.MeasureString("YOU LOSE") / 2), Color.DarkRed);
                    Batch.End();
                }
                else if (GameState == GameState.Win)
                {
                    Device.Clear(Color.Black);
                    Batch.Begin();
                    Batch.DrawString(HUD.Font, "YOU WIN !", Center - (HUD.Font.MeasureString("YOU WIN !") / 2), Color.White);
                    Batch.End();
                }
                else
                {
                    Device.Clear(Color.DeepSkyBlue);
                    Maze.Draw(Camera, effect);
                    hud.Draw();
                    if (Convertible.IsConversionOn)
                        conversionManager.Draw();
                }
            }
        }

        void HandleGameOverEvent(OnGameOverEvent e)
        {
            GameState = GameState.GameOver;
            SoundManager.Play("GameOver");
        }

        void HandleOnLevelEndEvent(OnLevelEndEvent e)
        {
            if (LevelNumber == 1)
                LoadLevel(2);
            else
            {
                GameState = GameState.Win;
                SoundManager.Play("Win");
            }
        }
    }
}
