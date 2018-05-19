using System;
using SwinGameSDK;

namespace MyGame
{
    public class WallHard : StationaryObject
    {
        private const int _sideL = 50;
        private const int _health = 10;


        public WallHard(int x, int y) : base(x, y, _sideL, _sideL, _health, Color.Blue, Color.White)
        {
        }

        public override void Draw()
        {

            SwinGame.DrawRectangle(OutlineColor, xloc, yloc, _sideL, _sideL);

        }

        public override void CheckHealth()
        {
            if (Health <= 0)
                HardPlayingField.DeleteWall(this);

        }

        public static int sideL
        {
            get
            {
                return _sideL;
            }
        }


    }
}
