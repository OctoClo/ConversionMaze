﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ConversionFPS
{
    class Door : Convertible
    {
        public bool IsOpen;

        public Door(Vector3 pos) : base("", pos, TileType.Door)
        {
            IsOpen = true;
        }

        protected override void Initialize(string texturePath)
        {
            base.Initialize("Conversion/Door");
            GenerateConversion();
        }

        protected override void GenerateConversion()
        {
            int start = Main.Rand.Next(16, 256);

            startBase = Base.Binary;
            endBase = Base.Hexadecimal;

            startValue = Convert.ToString(start, (int)startBase);
            endValue = Convert.ToString(start, (int)endBase);
        }

        protected override void HandleSuccess()
        {
            Open();
            EventManager.Instance.RemoveListener<OnConversionStartEvent>(HandleConversionStartEvent);
            EventManager.Instance.RemoveListener<OnConversionStopEvent>(HandleConversionStopEvent);
        }

        void Open()
        {
            IsOpen = true;
        }
    }
}
