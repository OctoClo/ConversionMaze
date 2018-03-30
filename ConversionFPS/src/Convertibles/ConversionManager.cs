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
    class ConversionManager
    {
        Convertible convertible;
        string number;
        bool firstInput;

        public void Initialize(Convertible convertible)
        {
            this.convertible = convertible;
            number = "";
            firstInput = true;
        }

        public void Update(GameTime gameTime)
        {
            if (firstInput)
            {
                firstInput = false;
                return;
            }

            if (Input.KeyPressed(Keys.Enter, true))
            {
                convertible.CheckSuccess(number);
                EventManager.Instance.Raise(new OnConversionStopEvent() { convertible = convertible });
            }
            else
            {
                Keys[] keys = Input.KeysPressed();

                if (keys.Length > 0)
                {
                    Keys key = keys[0];
                    
                    if (Input.KeyPressed(key, true))
                    {
                        if (key >= Keys.A && key <= Keys.F)
                            number += keys[0].ToString();
                        else if ((key >= Keys.NumPad0 && key <= Keys.NumPad9) || (key >= Keys.D0 && key <= Keys.D9))
                            number += GetNumberFromKey(key);
                    }
                }
            }
        }

        public void Draw()
        {
            Main.Batch.DrawString(HUD.Font, number, Main.Center - (HUD.Font.MeasureString(number) / 2), Color.White);
        }

        string GetNumberFromKey(Keys key)
        {
            string number = "";
            switch (key)
            {
                case Keys.NumPad0:
                case Keys.D0:
                    number = "0";
                    break;

                case Keys.NumPad1:
                case Keys.D1:
                    number = "1";
                    break;

                case Keys.NumPad2:
                case Keys.D2:
                    number = "2";
                    break;

                case Keys.NumPad3:
                case Keys.D3:
                    number = "3";
                    break;

                case Keys.NumPad4:
                case Keys.D4:
                    number = "4";
                    break;

                case Keys.NumPad5:
                case Keys.D5:
                    number = "5";
                    break;

                case Keys.NumPad6:
                case Keys.D6:
                    number = "6";
                    break;

                case Keys.NumPad7:
                case Keys.D7:
                    number = "7";
                    break;

                case Keys.NumPad8:
                case Keys.D8:
                    number = "8";
                    break;

                case Keys.NumPad9:
                case Keys.D9:
                    number = "9";
                    break;
            }
            return number;
        }
    }
}
