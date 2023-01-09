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
        private CircleMover circleMover;
        private bool interpolateColor;
        public Visualizer()
        {
            InitializeComponent();

            fastBitmap = new FastBitmap(canvas.Width, canvas.Height);
            canvas.Image = fastBitmap.Bitmap;

            //modelCollection = SceneLoaders.SceneLoader.LoadChess(canvas.Width / canvas.Height);
            modelCollection = SceneLoaders.SceneLoader.LoadBasic(canvas.Width / canvas.Height);
            moving = modelCollection.Last;

            var pointLight = new PointLight(new Vector3(1, 1, 1), new Vector3(4, 4, 10), fastBitmap.Width, fastBitmap.Height);
            LightSingleton.AddLight(pointLight);

            //var spotLight = new SpotLight(new Vector3(1, 1, 1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0), fastBitmap.Width, fastBitmap.Height, MathF.PI / 4);
            //LightSingleton.AddLight(spotLight);

            cameraType = CameraType.Stationary;
            circleMover = new CircleMover(modelCollection.CameraPos.Length(), 0.1f, modelCollection.CameraPos.Z);
            interpolateColor = true;

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
                case CameraType.Rotating:
                    {
                        modelCollection.CameraPos = circleMover.GetNewPosition();
                        modelCollection.CameraTarget = modelCollection.cameraTargetOrigin;
                        break;
                    }
            }
            //light.Position = moving.Movement;
            modelCollection.Rotate();
            modelCollection.Draw(fastBitmap, interpolateColor);
            

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
                case Keys.F4:
                    {
                        cameraType = CameraType.Rotating;
                        Redraw(false);
                        break;
                    }
                case Keys.D1:
                    {
                        interpolateColor = true;
                        Redraw(false);
                        break;
                    }
                case Keys.D2:
                    {
                        interpolateColor = false;
                        Redraw(false);
                        break;
                    }
            }
        }
    }
}