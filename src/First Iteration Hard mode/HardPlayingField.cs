using System;
using System.IO;
using SwinGameSDK;
using System.Collections.Generic;

namespace MyGame
{
    class HardPlayingField
    {
        public static int SW = 800;
        public static int SH = 600;

        public static List<BrickHard> _Bricks = new List<BrickHard>();
        public static List<WallHard> _Walls = new List<WallHard>();
        public static Player myPlayer = new Player();


        public static Color BC = Color.Red;
        public static Color BOC = Color.DarkRed;

        public static Random myRandom = new Random();
        public static Random myRandom2 = new Random();
        public static Random healthRandom = new Random();
        private static float randomXDirection = myRandom.Next(-2, 3);
        public static BallH myBall = new BallH(randomXDirection);
        int i;
        public static void GenerateBricks()
        {

            for (int i = WallHard.sideL * 2; i <= SW - WallHard.sideL * 2 - BrickHard.BrickW; i = i + BrickHard.BrickW)
            {
                BrickHard newBrick = new BrickHard(i, WallHard.sideL * 2);
                newBrick.Health = healthRandom.Next(1, 5);
                newBrick.Special = myRandom2.Next(1, 3);

                switch (newBrick.Special)
                {
                    case 1:
                        newBrick.STrue = true;
                        newBrick.Color = Color.Blue;
                        newBrick.OutlineColor = Color.Black;
                        break;
                    case 2:

                        newBrick.BTrue = true;
                        newBrick.OutlineColor = Color.Black;
                        break;
                    default:
                        newBrick.BTrue = true;
                        newBrick.OutlineColor = Color.Black;
                        break;
                }
                _Bricks.Add(newBrick);

            }

            for (int i = WallHard.sideL * 2; i <= SW - WallHard.sideL * 2 - BrickHard.BrickW; i = i + BrickHard.BrickW)
            {
                BrickHard newBrick1 = new BrickHard(i, WallHard.sideL * 2 + BrickHard.BrickH);
                newBrick1.Health = healthRandom.Next(1, 5);
                newBrick1.Special = myRandom2.Next(1, 3);

                switch (newBrick1.Special)
                {
                    case 1:
                        newBrick1.STrue = true;
                        newBrick1.Color = Color.Blue;
                        newBrick1.OutlineColor = Color.Black;
                        break;
                    case 2:
                        newBrick1.BTrue = true;
                        newBrick1.OutlineColor = Color.Black;
                        break;
                    default:
                        newBrick1.BTrue = true;
                        newBrick1.OutlineColor = Color.Black;
                        break;
                }

                _Bricks.Add(newBrick1);


            }

            for (int i = WallHard.sideL * 2; i <= SW - WallHard.sideL * 2 - BrickHard.BrickW; i = i + BrickHard.BrickW)
            {
                BrickHard newBrick2 = new BrickHard(i, WallHard.sideL * 2 + BrickHard.BrickH * 2);
                newBrick2.Health = healthRandom.Next(1, 2);
                newBrick2.Special = myRandom2.Next(1, 3);

                switch (newBrick2.Special)
                {
                    case 1:
                        newBrick2.STrue = true;
                        newBrick2.Color = Color.Blue;
                        newBrick2.OutlineColor = Color.Black;
                        break;
                    case 2:

                        newBrick2.BTrue = true;
                        newBrick2.OutlineColor = Color.Black;
                        break;
                    default:
                        newBrick2.BTrue = true;
                        newBrick2.OutlineColor = Color.Black;
                        break;
                }
                _Bricks.Add(newBrick2);


            }

            for (int i = WallHard.sideL * 2; i <= SW - WallHard.sideL * 2 - BrickHard.BrickW; i = i + BrickHard.BrickW)
            {
                BrickHard newBrick3 = new BrickHard(i, WallHard.sideL * 2 + BrickHard.BrickH * 3);
                newBrick3.Health = healthRandom.Next(1, 2);
                newBrick3.Special = myRandom2.Next(1, 3);
              
                switch (newBrick3.Special)
                {
                    case 1:
                        newBrick3.STrue = true;
                        newBrick3.Color = Color.Blue;
                        newBrick3.OutlineColor = Color.Black;
                        break;
                    case 2:
                      
                        newBrick3.BTrue = true;
                        newBrick3.OutlineColor = Color.Black;
                        break;
                    default:
                        newBrick3.BTrue = true;
                        newBrick3.OutlineColor = Color.Black;
                        break;
                }
                _Bricks.Add(newBrick3);

            }

            List<float[]> Positions = new List<float[]>();
            foreach (BrickHard b in Bricks)
            {
                Positions.Add(new float[] { b.xloc, b.yloc });
            }
        }
        public static Random random = new Random();

