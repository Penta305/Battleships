using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace Battleship
{
    // The GameController is responsible for controlling the game,
    // managing user input, and displaying the current state of the
    // game.
    public static class GameController
    {

        private static BattleShipsGame _theGame;

        private static Player _human;
        private static Player CloneH;
        private static AIPlayer _ai;
        private static AIPlayer CloneA;
        private static Stack<GameState> _state = new Stack<GameState>();

        private static AIOption _aiSetting;

        public static string a = "";
        // Returns to the current state of the game, indicating which screen is
        // currently being used.

        private static List<ShipName> _playableShips = new List<ShipName>();
      
        public static GameState CurrentState
        {
            get
            {
                return _state.Peek();
            }
        }

        public static Player HumanPlayer
        {
            get
            {
                return _human;
            }
        }

        public static Player ComputerPlayer
        {
            get
            {
                return _ai;
            }
        }

        public static List<ShipName> PlayableShips
        {
            get
            {
                return _playableShips;
            }

            set
            {
                _playableShips = value;
            }
        }

        static GameController()
        {
            // Th bottom state will be quitting. If the player exits to the
            // main menu, then the game will end.
            _state.Push(GameState.Quitting);

            // At the start, the player will be viewing the main menu screen.
            _state.Push(GameState.ViewingMainMenu);
        }

        // Starts a new game, and creates an AI player based upon
        // the _aiSetting currently set
        public static void StartGame()
        {
            if (!(_theGame == null))
            {
                GameController.EndGame();
            }
            
            // Create the game
            _theGame = new BattleShipsGame();

            if (_playableShips.Count == 0)
            {
                foreach (ShipName name in Enum.GetValues(typeof(ShipName)))
                {
                    if (name != ShipName.None)
                    {
                        _playableShips.Add(name);
                    }
                }
            }

            // create the players
            switch (_aiSetting)
            {
                case AIOption.Easy:
                    _ai = new AIEasyPlayer(_theGame, _playableShips);
                    Audio.PlaySoundEffect(GameResources.GameSound("Easy"));
                    break;

                case AIOption.Medium:
                    _ai = new AIMediumPlayer(_theGame, _playableShips);
                    Audio.PlaySoundEffect(GameResources.GameSound("Medium"));
                    break;

                case AIOption.Hard:
                    _ai = new AIHardPlayer(_theGame, _playableShips);
                    Audio.PlaySoundEffect(GameResources.GameSound("Hard"));
                    break;

                default:
                    _ai = new AIMediumPlayer(_theGame, _playableShips);
                    Audio.PlaySoundEffect(GameResources.GameSound("Medium"));
                    break;
            }
            _human = new Player(_theGame, _playableShips);
            // AddHandler _human.PlayerGrid.Changed, AddressOf GridChanged
            _ai.PlayerGrid.Changed += GridChanged;
            _theGame.AttackCompleted += AttackCompleted;
            GameController.AddNewState(GameState.Deploying);

            // Stops listening to the old game once a new game has been started
        }

        static void EndGame()
        {
            // RemoveHandler _human.PlayerGrid.Changed, AddressOf GridChanged
            _ai.PlayerGrid.Changed -= GridChanged;
            _theGame.AttackCompleted -= AttackCompleted;
        }

        // Checks the game grids for any changes and redraws the screen
        // if there is
        private static void GridChanged(object sender, EventArgs args)
        {
            GameController.DrawScreen();
            SwinGame.RefreshScreen();
        }

        private static void PlayHitSequence(int row, int column, bool showAnimation)
        {
            if (showAnimation)
            {
                UtilityFunctions.AddExplosion(row, column);
            }

            Audio.PlaySoundEffect(GameResources.GameSound("Hit"));
            UtilityFunctions.DrawAnimationSequence();
        }

        private static void PlayMissSequence(int row, int column, bool showAnimation)
        {
            if (showAnimation)
            {
                UtilityFunctions.AddSplash(row, column);
            }

            Audio.PlaySoundEffect(GameResources.GameSound("Miss"));
            UtilityFunctions.DrawAnimationSequence();
        }

        // Checks for when an attack has been completed. If so, it displays
        // a message, plays a sound and redraws the screen.
        private static void AttackCompleted(object sender, AttackResult result)
        {
            bool isHuman;
            isHuman = (_theGame.Player == HumanPlayer);
            if (isHuman)
            {
                UtilityFunctions.Message = ("You " + result.ToString());
            }
            else
            {
                UtilityFunctions.Message = ("The AI " + result.ToString());
            }

            switch (result.Value)
            {
                case ResultOfAttack.Destroyed:
                    GameController.PlayHitSequence(result.Row, result.Column, isHuman);
                    Audio.PlaySoundEffect(GameResources.GameSound("Sink"));
                    break;
                case ResultOfAttack.GameOver:
                    GameController.PlayHitSequence(result.Row, result.Column, isHuman);
                    Audio.PlaySoundEffect(GameResources.GameSound("Sink"));
                    while (Audio.SoundEffectPlaying(GameResources.GameSound("Sink")))
                    {
                        SwinGame.Delay(10);
                        SwinGame.RefreshScreen();
                    }

                    if (HumanPlayer.IsDestroyed)
                    {
                        Audio.PlaySoundEffect(GameResources.GameSound("Lose"));
                    }
                    else
                    {
                        Audio.PlaySoundEffect(GameResources.GameSound("Winner"));
                    }

                    break;
                case ResultOfAttack.Hit:
                    GameController.PlayHitSequence(result.Row, result.Column, isHuman);
                    break;
                case ResultOfAttack.Miss:
                    GameController.PlayMissSequence(result.Row, result.Column, isHuman);
                    break;
                case ResultOfAttack.ShotAlready:
                    Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                    break;
            }
        }

        // Completes the deployment phase of the game and
        // switches to the battle mode (Discovering state)
        public static void EndDeployment()
        {
            // deploy the players
            _theGame.AddDeployedPlayer(_human);
            _theGame.AddDeployedPlayer(_ai);
           
            GameController.SwitchState(GameState.Discovering);
        }

        // Gets the player to attack the indicated row and column.
        public static void Attack(int row, int col)
        {
            AttackResult result;
            result = _theGame.Shoot(row, col);
            GameController.CheckAttackResult(result);
        }

        // Gets the AI to attack.
        // Checks the attack result once the attack is complete.
        private static void AIAttack()
        {
            AttackResult result;
            result = _theGame.Player.Attack();
            GameController.CheckAttackResult(result);
        }

        // Checks the results of the attack and ends the game if the
        // result was a game over. Get's the AI to attack if the
        // result switched to the AI player.
        private static void CheckAttackResult(AttackResult result)
        {
            switch (result.Value)
            {
                case ResultOfAttack.Miss:
                    if ((_theGame.Player == ComputerPlayer))
                    {
                        GameController.AIAttack();
                    }

                    break;
                case ResultOfAttack.GameOver:
                    GameController.SwitchState(GameState.EndingGame);
                    break;
            }
        }

        // Handles the user SwinGame.
        // Reads key and mouse input and converts these into
        // actions for the game to perform. The actions
        // performed depend on the current state of the game.
        public static void HandleUserInput()
        {
            // Read incoming input events
            SwinGame.ProcessEvents();
            switch (CurrentState)
            {
                case GameState.ViewingMainMenu:
                    MenuController.HandleMainMenuInput();
                    break;
                case GameState.ViewingGameMenu:
                    MenuController.HandleGameMenuInput();
                    break;
                case GameState.AlteringSettings:
                    MenuController.HandleSetupMenuInput();
                    break;
                case GameState.AlteringShipSettings:
                    MenuController.HandleShipsMenuInput();
                    break;
                case GameState.Deploying:
                    DeploymentController.HandleDeploymentInput();
                    break;
                case GameState.Discovering:
                    DiscoveryController.HandleDiscoveryInput();
                    break;
                case GameState.ReDiscovering:
                    _theGame.ResetGame();
                    DiscoveryController.HandleDiscoveryInput();
                    break;
                case GameState.EndingGame:
                    EndingGameController.HandleEndOfGameInput();
                    break;
                case GameState.ViewingHighScores:
                    HighScoreController.HandleHighScoreInput();
                    break;
            }
            UtilityFunctions.UpdateAnimations();
        }

        // Draws the current state of the game to the screen.
        // What is drawn depends on the current state of the game.
        public static void DrawScreen()
        {
            UtilityFunctions.DrawBackground();
            switch (CurrentState)
            {
                case GameState.ViewingMainMenu:
                    MenuController.DrawMainMenu();
                    break;
                case GameState.ViewingGameMenu:
                    MenuController.DrawGameMenu();
                    break;
                case GameState.AlteringSettings:
                    MenuController.DrawSettings();
                    break;
                case GameState.AlteringShipSettings:
                    MenuController.DrawShipsMenu();
                    break;
                case GameState.Deploying:
                    DeploymentController.DrawDeployment();
                    break;
                case GameState.Discovering:
                    DiscoveryController.DrawDiscovery();
                    break;
                case GameState.EndingGame:
                    EndingGameController.DrawEndOfGame();
                    break;
                case GameState.ViewingHighScores:
                    HighScoreController.DrawHighScores();
                    break;
            }
            UtilityFunctions.DrawAnimations();
            SwinGame.RefreshScreen();
        }

        // Move the game to a new state. The current state is maintained
        // so that it can be returned to later.
        public static void AddNewState(GameState state)
        {
            _state.Push(state);
            UtilityFunctions.Message = "";
        }

        // Ends the current state and adds in the new state.
        public static void SwitchState(GameState newState)
        {
            GameController.EndCurrentState();
            GameController.AddNewState(newState);
        }

        // Ends the current state and returns to the prior state
        public static void EndCurrentState()
        {
            _state.Pop();
        }

        // Sets the difficulty for the next level of the game.
        public static void SetDifficulty(AIOption setting)
        {
            _aiSetting = setting;
        }

        public static void SetShips(int numberOfShips)
        {
            _playableShips.Clear();

            for (int i = 0; i < numberOfShips; i++)
            {
                _playableShips.Add((ShipName)(i+1));
            }
        }

        public static void SetShips(ShipName[] shipsToAdd)
        {
            _playableShips.Clear();

            foreach (ShipName ship in shipsToAdd)
            {
                _playableShips.Add(ship);
            }

            _playableShips.Sort();
        }
    }
}