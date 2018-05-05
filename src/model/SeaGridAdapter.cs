
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;

// The SeaGridAdapter allows for the change in a sea grid view. Whenever a ship is
// presented it changes the view into a sea tile instead of a ship tile.

public class SeaGridAdapter : ISeaGrid
{


	private SeaGrid _MyGrid;

    // Create the SeaGridAdapter, with the grid, and it will allow it to be changed
	public SeaGridAdapter(SeaGrid grid)
	{
		_MyGrid = grid;
		_MyGrid.Changed += new EventHandler(MyGrid_Changed);
	}


	// MyGrid_Changed causes the grid to be redrawn by raising a changed event

	private void MyGrid_Changed(object sender, EventArgs e)
	{
		if (Changed != null) {
			Changed(this, e);
		}
	}

	#region "ISeaGrid Members"

    // Changes the discovery grid. It returns a tile or, if it was a ship,
    // it will return a sea tileWhere there is a ship we will sea water.

	public TileView this[int x, int y] {
		get {
			TileView result = _MyGrid[x, y];

			if (result == TileView.Ship) {
				return TileView.Sea;
			} else {
				return result;
			}
		}
	}

	public event EventHandler Changed;

	public int Width {
		get { return _MyGrid.Width; }
	}

	public int Height {
		get { return _MyGrid.Height; }
	}

	public AttackResult HitTile(int row, int col)
	{
		return _MyGrid.HitTile(row, col);
	}
	#endregion

}