using System;

// The SeaGridAdapter allows for the change in a sea grid view. Whenever a ship is
// presented it changes the view into a sea tile instead of a ship tile.

namespace Battleship
{
    public class SeaGridAdapter : ISeaGrid
    {

        private SeaGrid _MyGrid;

        // Create the SeaGridAdapter, with the grid, and it will allow it to be changed

        public SeaGridAdapter(SeaGrid grid)
        {
            _MyGrid = grid;
            _MyGrid.Changed += new EventHandler(MyGrid_Changed);
        }

//<<<<<<< HEAD

        // MyGrid_Changed causes the grid to be redrawn by raising a changed event

        private void MyGrid_Changed(object sender, EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        // TODO: FIXME: Start region was specified here - Elijah
        // #Region "ISeaGrid Members"

//<<<<<<< HEAD
        // Changes the discovery grid. It returns a tile or, if it was a ship,
        // it will return a sea tileWhere there is a ship we will sea water.

        public TileView Item(int x, int y)
        {

            TileView result = _MyGrid.Item(x, y);

            if (result == TileView.Ship)
            {
                return TileView.Sea;
            }
            else
            {
                return result;
            }
        }

//<<<<<<< HEAD
        /* TODO: FIXME VB code below- Elijah
        Public ReadOnly Property Item(ByVal x As Integer, ByVal y As Integer) As TileView Implements ISeaGrid.Item
                Get
                    Dim result As TileView = _MyGrid.Item(x, y)

                    If result = TileView.Ship Then
                        Return TileView.Sea
                    Else
                        Return result
                    End If
                End Get
            End Property
        */

        /* TODO: FIXME VB code below- Elijah
        Public Event Changed As EventHandler Implements ISeaGrid.Changed
        */
        
        // Indicates that the grid has been changed
        public event EventHandler Changed;

        // Get the width of a tile
        public int Width
        {
            get
            {
                return _MyGrid.Width;
            }

        }

        public int Height
        {
            get
            {
                return _MyGrid.Height;
            }
        }

        public AttackResult HitTile(int row, int col)
        {
            return _MyGrid.HitTile(row, col);
        }

        // TODO: FIXME: end region specified here - Elijah
        // #End Region
        // FIXME: end class
    }
}