using SwinGameSDK;

// '' <summary>
// '' The AIPlayer is a type of player. It can readomly deploy ships, it also has the
// '' functionality to generate coordinates and shoot at tiles
// '' </summary>
// ''Public MustInherit Class AIPlayer : Inherits Player
namespace Battleship
{
    public class AIPlayer
    {

        // '' <summary>
        // '' Location can store the location of the last hit made by an
        // '' AI Player. The use of which determines the difficulty.
        // '' </summary>
        class Location
        {

            private int _Row;

            private int _Column;

            // '' <summary>
            // '' The row of the shot
            // '' </summary>
            // '' <value>The row of the shot</value>
            // '' <returns>The row of the shot</returns>
            public int Row
            {
                get
                {
                    return _Row;
                }
                set
                {
                    _Row = value;
                }
            }

            public int Column
            {
                get
                {
                    return _Column;
                }
                set
                {
                    _Column = value;
                }
            }

            public Location(int row, int column)
            {
                _Column = column;
                _Row = row;
            }
        }

        public AIPlayer(BattleShipsGame game) :
                base(game)
        {
        }

        // '' <summary>
        // '' Generate a valid row, column to shoot at
        // '' </summary>
        // '' <param name="row">output the row for the next shot</param>
        // '' <param name="column">output the column for the next show</param>
        // ''TODO: FIXME - Elijah
        // ''Protected MustOverride Sub GenerateCoords(ByRef row As Integer, ByRef column As Integer)
        // '' <summary>
        // '' The last shot had the following result. Child classes can use this
        // '' to prepare for the next shot.
        // '' </summary>
        // '' <param name="result">The result of the shot</param>
        // '' <param name="row">the row shot</param>
        // '' <param name="col">the column shot</param>
        // ''TODO: FIXME - Elijah
        // ''protected mustoverride sub ProcessShot(row as integer, col as integer, result as AttackResult)
        // '' <summary>
        // '' The AI takes its attacks until its go is over.
        // '' </summary>
        // '' <returns>The result of the last attack</returns>
        public override AttackResult Attack()
        {
            AttackResult result;
            int row = 0;
            int column = 0;
            for (
            ; ((result.Value != ResultOfAttack.Miss)
                        && ((result.Value != ResultOfAttack.GameOver)
                        && !SwinGame.WindowCloseRequested));
            )
            {
                this.Delay();
                GenerateCoords(row, column);
                // generate coordinates for shot
                result = _game.Shoot(row, column);
                // take shot
                ProcessShot(row, column, result);
            }

            return result;
        }

        // '' <summary>
        // '' Wait a short period to simulate the think time
        // '' </summary>
        private void Delay()
        {
            int i;
            for (i = 0; (i <= 150); i++)
            {
                // Dont delay if window is closed
                if (SwinGame.WindowCloseRequested)
                {
                    return SwinGame.Delay(5);
                }

                SwinGame.ProcessEvents();
                SwinGame.RefreshScreen();
            }

        }
    }
}