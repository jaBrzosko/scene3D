using Scene3D.Helper;
using Scene3D.Objects;
using Scene3D.Lights;
using System.Numerics;
using Scene3D.Movers;

namespace Scene3D
{
    public partial class Visualizer : Form
    {
        FastBitmap fastBitmap;
        ModelCollection modelCollection;
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
            newModel.AngleStep = new Vector3(0.09f, -0.01f, 0.01f);
            newModel.Scale = 0.5f;
            newModel.ObjectColor = Color.Blue;

            model.Mover = new CircleMover(0.2f, MathF.PI / 10, 0);

            modelCollection.AddModel(model);
            modelCollection.AddModel(newModel);

            LightSingleton.AddLight(new Light(new Vector3(1, 1, 1), new Vector3(2, 4, 0), fastBitmap.Width, fastBitmap.Height));

            Redraw();
        }

        public void Redraw()
        {
            using (Graphics g = Graphics.FromImage(fastBitmap.Bitmap))
            {
                g.Clear(Color.White);
            }

            modelCollection.MakeSteps();
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
    }
}