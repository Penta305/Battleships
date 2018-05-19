using System;
using System.Threading;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;


namespace MyGame
{
    class HardGameRun
    {
        public static void Run()
        {
            //Open the game window


            //make game objects

            //draw the field




            //Run the game loop

            //Fetch the next batch of UI interaction
            SwinGame.ProcessEvents();


            HardPlayingField.DrawField();



            HardPlayingField.ProcessInput();
            HardPlayingField.ProcessMovement();


            HardPlayingField.CheckHealthOfField();


            if (HardPlayingField.NumberOfBricks <= 0)
            {
                HardPlayingField.ResetBricks();
                HardPlayingField.myBall.ResetLocation();
                HardPlayingField.myBall.IncreaseSpeed();
                HardPlayingField.DisplayResetBricksScreen();
                Thread.Sleep(1000);
                HardPlayingField.DrawField();
                Thread.Sleep(1000);
            }




        }
    }
}
