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
        private bool showFog;
        private bool doVibrate;
        private bool rotateMoving;
        private float nightAndDayDirection;
        private Vector3 freeCamera;
        private Vector3 cameraAngle;
        public Visualizer()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            fastBitmap = new FastBitmap(canvas.Width, canvas.Height);
            canvas.Image = fastBitmap.Bitmap;

            //modelCollection = SceneLoaders.SceneLoader.LoadChess(canvas.Width / canvas.Height);
            //chessGame = SceneLoaders.SceneLoader.LoadBasic(canvas.Width, canvas.Height);
            //modelCollection = SceneLoaders.SceneLoader.SimpleSphere(canvas.Width / canvas.Height);
            chessGame = SceneLoaders.SceneLoader.LoadBetterChess(canvas.Width, canvas.Height);
            moving = chessGame.Last;
            movingIndex = chessGame.Count - 1;

            cameraType = CameraType.StationaryWhite;
            circleMover = new CircleMover(chessGame.cameraDistance.Length(), 0.1f, chessGame.cameraPosOrigin.Z);
            interpolateColor = true;
            doVibrate = false;
            showFog = false;
            rotateMoving = false;
            nightAndDayDirection = 0.1f;

            Redraw();
        }

        public void Redraw(bool makeMove = true)
        {
            using (Graphics g = Graphics.FromImage(fastBitmap.Bitmap))
            {
                g.Clear(Color.White);
            }
            if (chessGame.Moving is not null)
                moving = chessGame.Moving;
            if (makeMove)
            {
                if(doVibrate)
                {
                    chessGame.ResetVibrable();
                    chessGame.SetVibrable(moving);
                }
                chessGame.MakeSteps(doVibrate);
                chessGame.MakeChessMove(rotateMoving);
                ChangeSpotLights();
            }
            switch (cameraType)
            {
                case CameraType.StationaryWhite:
                    {
                        chessGame.CameraPos = chessGame.cameraPosOrigin;
                        chessGame.CameraTarget = chessGame.cameraTargetOrigin;
                        break;
                    }
                case CameraType.StationaryBlack:
                    {
                        var t = chessGame.cameraPosOrigin;
                        chessGame.CameraPos = new Vector3(-t.X, -t.Y, t.Z);
                        chessGame.CameraTarget = chessGame.cameraTargetOrigin;
                        break;
                    }
                case CameraType.FollowingWhite:
                    {
                        chessGame.CameraPos = chessGame.cameraPosOrigin;
                        chessGame.CameraTarget = moving.Movement;
                        break;
                    }
                case CameraType.FollowingBlack:
                    {
                        var t = chessGame.cameraPosOrigin;
                        chessGame.CameraPos = new Vector3(-t.X, -t.Y, t.Z);
                        chessGame.CameraTarget = moving.Movement;
                        break;
                    }
                case CameraType.MovingWhite:
                    {
                        chessGame.CameraPos = moving.Movement + chessGame.cameraDistance;
                        chessGame.CameraTarget = moving.Movement;
                        break;
                    }
                case CameraType.MovingBlack:
                    {
                        var t = chessGame.cameraPosOrigin;
                        chessGame.CameraPos = moving.Movement + new Vector3(-t.X, -t.Y, t.Z);
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
            chessGame.Draw(fastBitmap, interpolateColor, showFog);
            

            canvas.Refresh();
        }

        private void AnimationStartStop()
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
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
                        cameraType = CameraType.StationaryWhite;
                        Redraw(false);
                        break;
                    }
                case Keys.F2:
                    {
                        cameraType = CameraType.StationaryBlack;
                        Redraw(false);
                        break;
                    }
                case Keys.F3:
                    {
                        cameraType = CameraType.FollowingWhite;
                        Redraw(false);
                        break;
                    }
                case Keys.F4:
                    {
                        cameraType = CameraType.FollowingBlack;
                        Redraw(false);
                        break;
                    }
                case Keys.F5:
                    {
                        cameraType = CameraType.MovingWhite;
                        Redraw(false);
                        break;
                    }
                case Keys.F6:
                    {
                        cameraType = CameraType.MovingBlack;
                        Redraw(false);
                        break;
                    }
                case Keys.F7:
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
                case Keys.N:
                    {
                        foreach(var light in LightSingleton.GetInstance())
                        {
                            if (light is PointLight)
                                light.ChangeOnOff();
                        }
                        Redraw(false);
                        break;
                    }
                case Keys.M:
                    {
                        nightAndDayDirection *= -1;
                        Redraw(false);
                        break;
                    }
                case Keys.R:
                    {
                        rotateMoving = !rotateMoving;
                        Redraw(false);
                        break;
                    }
                case Keys.Oemplus:
                    {
                        foreach (var light in LightSingleton.GetInstance())
                        {
                            if (light is SpotLight)
                                ((SpotLight)light).ChangeDirection(true);
                        }
                        Redraw(false);
                        break;
                    }
                case Keys.OemMinus:
                    {
                        foreach (var light in LightSingleton.GetInstance())
                        {
                            if (light is SpotLight)
                                ((SpotLight)light).ChangeDirection(false);
                        }
                        Redraw(false);
                        break;
                    }
                case Keys.NumPad0:
                    {
                        TurnOffSpotLight(0);
                        Redraw(false);
                        break;
                    }
                case Keys.NumPad1:
                    {
                        TurnOffSpotLight(1);
                        Redraw(false);
                        break;
                    }
                case Keys.NumPad2:
                    {
                        TurnOffSpotLight(2);
                        Redraw(false);
                        break;
                    }
                case Keys.NumPad3:
                    {
                        TurnOffSpotLight(3);
                        Redraw(false);
                        break;
                    }
                case Keys.F:
                    {
                        showFog = !showFog;
                        Redraw(false);
                        break;
                    }
                case Keys.Escape:
                    {
                        LightSingleton.Reset();
                        chessGame = SceneLoaders.SceneLoader.LoadBetterChess(fastBitmap.Width, fastBitmap.Height);
                        Redraw(true);
                        break;
                    }
            }
        }

        private void TurnOffSpotLight(int n)
        {
            foreach (var light in LightSingleton.GetInstance())
            {
                if (light is SpotLight)
                {
                    if (n == 0)
                    {
                        light.ChangeOnOff();
                        return;
                    }
                    n--;
                }
            }
        }

        private void ChangeSpotLights()
        {
            foreach (var light in LightSingleton.GetInstance())
            {
                if (light is PointLight)
                {
                    ((PointLight)light).LightCoef += nightAndDayDirection;
                    //if((nightAndDayDirection < 0 && ((PointLight)light).LightCoef == 0) 
                    //    || (nightAndDayDirection < 0 && ((PointLight)light).LightCoef == 0))
                    //{
                    //    nightAndDayDirection = 0;
                    //}
                }
            }
        }
    }
}