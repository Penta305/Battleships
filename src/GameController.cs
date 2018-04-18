using System;
using System.Collections.Generic;
using SwinGameSDK;

// The GameController is responsible for controlling the game,
// managing user input, and displaying the current state of the
// game.

namespace Battleship
{
    public class GameController
    {

        private BattleShipsGame _theGame;

		// CHECK
        // private Player _human;
        
        private static Player _human;

        private AIPlayer _ai;

        private Stack<GameState> _state = new Stack<GameState>();

        private AIOption _aiSetting;

        // Returns to the current state of the game, indicating which screen is
        // currently being used.

        public GameState CurrentState
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

        public  Player ComputerPlayer
        {
            get
            {
                return _ai;
            }
        }

        public GameController()
        {
            // Th bottom state will be quitting. If the player exits to the
            // main menu, then the game will end.

            _state.Push(GameState.Quitting);
            
            // At the start, the player will be viewing the main menu screen.

            _state.Push(GameState.ViewingMainMenu);
        }

        // Checks the game grids for any changes and redraws the screen
        // if there is

        // CHECK
        // <param name="sender">the grid that changed</param>
        // <param name="args">not used</param>
        private static void GridChanged(object sender, EventArgs args)
        {
            DrawScreen();
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

        // CHECK
        // <param name="sender">the game</param>
        // <param name="result">the result of the attack</param>

        private static void AttackCompleted(object sender, AttackResult result)
        {
            GameController _AttackComplete = new GameController();
            UtilityFunctions _AttackComplete1 = new UtilityFunctions();
            bool isHuman;
            isHuman = (_AttackComplete._theGame.Player == HumanPlayer);
            if (isHuman)
            {
                _AttackComplete1.Message = ("You " + result.ToString());
            }
            else
            {
                _AttackComplete1.Message = ("The AI " + result.ToString());
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

        // Starts a new game, and creates an AI player based upon
        // the _aiSetting currently set
        public static void StartGame()
        {
            GameController _StartGame = new GameController();
            if (_StartGame._theGame == null)
            {
                EndGame();
            }

            // Create the game
            _StartGame._theGame = new BattleShipsGame();
            // create the players
            switch (_StartGame._aiSetting)
            {
                case AIOption.Medium:
                    _StartGame._ai = new AIMediumPlayer(_StartGame._theGame);
                    break;
                case AIOption.Hard:
                    _StartGame._ai = new AIHardPlayer(_StartGame._theGame);
                    break;
                default:
                    _StartGame._ai = new AIHardPlayer(_StartGame._theGame);
                    break;
            }
            _human = new Player(_StartGame._theGame);
            // AddHandler _human.PlayerGrid.Changed, AddressOf GridChanged
            _StartGame._ai.PlayerGrid.Changed += new EventHandler(GridChanged);
            _StartGame._theGame.AttackCompleted += AttackCompleted;
            GameController.AddNewState(GameState.Deploying);


            // Stops listening to the old game once a new game has been started
        }

        public static void EndGame()
        {
            GameController _EndGame = new GameController();
            //RemoveHandler _human.PlayerGrid.Changed, AddressOf GridChanged
            _EndGame._ai.PlayerGrid.Changed -= GridChanged;
            _EndGame._theGame.AttackCompleted -= AttackCompleted;
        }



        // Completes the deployment phase of the game and
        // switches to the battle mode (Discovering state)

        // This adds the players to the game before switching
        // state.
        public static void EndDeployment()
        {
            GameController _EndDeployment = new GameController();
            // deploy the players
            _EndDeployment._theGame.AddDeployedPlayer(_human);
            _EndDeployment ._theGame.AddDeployedPlayer(_EndDeployment ._ai);
            GameController.SwitchState(GameState.Discovering);
        }

        // Gets the player to attack the indicated row and column.

        // CHECK
        // <param name="row">the row to attack</param>
        // <param name="col">the column to attack</param>

        // Checks the attack result once the attack is complete

        public static void Attack(int row, int col)
        {
            GameController _Attack = new GameController();
            AttackResult result;
            result = _Attack._theGame.Shoot(row, col);
            GameController.CheckAttackResult(result);
        }

        // Gets the AI to attack.
        // Checks the attack result once the attack is complete.

        private static void AIAttack()
        {
            GameController _AIAttack = new GameController();
            AttackResult result;
            result = _AIAttack._theGame.Player.Attack();
            GameController.CheckAttackResult(result);
        }

        // Checks the results of the attack and ends the game if the
        // result was a game over. Get's the AI to attack if the
        // result switched to the AI player.

        // CHECK
        // <param name="result">the result of the last
        // attack</param>

        private static void CheckAttackResult(AttackResult result)
        {
            GameController _CheckAttackResult = new GameController();
            switch (result.Value)
            {
                case ResultOfAttack.Miss:
                    if ((_CheckAttackResult._theGame.Player == _CheckAttackResult.ComputerPlayer))
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
            GameController _HandleUserInput = new GameController();
            MenuController _HandleUserInput1 = new MenuController();
            DeploymentController _HandleUserInput2 = new DeploymentController();
            DiscoveryController _HandleUserInput3 = new DiscoveryController();
            EndingGameController _HandleUserInput4 = new EndingGameController();
            HighScoreController _HandleUserInput5 = new HighScoreController();
            // Read incoming input events
            SwinGame.ProcessEvents();
            switch (_HandleUserInput.CurrentState)
            {
                case GameState.ViewingMainMenu:
                    _HandleUserInput1.HandleMainMenuInput();
                    break;
                case GameState.ViewingGameMenu:
                    _HandleUserInput1.HandleGameMenuInput();
                    break;
                case GameState.AlteringSettings:
                    _HandleUserInput1.HandleSetupMenuInput();
                    break;
                case GameState.Deploying:
                    _HandleUserInput2.HandleDeploymentInput();
                    break;
                case GameState.Discovering:
                    _HandleUserInput3.HandleDiscoveryInput();
                    break;
                case GameState.EndingGame:
                    _HandleUserInput4.HandleEndOfGameInput();
                    break;
                case GameState.ViewingHighScores:
                    _HandleUserInput5.HandleHighScoreInput();
                    break;
            }
            UtilityFunctions.UpdateAnimations();
        }

        // Draws the current state of the game to the screen.
        // What is drawn depends on the current state of the game.

        public static void DrawScreen()
        {
            GameController _DrawScreen = new GameController();
            DeploymentController _DrawScreen0 = new DeploymentController();
            MenuController _DrawScreen1 = new MenuController();
            DiscoveryController _DrawScreen2 = new DiscoveryController();
            EndingGameController _DrawScreen3 = new EndingGameController();
            HighScoreController _DrawScreen4 = new HighScoreController();

            UtilityFunctions.DrawBackground();

            switch (_DrawScreen.CurrentState)
            {
                case GameState.ViewingMainMenu:
                    _DrawScreen1.DrawMainMenu();
                    break;
                case GameState.ViewingGameMenu:
                    _DrawScreen1.DrawGameMenu();
                    break;
                case GameState.AlteringSettings:
                    _DrawScreen1.DrawSettings();
                    break;
                case GameState.Deploying:
                    _DrawScreen0.DrawDeployment();
                    break;
                case GameState.Discovering:
                    _DrawScreen2.DrawDiscovery();
                    break;
                case GameState.EndingGame:
                    _DrawScreen3.DrawEndOfGame();
                    break;
                case GameState.ViewingHighScores:
                    _DrawScreen4.DrawHighScores();
                    break;
            }
            UtilityFunctions.DrawAnimations();
            SwinGame.RefreshScreen();
        }

        // Move the game to a new state. The current state is maintained
        // so that it can be returned to later.

        // CHECK
        // <param name="state">the new game state</param>
        public static void AddNewState(GameState state)
        {
            GameController _AddNewState = new GameController();
            UtilityFunctions _AddNewState1 = new UtilityFunctions();
            _AddNewState._state.Push(state);
            _AddNewState1.Message = "";
        }

        // Ends the current state and adds in the new state.

        // CHECK
        // <param name="newState">the new state of the game</param>
        public static void SwitchState(GameState newState)
        {
            GameController.EndCurrentState();
            GameController.AddNewState(newState);
        }

        // Ends the current state and returns to the prior state

        public static void EndCurrentState()
        {
            GameController _EndCurrentState = new GameController();
            _EndCurrentState._state.Pop();
        }

        // Sets the difficulty for the next level of the game.

        // CHECK
        // <param name="setting">the new difficulty level</param>
        public static void SetDifficulty(AIOption setting)
        {
            GameController _SetDifficulty = new GameController();
            _SetDifficulty._aiSetting = setting;
        }
    }
}