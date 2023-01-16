using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Scene3D.Helper;
using Scene3D.Movers;

namespace Scene3D.Objects
{
    internal class ModelCollection
    {
        private List<Model> models;
        private List<Model> vibrable;

        public Model this[int key]
        {
            get => models[key];
        }
        public int Count => models.Count;

        // Rotation variables
        private readonly Vector3 cameraUpVector = Vector3.UnitZ;
        public Vector3 cameraDistance = new Vector3(4, 0, -2);
        public Vector3 cameraTargetOrigin = Vector3.Zero;
        public Vector3 cameraPosOrigin = new Vector3(15, 0, 0);
        public Vector3 CameraTarget { get; set; }
        public Vector3 CameraPos { get; set; }
        public Vibrator MoveVibrator { get; set; }
        
        private float nearPlaneDistance = 1.0f;
        private float farPlaneDistance = 100.0f;
        private float fov = 30 * MathF.PI / 180;
        private float aspectRatio;
        
        public Model Last { get { return models.Last(); } }
        public ModelCollection(float aspectRatio)
        {
            models = new List<Model>();
            vibrable = new List<Model>();
            this.aspectRatio = aspectRatio;
            CameraTarget = cameraTargetOrigin;
            CameraPos = cameraPosOrigin;
            MoveVibrator = new Vibrator(0);
        }

        public void AddModel(Model model, bool isVibrable = false)
        {
            models.Add(model);
            if (isVibrable)
                vibrable.Add(model);
        }
        public void RemoveModel(Model model)
        {
            models.Remove(model);
            vibrable.Remove(model);
        }

        public void MakeSteps(bool doVibrate)
        {
            foreach(var model in models)
            {
                model.MakeMovementStep();
                model.MakeRotationStep();
            }
            if (!doVibrate) return;
            foreach(var model in vibrable)
            {
                model.Vibrate(MoveVibrator);
            }
        }
        public void Rotate()
        {
            var loakAtMat = Matrix4x4.CreateLookAt(CameraPos, CameraTarget, cameraUpVector);
            var perspectiveMat = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
            foreach (var model in models)
            {
                model.RotateAndMove(loakAtMat, perspectiveMat);
            }
            foreach (var light in Lights.LightSingleton.GetInstance())
            {
                light.Rotate(loakAtMat, perspectiveMat);
            }
        }

        public void Draw(FastBitmap fastBitmap, bool interpolateColor)
        {
                var drawer = DrawerSingleton.GetInstance(fastBitmap.Width, fastBitmap.Height);
            drawer.Reset();
            Vector3 passedCameraPos = new Vector3(CameraPos.X * fastBitmap.Width, CameraPos.Y * fastBitmap.Height, CameraPos.Z);
            foreach (var model in models)
            {
                model.Draw(fastBitmap, passedCameraPos, interpolateColor);
            }
            //Parallel.ForEach(models, model =>
            //{
            //    model.Draw(fastBitmap, passedCameraPos, interpolateColor);
            //});
        }
    }
}
