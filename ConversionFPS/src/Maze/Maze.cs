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
        public const int mazeWidth = 20;
        public const int mazeHeight = 20;

        GraphicsDevice device;
        VertexBuffer floorBuffer;

        public Maze()
        {
            device = Main.Device;
            BuildFloorBuffer();
        }

        public void Draw(Camera camera, BasicEffect effect)
        {
            effect.TextureEnabled = false;
            effect.VertexColorEnabled = true;
            effect.World = Matrix.Identity;
            effect.View = camera.View;
            effect.Projection = camera.Projection;
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
            List<VertexPositionColor> vertexTile;

            for (int x = 0; x < mazeWidth; x++)
            {
                for (int z = 0; z < mazeHeight; z++)
                {
                    vertexTile = FloorTile(x, z, Color.Gainsboro);
                    foreach (VertexPositionColor vertex in vertexTile)
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