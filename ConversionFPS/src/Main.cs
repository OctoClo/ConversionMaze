using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

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
        
        Convertible closestConvertible; // Convertible targeted by the conversion 

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
            MazeBuilder.Initialize(1);

            SoundManager.AddEffect("Win", "YouWin");
            SoundManager.AddEffect("GameOver", "YouLose");

            Camera = new Camera(new Vector3(1.2f, 0.8f, 1.2f), Vector3.Zero, 5f);
            hud = new HUD();

            effect = new BasicEffect(Device);
            maze = new Maze();
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
                    closestConvertible = null;
                    double minDist = 9999 ;

                    // Find the closest Convertibles in the front of the camera 
                    foreach (Convertible c in maze.convertibleElements)
                    {

                        if ( Maze.IsInFront(c, Camera) )
                        {
                            // Check if the distance is less than the previous minimum
                            double distance = Math.Sqrt((double)(Math.Pow(c.position.X - Camera.Position.X, 2) + Math.Pow(c.position.Y - Camera.Position.Z, 2)));
                            if (distance < minDist)
                            {
                                // Check if said convertible is visible by the camera
                                if (Maze.IsPathClear(c, Camera, maze))
                                {
                                    minDist = distance;
                                    closestConvertible = c;
                                }
                            }
                        }
                    }

                    if (closestConvertible != null) Debug.Print("Closest : " + closestConvertible.position.X + ";" + closestConvertible.position.Y);
                    else Debug.Print("null");
                    
                    if (closestConvertible != null)
                    {
                        EventManager.Instance.Raise(new OnConversionStartEvent() { convertible = closestConvertible });
                        conversionManager.Initialize(closestConvertible);
                    }
                }
                else if (Input.KeyPressed(Keys.Escape, true) && Convertible.IsConversionOn)
                    EventManager.Instance.Raise(new OnConversionStopEvent() { convertible = closestConvertible });
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
                maze.Draw(Camera, effect);
                foreach(Convertible c in maze.convertibleElements)
                    c.Draw(Camera, effect);
                hud.Draw();
                if (Convertible.IsConversionOn)
                    conversionManager.Draw();
            }
        }

        void HandleGameOverEvent(OnGameOverEvent e)
        {
            GameState = GameState.GameOver;
            SoundManager.Play("GameOver");
        }
    }
}
