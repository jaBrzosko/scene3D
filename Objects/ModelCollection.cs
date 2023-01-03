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
        private readonly Vector3 cameraUpVector = Vector3.UnitZ;
        public readonly Vector3 cameraDistance = new Vector3(10, 0, -6);
        public readonly Vector3 cameraTargetOrigin = Vector3.Zero;
        public readonly Vector3 cameraPosOrigin = new Vector3(15, 0, 0);
        public Vector3 CameraTarget { get; set; }
        public Vector3 CameraPos { get; set; }
        private float nearPlaneDistance = 1.0f;
        private float farPlaneDistance = 2.0f;
        private float fov = 30 * MathF.PI / 180;
        private float aspectRatio;
        public ModelCollection(float aspectRatio)
        {
            models = new List<Model>();
            this.aspectRatio = aspectRatio;
            CameraTarget = cameraTargetOrigin;
            CameraPos = cameraPosOrigin;
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
            var looakAtMat = Matrix4x4.CreateLookAt(CameraPos, CameraTarget, cameraUpVector);
            var perspectiveMat = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
            foreach (var model in models)
            {
                model.RotateAndMove(looakAtMat, perspectiveMat);
            }
        }

        public void Draw(FastBitmap fastBitmap)
        {
            var drawer = DrawerSingleton.GetInstance(fastBitmap.Width, fastBitmap.Height);
            drawer.Reset();
            Vector3 passedCameraPos = new Vector3(CameraPos.X * fastBitmap.Width, CameraPos.Y * fastBitmap.Height, CameraPos.Z);
            foreach(var model in models)
            {
                model.Draw(fastBitmap, passedCameraPos);
            }
        }
    }
}
