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
        float timeF;
        int time, minutes, seconds;
        string timer, level;

        Vector2 timePosition, levelPosition;

        int slowDown;

        public Timer()
        {
            timeF = 180f;
            slowDown = 1;
            timePosition = new Vector2((Main.Width / 2) - (HUD.Font.MeasureString("2:22").X / 2), 50);
            levelPosition = new Vector2((Main.Width / 2) - (HUD.FontTiny.MeasureString("level 2").X / 2), 15);
            EventManager.Instance.AddListener<OnConversionStartEvent>(HandleConversionStartEvent);
            EventManager.Instance.AddListener<OnConversionStopEvent>(HandleConversionStopEvent);
        }

        public void Update(GameTime gameTime)
        {
            timeF -= (gameTime.ElapsedGameTime.Milliseconds / 1000f) / slowDown;
            if (timeF < 0)
                Main.GameState = GameState.GameOver;

            time = MathHelper.Clamp((int)(Math.Round(timeF) + .5f), 0, 999);
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
            Main.Batch.DrawString(HUD.FontTiny, level, levelPosition, Color.White);
            if ( time >= 10 )
                Main.Batch.DrawString(HUD.Font, timer, timePosition, Color.White);
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
