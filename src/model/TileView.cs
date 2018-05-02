
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;

// The values that are visable for a given tile
public enum TileView
{
	// The viewer can see sea
	Sea,

	// The viewer knows that the tile was attacked but nothing was hit
	Miss,

	// The viewer can see a ship on this tile
	Ship,

	// The viewer knows that the tile was attacked and something was hit
	Hit
}