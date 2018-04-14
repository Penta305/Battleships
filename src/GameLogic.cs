using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+,

namespace Battleship
{
    public class GameLogic
    {
        public void Main()
        {
            // Opens up a new graphics window

            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);

            GameResources.LoadResources();
            SwinGame.PlayMusic(GameMusic("Background"));
            do
            {
                GameController.HandleUserInput();
                GameController.DrawScreen();
            }
            while (!SwinGame.WindowCloseRequested() == true | CurrentState == GameState.Quitting);

            SwinGame.StopMusic();

            // Free resources and close audio to end the program.
            
            GameResources.FreeResources();
        }
    }
}
