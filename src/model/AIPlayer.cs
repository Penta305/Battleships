using SwinGameSDK;

// The AIPlayer is a type of player. It can randomly deploy ships and it has the
// functionality to generate coordinates and shoot at tiles

// CHECK
// Public MustInherit Class AIPlayer : Inherits Player
namespace Battleship
{
    public class AIPlayer
    {

        // Location can store the location of the last hit made by an
        // AI Player. The use of which determines the difficulty.

        class Location
        {

            private int _Row;

            private int _Column;

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

            // Sets the last hit made to the local variables

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

        // Generate a valid row and column to shoot at

        // TODO: FIXME - Elijah
        // Protected MustOverride Sub GenerateCoords(ByRef row As Integer, ByRef column As Integer)

        // The last shot had the following result. Child classes can use this
        // to prepare for the next shot.

        // TODO: FIXME - Elijah
        // Protected MustOverride Sub ProcessShot(row as integer, col as integer, result as AttackResult)

        // The AI keeps attacking until its turn is over.

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

                // Generate coordinates for shot
                result = _game.Shoot(row, column);

                // Take shot
                ProcessShot(row, column, result);
            }

            return result;
        }

        // Wait a short period to simulate the think time

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