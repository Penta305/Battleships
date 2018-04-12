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
                AddNewState(GameState.ViewingGameMenu);
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
            row = Convert.ToInt32(Math.Floor((mouse.Y - FIELD_TOP) / (CELL_HEIGHT + CELL_GAP)));
            col = Convert.ToInt32(Math.Floor((mouse.X - FIELD_LEFT) / (CELL_WIDTH + CELL_GAP)));
            if (row >= 0 & row < HumanPlayer.EnemyGrid.Height)
            {
                if (col >= 0 & col < HumanPlayer.EnemyGrid.Width)
                {
                    Attack(row, col);
                }
            }
        }

        // Draws the game field and interface during the attack phase.

        public void DrawDiscovery()
        {
            const int SCORES_LEFT = 172;
            const int SHOTS_TOP = 157;
            const int HITS_TOP = 206;
            const int SPLASH_TOP = 256;
            if ((SwinGame.KeyDown(KeyCode.LeftShiftKey) | SwinGame.KeyDown(KeyCode.RightShiftKey)) & SwinGame.KeyDown(KeyCode.CKey))
            {
                DrawField(HumanPlayer.EnemyGrid, ComputerPlayer, true);
            }
            else
            {
                DrawField(HumanPlayer.EnemyGrid, ComputerPlayer, false);
            }

            DrawSmallField(HumanPlayer.PlayerGrid, HumanPlayer);
            DrawMessage();
            SwinGame.DrawText(HumanPlayer.Shots.ToString(), Color.White, GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
            SwinGame.DrawText(HumanPlayer.Hits.ToString(), Color.White, GameFont("Menu"), SCORES_LEFT, HITS_TOP);
            SwinGame.DrawText(HumanPlayer.Missed.ToString(), Color.White, GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
        }
    }

}