        public static List<WallHard> Walls
        {
            get
            {
                return _Walls;
            }
        }

        public static List<BrickHard> Bricks
        {
            get
            {
                return _Bricks;
            }
        }

        public static void DisplayResetBricksScreen()
        {
            SwinGame.ClearScreen(Color.Black);
            SwinGame.DrawText("Loading New Level....", Color.White, 300, 300);
            SwinGame.RefreshScreen();
        }

        public static void DrawField()
        {
            SwinGame.ClearScreen(Color.Black);

            myBall.Draw();
            myPlayer.Draw();

            foreach (WallHard w in _Walls)
            {
                w.Draw();
            }

            foreach (BrickHard b in _Bricks)
            {
                b.Draw();
            }

            SwinGame.DrawText("Score: " + myPlayer.Points, Color.White, 800 - 150, 600 - 20);
            SwinGame.RefreshScreen(60);
        }

        public static void ProcessMovement()
        {
            myBall.Move();
            myBall.CheckCollision();
            myPlayer.CheckCollision();

        }

        public static void ProcessInput()
        {
            if (SwinGame.KeyDown(KeyCode.RightKey))
                myPlayer.MoveRight();

            if (SwinGame.KeyDown(KeyCode.LeftKey))
                myPlayer.MoveLeft();
        }

        public static void CheckHealthOfField()
        {

            foreach (BrickHard b in Bricks)
            {
                b.CheckHealth();
            }
        }

        public static int NumberOfBricks
        {
            get
            {
                return Bricks.Count;
            }
        }

        public static void GenerateWalls()
        {

            for (int i = 0; i <= SW - WallHard.sideL; i = i + WallHard.sideL)
            {
                _Walls.Add(new WallHard(i, 0));
            }


            for (int i = WallHard.sideL; i <= SH - WallHard.sideL; i = i + WallHard.sideL)
            {
                _Walls.Add(new WallHard(0, i));
            }


            for (int i = WallHard.sideL; i <= SH - WallHard.sideL; i = i + WallHard.sideL)
            {
                _Walls.Add(new WallHard(SW - WallHard.sideL, i));
            }
        }


        public static void DeleteBrick(BrickHard b)
        {
            if(b.BTrue == true)
            {
                BallH.ChangeSize();
                myPlayer.ChangeSizeSmaller();
            }
            if(b.STrue == true)
            {
                BallH.SmallBall();
                myPlayer.ChangeSizeBigger();

            }
            List<BrickHard> NewBricks = new List<BrickHard>();
            foreach (BrickHard brick in Bricks)
            {
                if (brick != b)
                    NewBricks.Add(brick);
            }
            _Bricks = NewBricks;
        }

        public static void DeleteWall(WallHard w)
        {
            List<WallHard> NewWalls = new List<WallHard>();
            foreach (WallHard wall in Walls)
            {
                if (wall != w)
                    NewWalls.Add(wall);
            }
            _Walls = NewWalls;

        }

        public static bool CheckGameOver()
        {
            return myBall.limit;
        }

        public static void DisplayGameOver()
        {
            SwinGame.ClearScreen(Color.Black);
            SwinGame.DrawText("Game Over! Points scored: " + myPlayer.Points, Color.White, 300, 300);
            SwinGame.RefreshScreen();
        }


        public static void ResetBricks()
        {
            _Bricks.Clear();
            GenerateBricks();
            DrawField();

        }




    }
}
