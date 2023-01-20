using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scene3D.Objects;
using Scene3D.Helper;
using Scene3D.Movers;
using System.Numerics;
using Scene3D.Lights;
using Scene3D.Objects.Chess;

namespace Scene3D.SceneLoaders
{
    internal class SceneLoader
    {
        public static ModelCollection LoadChess(float aspectRatio)
        {
            ModelCollection modelCollection = new ModelCollection(aspectRatio);

            modelCollection.cameraPosOrigin = new Vector3(50, 50, -50) * 4/5;
            modelCollection.cameraDistance = new Vector3(20, 20, -20);

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

        public static ModelCollection SimpleSphere(float aspectRatio)
        {
            ModelCollection modelCollection = new ModelCollection(aspectRatio);
            string objName = "FullSphereSmooth.obj";
            //string objName = "FullSphere.obj";
            string pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var cube = FileReader.ReadObj(pathModel);
            //cube.Angle = new Vector3(MathF.PI / 8, MathF.PI / 8, 0);
            cube.AngleStep = new Vector3(0.02f, 0.02f, 0.02f);
            cube.Scale = 4;
            modelCollection.cameraPosOrigin = new Vector3(16.5f, 0, 0);
            modelCollection.AddModel(cube);

            return modelCollection;
        }

        public static ModelCollection LoadBasic(int width, int height)
        {
            ModelCollection modelCollection = new ModelCollection(width / height);
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
            torus1.InitialPosition = new Vector3(0, 1.25f, 0);
            torus2.InitialPosition = new Vector3(0, -1.25f, 0);

            torus1.ObjectColor = Color.Chocolate;
            torus2.ObjectColor = Color.DarkKhaki;

            objName = "FullSphereSmooth.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var sphere1 = FileReader.ReadObj(pathModel);
            var centerSpher = new Model(sphere1);
            sphere1.ObjectColor = Color.Magenta;
            sphere1.Scale = 0.5f;
            var sphere2 = new Model(sphere1);

            sphere1.Mover = new LineMover(4, new Vector3(0, 0, 0.1f), new Vector3(0, -1.25f, 0));
            sphere2.Mover = new LineMover(4, new Vector3(0, 0, -0.1f), new Vector3(0, 1.25f, 0));
            sphere2.ObjectColor = Color.YellowGreen;


            //modelCollection.AddModel(centerSpher);
            modelCollection.MoveVibrator = new Vibrator(0.1f);
            modelCollection.AddModel(torus1);
            modelCollection.AddModel(torus2);
            modelCollection.AddModel(sphere1);
            modelCollection.AddModel(sphere2);
            modelCollection.AddModel(flyingTorus, true);

            modelCollection.cameraPosOrigin = new Vector3(10, 0, -5);

            // Light
            var pointLight = new PointLight(new Vector3(1, 1, 1), new Vector3(10, 0, -5), width, height);
            LightSingleton.AddLight(pointLight);
            
            //var spotLight = new SpotLight(new Vector3(1, 1, 1), new Vector3(2, 0, 0), new Vector3(-1, 0, 0), width, height, 2);
            //LightSingleton.AddLight(spotLight);


            return modelCollection;
        }

        public static ChessGame LoadBetterChess(int width, int height)
        {
            ChessGame chessGame = new ChessGame(width / height);
            chessGame.cameraPosOrigin = new Vector3(30, 0, -30);
            chessGame.cameraDistance = new Vector3(20, 0, -20);


            string objName = "Cube.obj";
            string pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var cube = FileReader.ReadObj(pathModel);

            var whiteColor = Color.Wheat;
            var blackColor = Color.Gray;

            var pieceWhite = Color.AntiqueWhite;
            var pieceBlack = Color.DarkGray;

            var initPos = 7f;
            var step = -2f;

            int numberOfSteps = 5;

            // Chessboard
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    var model = new Model(cube);
                    model.InitialPosition = new Vector3(initPos + i * step, initPos + j * step, 0);
                    model.ObjectColor = ((i + j) & 1) == 0 ? whiteColor : blackColor;
                    chessGame.AddModel(model);
                }
            }

