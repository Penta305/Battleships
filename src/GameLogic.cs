using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+,

namespace Battleship
{
    public class GameLogic
    {
        public static void Main()
        {
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);

            GameResources.LoadResources();
            SwinGame.PlayMusic(GameResources.GameMusic("Background"));
            do
            {
                GameController.HandleUserInput();
                GameController.DrawScreen();
            }
            while (!SwinGame.WindowCloseRequested() == true | GameController.GameCurrentState == GameState.Quitting);
            SwinGame.StopMusic();
            GameResources.FreeResources();
        }
    }
}
//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by Refactoring Essentials.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
