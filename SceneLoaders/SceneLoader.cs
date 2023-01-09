using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scene3D.Objects;
using Scene3D.Helper;
using Scene3D.Movers;
using System.Numerics;

namespace Scene3D.SceneLoaders
{
    internal class SceneLoader
    {
        public static ModelCollection LoadChess(float aspectRatio)
        {
            ModelCollection modelCollection = new ModelCollection(aspectRatio);

            modelCollection.cameraPosOrigin = new Vector3(50, 50, -50) * 3/5;

            // Chess board;
            string halfChessboardName = "HalfChessBoard.obj";
            string halfChessboardPath = Path.Combine(Environment.CurrentDirectory, "data\\", halfChessboardName);
            var chessBoardHalf = FileReader.ReadObj(halfChessboardPath);

            chessBoardHalf.ObjectColor = Color.Beige;
            chessBoardHalf.Mover = new StationaryMover(new Vector3(0, 0, 3f));

            var anotherHalf = new Model(chessBoardHalf);
            anotherHalf.ObjectColor = Color.DarkGray;
            anotherHalf.Angle = new Vector3(0, MathF.PI, 0);

            modelCollection.AddModel(chessBoardHalf);
            modelCollection.AddModel(anotherHalf);

            // Pawns

            string pawnName = "Pawn.obj";
            string pawnPath = Path.Combine(Environment.CurrentDirectory, "data\\", pawnName);
            var pawn0 = FileReader.ReadObj(pawnPath);
            pawn0.Scale = 0.3f;
            pawn0.ObjectColor = Color.Magenta;

            var pawn1 = new Model(pawn0);
            var pawn2 = new Model(pawn0);
            var pawn3 = new Model(pawn0);
            var pawn4 = new Model(pawn0);
            var pawn5 = new Model(pawn0);
            var pawn6 = new Model(pawn0);
            var pawn7 = new Model(pawn0);
            float yMove = -4;
            float xMoveStep = 1.9f;
            pawn0.Mover = new StationaryMover(new Vector3(-3 * xMoveStep, yMove, 0));
            pawn1.Mover = new StationaryMover(new Vector3(-2 * xMoveStep, yMove, 0));
            pawn2.Mover = new StationaryMover(new Vector3(-1 * xMoveStep, yMove, 0));
            pawn3.Mover = new StationaryMover(new Vector3( 0 * xMoveStep, yMove, 0));
            pawn4.Mover = new StationaryMover(new Vector3( 1 * xMoveStep, yMove, 0));
            pawn5.Mover = new StationaryMover(new Vector3( 2 * xMoveStep, yMove, 0));
            pawn6.Mover = new StationaryMover(new Vector3( 3 * xMoveStep, yMove, 0));
            pawn7.Mover = new StationaryMover(new Vector3( 4 * xMoveStep, yMove, 0));

            modelCollection.AddModel(pawn0);
            modelCollection.AddModel(pawn1);
            modelCollection.AddModel(pawn2);
            modelCollection.AddModel(pawn3);
            modelCollection.AddModel(pawn4);
            modelCollection.AddModel(pawn5);
            modelCollection.AddModel(pawn6);
            modelCollection.AddModel(pawn7);

            var pawnb0 = new Model(pawn0);
            pawnb0.ObjectColor = Color.AntiqueWhite;

            var pawnb1 = new Model(pawnb0);
            var pawnb2 = new Model(pawnb0);
            var pawnb3 = new Model(pawnb0);
            var pawnb4 = new Model(pawnb0);
            var pawnb5 = new Model(pawnb0);
            var pawnb6 = new Model(pawnb0);
            var pawnb7 = new Model(pawnb0);

            float mult = -1.5f;

            pawnb0.Mover = new StationaryMover(new Vector3(-3 * xMoveStep, mult * yMove, 0));
            pawnb1.Mover = new StationaryMover(new Vector3(-2 * xMoveStep, mult * yMove, 0));
            pawnb2.Mover = new StationaryMover(new Vector3(-1 * xMoveStep, mult * yMove, 0));
            pawnb3.Mover = new StationaryMover(new Vector3(0 * xMoveStep,  mult * yMove, 0));
            pawnb4.Mover = new StationaryMover(new Vector3(1 * xMoveStep,  mult * yMove, 0));
            pawnb5.Mover = new StationaryMover(new Vector3(2 * xMoveStep,  mult * yMove, 0));
            pawnb6.Mover = new StationaryMover(new Vector3(3 * xMoveStep,  mult * yMove, 0));
            pawnb7.Mover = new StationaryMover(new Vector3(4 * xMoveStep,  mult * yMove, 0));

            modelCollection.AddModel(pawnb0);
            modelCollection.AddModel(pawnb1);
            modelCollection.AddModel(pawnb2);
            modelCollection.AddModel(pawnb3);
            modelCollection.AddModel(pawnb4);
            modelCollection.AddModel(pawnb5);
            modelCollection.AddModel(pawnb6);
            modelCollection.AddModel(pawnb7);

            string objName = "FullTorusNormalized.obj";
            //string objName = "FullSphere.obj";
            string pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var model = FileReader.ReadObj(pathModel);
            model.AngleStep = new Vector3(0.02f, 0.2f, 0);
            model.Scale = 5f;
            model.ObjectColor = Color.Blue;

            model.Mover = new CircleMover(14.0f, MathF.PI / 40, 0);

            modelCollection.AddModel(model);

            return modelCollection;
        }

        public static ModelCollection LoadBasic(float aspectRatio)
        {
            ModelCollection modelCollection = new ModelCollection(aspectRatio);
            string objName = "FullTorusNormalized.obj";
            //string objName = "FullSphere.obj";
            string pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var torus1 = FileReader.ReadObj(pathModel);
            var flyingTorus = new Model(torus1);
            flyingTorus.AngleStep = new Vector3(0.02f, 0.2f, 0);
            flyingTorus.Scale = 0.5f;
            flyingTorus.ObjectColor = Color.Blue;

            flyingTorus.Mover = new SphereMover(4.0f, MathF.PI / 40, 0);

            var torus2 = new Model(torus1);
            torus1.Mover = new StationaryMover(new Vector3(0, 1.25f, 0));
            torus2.Mover = new StationaryMover(new Vector3(0, -1.25f, 0));

            torus1.ObjectColor = Color.Chocolate;
            torus2.ObjectColor = Color.DarkKhaki;

            objName = "FullSphereSmooth.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var sphere1 = FileReader.ReadObj(pathModel);
            var centerSpher = new Model(sphere1);
            //modelCollection.AddModel(centerSpher);
            sphere1.ObjectColor = Color.Magenta;
            sphere1.Scale = 0.5f;
            var sphere2 = new Model(sphere1);

            sphere1.Mover = new LineMover(4, new Vector3(0, 0, 0.1f), new Vector3(0, -1.25f, 0));
            sphere2.Mover = new LineMover(4, new Vector3(0, 0, -0.1f), new Vector3(0, 1.25f, 0));
            sphere2.ObjectColor = Color.YellowGreen;

            modelCollection.AddModel(torus1);
            modelCollection.AddModel(torus2);
            modelCollection.AddModel(sphere1);
            modelCollection.AddModel(sphere2);
            modelCollection.AddModel(flyingTorus);

            return modelCollection;
        }

    }
}