            objName = "Pawn.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var pawn = FileReader.ReadObj(pathModel);
            pawn.Scale = 0.6f;

            // Pawn
            for(int i = 0; i < 8; i++)
            {
                // white
                var piece = new Pawn(pawn, 1, i, initPos, step, numberOfSteps, true);
                piece.ObjectColor = pieceWhite;
                chessGame.AddPiece(piece, i, 1);

                // black
                piece = new Pawn(pawn, 6, i, initPos, step, numberOfSteps, false);
                piece.ObjectColor = pieceBlack;
                chessGame.AddPiece(piece, i, 6);
            }

            // Rook

            objName = "Rook.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var rook = FileReader.ReadObj(pathModel);
            rook.Scale = 0.6f;


            rook.ObjectColor = pieceWhite;
            var rookWhite1 = new Rook(rook, 0, 0, initPos, step, numberOfSteps, true);
            var rookWhite2 = new Rook(rook, 0, 7, initPos, step, numberOfSteps, true);

            rook.ObjectColor = pieceBlack;
            var rookBlack1 = new Rook(rook, 7, 0, initPos, step, numberOfSteps, false);
            var rookBlack2 = new Rook(rook, 7, 7, initPos, step, numberOfSteps, false);

            chessGame.AddPiece(rookWhite1, 0, 0);
            chessGame.AddPiece(rookWhite2, 7, 0);
            chessGame.AddPiece(rookBlack1, 0, 7);
            chessGame.AddPiece(rookBlack2, 7, 7);

            // Bishop
            objName = "Bishop.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var bishop = FileReader.ReadObj(pathModel);
            bishop.Scale = 0.5f;


            bishop.ObjectColor = pieceWhite;
            var bishopWhite1 = new Bishop(bishop, 0, 2, initPos, step, numberOfSteps, true);
            var bishopWhite2 = new Bishop(bishop, 0, 5, initPos, step, numberOfSteps, true);

            bishop.ObjectColor = pieceBlack;
            var bishopBlack1 = new Bishop(bishop, 7, 2, initPos, step, numberOfSteps, false);
            var bishopBlack2 = new Bishop(bishop, 7, 5, initPos, step, numberOfSteps, false);

            chessGame.AddPiece(bishopWhite1, 2, 0);
            chessGame.AddPiece(bishopWhite2, 5, 0);
            chessGame.AddPiece(bishopBlack1, 2, 7);
            chessGame.AddPiece(bishopBlack2, 5, 7);

            // Knight
            objName = "Knight.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var knight = FileReader.ReadObj(pathModel);
            knight.Scale = 0.6f;


            knight.ObjectColor = pieceWhite;
            knight.Angle = new Vector3(0, 0, -MathF.PI / 2);
            var knightWhite1 = new Knight(knight, 0, 1, initPos, step, numberOfSteps, true);
            var knightWhite2 = new Knight(knight, 0, 6, initPos, step, numberOfSteps, true);

            knight.ObjectColor = pieceBlack;
            knight.Angle = new Vector3(0, 0, MathF.PI / 2);
            var knightBlack1 = new Knight(knight, 7, 1, initPos, step, numberOfSteps, false);
            var knightBlack2 = new Knight(knight, 7, 6, initPos, step, numberOfSteps, false);

            chessGame.AddPiece(knightWhite1, 1, 0);
            chessGame.AddPiece(knightWhite2, 6, 0);
            chessGame.AddPiece(knightBlack1, 1, 7);
            chessGame.AddPiece(knightBlack2, 6, 7);

            // Queen

            objName = "Queen.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var queen = FileReader.ReadObj(pathModel);
            queen.Scale = 0.5f;

            var queenWhite = new Queen(queen, 0, 4, initPos, step, numberOfSteps, true);
            queenWhite.ObjectColor = pieceWhite;
            chessGame.AddPiece(queenWhite, 4, 0);

