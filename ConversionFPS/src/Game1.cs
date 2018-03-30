using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConversionFPS
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Main main;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            main = new Main(this, graphics);
            main.Initialize();
            base.Initialize();
        }
        
        protected override void LoadContent()
        { }
        
        protected override void UnloadContent()
        { }
        
        protected override void Update(GameTime gameTime)
        {
            main.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            main.Draw();
            base.Draw(gameTime);
        }
    }
}
