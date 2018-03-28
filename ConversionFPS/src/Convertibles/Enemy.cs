using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ConversionFPS
{
    class Enemy : Convertible
    {
        static int NbKills;

        int speed, damages;

        public Enemy(Vector3 pos) : base(pos)
        {
            Initialize();
        }

        void Initialize()
        {
            if (NbKills == 0)
                level = 1;
            else if (NbKills >= 1 && NbKills <= 3)
                level = 2;
            else
                level = 3;

            speed = level;
            damages = level * 5;

            GenerateConversion();
        }

        protected override void GenerateConversion()
        {
            int start = Main.Rand.Next((int)Base.Hexadecimal);

            if (level == 1)
            {
                startBase = Base.Hexadecimal;
                endBase = Base.Decimal;
            }
            else if (level == 2)
            {
                startBase = Base.Decimal;
                endBase = Base.Binary;
            }
            else
            {
                startBase = Base.Hexadecimal;
                endBase = Base.Binary;
            }

            startValue = Convert.ToString(start, (int)startBase);
            endValue = Convert.ToString(start, (int)endBase);
        }

        protected override void HandleSuccess()
        {
            NbKills++;
            Respawn();
        }

        void Respawn()
        {
            Initialize();
            GenerateConversion();
        }
    }
}
