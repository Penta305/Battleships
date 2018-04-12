using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+,

namespace Battleship
{
    public class GameLogic
    {
        public void Main()
        {
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
