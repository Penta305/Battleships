using System;
using SwinGameSDK;
namespace MyGame
{
    public class BrickHard : StationaryObject
    {
        private const int _BrickW = 100;
        private const int _BrickH = 20;
        private const int _health = 2;
        private int _special;
        private int _points = 200;
        private  bool _bTrue = false;
        private bool _sTrue = false;
        public BrickHard(float x, float y) : base(x, y, _BrickW, _BrickH, _health, HardPlayingField.BC, HardPlayingField.BOC)
        {
        }


        public override void Draw()
        {
            SwinGame.FillRectangle(Color, xloc, yloc, Width, Height);
            SwinGame.DrawRectangle(OutlineColor, xloc, yloc, Width, Height);
        }

        public override void CheckHealth()
        {
            if (Health <= 0)
            {
                HardPlayingField.DeleteBrick(this);
                HardPlayingField.myPlayer.AddToPoints(_points);


                int i = HardPlayingField.random.Next(1, 101);

            }
        }

        public static int BrickW
        {
            get
            {
                return _BrickW;
            }
        }

        public static int BrickH
        {
            get
            {
                return _BrickH;
            }
        }

        public  bool BTrue
        {
            get
            {
                return _bTrue;
            }
            set
            {
                _bTrue = value;
            }
        }

        public bool STrue
        {
            get
            {
                return _sTrue;
            }
            set
            {
                _sTrue = value;
            }
        }
        public int Special
        {
            get
            {
                return _special;
            }
            set
            {
                _special = value;
            }
        }
    }
}