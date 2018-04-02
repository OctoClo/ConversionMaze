using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConversionFPS
{
    class Enemy : Convertible
    {
        static int NbKills;

        int speed, damages;

        public Enemy(Vector3 pos) : base("", pos, TileType.Enemy)
        { }

        protected override void Initialize(string texturePath)
        {
            if (Main.LevelNumber == 1)
            {
                if (NbKills == 0)
                    level = 1;
                else if (NbKills >= 3)
                    level = 2;
            }
            else
            {
                if (NbKills == 0)
                    level = 2;
                else if (NbKills >= 3)
                    level = 3;
            }

            speed = level;
            damages = level * 5;

            switch (level)
            {
                case 1:
                    base.Initialize("Conversion/EnemyLevel1");
                    break;

                case 2:
                    base.Initialize("Conversion/EnemyLevel2");
                    break;

                case 3:
                    base.Initialize("Conversion/EnemyLevel3");
                    break;
            }

            GenerateConversion();
        }

        protected override void GenerateConversion()
        {
            int start = Main.Rand.Next(16, 256);

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
            Initialize("");
            GenerateConversion();
        }
    }
}
