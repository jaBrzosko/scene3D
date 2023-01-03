using Scene3D.Helper;
using Scene3D.Objects;
using Scene3D.Lights;
using System.Numerics;
using Scene3D.Movers;
using Scene3D.Enums;

namespace Scene3D
{
    public partial class Visualizer : Form
    {
        private FastBitmap fastBitmap;
        private ModelCollection modelCollection;
        private Model moving;
        private CameraType cameraType;
        public Visualizer()
        {
            InitializeComponent();

            fastBitmap = new FastBitmap(canvas.Width, canvas.Height);
            canvas.Image = fastBitmap.Bitmap;
            modelCollection = new ModelCollection(canvas.Width / canvas.Height);

            string objName = "FullTorusNormalized.obj";
            //string objName = "FullSphere.obj";
            string pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var model = FileReader.ReadObj(pathModel);
            var newModel = new Model(model);
            //newModel.AngleStep = new Vector3(0.01f, 0, 0);
            newModel.Scale = 0.5f;
            newModel.ObjectColor = Color.Blue;
            
            newModel.Mover = new CircleMover(4.0f, MathF.PI / 40, 0);

            moving = newModel;

            var anotherModel = new Model(model);
            model.Mover = new StationaryMover(new Vector3(0, 1.25f, 0));
            anotherModel.Mover = new StationaryMover(new Vector3(0, -1.25f, 0));

            objName = "FullSphere.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var sphere1 = FileReader.ReadObj(pathModel);
            sphere1.ObjectColor = Color.Magenta;
            sphere1.Scale = 0.5f;
            var sphere2 = new Model(sphere1);

            sphere1.Mover = new LineMover(4, new Vector3(0, 0, 0.1f), new Vector3(0, -2.5f, 0));
            sphere2.Mover = new LineMover(4, new Vector3(0, 0, -0.1f), new Vector3(0, 2.5f, 0));


            modelCollection.AddModel(model);
            modelCollection.AddModel(newModel);
            modelCollection.AddModel(anotherModel);
            modelCollection.AddModel(sphere1);
            modelCollection.AddModel(sphere2);


            LightSingleton.AddLight(new Light(new Vector3(1, 1, 1), new Vector3(2, 4, 0), fastBitmap.Width, fastBitmap.Height));
            cameraType = CameraType.Stationary;

            Redraw();
        }

        public void Redraw(bool makeMove = true)
        {
            using (Graphics g = Graphics.FromImage(fastBitmap.Bitmap))
            {
                g.Clear(Color.White);
            }
            if(makeMove)
                modelCollection.MakeSteps();
            switch (cameraType)
            {
                case CameraType.Stationary:
                    {
                        modelCollection.CameraPos = modelCollection.cameraPosOrigin;
                        modelCollection.CameraTarget = modelCollection.cameraTargetOrigin;
                        break;
                    }
                case CameraType.Following:
                    {
                        modelCollection.CameraPos = modelCollection.cameraPosOrigin;
                        modelCollection.CameraTarget = moving.Movement * moving.Scale;
                        break;
                    }
                case CameraType.Moving:
                    {
                        modelCollection.CameraPos = moving.Movement * moving.Scale + modelCollection.cameraDistance;
                        modelCollection.CameraTarget = moving.Movement * moving.Scale;
                        break;
                    }
            }
            modelCollection.Rotate();
            modelCollection.Draw(fastBitmap);
            

            canvas.Refresh();
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {

            if(timer.Enabled)
            {
                timer.Stop();
                buttonStep.Text = "Start";
            }
            else
            {
                timer.Start();
                buttonStep.Text = "Stop";
            }
            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Redraw();
        }

        private void Visualizer_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.F1:
                    {
                        cameraType = CameraType.Stationary;
                        Redraw(false);
                        break;
                    }
                case Keys.F2:
                    {
                        cameraType = CameraType.Following;
                        Redraw(false);
                        break;
                    }
                case Keys.F3:
                    {
                        cameraType = CameraType.Moving;
                        Redraw(false);
                        break;
                    }
            }
        }
    }
}