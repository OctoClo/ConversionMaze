using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConversionFPS
{
    class OnConversionStartEvent : GameEvent { public Convertible convertible; }

    class OnConversionStopEvent : GameEvent { public Convertible convertible; }

    enum Base { Binary = 2, Decimal = 10, Hexadecimal = 16 };

    

    abstract class Convertible : Cube
    {
        public static bool IsConversionOn = false;
        public bool DisplayConv;

        protected int level;

        protected string startValue, endValue;
        protected Base startBase, endBase;

        public Convertible(string texturePath, Vector3 pos, TileType type) : base(texturePath, pos, type)
        {
            EventManager.Instance.AddListener<OnConversionStartEvent>(HandleConversionStartEvent);
            EventManager.Instance.AddListener<OnConversionStopEvent>(HandleConversionStopEvent);

            DisplayConv = false;

        }

        protected abstract void GenerateConversion();

        public bool CheckSuccess(string number)
        {
            if (number.Equals(endValue))
            {
                HandleSuccess();
                return true;
            }
            return false;
        }

        protected abstract void HandleSuccess();

        public virtual void Update(GameTime gameTime)
        { }

        public override void Draw(Camera camera, BasicEffect effect, float scale = 1)
        {

            Main.Device.BlendState = BlendState.Opaque;
            Main.Device.DepthStencilState = DepthStencilState.Default;
            base.Draw(camera, effect, scale);

            if (DisplayConv)
            {
                Main.Batch.Begin();
                Main.Batch.Draw(Main.TextureRect, new Rectangle(Main.Width/2 - 320, Main.Height/2 - 55, 750, 40), Color.White);
                string displayDebug = "Start (" + (int)startBase + ") : " + startValue + " - End (" + (int)endBase + ") : " + endValue; 
                string display = "Convert " + startValue + " from base " + (int)startBase + " to base " + (int)endBase + "   (psst : " + endValue + ")" ;
                Main.Batch.DrawString(HUD.FontTiny, display, new Vector2(Main.Center.X - (HUD.Font.MeasureString("YOU WIN !ZZZZZZ").X / 2),
                                                                    Main.Center.Y - 50), Color.Black);
            Main.Batch.End();
            }
        }

        protected void HandleConversionStartEvent(OnConversionStartEvent e)
        {
            if (e.convertible == this)
            {
                IsConversionOn = true;
            }
        }

        protected void HandleConversionStopEvent(OnConversionStopEvent e)
        {
            if (e.convertible == this)
            {
                IsConversionOn = false;
            }
        }
    }
}
