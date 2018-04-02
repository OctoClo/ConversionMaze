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

        public List<Convertible> Convertibles { get { return convertibles; } }

        GraphicsDevice device;
        Cube[,] maze;
        List<Convertible> convertibles;
        VertexBuffer floorBuffer;

        public Maze()
        {
            device = Main.Device;
            Width = MazeBuilder.ElementsPerRow;
            Height = MazeBuilder.ElementsPerRow;
            maze = MazeBuilder.GenerateMaze();

            SpawnEnemies();
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

        void SpawnEnemies()
        {
            convertibles = new List<Convertible>();

            foreach (Cube cube in maze)
            {
                if (cube.Type == TileType.Spawn)
                    convertibles.Add(new Enemy(cube.Position));
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

        // *** Functions to check Convertible presence in field of view ***

        // Check if Convertible is in front of the camera
        public static bool IsInFront(Convertible c, Camera cam) { return ( IsVisibleUnder(c, cam) || IsVisibleOver(c, cam) || IsVisibleRight(c, cam) || IsVisibleLeft(c, cam) ); }

        // Check if Convertible in same row
        private static bool IsInSameRow(Convertible c, Camera cam) { return ((int)c.Position.Z == (int)cam.Position.Z); }

        // Check if Convertible in same column
        private static bool IsInSameColumn(Convertible c, Camera cam) { return ((int)c.Position.X == (int)cam.Position.X); }

        // Check if Convertible Z value is greater than camera's
        private static bool IsUnderCam(Convertible c, Camera cam) { return (IsInSameColumn(c, cam) && (int)c.Position.Z >= cam.Position.Z); }

        // Check if Convertible Z value is lower than camera's
        private static bool IsOverCam(Convertible c, Camera cam) { return (IsInSameColumn(c, cam) && (int)c.Position.Z <= cam.Position.Z); }

        // Check if Convertible X value is greater than camera's
        private static bool IsRightOfCam(Convertible c, Camera cam) { return (IsInSameRow(c, cam) && (int)c.Position.X >= cam.Position.X); }

        // Check if Convertible X value is lower than camera's
        private static bool IsLeftOfCam(Convertible c, Camera cam) { return (IsInSameRow(c, cam) && (int)c.Position.X <= cam.Position.X); }

        // Check if Convertible is visible under the cam
        private static bool IsVisibleUnder(Convertible c, Camera cam) { return (IsUnderCam(c, cam) && (MathHelper.ToDegrees(cam.Rotation.Y) > -45) && (MathHelper.ToDegrees(cam.Rotation.Y) < 45)); }

        // Check if Convertible is visible over the cam
        private static bool IsVisibleOver(Convertible c, Camera cam) { return (IsOverCam(c, cam) && (MathHelper.ToDegrees(cam.Rotation.Y) > 135) || (MathHelper.ToDegrees(cam.Rotation.Y) < -135)); }

        // Check if Convertible is visible at the right of the cam
        private static bool IsVisibleRight(Convertible c, Camera cam) { return (IsRightOfCam(c, cam) && (MathHelper.ToDegrees(cam.Rotation.Y) > 45) && (MathHelper.ToDegrees(cam.Rotation.Y) < 135)); }

        // Check if Convertible is visible at the left of the cam
        private static bool IsVisibleLeft(Convertible c, Camera cam) { return (IsLeftOfCam(c, cam) && (MathHelper.ToDegrees(cam.Rotation.Y) > -135) && (MathHelper.ToDegrees(cam.Rotation.Y) < -45)); }

        
        // Function to check if path is free of any wall
        public static bool IsPathClear(Convertible c, Camera cam, Maze maze)
        {
            bool result = true;
            int x1, y1, x2, y2;
            x1 = (int)cam.Position.X;
            y1 = (int)cam.Position.Z;
            x2 = (int)c.Position.X;
            y2 = (int)c.Position.Z;

            if (IsInSameRow(c, cam))
            {
                if ( x1 <= x2)
                {
                    while (result && x1 != x2)
                    {
                        x1++;
                        result = !(maze.GetCube(x1, y1).Type == TileType.Wall);
                    }
                }
                else
                {
                    while (result && x1 != x2)
                    {
                        x2++;
                        result = !(maze.GetCube(x2, y2).Type == TileType.Wall);
                    }
                }
            }
            else
            {
                if (x1 <= x2)
                {
                    while (result && y1 != y2)
                    {
                        y1++;
                        result = !(maze.GetCube(x1, y1).Type == TileType.Wall);
                    }
                }
                else
                {
                    while (result && y1 != y2)
                    {
                        y2++;
                        result = !(maze.GetCube(x2, y2).Type == TileType.Wall);
                    }
                }
            }
            
            return result;
        }
    
    }
}