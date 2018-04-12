using System;
using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+, 

namespace Battleship
{
    public class DiscoveryController
    {
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

        private void DoAttack()
        {
            Point2D mouse;
            mouse = SwinGame.MousePosition();
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
    //=======================================================
    //Service provided by Telerik (www.telerik.com)
    //Conversion powered by Refactoring Essentials.
    //Twitter: @telerik
    //Facebook: facebook.com/telerik
    //=======================================================
}