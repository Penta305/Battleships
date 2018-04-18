using System;
using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+, 

// The DiscoveryController controls the battle phase.

namespace Battleship
{
    public class DiscoveryController
    {

        // Handles the user input during the discovery phase.

        // Escape opens the game menu. Clicking the mouse will launch
        // an attack at that location.

        public void HandleDiscoveryInput()
        {
            if (SwinGame.KeyTyped(KeyCode.EscapeKey))
            {
                GameController.AddNewState(GameState.ViewingGameMenu);
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                DoAttack();
            }
        }

        // Launches an attack at the current mouse position.

        private void DoAttack()
        {
            Point2D mouse;
            mouse = SwinGame.MousePosition();

            // Calculate the row and column selected.

            int row, col;
            row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
            col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));
            if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height)
            {
                if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width)
                {
                    GameController.Attack(row, col);
                }
            }
        }


        // Draws the game field and interface during the attack phase.

        public void DrawDiscovery()

        {
            GameController _DrawDiscovery = new GameController();
            const int SCORES_LEFT = 172;
            const int SHOTS_TOP = 157;
            const int HITS_TOP = 206;
            const int SPLASH_TOP = 256;
            if ((SwinGame.KeyDown(KeyCode.LeftShiftKey) | SwinGame.KeyDown(KeyCode.RightShiftKey)) & SwinGame.KeyDown(KeyCode.CKey))
            {
                UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, _DrawDiscovery.ComputerPlayer, true);
            }
            else
            {
                UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, _DrawDiscovery.ComputerPlayer, false);
            }

            UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
            UtilityFunctions.DrawMessage();
            SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
            SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
            SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
        }
    }

}
