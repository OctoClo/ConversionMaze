using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConversionFPS
{
    class Map
    {
        MapElement[,] elements;
        Texture2D textureMap;
        int difficulty;
        string filePath;


        public Map(int difficulty)
        {
            elements = new MapElement[25,25];
            textureMap = new Texture2D(Main.Device, 270, 270);
            this.difficulty = difficulty;

            Random rand = new Random();
            int randVal = rand.Next(1, 3); 

            filePath = "../../../../Content/Labyrinths/" + difficulty + randVal + ".txt";

            loadFile();
            generateTexture();
        }

        private void loadFile()
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

                for (int i = 0; i < 25; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        Array types = (Enum.GetValues(typeof(TileType)));

                        int valueTile;
                        if (Int32.TryParse(reading[i][j].ToString(), out valueTile))
                            elements[i, j] = new MapElement( (TileType)types.GetValue(valueTile));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Print("Error while reading the file:");
                Debug.Print(e.Message);
            }
        }

        public void generateTexture()
        {
            // With 10*10 pixels
            Color[,] colors = new Color[270, 270];

            // Loop through the tiles array
            for (int i = 0; i < 270; i++)
            {
                for (int j = 0; j < 270; j++)
                {
                    if (i < 10 || i >= 260 || j < 10 || j >= 260)
                        colors[i, j] = Color.Transparent;
                    else
                        colors[i, j] = elements[(i - 10) / 10, (j - 10) / 10].getColor();
                }
            }

            int count = 0;
            Color[] colors1D = new Color[270 * 270];

            foreach (Color c in colors)
                colors1D[count++] = c;

            textureMap.SetData<Color>(colors1D);

        }

        public Texture2D getTexture()
        {
            return textureMap;
        }
    }
}
