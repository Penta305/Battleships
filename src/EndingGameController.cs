using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+, 

namespace Battleship
{
    public class EndingGameController
    {
        public void DrawEndOfGame()
        {
            GameController _DrawEndOfGame = new GameController();
            Rectangle toDraw = new Rectangle();
            string whatShouldIPrint;
            UtilityFunctions.DrawField(_DrawEndOfGame.ComputerPlayer.PlayerGrid, _DrawEndOfGame.ComputerPlayer, true);
            UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
            toDraw.X = 0;
            toDraw.Y = 250;
            toDraw.Width = ScreenWidth();
            toDraw.Height = ScreenHeight();
            if (GameController.HumanPlayer.IsDestroyed)
            {
                whatShouldIPrint = "YOU LOSE!";
            }
            else
            {
                whatShouldIPrint = "-- WINNER --";
            }
            
            DrawText(whatShouldIPrint, Color.White, Color.Transparent, GameResources.GameFont("ArialLarge"), FontAlignment.AlignCenter, toDraw);
        }

        public void HandleEndOfGameInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.ReturnKey) || SwinGame.KeyTyped(KeyCode.EscapeKey))
            {
                HighScoreController.ReadHighScore(GameController.HumanPlayer.Score);
                GameController.EndCurrentState();
            }
        }
    }
    //=======================================================
    //Service provided by Telerik (www.telerik.com)
    //Conversion powered by Refactoring Essentials.
    //Twitter: @telerik
    //Facebook: facebook.com/telerik
    //=======================================================
}