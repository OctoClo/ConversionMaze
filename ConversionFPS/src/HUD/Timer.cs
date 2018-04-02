using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConversionFPS
{
    class Timer
    {
        public static float TimeF;
        int time, minutes, seconds;
        string timer, level;

        Vector2 timePosition, levelPosition;

        int slowDown;

        public Timer()
        {
            TimeF = 180f;
            slowDown = 1;
            timePosition = new Vector2((Main.Width / 2) - (HUD.Font.MeasureString("2:22").X / 2), 50);
            levelPosition = new Vector2((Main.Width / 2) - (HUD.FontTiny.MeasureString("level 2").X / 2), 15);

            level = "level " + Main.LevelNumber;
            timer = minutes + ":" + seconds;

            EventManager.Instance.AddListener<OnConversionStartEvent>(HandleConversionStartEvent);
            EventManager.Instance.AddListener<OnConversionStopEvent>(HandleConversionStopEvent);
        }

        public void Update(GameTime gameTime)
        {
            TimeF -= (gameTime.ElapsedGameTime.Milliseconds / 1000f) / slowDown;
            if (TimeF < 0)
                EventManager.Instance.Raise(new OnGameOverEvent());

            time = MathHelper.Clamp((int)(Math.Round(TimeF) + .5f), 0, 999);
            minutes = time / 60;
            seconds = time % 60;

            level = "level " + Main.LevelNumber;

            if (seconds < 10)
                timer = minutes + ":0" + seconds;
            else
                timer = minutes + ":" + seconds;
        }

        public void Draw()
        {
            Main.Batch.Draw(Main.TextureRect, new Rectangle((int)levelPosition.X - 20, (int)levelPosition.Y - 5, 130, 115), Color.White);
            Main.Batch.DrawString(HUD.FontTiny, level, levelPosition, Color.Black);
            if ( time >= 10 )
                Main.Batch.DrawString(HUD.Font, timer, timePosition, Color.Black);
            else
                Main.Batch.DrawString(HUD.Font, timer, timePosition, Color.DarkRed);
        }

        protected void HandleConversionStartEvent(OnConversionStartEvent e)
        {
            slowDown = 2;
        }

        protected void HandleConversionStopEvent(OnConversionStopEvent e)
        {
            slowDown = 1;
        }
    }
}
