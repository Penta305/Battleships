namespace Battleship
{
    public class Tile
    {
        private readonly int _RowValue;
        private readonly int _ColumnValue;
        private Ship _Ship = null;
        private bool _Shot = false;
        public bool Shot
        {
            private get
            {
                return _Shot;
            }

            private set
            {
                _Shot = value;
            }
        }

        public int Row
        {
            private get
            {
                return _RowValue;
            }
        }

        public int Column
        {
            private get
            {
                return _ColumnValue;
            }
        }

        public Ship Ship
        {
            private get
            {
                return _Ship;
            }

            private set
            {
                if (_Ship == null)
                {
                    _Ship = value;
                    if (value != null)
                    {
                        _Ship.AddTile(this);
                    }
                }
                else
                {
                    throw new InvalidOperationException("There is already a ship at [" + Row + ", " + Column + "]");
                }
            }
        }

        public Tile(int row, int col, Ship ship)
        {
            _RowValue = row;
            _ColumnValue = col;
            _Ship = ship;
        }

        public void ClearShip()
        {
            _Ship = null;
        }

        public TileView View
        {
            private get
            {
                if (_Ship == null)
                {
                    if (_Shot)
                    {
                        return TileView.Miss;
                    }
                    else
                    {
                        return TileView.Sea;
                    }
                }
                else
                {
                    if ((_Shot))
                    {
                        return TileView.Hit;
                    }
                    else
                    {
                        return TileView.Ship;
                    }
                }
            }
        }

        internal void Shoot()
        {
            if ((false == Shot))
            {
                Shot = true;
                if (_Ship != null)
                {
                    _Ship.Hit();
                }
            }
            else
            {
                throw new ApplicationException("You have already shot this square");
            }
        }
    }
    //=======================================================
    //Service provided by Telerik (www.telerik.com)
    //Conversion powered by Refactoring Essentials.
    //Twitter: @telerik
    //Facebook: facebook.com/telerik
    //=======================================================
}