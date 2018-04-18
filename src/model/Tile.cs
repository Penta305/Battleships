using System;

namespace Battleship
{
    // Tile knows its location on the grid, if it's a ship and if it has
    // been shot before

    public class Tile
    {
        private readonly int _RowValue;
        private readonly int _ColumnValue;
        private Ship _Ship = null;
        private bool _Shot = false;

        // Indicates whether a tile has been shot
        public bool Shot
        {
            get
            {
                return _Shot;
            }

            set
            {
                _Shot = value;
            }
        }

        public int Row
        {
            get
            {
                return _RowValue;
            }
        }

        public int Column
        {
            get
            {
                return _ColumnValue;
            }
        }

        // Ship allows for a tile to check if there is a ship and add a ship
        // to a tile

        public Ship Ship
        {
            get
            {
                return _Ship;
            }

            set
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

        // The tile constructor will know where it is on the grid and if
        // it's a ship

        public Tile(int row, int col, Ship ship)
        {
            _RowValue = row;
            _ColumnValue = col;
            _Ship = ship;
        }

        // ClearShip will remove a ship from its tile

        public void ClearShip()
        {
            _Ship = null;
        }

        // View is able to tell the grid what the tile is

        public TileView View
        {
            get
            {
                // If there is no ship in the tile
                if (_Ship == null)
                {
                    // and the tile has been hit
                    if (_Shot)
                    {
                        return TileView.Miss;
                    }
                    // and the tile hasn't been hit
                    else
                    {
                        return TileView.Sea;
                    }
                }
                else
                {
                    // if there is a ship and it has been hit
                    if ((_Shot))
                    {
                        return TileView.Hit;
                    }
                    // if there is a ship and it hasn't been hit
                    else
                    {
                        return TileView.Ship;
                    }
                }
            }
        }

        // Shoot allows a tile to be shot at and whether the tile has been
        // hit before

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
}