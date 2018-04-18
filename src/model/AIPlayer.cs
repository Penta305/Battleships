using SwinGameSDK;

// The AIPlayer is a type of player. It can randomly deploy ships and it has the
// functionality to generate coordinates and shoot at tiles

// CHECK
// Public MustInherit Class AIPlayer : Inherits Player
namespace Battleship
{
    public abstract class AIPlayer : Player
    {
        // Location can store the location of the last hit made by an
        // AI Player. The use of which determines the difficulty.
        protected class Location
        {
            private int _Row;

            private int _Column;

            public int Row
            {
                get { return _Row; }
                set { _Row = value; }
            }

            public int Column
            {
                get { return _Column; }
                set { _Column = value; }
            }

            // Sets the last hit made to the local variables
            public Location(int row, int column)
            {
                _Column = column;
                _Row = row;
            }

            /// <summary>
            /// Check if two locations are equal
            /// </summary>
            /// <param name="this">location 1</param>
            /// <param name="other">location 2</param>
            /// <returns>true if location 1 and location 2 are at the same spot</returns>
            public static bool operator ==(Location @this, Location other)
            {
                return @this != null && other != null && @this.Row == other.Row && @this.Column == other.Column;
            }

            /// <summary>
            /// Check if two locations are not equal
            /// </summary>
            /// <param name="this">location 1</param>
            /// <param name="other">location 2</param>
            /// <returns>true if location 1 and location 2 are not at the same spot</returns>
            public static bool operator !=(Location @this, Location other)
            {
                return @this == null || other == null || @this.Row != other.Row || @this.Column != other.Column;
            }
        }

        public AIPlayer(BattleShipsGame game) : base(game)
        {
        }

        // Generate a valid row and column to shoot at
        protected abstract void GenerateCoords(ref int row, ref int column);

        // The last shot had the following result. Child classes can use this
        // to prepare for the next shot.
        protected abstract void ProcessShot(int row, int col, AttackResult result);

        // The AI keeps attacking until its turn is over.
        public override AttackResult Attack()
        {
            AttackResult result;
            int row = 0;
            int column = 0;

            //keep hitting until a miss
            do
            {
                Delay();

                GenerateCoords(ref row, ref column);
                // Generate coordinates for shot
                result = _game.Shoot(row, column);
                // Take shot
                ProcessShot(row, column, result);
            } while (result.Value != ResultOfAttack.Miss && result.Value != ResultOfAttack.GameOver && !SwinGame.WindowCloseRequested());

            return result;
        }

        // Wait a short period to simulate the think time
        private void Delay()
        {
            int i;
            for (i = 0; i <= 150; i++)
            {
                //Dont delay if window is closed
                if (SwinGame.WindowCloseRequested())
                    return;

                SwinGame.Delay(5);
                SwinGame.ProcessEvents();
                SwinGame.RefreshScreen();
            }
        }
    }
}