using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConversionFPS
{
    class Maze
    {
        public static int Width;
        public static int Height;

        GraphicsDevice device;
        VertexBuffer floorBuffer;

        public Maze()
        {
            device = Main.Device;
            Width = (int)Minimap.Size.X;
            Height = (int)Minimap.Size.Y;
            BuildFloorBuffer();
        }

        public void Draw(Camera camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = false;

            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = Matrix.Identity;
            
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(floorBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, floorBuffer.VertexCount / 3);
            }
        }

        void BuildFloorBuffer()
        {
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();

            Color[] colors = { Color.Black, Color.White };
            int colorCounter = 0;

            for (int x = 0; x < Width; x++)
            {
                colorCounter++;
                for (int z = 0; z < Height; z++)
                {
                    colorCounter++;
                    foreach (VertexPositionColor vertex in FloorTile(x, z, colors[colorCounter % 2]))
                        vertexList.Add(vertex);
                }
            }

            floorBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, vertexList.Count, BufferUsage.WriteOnly);
            floorBuffer.SetData(vertexList.ToArray());
        }

        List<VertexPositionColor> FloorTile(int xOffset, int zOffset, Color tileColor)
        {
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>
            {
                new VertexPositionColor(new Vector3(0 + xOffset, 0, 0 + zOffset), tileColor),
                new VertexPositionColor(new Vector3(1 + xOffset, 0, 0 + zOffset), tileColor),
                new VertexPositionColor(new Vector3(0 + xOffset, 0, 1 + zOffset), tileColor),
                new VertexPositionColor(new Vector3(1 + xOffset, 0, 0 + zOffset), tileColor),
                new VertexPositionColor(new Vector3(1 + xOffset, 0, 1 + zOffset), tileColor),
                new VertexPositionColor(new Vector3(0 + xOffset, 0, 1 + zOffset), tileColor)
            };
            return vertexList;
        }
    }
}