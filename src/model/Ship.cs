
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// A Ship has all the details about what ship it is, inluding shipname,
// size, number of hits taken and the location. It's able to add tiles,
// remove, hits taken and whether or not it's delpoyed or destroyed.
[Serializable]
public class Ship
{
	private ShipName _shipName;
	private int _sizeOfShip;
	private int _hitsTaken = 0;
	private List<Tile> _tiles;
	private int _row;
	private int _col;

	private Direction _direction;

	public string Name {
		get {
			if (_shipName == ShipName.AircraftCarrier) {
				return "Aircraft Carrier";
			}

			return _shipName.ToString();
		}
	}

	// The number of cells that this ship occupies.
	public int Size {
		get { return _sizeOfShip; }
	}

	// The number of hits that the ship has taken.
	public int Hits {
		get { return _hitsTaken; }
        set { _hitsTaken = value; }
	}

	public int Row {
		get { return _row; }
	}

	public int Column {
		get { return _col; }
	}

	public Direction Direction {
		get { return _direction; }
	}

	public Ship(ShipName ship)
	{
		_shipName = ship;
		_tiles = new List<Tile>();

		//It gets the ship size from the enumarator
		_sizeOfShip = (int)_shipName;
	}

	public void AddTile(Tile tile)
	{
		_tiles.Add(tile);
	}

	// Remove clears the tile back to a sea tile
	public void Remove()
	{
		foreach (Tile tile in _tiles) {
			tile.ClearShip();
		}
		_tiles.Clear();
	}

	public void Hit()
	{
		_hitsTaken = _hitsTaken + 1;
	}

	// IsDeployed returns if the ship is deployed
	public bool IsDeployed {
		get { return _tiles.Count > 0; }
	}

	public bool IsDestroyed {
		get { return Hits == Size; }
	}

	// Record that the ship is now deployed
	internal void Deployed(Direction direction, int row, int col)
	{
		_row = row;
		_col = col;
		_direction = direction;
	}
}