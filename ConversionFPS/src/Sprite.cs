using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConversionFPS
{
    public class Sprite
    {
        public Rectangle Hitbox
        {
            get
            {
                if (position != null)
                {
                    Vector2 positionV = (Vector2)position;
                    return new Rectangle((int)positionV.X, (int)positionV.Y, texture.Width, texture.Height);
                }
                else
                    return new Rectangle();
            }
        }

        public int Width
        {
            get { return texture.Width; }
        }

        public int Height
        {
            get { return texture.Height; }
        }

        Texture2D texture;
        Vector2? position;

        public Sprite(string textureName, Vector2? position = null)
        {
            texture = Main.Instance.Content.Load<Texture2D>(textureName); ;
            this.position = position;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw()
        {
            Main.Batch.Draw(texture, (Vector2)position, Color.White);
        }

        public virtual void Draw(Rectangle destRect)
        {
            if (position != null)
                Main.Batch.Draw(texture, (Vector2)position, destRect, Color.White);
            else
                Main.Batch.Draw(texture, destRect, Color.White);
        }

        public virtual void Draw(Rectangle destRect, Rectangle sourceRect)
        {
            Main.Batch.Draw(texture, destRect, sourceRect, Color.White);
        }

        public virtual void Draw(Rectangle destRect, Rectangle? sourceRect, float rotation, Vector2 origin)
        {
            Main.Batch.Draw(texture, destRect, sourceRect, Color.White, rotation, origin, SpriteEffects.None, 0f);
        }
    }
}
