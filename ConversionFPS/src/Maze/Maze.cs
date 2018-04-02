using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConversionFPS
{
    class OnLevelEndEvent : GameEvent { }

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
            convertibles = new Convertible[1];
            convertibles[0] = new Enemy(new Vector3(5, 0, 1));
            BuildFloorBuffer();
        }

        public bool IsOnPlayerCube(Cube cube)
        {
            Cube playerCube = GetPlayerCube();
            return (playerCube.Position.X == (int)Math.Round(cube.Position.X) &&
                    playerCube.Position.Z == (int)Math.Round(cube.Position.Z));
        }

        public Cube GetPlayerCube()
        {
            return GetCube((int)Main.Camera.Position.X, (int)Main.Camera.Position.Z);
        }

        public Cube GetCube(int x, int y)
        {
            return maze[y, x];
        }

        public List<Cube> GetAdjacentCubes(Cube cube)
        {
            List<Cube> neighbours = new List<Cube>();
            int x = (int)cube.Position.X;
            int y = (int)cube.Position.Z;

            if (x > 0 && maze[y, x - 1].Type != TileType.Wall)
                neighbours.Add(maze[y, x - 1]);
            if (y > 0 && maze[y - 1, x].Type != TileType.Wall)
                neighbours.Add(maze[y - 1, x]);
            if (x < MazeBuilder.ElementsPerRow - 1 && maze[y, x + 1].Type != TileType.Wall)
                neighbours.Add(maze[y, x + 1]);
            if (y < MazeBuilder.ElementsPerRow - 1 && maze[y + 1, x].Type != TileType.Wall)
                neighbours.Add(maze[y + 1, x]);

            return neighbours;
        }

        public int GetDistance(Cube cube1, Cube cube2)
        {
            int xDistance = (int)cube1.Position.X - (int)cube2.Position.X;
            int zDistance = (int)cube1.Position.Z - (int)cube2.Position.Z;
            if (xDistance < 0)
                xDistance *= -1;
            if (zDistance < 0)
                zDistance *= -1;
            return (xDistance + zDistance);
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