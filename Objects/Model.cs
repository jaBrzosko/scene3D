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
    internal class Model
    {
        // All faces that make 3D model
        private Triangle[] faces;

        // Rotation angle and step by which it changes
        public Vector3 Angle { get; set; }
        public Vector3 AngleStep { get; set; }

        // Object relative movement and rate of its change
        public Vector3 Movement { get; set; }
        public IMover Mover { get; set; }

        // Scale uniform in all three dimensions
        public float Scale { get; set; }

        public Color ObjectColor { get; set; }

        public Vector3 RotatedCenter { get; set; }

        public Model(Triangle[] faces, Vector3 angle, Vector3 angleStep, Vector3 movement, IMover mover, float scale)
        {
            this.faces = faces;
            Angle = angle;
            AngleStep = angleStep;
            Movement = movement;
            Scale = scale;
            Mover = mover;
            ObjectColor = Color.DarkOliveGreen;
            RotatedCenter = Vector3.Zero;
        }

        public Model(Model model)
        {
            faces = new Triangle[model.faces.Length];
            for(int i = 0; i < faces.Length; i++)
            {
                faces[i] = new Triangle(model.faces[i]);
            }
            Angle = model.Angle;
            AngleStep = model.AngleStep;
            Movement = model.Movement;
            Mover = model.Mover;
            Scale = model.Scale;
            ObjectColor = model.ObjectColor;
        }


        public void MakeRotationStep()
        {
            Angle += AngleStep;
        }

        public void MakeMovementStep()
        {
            Movement = Mover.GetNewPosition();
        }

        public void RotateAndMove(Matrix4x4 lookAtMatrix, Matrix4x4 perspectiveMatrix)
        {
            var translation = Matrix4x4.CreateTranslation(Movement);
            var scale = Matrix4x4.CreateScale(Scale);
            var rotX = Matrix4x4.CreateRotationX(Angle.X);
            var rotY = Matrix4x4.CreateRotationY(Angle.Y);
            var rotZ = Matrix4x4.CreateRotationZ(Angle.Z);
            var model = translation * scale * rotX * rotY * rotZ;
            //var PVM = perspectiveMatrix * lookAtMatrix * model;
            var PVM = model * lookAtMatrix * perspectiveMatrix;
            foreach(var triangle in faces)
            {
                triangle.Rotate(PVM, model);
            }
            var temp = Vector4.Transform(Vector3.Zero, PVM);
            temp /= temp.W;
            RotatedCenter = new Vector3(temp.X, temp.Y, temp.Z);
        }

        public void Draw(FastBitmap fastBitmap, Vector3 cameraPos)
        {
            var drawer = DrawerSingleton.GetInstance(fastBitmap.Width, fastBitmap.Height);
            Vector3 vectorColor = new Vector3((float)ObjectColor.R / 255, (float)ObjectColor.G / 255, (float)ObjectColor.B / 255);
            Parallel.ForEach(faces, face =>
           {
               drawer.Draw(fastBitmap, face, vectorColor, cameraPos);
           });
            //foreach(var face in faces)
            //{
            //    drawer.Draw(fastBitmap, face, vectorColor, cameraPos);
            //}
        }
    }
}
