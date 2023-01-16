using Scene3D.Helper;
using Scene3D.Objects;
using Scene3D.Lights;
using System.Numerics;
using Scene3D.Movers;
using Scene3D.Enums;
using System.Globalization;
using Scene3D.Objects.Chess;

namespace Scene3D
{
    public partial class Visualizer : Form
    {
        private FastBitmap fastBitmap;
        private ChessGame chessGame;
        private Model moving;
        private int movingIndex;

        private CameraType cameraType;
        private CircleMover circleMover;
        private bool interpolateColor;
        private bool doVibrate;
        public Visualizer()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            fastBitmap = new FastBitmap(canvas.Width, canvas.Height);
            canvas.Image = fastBitmap.Bitmap;

            //modelCollection = SceneLoaders.SceneLoader.LoadChess(canvas.Width / canvas.Height);
            //modelCollection = SceneLoaders.SceneLoader.LoadBasic(canvas.Width, canvas.Height);
            //modelCollection = SceneLoaders.SceneLoader.SimpleSphere(canvas.Width / canvas.Height);
            chessGame = SceneLoaders.SceneLoader.LoadBetterChess(canvas.Width, canvas.Height);
            moving = chessGame.Last;
            movingIndex = chessGame.Count - 1;

            cameraType = CameraType.Stationary;
            circleMover = new CircleMover(chessGame.cameraDistance.Length(), 0.1f, chessGame.cameraPosOrigin.Z);
            interpolateColor = true;
            doVibrate = false;

            Redraw();
        }

        public void Redraw(bool makeMove = true)
        {
            using (Graphics g = Graphics.FromImage(fastBitmap.Bitmap))
            {
                g.Clear(Color.White);
            }
            if(makeMove)
            {
                chessGame.MakeSteps(doVibrate);
                chessGame.MakeChessMove();
            }
            switch (cameraType)
            {
                case CameraType.Stationary:
                    {
                        chessGame.CameraPos = chessGame.cameraPosOrigin;
                        chessGame.CameraTarget = chessGame.cameraTargetOrigin;
                        break;
                    }
                case CameraType.Following:
                    {
                        chessGame.CameraPos = chessGame.cameraPosOrigin;
                        chessGame.CameraTarget = moving.Movement;
                        break;
                    }
                case CameraType.Moving:
                    {
                        chessGame.CameraPos = moving.Movement + chessGame.cameraDistance;
                        chessGame.CameraTarget = moving.Movement;
                        break;
                    }
                case CameraType.Rotating:
                    {
                        chessGame.CameraPos = circleMover.GetNewPosition(Vector3.Zero);
                        chessGame.CameraTarget = chessGame.cameraTargetOrigin;
                        break;
                    }
            }
            //light.Position = moving.Movement;
            chessGame.Rotate();
            chessGame.Draw(fastBitmap, interpolateColor);
            

            canvas.Refresh();
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            AnimationStartStop();
        }

        private void AnimationStartStop()
        {
            if (timer.Enabled)
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
                case Keys.T:
                    {
                        doVibrate = !doVibrate;
                        Redraw(false);
                        break;
                    }
                case Keys.Tab:
                    {
                        movingIndex++;
                        if(movingIndex >= chessGame.Count)
                            movingIndex = 0;
                        moving = chessGame[movingIndex];
                        Redraw(false);
                        break;
                    }
                case Keys.P:
                    {
                        AnimationStartStop();
                        break;
                    }
                case Keys.O:
                    {
                        Redraw();
                        break;
                    }
            }
        }
    }
}