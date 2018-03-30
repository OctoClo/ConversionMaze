using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ConversionFPS
{
    class Camera
    {
        public Matrix Projection;

        public Matrix View
        {
            get
            {
                if (needViewResync)
                    cachedViewMatrix = Matrix.CreateLookAt(Position, lookAt, Vector3.Up);
                return cachedViewMatrix;
            }
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

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                UpdateLookAt();
            }
        }
        
        Matrix cachedViewMatrix;
        Vector3 position = Vector3.Zero;
        float rotation;

        Vector3 lookAt;
        Vector3 baseCameraReference;
        bool needViewResync;

        public Camera(Vector3 position, float rotation, float aspectRatio, float nearClip, float farClip)
        {
            baseCameraReference = new Vector3(0, 0, 1);
            needViewResync = true;

            Projection = Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4, aspectRatio, nearClip, farClip);
            MoveTo(position, rotation);
        }

        public void MoveTo(Vector3 position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;
            UpdateLookAt();
        }

        void UpdateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(rotation);
            Vector3 lookAtOffset = Vector3.Transform(
            baseCameraReference,
            rotationMatrix);
            lookAt = position + lookAtOffset;
            needViewResync = true;
        }
    }
}