            var queenBlack = new Queen(queen, 7, 4, initPos, step, numberOfSteps, false);
            queenBlack.ObjectColor = pieceBlack;
            chessGame.AddPiece(queenBlack, 4, 7);

            // King

            objName = "King.obj";
            pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            var king = FileReader.ReadObj(pathModel);
            king.Scale = 0.9f;
            
            var kingWhite = new King(king, 0, 3, initPos, step, numberOfSteps, true);
            kingWhite.ObjectColor = pieceWhite;
            chessGame.AddPiece(kingWhite, 3, 0);

            var kingBlack = new King(king, 7, 3, initPos, step, numberOfSteps, false);
            kingBlack.ObjectColor = pieceBlack;
            chessGame.AddPiece(kingBlack, 3, 7);

            // Moves

            // French - advanced

            //chessGame.AddMove("e4");
            //chessGame.AddMove("e6");
            //chessGame.AddMove("d4");
            //chessGame.AddMove("d5");
            //chessGame.AddMove("e5");

            // Danish gambit

            //chessGame.AddMove("e4");
            //chessGame.AddMove("e5");
            //chessGame.AddMove("d4");
            //chessGame.AddMove("exd4");

            // Scandinavian

            //chessGame.AddMove("e4");
            //chessGame.AddMove("d5");
            //chessGame.AddMove("exd5");

            //chessGame.AddMove("e4");
            //chessGame.AddMove("Nc6");
            //chessGame.AddMove("e5");
            //chessGame.AddMove("Nxe5");
            //chessGame.AddMove("a3");
            //chessGame.AddMove("Nc4");

            //chessGame.AddMove("b3");
            //chessGame.AddMove("b6");
            //chessGame.AddMove("Bb2");
            //chessGame.AddMove("Ba6");
            //chessGame.AddMove("Bxg7");
            //chessGame.AddMove("Bxg7");
            //chessGame.AddMove("Nf3");
            //chessGame.AddMove("Bxa1");

            //chessGame.AddMove("a4");
            //chessGame.AddMove("h5");
            //chessGame.AddMove("Ra3");
            //chessGame.AddMove("Rh6");
            //chessGame.AddMove("Rb3");
            //chessGame.AddMove("a5");
            //chessGame.AddMove("Rxb7");
            //chessGame.AddMove("Bxb7");
            //chessGame.AddMove("Nf3");
            //chessGame.AddMove("Bxc6");

            // Fools mate

            //chessGame.AddMove("e4");
            //chessGame.AddMove("e5");
            //chessGame.AddMove("Qh5");
            //chessGame.AddMove("Nc6");
            //chessGame.AddMove("Bc4");
            //chessGame.AddMove("h6");
            //chessGame.AddMove("Qxf7");

            //  Bongcloud
            //chessGame.AddMove("e4");
            //chessGame.AddMove("e5");
            //chessGame.AddMove("Ke2");
            //chessGame.AddMove("Ke7");

