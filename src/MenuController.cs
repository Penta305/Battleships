using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+,

namespace Battleship
{
    public class MenuController
    {
        private readonly string[][] _menuStructure = { new string[] { "PLAY", "SETUP", "SCORES", "QUIT" }, new string[] { "RETURN", "SURRENDER", "QUIT" }, new string[] { "EASY", "MEDIUM", "HARD" } };
        private const int MENU_TOP = 575;
        private const int MENU_LEFT = 30;
        private const int MENU_GAP = 0;
        private const int BUTTON_WIDTH = 75;
        private const int BUTTON_HEIGHT = 15;
        private const int BUTTON_SEP = BUTTON_WIDTH + MENU_GAP;
        private const int TEXT_OFFSET = 0;
        private const int MAIN_MENU = 0;
        private const int GAME_MENU = 1;
        private const int SETUP_MENU = 2;
        private const int MAIN_MENU_PLAY_BUTTON = 0;
        private const int MAIN_MENU_SETUP_BUTTON = 1;
        private const int MAIN_MENU_TOP_SCORES_BUTTON = 2;
        private const int MAIN_MENU_QUIT_BUTTON = 3;
        private const int SETUP_MENU_EASY_BUTTON = 0;
        private const int SETUP_MENU_MEDIUM_BUTTON = 1;
        private const int SETUP_MENU_HARD_BUTTON = 2;
        private const int SETUP_MENU_EXIT_BUTTON = 3;
        private const int GAME_MENU_RETURN_BUTTON = 0;
        private const int GAME_MENU_SURRENDER_BUTTON = 1;
        private const int GAME_MENU_QUIT_BUTTON = 2;
        private readonly Color MENU_COLOR = SwinGame.RGBAColor(2, 167, 252, 255);
        private readonly Color HIGHLIGHT_COLOR = SwinGame.RGBAColor(1, 57, 86, 255);
        public void HandleMainMenuInput()
        {
            HandleMenuInput(MAIN_MENU, 0, 0);
        }

        public void HandleSetupMenuInput()
        {
            bool handled;
            handled = HandleMenuInput(SETUP_MENU, 1, 1);
            if (!handled)
            {
                HandleMenuInput(MAIN_MENU, 0, 0);
            }
        }

        public void HandleGameMenuInput()
        {
            HandleMenuInput(GAME_MENU, 0, 0);
        }

        private bool HandleMenuInput(int menu, int level, int xOffset)
        {
            if (SwinGame.KeyTyped(KeyCode.VK_ESCAPE))
            {
                EndCurrentState();
                return true;
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                int i;
                for (i = 0; i <= _menuStructure(menu).Length - 1; i++)
                {
                    if (IsMouseOverMenu(i, level, xOffset))
                    {
                        PerformMenuAction(menu, i);
                        return true;
                    }
                }

                if (level > 0)
                {
                    EndCurrentState();
                }
            }

            return false;
        }

        public void DrawMainMenu()
        {
            DrawButtons(MAIN_MENU);
        }

        public void DrawGameMenu()
        {
            DrawButtons(GAME_MENU);
        }

        public void DrawSettings()
        {
            DrawButtons(MAIN_MENU);
            DrawButtons(SETUP_MENU, 1, 1);
        }

        private void DrawButtons(int menu)
        {
            DrawButtons(menu, 0, 0);
        }

        private void DrawButtons(int menu, int level, int xOffset)
        {
            int btnTop;
            Rectangle toDraw;
            btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
            int i;
            for (i = 0; i <= _menuStructure(menu).Length - 1; i++)
            {
                int btnLeft;
                btnLeft = MENU_LEFT + BUTTON_SEP * (i + xOffset);
                toDraw.X = btnLeft + TEXT_OFFSET;
                toDraw.Y = btnTop + TEXT_OFFSET;
                toDraw.Width = BUTTON_WIDTH;
                toDraw.Height = BUTTON_HEIGHT;
                SwinGame.DrawTextLines(_menuStructure(menu)(i), MENU_COLOR, Color.Black, GameFont("Menu"), FontAlignment.AlignCenter, toDraw);
                if (SwinGame.MouseDown(MouseButton.LeftButton) & IsMouseOverMenu(i, level, xOffset))
                {
                    SwinGame.DrawRectangle(HIGHLIGHT_COLOR, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
                }
            }
        }

        private bool IsMouseOverButton(int button)
        {
            return IsMouseOverMenu(button, 0, 0);
        }

        private bool IsMouseOverMenu(int button, int level, int xOffset)
        {
            int btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
            int btnLeft = MENU_LEFT + BUTTON_SEP * (button + xOffset);
            return IsMouseInRectangle(btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
        }

        private void PerformMenuAction(int menu, int button)
        {
            switch (menu)
            // TODO Implement Case Statement
            { }
            // PerformMainMenuAction(button);
            // PerformSetupMenuAction(button);
            // PerformGameMenuAction(button);
        }

        private void PerformMainMenuAction(int button)
        {
            // TODO Implement Case Statement
            switch (button)
            { }
            // StartGame();
            // AddNewState(GameState.AlteringSettings);
            // AddNewState(GameState.ViewingHighScores);
            // EndCurrentState();
            // }
        }

        private void PerformSetupMenuAction(int button)
        {
            // TODO Implement Case Statement
            switch (button)
            { }
            // SetDifficulty(AIOption.Hard);
            // SetDifficulty(AIOption.Hard);
            // SetDifficulty(AIOption.Hard);
            // }

            EndCurrentState();
        }

        private void PerformGameMenuAction(int button)
        {
            // TODO Implement Case Statement
            switch (button)
            { }
            // EndCurrentState();
            // EndCurrentState();
            // EndCurrentState();
            // AddNewState(GameState.Quitting);
            // }
        }
    }
    //=======================================================
    //Service provided by Telerik (www.telerik.com)
    //Conversion powered by Refactoring Essentials.
    //Twitter: @telerik
    //Facebook: facebook.com/telerik
    //=======================================================
}