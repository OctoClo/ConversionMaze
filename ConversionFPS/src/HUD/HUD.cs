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
    class HUD
    {
        public static SpriteFont Font, FontTiny;

        HealthBar healthBar;
        Timer timer;
        Minimap minimap;
        
        public HUD()
        {
            Font = Main.Instance.Content.Load<SpriteFont>("HUD/atwriter");
            FontTiny = Main.Instance.Content.Load<SpriteFont>("HUD/atwriterTiny");

            healthBar = new HealthBar();
            timer = new Timer();
            minimap = new Minimap();
        }

        public void Update(GameTime gameTime)
        {
            healthBar.Update(gameTime);
            timer.Update(gameTime);
            minimap.Update(gameTime);
        }

        public void Draw()
        {
            Main.Batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp); // Prevent anti-aliasing in minimap zoom

            healthBar.Draw();
            timer.Draw();
            minimap.Draw();

            Main.Batch.End();
        }
    }
}
