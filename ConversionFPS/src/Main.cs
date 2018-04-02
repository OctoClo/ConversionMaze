using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

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
        public static Texture2D TextureRect;

        public static int Height, Width;
        public static Vector2 Center;

        public static GameState GameState;
        public static int LevelNumber;

        public static Camera Camera;
        public static Maze Maze;

        ConversionManager conversionManager;
        HUD hud;
        BasicEffect effect;
        
        Convertible closestConvertible; // Convertible targeted by the conversion
		
        public Main(Game1 game, GraphicsDeviceManager graphics)
        {
            Instance = game;
            Graphics = graphics;
            Device = Graphics.GraphicsDevice;
            Batch = new SpriteBatch(Device);
            Rand = new Random();
            TextureRect = new Texture2D(Main.Device, 1, 1);
            TextureRect.SetData<Color>(new Color[1] { Color.LightGray });

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

                    if (Input.KeyPressed(Keys.E, true) && !Convertible.IsConversionOn)
                    {
                        closestConvertible = null;
                        double minDist = 9999;

                        // Find the closest Convertibles in the front of the camera 
                        foreach (Convertible c in Maze.Convertibles)
                        {

                            if (Maze.IsInFront(c, Camera))
                            {
                                // Check if the distance is less than the previous minimum
                                double distance = Math.Sqrt((double)(Math.Pow(c.Position.X - Camera.Position.X, 2) + Math.Pow(c.Position.Z - Camera.Position.Z, 2)));
                                if (distance < minDist)
                                {
                                    // Check if said convertible is visible by the camera
                                    if (Maze.IsPathClear(c, Camera, Maze))
                                    {
                                        minDist = distance;
                                        closestConvertible = c;
                                    }
                                }
                            }
                        }

                        if (closestConvertible != null)
                        {
                            Debug.Print("Closest : " + closestConvertible.Position.X + ";" + closestConvertible.Position.Y);
                            closestConvertible.DisplayConv = true;
                        }
                        else Debug.Print("nothing found");

                        if (closestConvertible != null)
                        {
                            EventManager.Instance.Raise(new OnConversionStartEvent() { convertible = closestConvertible });
                            conversionManager.Initialize(closestConvertible);
                        }
                    }
                    else if (Input.KeyPressed(Keys.Escape, true) && Convertible.IsConversionOn)
                    {
                        EventManager.Instance.Raise(new OnConversionStopEvent() { convertible = closestConvertible });
                        closestConvertible.DisplayConv = false;
                    }
                    else if (Input.KeyPressed(Keys.Escape, true))
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
