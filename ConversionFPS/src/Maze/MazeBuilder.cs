using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConversionFPS
{
    static class MazeBuilder
    {
        public static Texture2D TextureMinimap { get; private set; }
        public static int ElementsPerRow, ElementSize, rowSize;
        static TileType[,] elements;

        public static void Initialize(int difficulty)
        {
            ElementsPerRow = 25;
            ElementSize = 10;
            elements = new TileType[ElementsPerRow, ElementsPerRow];

            rowSize = (ElementsPerRow + 2) * ElementSize;
            TextureMinimap = new Texture2D(Main.Device, rowSize, rowSize);

            int randVal = Main.Rand.Next(1, 3);
            string filePath = "../../../../Content/Labyrinths/" + difficulty + randVal + ".txt";

            LoadFile(filePath);
            GenerateTexture();
        }

        public static Cube[,] GenerateMaze()
        {
            Cube[,] maze = new Cube[ElementsPerRow, ElementsPerRow];
            TileType type;

            for (int i = 0; i < ElementsPerRow; i++)
            {
                for (int j = 0; j < ElementsPerRow; j++)
                {
                    type = elements[i, j];
                    if (type == TileType.Wall)
                        maze[i, j] = new Cube("Wall", new Vector3(j, 0, i), type);
                    else if (type == TileType.Door)
                        maze[i, j] = new Door(new Vector3(j, 0, i));
                    else if (type == TileType.Enemy)
                        maze[i, j] = new Enemy(new Vector3(j, 0, i));
                    else
                        maze[i, j] = new Cube("", new Vector3(j, 0, i), type);
                }
            }

            return maze;
        }

        static void LoadFile(string filePath)
        {
            List<string> reading = new List<string>();

            string buffer;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while ((buffer = sr.ReadLine()) != null)
                    {
                        reading.Add(buffer);
                    }
                }

                for (int i = 0; i < ElementsPerRow; i++)
                {
                    for (int j = 0; j < ElementsPerRow; j++)
                    {
                        int valueTile;
                        if (Int32.TryParse(reading[i][j].ToString(), out valueTile))
                            elements[i, j] = (TileType)valueTile;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Print("Error while reading the file:");
                Debug.Print(e.Message);
            }
        }

        static void GenerateTexture()
        {
            // With 10*10 pixels
            Color[,] colors = new Color[rowSize, rowSize];

            // Dark pixels in door parts of the minimap
            List<Vector2> doorPixelPattern = new List<Vector2>();
            int[] pixX = new int[22] { 4, 5, 3, 4, 5, 6, 3, 4, 5, 6, 3, 4, 5, 6, 4, 5, 4, 5, 4, 5, 4, 5 };
            int[] pixY = new int[22] { 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };
            for (int i = 0; i < pixX.Length; i++)
                doorPixelPattern.Add(new Vector2(pixX[i], pixY[i]));
            
            // Loop through the tiles array
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < rowSize; j++)
                {
                    if (i < ElementSize || i >= rowSize - ElementSize || j < ElementSize || j >= rowSize - ElementSize)
                        colors[i, j] = Color.Transparent;
                    else if (GetColor(elements[(i - ElementSize) / ElementSize, (j - ElementSize) / ElementSize]) == Color.Gold)
                    {
                        Vector2 relativePos = new Vector2(j - (j / ElementSize) * ElementSize, i - (i / ElementSize) * ElementSize);
                        if (doorPixelPattern.Contains(relativePos))
                            colors[i, j] = Color.Black;
                        else
                            colors[i, j] = Color.Gold;
                    }
                    else
                        colors[i, j] = GetColor(elements[(i - ElementSize) / ElementSize, (j - ElementSize) / ElementSize]);
                }
            }

            int count = 0;
            Color[] colors1D = new Color[rowSize * rowSize];

            foreach (Color c in colors)
                colors1D[count++] = c;

            TextureMinimap.SetData<Color>(colors1D);
        }

        static Color GetColor(TileType type)
        {
            Color color = Color.Transparent;

            switch (type)
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
