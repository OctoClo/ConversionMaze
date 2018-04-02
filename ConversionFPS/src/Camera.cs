using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConversionFPS
{
    class Camera
    {
        public Matrix Projection { get; private set; }

        public Matrix View
        {
            get { return Matrix.CreateLookAt(position, lookAt, Vector3.Up); }
        }
        
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                UpdateLookAt();
            }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                UpdateLookAt();
            }
        }

        Vector3 position;
        Vector3 rotation;
        Vector3 lookAt;
        
        float cameraSpeed;
        Vector3 mouseRotation;

        public Camera(Vector3 position, Vector3 rotation, float speed)
        {
            cameraSpeed = speed;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Main.Device.Viewport.AspectRatio, 0.05f, 50.0f);
            MoveTo(position, rotation);

            Main.Instance.IsMouseVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 moveVector = Vector3.Zero;

            if (Input.KeyPressed(Keys.Z, false))
                moveVector.Z += 1;
            if (Input.KeyPressed(Keys.S, false))
                moveVector.Z -= 1;
            if (Input.KeyPressed(Keys.Q, false))
                moveVector.X += 1;
            if (Input.KeyPressed(Keys.D, false))
                moveVector.X -= 1;

            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();
                moveVector *= elapsed * cameraSpeed;
                Vector3 preview = PreviewMove(moveVector);
                if (Main.Maze.GetCube((int)(preview.X + 0.1), (int)(preview.Z + 0.1)).Type == TileType.End)
                    EventManager.Instance.Raise(new OnLevelEndEvent());
                else if (preview.X >= 1.1 && preview.X < Maze.Width - 1.1 && preview.Z >= 1.1 && preview.Z < Maze.Height - 1.1)
                {
                    Cube cube = Main.Maze.GetCube((int)(preview.X + 0.1), (int)(preview.Z + 0.1));
                    if (cube.Type == TileType.Door)
                    {
                        Door door = (Door)cube;
                        if (door.IsOpen)
                            Move(moveVector);
                    }
                    else if (cube.Type != TileType.Wall)
                        Move(moveVector);
                }
            }

            float deltaX, deltaY;

            if (Input.PreviousMouseState != Input.CurrentMouseState)
            {
                deltaX = Input.CurrentMouseState.X - Main.Center.X;
                deltaY = Input.CurrentMouseState.Y - Main.Center.Y;

                mouseRotation.X -= 0.1f * deltaX * elapsed;
                mouseRotation.Y -= 0.1f * deltaY * elapsed;

                if (mouseRotation.Y < MathHelper.ToRadians(-75.0f))
                    mouseRotation.Y = mouseRotation.Y - (mouseRotation.Y - MathHelper.ToRadians(-75.0f));
                if (mouseRotation.Y > MathHelper.ToRadians(75.0f))
                    mouseRotation.Y = mouseRotation.Y - (mouseRotation.Y - MathHelper.ToRadians(75.0f));

                Rotation = new Vector3(-MathHelper.Clamp(mouseRotation.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                                       MathHelper.WrapAngle(mouseRotation.X), 0);

                deltaX = 0;
                deltaY = 0;
            }

            Mouse.SetPosition((int)Main.Center.X, (int)Main.Center.Y);
        }

        Vector3 PreviewMove(Vector3 scale)
        {
            Matrix rotate = Matrix.CreateRotationY(rotation.Y);
            Vector3 forward = new Vector3(scale.X, scale.Y, scale.Z);
            forward = Vector3.Transform(forward, rotate);
            return (position + forward);
        }

        void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);
        }

        void MoveTo(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        void UpdateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y);
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
            lookAt = position + lookAtOffset;
        }
    }
}