            var moves = "d4;Nf6;c4;g6;Nc3;Bg7;e4;d6;Nf3;O-O;Be2;e5;O-O;Nc6;d5;Ne7;Nd2;Ne8;b4;f5;c5;Nf6;f3;f4;Nc4;g5;a4;Ng6;Ba3;Rf7;b5;dxc5;Bxc5;h5;a5;g4;b6;g3;Kh1;Bf8;d6;axb6;Bg1;Nh4;Re1;Nxg2;dxc7;Nxe1;Qxe1;g2;Kxg2;Rg7;Kh1;Bh3;Bf1;Qd3;Nxe5;Bxf1;Qxf1;Qxc3;Rc1;Qxe5;c8=Q;Rxc8;Rxc8;Qe6";
            //var moves = "d4;d5;Nf3;c5;g3;Nc6;Bg2;Bg4;O-O;Nf6;h3;Bh5;g4;Bg6;Nh4;Be4;Ng6;Bxg2;f3;hxg6;Kxg2;e6;c3;Bd6;Be3;cxd4;Bf2;d3;exd3;Qc7;d4;Bh2;Nd2;Bf4;Qe2;Bxd2;Qxd2;O-O-O;Qc2;Na5;Qd3;Nc4;b3;Nd6;Rfc1;Nd7;a4;Nb6;c4;dxc4;bxc4;Nbxc4;Rc2;Qd7;Rac1;Kb8;Bg3;Rc8;Bxd6;Qxd6;Rxc4;Rxc4;Rxc4;Rc8;Qc3;Rxc4;Qxc4;Qc7;Qd3;Qb6;Qe3;Qb2;Kg3;Qb4;Qe5;Kc8;Qxg7;Qd6;Kg2;Qa3;Qxf7;Qa2;Kg3;Qxa4;Qxe6;Kc7;Qxg6;Qxd4;Qe4;Qd6;Qf4;Qxf4;Kxf4;a5;g5;a4;g6;a3;g7;a2;g8=Q;a1=Q;Qf7;Kb6;Qe6;Ka7;Qd7;Qa6;Qg4;Qd6;Kg5;Qc5;Kh4;Qf8;Kg3;Qh8;h4;Qe5;f4;Qe1;Kh3;Qe3";
            //var moves = "Nf3;h6;Na3;h5;Nc4;h4;Nfe5";
            //var moves = "h4;a5;h5;a4;h6;a3;hxg7;axb2;gxh8=Q;bxc1=R;Qg7;Rxd1;Kxd1";
            foreach (var move in moves.Split(";"))
            {
                chessGame.AddMove(move);
            }

            // Light
            var m = 20;
            var spotLightColor = Vector3.One / 2;
            var fx = 10;
            var fy = 10;
            var fz = -10;
            var spotLight = new SpotLight(spotLightColor, new Vector3(fx, fy, fz), new Vector3(-1, -1, 2), new Vector3(0, 0, -0.1f), width, height, m);
            LightSingleton.AddLight(spotLight);
            spotLight =     new SpotLight(spotLightColor, new Vector3(-fx, fy, fz), new Vector3(1, -1, 2), new Vector3(0, 0, -0.1f), width, height, m);
            LightSingleton.AddLight(spotLight);
            spotLight =     new SpotLight(spotLightColor, new Vector3(fx, -fy, fz), new Vector3(-1, 1, 2), new Vector3(0, 0, -0.1f), width, height, m);
            LightSingleton.AddLight(spotLight);
            spotLight =     new SpotLight(spotLightColor, new Vector3(-fx, -fy, fz), new Vector3(1, 1, 2), new Vector3(0, 0, -0.1f), width, height, m);
            LightSingleton.AddLight(spotLight);

            //var spotLight = new SpotLight(spotLightColor, new Vector3(10, 0, -10), new Vector3(-1, 0, 1), new Vector3(0, 0, 0.1f), width, height, m);
            //LightSingleton.AddLight(spotLight);

            var pointLight = new PointLight(new Vector3(1, 1, 1), new Vector3(0, 0, -10), width, height);
            LightSingleton.AddLight(pointLight);

            //pointLight = new PointLight(new Vector3(1, 1, 1) / 5, new Vector3(-m, -20, -m), width, height);
            //LightSingleton.AddLight(pointLight);
            //pointLight = new PointLight(new Vector3(1, 1, 1) / 5, new Vector3(-m, -20, m), width, height);
            //LightSingleton.AddLight(pointLight);
            //pointLight = new PointLight(new Vector3(1, 1, 1) / 5, new Vector3(m, -20, -m), width, height);
            //LightSingleton.AddLight(pointLight);
            //pointLight = new PointLight(new Vector3(1, 1, 1) / 5, new Vector3(m, -20, m), width, height);
            //LightSingleton.AddLight(pointLight);

            //objName = "FullTorus.obj";
            //pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
            //var torus = FileReader.ReadObj(pathModel);
            //torus.Mover = new CircleMover(10, 0.1f, 0.5f);
            //torus.AngleStep = Vector3.One / 10;
            //torus.ObjectColor = Color.Red;
            //chessGame.AddModel(torus);

            return chessGame;
        }

    }
}
