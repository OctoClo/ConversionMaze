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
        Cube[,] maze;
        Convertible[] convertibles;
        VertexBuffer floorBuffer;

        public Maze()
        {
            device = Main.Device;
            maze = MazeBuilder.GenerateMaze();
            Width = MazeBuilder.ElementsPerRow;
            Height = MazeBuilder.ElementsPerRow;
            convertibles = new Convertible[5];
            convertibles[0] = new Enemy(new Vector3(3, 0, 3));
            convertibles[1] = new Enemy(new Vector3(1, 0, 3));
            convertibles[2] = new Enemy(new Vector3(3, 0, 1));
            convertibles[3] = new Enemy(new Vector3(15, 0, 21));
            convertibles[4] = new Enemy(new Vector3(3, 0, 17));
            BuildFloorBuffer();
        }

        public Cube GetCube(int x, int y)
        {
            return maze[y, x];
        }

        public void Update(GameTime gameTime)
        {
            foreach (Convertible conv in convertibles)
                conv.Update(gameTime);
        }

        public void Draw(Camera camera, BasicEffect effect)
        {
            Main.Device.BlendState = BlendState.Opaque;
            Main.Device.DepthStencilState = DepthStencilState.Default;

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

            foreach (Cube cube in maze)
                cube.Draw(camera, effect);

            foreach (Convertible conv in convertibles)
                conv.Draw(camera, effect);
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