using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+,

namespace Battleship
{
    public class GameLogic
    {
        public static void Main()
        {
            // Opens up a new graphics window
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);
            
            GameResources.LoadResources();
            SwinGame.PlayMusic(GameResources.GameMusic("Background"));
            do
            {
                GameController.HandleUserInput();
                GameController.DrawScreen();
                if (GameController.CurrentState == GameState.Quitting)
                {
                    break;
                }
            }
            while (!SwinGame.WindowCloseRequested() == true | GameController.CurrentState == GameState.Quitting);
            SwinGame.StopMusic();
            GameResources.FreeResources();
        }
    }
}