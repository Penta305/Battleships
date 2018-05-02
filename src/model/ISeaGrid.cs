
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;

// The ISeaGrid defines the read only interface of a Grid. This
// allows each player to see and attack their opponents grid.
public interface ISeaGrid
{


	int Width { get; }

	int Height { get; }

	// Indicates that the grid has changed.
	event EventHandler Changed;

	// Provides access to the given row/column
	TileView this[int row, int col] { get; }

	// Mark the indicated tile as shot.
	AttackResult HitTile(int row, int col);
}