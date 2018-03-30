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
    class Minimap
    {
        Vector2 size, position;

        Sprite spriteContainer, spriteMinimap, spriteCursor;

        Rectangle containerZone; // Zone where the container is drawn
        Rectangle minimapDisplayZone; // Zone where the map is drawn
        Rectangle minimapDisplayPart; // Rectangle part of the map displayed
        
        int cursorSize;
        float cursorRot;
        Rectangle cursorRect;

        public Minimap()
        {
            size = new Vector2(400, 300);
            position = new Vector2(820, 50);

            spriteContainer = new Sprite("HUD/MinimapContainer");
            spriteMinimap = new Sprite(Main.map1.getTexture());
            spriteCursor = new Sprite("HUD/PlayerCursor");

            containerZone = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            minimapDisplayZone = new Rectangle((int)position.X + 10, (int)position.Y + 10, (int)size.X - 20, (int)size.Y - 20);
            UpdateMinimapDisplayPart();

            cursorSize = 40;
            cursorRot = 0f;
            cursorRect = new Rectangle((int)(position.X + size.X / 2), (int)(position.Y + size.Y / 2), cursorSize, cursorSize);
        }

        public void Update(GameTime gameTime)
        {
            cursorRot = (float)((2f * Math.PI * (Main.Rotation % 360) / 360f));
            UpdateMinimapDisplayPart();
        }

        void UpdateMinimapDisplayPart()
        {
            Vector2 location = new Vector2();
            Vector2 size = new Vector2(spriteMinimap.Width / 2f - 20, minimapDisplayZone.Height * spriteMinimap.Height / ( minimapDisplayZone.Width * 2f));
            location.X = -(size.X / 2 - 10) + Main.PlayerPosition.X * ((size.X * 2f + 20f) / (Main.MaxPlayerPosition.X));
            location.Y = -(size.Y / 2 - 10) + Main.PlayerPosition.Y * ((size.X * 2f + 20f) / (Main.MaxPlayerPosition.Y));

            minimapDisplayPart = new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y);
        }

        public void Draw()
        {
            spriteContainer.Draw(containerZone);
            spriteMinimap.Draw(minimapDisplayZone, minimapDisplayPart);
            spriteCursor.Draw(cursorRect, null, cursorRot, new Vector2((spriteCursor.Width / 2f), (spriteCursor.Width / 2f)));
        }
    }
}
