using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ConversionFPS
{
    enum TileType { Wall, Path, Begin, End, Door, Spawn };
    
    class MapElement
    {
        TileType tileType;

        public MapElement(TileType tileType)
        {
            this.tileType = tileType;
        }

        public Color getColor()
        {
            Color color = Color.Transparent;

            switch(tileType)
            {
                case TileType.Wall:
                    color = Color.White;
                    break;

                case TileType.Path:
                case TileType.Spawn:
                    color = Color.Transparent;
                    break;

                case TileType.Begin:
                    color = Color.Red;
                    break;

                case TileType.End:
                    color = Color.Green;
                    break;

                case TileType.Door:
                    color = Color.Gold;
                    break;
            }

            return color;
        }
    }
}
