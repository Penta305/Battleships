using System;
using System.Diagnostics;
using SwinGameSDK;

namespace MyGame
{

    public class BallH : MovableObject
    {
        private static int _radius = 10;
        private bool _limit = false;


        public BallH(float xspeed) : base(400, 300, xspeed, 5, Color.White)
        {

        }

        public float bounce(float n)
        {
            return n * (-1);
        }

        public void IncreaseSpeed()
        {
            if (YSpeed > 0)
                YSpeed += 3;
            else if (YSpeed < 0)
                YSpeed -= 3;
        }

        public void DecreaseSpeed()
        {
            float oldSpeed = YSpeed;

            if (YSpeed > 0)
                YSpeed -= 1;
            else if (YSpeed < 0)
                YSpeed += 1;

            if ((int)YSpeed == 0)
            {
                if (oldSpeed < 0)
                    YSpeed = -1;
                if (oldSpeed > 0)
                    YSpeed = 1;
            }

        }

        public bool limit
        {
            get
            {
                return _limit;
            }
        }

        public override void Draw()
        {
            SwinGame.FillCircle(Color, xloc, yloc, _radius);
        }


        public override void CheckCollision()
        {


            if (xloc + _radius > HardPlayingField.SW || xloc - _radius < 0 || yloc + _radius > HardPlayingField.SH || yloc - _radius < 0)
                _limit = true;


            foreach (WallHard w in HardPlayingField.Walls)
            {



                if (SwinGame.PointInRect(xloc - _radius, yloc, w.xloc, w.yloc, w.Width, w.Height))

                {
                    xloc = _radius + w.xloc + w.Width + 1;
                    XSpeed = bounce(XSpeed);
                    w.DecreaseHealth();
                }

                else if (SwinGame.PointInRect(xloc + _radius, yloc, w.xloc, w.yloc, w.Width, w.Height))

                {
                    xloc = w.xloc - _radius - 1;
                    XSpeed = bounce(XSpeed);
                    w.DecreaseHealth();
                }

                else if (SwinGame.PointInRect(xloc, yloc - _radius, w.xloc, w.yloc, w.Width, w.Height))

                {
                    yloc = w.yloc + w.Height + _radius + 1;
                    YSpeed = bounce(YSpeed);
                    w.DecreaseHealth();
                }

                else if (SwinGame.PointInRect(xloc, yloc + _radius, w.xloc, w.yloc, w.Width, w.Height))

                {
                    yloc = w.yloc - _radius - 1;
                    YSpeed = bounce(YSpeed);
                    w.DecreaseHealth();
                }
            }

            foreach (BrickHard b in HardPlayingField.Bricks)
            {

                if (SwinGame.PointInRect(xloc - _radius, yloc, b.xloc, b.yloc, b.Width, b.Height))
                {
                    xloc = _radius + b.xloc + b.Width + 1;
                    XSpeed = bounce(XSpeed);
                    b.DecreaseHealth();
                }

                else if (SwinGame.PointInRect(xloc + _radius, yloc, b.xloc, b.yloc, b.Width, b.Height))
                {
                    xloc = b.xloc - _radius - 1;
                    XSpeed = bounce(XSpeed);
                    b.DecreaseHealth();
                }

                else if (SwinGame.PointInRect(xloc, yloc - _radius, b.xloc, b.yloc, b.Width, b.Height))
                {
                    yloc = b.yloc + b.Height + _radius + 1;
                    YSpeed = bounce(YSpeed);
                    b.DecreaseHealth();
                }

                else if (SwinGame.PointInRect(xloc, yloc + _radius, b.xloc, b.yloc, b.Width, b.Height))
                {
                    yloc = b.yloc - _radius - 1;
                    YSpeed = bounce(YSpeed);
                    b.DecreaseHealth();
                }
            }


            float Xtemp = XSpeed;
            float Ytemp = YSpeed;
            if (SwinGame.PointInRect(xloc, yloc+_radius, HardPlayingField.myPlayer.xloc, HardPlayingField.myPlayer.yloc, HardPlayingField.myPlayer.Width, HardPlayingField.myPlayer.Height))
            {
                float ballFactor = (2 * (xloc - HardPlayingField.myPlayer.xloc) - HardPlayingField.myPlayer.Width) / HardPlayingField.myPlayer.Width;
                XSpeed = XSpeed + Math.Abs(XSpeed) * ballFactor;
                YSpeed = (float)-(Math.Sqrt(Xtemp * Xtemp + Ytemp * Ytemp - XSpeed * XSpeed));
                

            }
            
        }

        public void ResetLocation()
        {
            xloc = 400;
            yloc = 300;
            XSpeed = 0;
        }

       

        public static int Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
            }
        }

        
        public static void ChangeSize()
        {
                Radius = Radius + Radius/2;
             
        }


        public static void SmallBall()
        {
           
                Radius = Radius - Radius/5;
                
            
        }
    }

}

