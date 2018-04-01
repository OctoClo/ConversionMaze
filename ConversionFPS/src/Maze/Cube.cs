using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConversionFPS
{
    class Cube
    {
        protected Vector3 position;
        protected Texture2D texture;

        GraphicsDevice device;
        VertexBuffer cubeVertexBuffer;
        List<VertexPositionTexture> vertices;

        public Cube(string texturePath, Vector3 pos)
        {
            device = Main.Device;
            position = pos;
            Initialize(texturePath);
        }

        protected virtual void Initialize(string texturePath)
        {
            texture = Main.Instance.Content.Load<Texture2D>(texturePath);

            // Create the cube's vertical faces
            vertices = new List<VertexPositionTexture>();
            BuildFace(new Vector3(0, 0, 0), new Vector3(0, 1, 1));
            BuildFace(new Vector3(0, 0, 1), new Vector3(1, 1, 1));
            BuildFace(new Vector3(1, 0, 1), new Vector3(1, 1, 0));
            BuildFace(new Vector3(1, 0, 0), new Vector3(0, 1, 0));

            // Create the cube's horizontal faces
            BuildFaceHorizontal(new Vector3(0, 1, 0), new Vector3(1, 1, 1));
            BuildFaceHorizontal(new Vector3(0, 0, 1), new Vector3(1, 0, 0));

            cubeVertexBuffer = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, vertices.Count, BufferUsage.WriteOnly);
            cubeVertexBuffer.SetData(vertices.ToArray());
        }

        public virtual void Draw(Camera camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = texture;
            
            Matrix scale = Matrix.CreateScale(1);
            Matrix translate = Matrix.CreateTranslation(position);

            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = scale * translate;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(cubeVertexBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, cubeVertexBuffer.VertexCount / 3);
            }
        }

        void BuildFace(Vector3 p1, Vector3 p2)
        {
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 1, 0));
            vertices.Add(BuildVertex(p1.X, p2.Y, p1.Z, 1, 1));
            vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, 0, 1));
            vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, 0, 1));
            vertices.Add(BuildVertex(p2.X, p1.Y, p2.Z, 0, 0));
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 1, 0));
        }

        void BuildFaceHorizontal(Vector3 p1, Vector3 p2)
        {
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 0, 1));
            vertices.Add(BuildVertex(p2.X, p1.Y, p1.Z, 1, 1));
            vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, 1, 0));
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 0, 1));
            vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, 1, 0));
            vertices.Add(BuildVertex(p1.X, p1.Y, p2.Z, 0, 0));
        }

        VertexPositionTexture BuildVertex(float x, float y, float z, float u, float v)
        {
            return new VertexPositionTexture(new Vector3(x, y, z), new Vector2(u, v));
        }
    }
}
