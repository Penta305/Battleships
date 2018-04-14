using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+, 

// The EndingGameController is responsible for managing the interactions
// with the system at the end of the game.

namespace Battleship
{
    public class EndingGameController
    {
        // This draws the end of the game screen, which shows whether
        // the player wins or loses.

        public void DrawEndOfGame()
        {
            Rectangle toDraw;
            string whatShouldIPrint;

            DrawField(ComputerPlayer.PlayerGrid, ComputerPlayer, true);
            DrawSmallField(HumanPlayer.PlayerGrid, HumanPlayer);

            toDraw.X = 0;
            toDraw.Y = 250;
            toDraw.Width = SwinGame.ScreenWidth();
            toDraw.Height = SwinGame.ScreenHeight();

            if (HumanPlayer.IsDestroyed)
            {
                whatShouldIPrint = "YOU LOSE!";
            }
            else
            {
                whatShouldIPrint = "-- WINNER --";
            }

            SwinGame.DrawTextLines(whatShouldIPrint, Color.White, Color.Transparent, GameResources.GameFont("ArialLarge"), FontAlignment.AlignCenter, toDraw);
        }

        // This handles the player input at the end of the game. Any input
        // with the system will result in the display of the high scores
        // page being shown
        
        public void HandleEndOfGameInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.VK_RETURN) || SwinGame.KeyTyped(KeyCode.VK_ESCAPE))
            {
                ReadHighScore(HumanPlayer.Score);
                EndCurrentState();
            }
        }
    }
}