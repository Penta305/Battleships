
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// The SeaGrid is the grid upon which the ships are deployed.

// The grid is viewable via the ISeaGrid interface as a read only
// grid. This can be used in conjunction with the SeaGridAdapter to 
// mask the position of the ships.
[Serializable]
public class SeaGrid : ISeaGrid
{

	private const int _WIDTH = 10;

	private const int _HEIGHT = 10;
	private Tile[,] _GameTiles;
	private Dictionary<ShipName, Ship> _Ships;

	private int _ShipsKilled = 0;
	
	// The sea grid has changed and should be redrawn.
	public event EventHandler Changed;

	public int Width {
		get { return _WIDTH; }
	}

	public int Height {
		get { return _HEIGHT; }
	}

	// ShipsKilled returns the number of ships killed
	public int ShipsKilled {
		get { return _ShipsKilled; }
        set { _ShipsKilled = value; }
	}

	// Show the tile view
	public TileView this[int x, int y]
	{
		get { return _GameTiles[x, y].View; }
	}

	// AllDeployed checks whether or not all the ships have been deployed
	public bool AllDeployed {
		get {
			foreach (Ship s in _Ships.Values) {
				if (!s.IsDeployed) {
					return false;
				}
			}

			return true;
		}
	}

	// SeaGrid constructor, a seagrid has a number of tiles stored in an array
	public SeaGrid(Dictionary<ShipName, Ship> ships)
	{
		_GameTiles = new Tile[Width, Height];
		// Fill array with empty Tiles
		int i = 0;
		for (i = 0; i <= Width - 1; i++) {
			for (int j = 0; j <= Height - 1; j++) {
				_GameTiles[i, j] = new Tile(i, j, null);
			}
		}

		_Ships = ships;
	}

	// MoveShips allows for ships to be placed on the seagrid
	public void MoveShip(int row, int col, ShipName ship, Direction direction)
	{
		Ship newShip = _Ships[ship];
		newShip.Remove();
		AddShip(row, col, direction, newShip);
	}

	// AddShip add a ship to the SeaGrid
	private void AddShip(int row, int col, Direction direction, Ship newShip)
	{
		try {
			int size = newShip.Size;
			int currentRow = row;
			int currentCol = col;
			int dRow = 0;
			int dCol = 0;

			if (direction == Direction.LeftRight) {
				dRow = 0;
				dCol = 1;
			} else {
				dRow = 1;
				dCol = 0;
			}

			// Place ship's tiles in array and into ship object
			int i = 0;
			for (i = 0; i <= size - 1; i++) {
				if (currentRow < 0 | currentRow >= Width | currentCol < 0 | currentCol >= Height) {
					throw new InvalidOperationException("Ship can't fit on the board");
				}

				_GameTiles[currentRow, currentCol].Ship = newShip;

				currentCol += dCol;
				currentRow += dRow;
			}

			newShip.Deployed(direction, row, col);
		} catch (Exception e) {
			newShip.Remove();
			// If it fails, remove the ship
			throw new ApplicationException(e.Message);

		} finally {
			if (Changed != null) {
				Changed(this, EventArgs.Empty);
			}
		}
	}

	// HitTile hits a tile at a row/col and displays a result depending on
    // what was hit
	public AttackResult HitTile(int row, int col)
	{
		try {
			// Tile has already been hit
			if (_GameTiles[row, col].Shot) {
				return new AttackResult(ResultOfAttack.ShotAlready, "have already attacked [" + col + "," + row + "]!", row, col);
			}

			_GameTiles[row, col].Shoot();

			// There is no ship on the tile
			if (_GameTiles[row, col].Ship == null) {
				return new AttackResult(ResultOfAttack.Miss, "missed", row, col);
			}

			// All the ship's tiles have been destroyed
			if (_GameTiles[row, col].Ship.IsDestroyed) {
				_GameTiles[row, col].Shot = true;
				_ShipsKilled += 1;
				return new AttackResult(ResultOfAttack.Destroyed, _GameTiles[row, col].Ship, "destroyed the enemy's", row, col);
			}

			// Else hit, but not destroyed
			return new AttackResult(ResultOfAttack.Hit, "hit something!", row, col);
		} finally {
			if (Changed != null) {
				Changed(this, EventArgs.Empty);
			}
		}
	}
}