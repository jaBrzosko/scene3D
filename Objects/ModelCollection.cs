using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Scene3D.Helper;

namespace Scene3D.Objects
{
    internal class ModelCollection
    {
        private List<Model> models;

        // Rotation variables
        private Vector3 cameraUpVector = new Vector3(0, 0, 1);
        private Vector3 cameraTarget = new Vector3(0, 0, 0);
        private float nearPlaneDistance = 1.0f;
        private float farPlaneDistance = 2.0f;
        private float fov = 30 * MathF.PI / 180;
        private float aspectRatio;
        private Vector3 cameraPos = new Vector3(5, 0, 0);
        private Matrix4x4 looakAtMat;
        private Matrix4x4 perspectiveMat;
        public ModelCollection(float aspectRatio)
        {
            models = new List<Model>();
            this.aspectRatio = aspectRatio;
            looakAtMat = Matrix4x4.CreateLookAt(cameraPos, cameraTarget, cameraUpVector);
            perspectiveMat = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
        }

        public void AddModel(Model model) => models.Add(model);

        public void MakeSteps()
        {
            foreach(var model in models)
            {
                model.MakeMovementStep();
                model.MakeRotationStep();
            }
        }
        public void Rotate()
        {
            foreach (var model in models)
            {
                model.RotateAndMove(looakAtMat, perspectiveMat);
            }
        }

        public void Draw(FastBitmap fastBitmap)
        {
            var drawer = DrawerSingleton.GetInstance(fastBitmap.Width, fastBitmap.Height);
            drawer.Reset();
            Vector3 passedCameraPos = new Vector3(cameraPos.X * fastBitmap.Width, cameraPos.Y * fastBitmap.Height, cameraPos.Z);
            foreach(var model in models)
            {
                model.Draw(fastBitmap, passedCameraPos);
            }
        }
    }
}
