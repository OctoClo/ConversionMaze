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
        public static Vector2 Size;
        Vector2 position;

        Sprite spriteContainer, spriteMinimap, spriteCursor;

        Rectangle containerZone; // Zone where the container is drawn
        Rectangle minimapDisplayZone; // Zone where the map is drawn
        Rectangle minimapDisplayPart; // Rectangle part of the map displayed
        
        int cursorSize;
        float cursorRot;
        Rectangle cursorRect;

        public Minimap()
        {
            Size = new Vector2(400, 300);
            position = new Vector2(820, 50);

            spriteContainer = new Sprite("HUD/MinimapContainer");
            spriteMinimap = new Sprite("HUD/MinimapTest");
            spriteCursor = new Sprite("HUD/PlayerCursor");

            containerZone = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);
            minimapDisplayZone = new Rectangle((int)position.X + 10, (int)position.Y + 10, (int)Size.X - 20, (int)Size.Y - 20);
            UpdateMinimapDisplayPart();

            cursorSize = 40;
            cursorRot = 0f;
            cursorRect = new Rectangle((int)(position.X + Size.X / 2), (int)(position.Y + Size.Y / 2), cursorSize, cursorSize);
        }

        public void Update(GameTime gameTime)
        {
            cursorRot = Main.Camera.Rotation.Y + MathHelper.PiOver2;
            UpdateMinimapDisplayPart();
        }

        void UpdateMinimapDisplayPart()
        {
            minimapDisplayPart = new Rectangle((int)(Main.Camera.Position.Z - (spriteMinimap.Width / 4)),
                                            (int)(Main.Camera.Position.X - (spriteMinimap.Height / 4)),
                                            spriteMinimap.Width / 2, spriteMinimap.Height / 2);
        }

        public void Draw()
        {
            spriteContainer.Draw(containerZone);
            spriteMinimap.Draw(minimapDisplayZone, minimapDisplayPart);
            spriteCursor.Draw(cursorRect, null, cursorRot, new Vector2((spriteCursor.Width / 2f), (spriteCursor.Width / 2f)));
        }
    }
}
