
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// The AIMediumPlayer is the medium difficulty AI, which is designed to
// try and destroy ships once they've been found.
[Serializable]
public class AIMediumPlayer : AIPlayer
{
    // Private enumerator for AI states. Currently there are two states;
    // the AI can be searching for a ship, or if it has found a ship it
    // will continue to target it.

	private enum AIStates
	{
		Searching,
		TargetingShip
	}

	private AIStates _CurrentState = AIStates.Searching;

	private Stack<Location> _Targets = new Stack<Location>();
	public AIMediumPlayer(BattleShipsGame controller) : base(controller)
	{
	}

    public AIMediumPlayer(BattleShipsGame game, List<ShipName> ships) : base(game, ships)
    {

    }

    // GenerateCoords should generate random shooting coordinates only
    // when it is yet to locate a ship, or has just destroyed a ship and
    // needs new coordinates.
    protected override void GenerateCoords(ref int row, ref int column)
	{
		do {
            // Check which state the AI is in to decide with coordinate
            // generation method should be used.
			switch (_CurrentState) {
				case AIStates.Searching:
					SearchCoords(ref row, ref column);
					break;
				case AIStates.TargetingShip:
					TargetCoords(ref row, ref column);
					break;
				default:
					throw new ApplicationException("AI has gone in an imvalid state");
			}
		} while ((row < 0 || column < 0 || row >= EnemyGrid.Height || column >= EnemyGrid.Width || EnemyGrid[row, column] != TileView.Sea));
		//while inside the grid and not a sea tile do the search
	}

    // TargetCoords is used once a ship has been hit and it will then try
    // to destory said ship.

	private void TargetCoords(ref int row, ref int column)
	{
		Location l = _Targets.Pop();

		if ((_Targets.Count == 0))
			_CurrentState = AIStates.Searching;
		row = l.Row;
		column = l.Column;
	}

    // SearchCoords will randomly generate shots within the grid as long
    // as it hasn't already hit that tile.

	private void SearchCoords(ref int row, ref int column)
	{
		row = _Random.Next(0, EnemyGrid.Height);
		column = _Random.Next(0, EnemyGrid.Width);
	}

    // ProcessShot will be called when a ship is found. It will create a
    // stack with the targets it will try to hit. These targets will be
    // around the tile that has been hit.

	protected override void ProcessShot(int row, int col, AttackResult result)
	{
		if (result.Value == ResultOfAttack.Hit) {
			_CurrentState = AIStates.TargetingShip;
			AddTarget(row - 1, col);
			AddTarget(row, col - 1);
			AddTarget(row + 1, col);
			AddTarget(row, col + 1);
		} else if (result.Value == ResultOfAttack.ShotAlready) {
			throw new ApplicationException("Error in AI");
		}
	}

    // AddTarget will add the targets it will shoot onto a new stack
	private void AddTarget(int row, int column)
	{

		if (row >= 0 && column >= 0 && row < EnemyGrid.Height && column < EnemyGrid.Width && EnemyGrid[row, column] == TileView.Sea) {
			_Targets.Push(new Location(row, column));
		}
	}
}