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
                if (Position != null)
                {
                    Vector2 position = (Vector2)Position;
                    return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
                }
                else
                    return new Rectangle();
            }
        }

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }

        Texture2D Texture;
        Vector2? Position;

        public Sprite(string textureName, Vector2? position = null)
        {
            Texture = Main.Instance.Content.Load<Texture2D>(textureName); ;
            Position = position;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw()
        {
            Main.Batch.Draw(Texture, (Vector2)Position, Color.White);
        }

        public virtual void Draw(Rectangle destRect)
        {
            if (Position != null)
                Main.Batch.Draw(Texture, (Vector2)Position, destRect, Color.White);
            else
                Main.Batch.Draw(Texture, destRect, Color.White);
        }

        public virtual void Draw(Rectangle destRect, Rectangle sourceRect)
        {
            Main.Batch.Draw(Texture, destRect, sourceRect, Color.White);
        }

        public virtual void Draw(Rectangle destRect, Rectangle? sourceRect, float rotation, Vector2 origin)
        {
            Main.Batch.Draw(Texture, destRect, sourceRect, Color.White, rotation, origin, SpriteEffects.None, 0f);
        }
    }
}
