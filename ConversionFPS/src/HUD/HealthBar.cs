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
    class OnPlayerHitEvent : GameEvent { public int Damages; }

    class HealthBar
    {
        int _health;
        float lastTimeHit;

        // Drawing
        Sprite spriteBackground, spriteForeground;
        Vector2 position, greenBarWidth, textPos;
        string text;

        public HealthBar()
        {
            _health = 100;
            lastTimeHit = 999;
            position = new Vector2(50, 50); // Upper left point

            spriteBackground = new Sprite("HUD/HealthBar_Background", position);
            spriteForeground = new Sprite("HUD/HealthBar_Foreground", position);

            textPos = new Vector2(position.X + 100, position.Y + 8);    // Position of the text inside the bar
            text = "" + _health;

            EventManager.Instance.AddListener<OnPlayerHitEvent>(HandleOnPlayerHitEvent);
        }

        public void Update(GameTime gameTime)
        {
            if (_health <= 0)
                EventManager.Instance.Raise(new OnGameOverEvent());

            greenBarWidth = new Vector2(GetWidthFromHealth(), 50);
            text = "" + _health;
        }

        public void Draw()
        {
            spriteBackground.Draw();
            spriteForeground.Draw(new Rectangle(0, 0, (int)greenBarWidth.X, (int)greenBarWidth.Y));
            Main.Batch.DrawString(HUD.FontTiny, text, textPos, Color.White);
            Main.Batch.DrawString(HUD.FontTiny, "%", new Vector2(textPos.X + 40, textPos.Y), Color.White);
        }

        void HandleOnPlayerHitEvent(OnPlayerHitEvent e)
        {
            if (lastTimeHit - Timer.TimeF >= 1)
            {
                _health -= e.Damages;
                lastTimeHit = Timer.TimeF;
            }
        }

        private int GetWidthFromHealth()
        {
            return (5 + (int)(240f * _health / 100f));
        }
    }
}
