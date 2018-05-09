
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// The BattleShipsGame controls a big part of the game. It will add the two players
// to the game and make sure that both players' ships are all deployed before it starts.
// It also allows players to shoot and swap turns. It will also check if players' ships
// are destroyed.
[Serializable]
public class BattleShipsGame
{

	// The attack delegate type is used to send notifications of the end of an
    // attack by a player or the AI.
	public delegate void AttackCompletedHandler(object sender, AttackResult result);

	// The AttackCompleted event is raised when attack is completed.
    // It is used by the UI to play sound sound effects etc.
	public event AttackCompletedHandler AttackCompleted;
    public BattleShipsGame()
    {
        Clone = new Player[3];
    }
	private Player[] _players = new Player[3];
    private Player[] Clone;
    
    private int _playerIndex = 0;
	// The current player. This value will switch between the two players
    // each turn.
	public Player Player {
		get { return _players[_playerIndex]; }
	}

	// AddDeployedPlayer adds both players and will make sure that the AI
    // player has deployed all ships
	public void AddDeployedPlayer(Player p)
	{
		if (_players[0] == null) {
			_players[0] = p;
            Clone[0] = DeepClone.Clone(p);
        } else if (_players[1] == null) {
			_players[1] = p;
            Clone[1] = DeepClone.Clone(p);
            CompleteDeployment();
		} else {
			throw new ApplicationException("You cannot add another player, the game already has two players.");
		}
	}

	// Assigns each player the opponents grid as the enemy grid. This allows each player
    // to examine the details visable on the opponents sea grid.
	private void CompleteDeployment()
	{
       
            _players[0].Enemy = new SeaGridAdapter(_players[1].PlayerGrid);
            _players[1].Enemy = new SeaGridAdapter(_players[0].PlayerGrid);
  
    }
    public void ResetGame()
    {
        // _players[0].Enemy = new SeaGridAdapter(Clone[1].PlayerGrid);
        // _players[1].Enemy = new SeaGridAdapter(Clone[0].PlayerGrid);

        _players[0].PlayerGrid.ResetTiles();
        _players[1].PlayerGrid.ResetTiles();

        _players[0].PlayerGrid.ShipsKilled = 0;
        _players[1].PlayerGrid.ShipsKilled = 0;

        foreach (ShipName ship in _players[0].Shipss)
        {
            _players[0].Ship(ship).Hits = 0;
        }

        foreach (ShipName ship in _players[1].Shipss)
        {
            _players[1].Ship(ship).Hits = 0;
        }
    }
    // Shoot will swap between players and check if a player has been killed.
    // It also allows the current player to hit on the enemies grid.
    public AttackResult Shoot(int row, int col)
	{
		AttackResult newAttack = default(AttackResult);
		int otherPlayer = (_playerIndex + 1) % 2;

		newAttack = Player.Shoot(row, col);

		// Will exit the game when all players ships are destroyed
		if (_players[otherPlayer].IsDestroyed) {
			newAttack = new AttackResult(ResultOfAttack.GameOver, newAttack.Ship, newAttack.Text, row, col);
		}

		if (AttackCompleted != null) {
			AttackCompleted(this, newAttack);
		}

		// change player if the last hit was a miss
		if (newAttack.Value == ResultOfAttack.Miss) {
			_playerIndex = otherPlayer;
		}

		return newAttack;
	}
}