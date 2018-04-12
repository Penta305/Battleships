using System;
using SwinGameSDK;
using Battleship;

// The DeploymentController controls the players actions
// during the deployment phase, allowing them to place
// ships into their preferred positions for the game.


namespace Battleship
{
    class DeploymentController
    {

        private const int SHIPS_TOP = 98;

        private const int SHIPS_LEFT = 20;

        private const int SHIPS_HEIGHT = 90;

        private const int SHIPS_WIDTH = 300;

        private const int TOP_BUTTONS_TOP = 72;

        private const int TOP_BUTTONS_HEIGHT = 46;

        private const int PLAY_BUTTON_LEFT = 693;

        private const int PLAY_BUTTON_WIDTH = 80;

        private const int UP_DOWN_BUTTON_LEFT = 410;

        private const int LEFT_RIGHT_BUTTON_LEFT = 350;

        private const int RANDOM_BUTTON_LEFT = 547;

        private const int RANDOM_BUTTON_WIDTH = 51;

        private const int DIR_BUTTONS_WIDTH = 47;

        private const int TEXT_OFFSET = 5;

        private Direction _currentDirection = Direction.UpDown;

        private ShipName _selectedShip = ShipName.Tug;

 
        // Handles the user input for the Deployment phase of the game.
  
        // Includes options for selecting each ship, deloying each ship,
        // changing the direction, each ship is facing, random deployment,
        // and ending deployment (starting the game).

        public void HandleDeploymentInput()
        {
            if (SwinGame.KeyTyped(KeyCode.EscapeKey))
            {
                AddNewState(GameState.ViewingGameMenu);
            }

            if ((SwinGame.KeyTyped(KeyCode.UpKey) || SwinGame.KeyTyped(KeyCode.DownKey)))
            {
                _currentDirection = Direction.UpDown;
            }

            if ((SwinGame.KeyTyped(KeyCode.LeftKey) || SwinGame.KeyTyped(KeyCode.RightKey)))
            {
                _currentDirection = Direction.LeftRight;
            }

            if (SwinGame.KeyTyped(KeyCode.RightKey))
            {
                HumanPlayer.RandomizeDeployment();
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                ShipName selected;
                selected = DeploymentController.GetShipMouseIsOver();
                if ((selected != ShipName.None))
                {
                    _selectedShip = selected;
                }
                else
                {
                    DeploymentController.DoDeployClick();
                }

                if ((HumanPlayer.ReadyToDeploy && IsMouseInRectangle(PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP, PLAY_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT)))
                {
                    EndDeployment();
                }
                else if (IsMouseInRectangle(UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    _currentDirection = Direction.LeftRight;
                }
                else if (IsMouseInRectangle(LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    _currentDirection = Direction.LeftRight;
                }
                else if (IsMouseInRectangle(RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP, RANDOM_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    HumanPlayer.RandomizeDeployment();
                }

            }

        }

        // If the user has clicked somewhere on the screen, check if its a deployment
        // and, if it's true, deploy the currently selected ship with the
        // direction indicated by the controller.

        private static void DoDeployClick()
        {
            Point2D mouse;
            mouse = SwinGame.MousePosition();

            // Calculate the row and column selected

            int row;
            int col;
            row = Convert.ToInt32(Math.Floor((mouse.Y
                                / (CELL_HEIGHT + CELL_GAP))));
            col = Convert.ToInt32(Math.Floor(((mouse.X - FIELD_LEFT)
                                / (CELL_WIDTH + CELL_GAP))));
            if (((row >= 0)
                        && (row < HumanPlayer.PlayerGrid.Height)))
            {
                if (((col >= 0)
                            && (col < HumanPlayer.PlayerGrid.Width)))
                {

                    // If the click is within the area, try to deploy

                    try
                    {
                        HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
                    }
                    catch (Exception ex)
                    {
                        Audio.PlaySoundEffect(GameSound("Error"));
                        Message = ex.Message;
                    }

                }

            }

        }

        // Draws the deployment screen showing the field and the ships
        // that the player can deploy.

        public static void DrawDeployment()
        {
            DrawField(HumanPlayer.PlayerGrid, HumanPlayer, true);
            
            // Draw the Left/Right and Up/Down buttons

            if ((_currentDirection == Direction.LeftRight))
            {
                SwinGame.DrawBitmap(GameImage("LeftRightButton"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);
                
                // CHECK
                // SwinGame.DrawText("U/D", Color.Gray, GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                // SwinGame.DrawText("L/R", Color.White, GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
                // CHECK
            }
            else
            {
                SwinGame.DrawBitmap(GameImage("UpDownButton"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);
                
                // CHECK
                // SwinGame.DrawText("U/D", Color.White, GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                // SwinGame.DrawText("L/R", Color.Gray, GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)

            }
            
            // CHECK
            // DrawShips
            foreach (ShipName sn in Enum.GetValues(typeof(ShipName)))
            {
                int i;
                i = (Int(sn) - 1);
                if ((i >= 0))
                {
                    if ((sn == _selectedShip))
                    {
                        SwinGame.DrawBitmap(GameImage("SelectedShip"), SHIPS_LEFT, (SHIPS_TOP
                                        + (i * SHIPS_HEIGHT)));
                        
                        // CHECK
                        //     SwinGame.FillRectangle(Color.LightBlue, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                        // Else
                        //     SwinGame.FillRectangle(Color.Gray, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)

                    }

                    // CHECK
                    // SwinGame.DrawRectangle(Color.Black, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                    // SwinGame.DrawText(sn.ToString(), Color.Black, GameFont("Courier"), SHIPS_LEFT + TEXT_OFFSET, SHIPS_TOP + i * SHIPS_HEIGHT)
                    // CHECK

                }

            }

            if (HumanPlayer.ReadyToDeploy)
            {
                SwinGame.DrawBitmap(GameImage("PlayButton"), PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP);
                
                //CHECK
                // SwinGame.FillRectangle(Color.LightBlue, PLAY_BUTTON_LEFT, PLAY_BUTTON_TOP, PLAY_BUTTON_WIDTH, PLAY_BUTTON_HEIGHT)
                // SwinGame.DrawText("PLAY", Color.Black, GameFont("Courier"), PLAY_BUTTON_LEFT + TEXT_OFFSET, PLAY_BUTTON_TOP)
            }

            SwinGame.DrawBitmap(GameImage("RandomButton"), RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP);
            DrawMessage();
        }

        // Selects the ship that the mouse is currently hovered over
        // in the selection panel.

        private static ShipName GetShipMouseIsOver()
        {
            foreach (ShipName sn in Enum.GetValues(typeof(ShipName)))
            {
                int i;
                i = (Int(sn) - 1);
                if (IsMouseInRectangle(SHIPS_LEFT, (SHIPS_TOP
                                + (i * SHIPS_HEIGHT)), SHIPS_WIDTH, SHIPS_HEIGHT))
                {
                    return sn;
                }

            }

            return ShipName.None;
        }
    }
}