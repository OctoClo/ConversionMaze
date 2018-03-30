using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ConversionFPS
{
    class OnConversionStartEvent : GameEvent { public Convertible convertible; }

    class OnConversionStopEvent : GameEvent { public Convertible convertible; }

    enum Base { Binary = 2, Decimal = 10, Hexadecimal = 16 };

    abstract class Convertible
    {
        public static bool IsConversionOn = false;

        protected int level;
        protected Vector3 position;

        protected string startValue, endValue;
        protected Base startBase, endBase;

        public Convertible(Vector3 pos)
        {
            position = pos;
            EventManager.Instance.AddListener<OnConversionStartEvent>(HandleConversionStartEvent);
            EventManager.Instance.AddListener<OnConversionStopEvent>(HandleConversionStopEvent);
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

        public void Draw()
        {
            Vector2 pos = new Vector2(position.X, position.Y);
            string display = "Start (" + (int)startBase + ") : " + startValue + " - End (" + (int)endBase + ") : " + endValue; 
            Main.Batch.DrawString(HUD.FontTiny, display, pos, Color.White);
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
