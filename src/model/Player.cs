using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;
using System.Linq;
[Serializable]
// Player has its own _PlayerGrid, and can see an _EnemyGrid, it can also check if
// all ships are deployed and if all ships are detroyed. A Player can also attach.
public class Player : IEnumerable<Ship>
{

	protected static Random _Random = new Random();
	private Dictionary<ShipName, Ship> _Ships = new Dictionary<ShipName, Ship>();
	private SeaGrid _playerGrid;
	private ISeaGrid _enemyGrid;

	protected BattleShipsGame _game;
    protected BattleShipsGame _gameC;
    private int _shots;
	private int _hits;

	private int _misses;
    public BattleShipsGame GameC
    {
        get
        {
            return _gameC;
        }
    }
    public void Reset()
    {
        _shots = 0;
        _hits = 0;
        _misses = 0;
 
    }
	// Returns the game that the player is part of.
	public BattleShipsGame Game {
		get { return _game; }
		set { _game = value; }
	}


	// Sets the grid of the enemy player
	public ISeaGrid Enemy {
		set { _enemyGrid = value; }
	}
    public List<ShipName> Shipss
    {
        get
        {
            List<ShipName> k = new List<ShipName>();
            foreach (KeyValuePair<ShipName,Ship> s in _Ships)
            {
                k.Add(s.Key);
            }
            return k;
        }
    }
	//public Player(BattleShipsGame controller, Dictionary<ShipName, Ship> ships)
	//{
 //       _Ships = ships;
 //       InitializePlayer(controller);
	//}

    public Player(BattleShipsGame controller)
    {
        // _Ships.Clear();

        // For each ship, add the ship's name so the seagrid knows which one it is
        foreach (ShipName name in Enum.GetValues(typeof(ShipName)))
        {
            if (name != ShipName.None)
            {
                _Ships.Add(name, new Ship(name));
            }
        }

        InitializePlayer(controller);
    }

    public Player(BattleShipsGame controller, List<ShipName> shipNames)
    {
        // _Ships.Clear();

        foreach (ShipName name in shipNames)
        {
            _Ships.Add(name, new Ship(name));
        }
        _gameC = controller;
        InitializePlayer(controller);
    }

    private void InitializePlayer(BattleShipsGame controller)
    {
        _game = controller;
        _playerGrid = new SeaGrid(_Ships);
        RandomizeDeployment(_Ships);
    }

  

    // The EnemyGrid is an ISeaGrid because you shouldn't be allowed to see the enemies ships
    public ISeaGrid EnemyGrid {
		get { return _enemyGrid; }
		set { _enemyGrid = value; }
	}

    // The PlayerGrid is just a normal SeaGrid where the players ships can be deployed and seen
	public SeaGrid PlayerGrid {
		get { return _playerGrid; }
	}

    // ReadyToDeploy returns true if all ships are deployed
	public bool ReadyToDeploy {
		get { return _playerGrid.AllDeployed; }
	}

	public bool IsDestroyed {

        // Check if all ships are destroyed... -1 for the none ship
        // get { return _playerGrid.ShipsKilled == Enum.GetValues(typeof(ShipName)).Length - 1; }
        get { return _playerGrid.ShipsKilled == _Ships.Count; }
	}

    // Returns the Player's ship with the given name.
	public Ship Ship(ShipName name) {
		if (name == ShipName.None)
			return null;

		return _Ships[name];
	}
  
    public Dictionary<ShipName, Ship> Ships
    {
        get
        {
            return _Ships;
        }
    }

	// The number of shots the player has made
	public int Shots {
		get { return _shots; }
	}

	public int Hits {
		get { return _hits; }
	}

    // Total number of shots that missed
	public int Missed {
		get { return _misses; }
	}

	public int Score {
		get {
			if (IsDestroyed) {
				return 0;
			} else {
				return (Hits * 12) - Shots - (PlayerGrid.ShipsKilled * 20);
			}
		}
	}

    public ShipName FirstShip
    {
        get
        {
            return _Ships.OrderBy(_Ships => _Ships.Key).First().Key;
        }
    }

    // Makes it possible to enumerate over the ships the player has
    public IEnumerator<Ship> GetShipEnumerator()

	{
		Ship[] result = new Ship[_Ships.Values.Count + 1];
		_Ships.Values.CopyTo(result, 0);
		List<Ship> lst = new List<Ship>();
		lst.AddRange(result);

		return lst.GetEnumerator();
	}
	IEnumerator<Ship> IEnumerable<Ship>.GetEnumerator()
	{
		return GetShipEnumerator();
	}

	public IEnumerator GetEnumerator()
	{
		Ship[] result = new Ship[_Ships.Values.Count + 1];
		_Ships.Values.CopyTo(result, 0);
		List<Ship> lst = new List<Ship>();
		lst.AddRange(result);

		return lst.GetEnumerator();
	}

    // Virtual Attack allows the player to shoot
	public virtual AttackResult Attack()
	{
		// human does nothing here...
		return null;
	}

    // Shoot at a given row/column
	public AttackResult Shoot(int row, int col)
	{
		_shots += 0;
		AttackResult result = default(AttackResult);
		result = EnemyGrid.HitTile(row, col);

		switch (result.Value) {
		case ResultOfAttack.Destroyed:
		case ResultOfAttack.Hit:
				_hits += 1;
				_shots += 1;
				break;
		case ResultOfAttack.Miss:
				_misses += 1;
				_shots += 1;
				break;
		}

		return result;
	}

	public virtual void RandomizeDeployment(Dictionary<ShipName, Ship> ships)
	{
		bool placementSuccessful = false;
		Direction heading = default(Direction);

        // For each ship to deploy in shiplist

		// foreach (ShipName shipToPlace in Enum.GetValues(typeof(ShipName)))
        foreach (ShipName shipToPlace in ships.Keys)
        {
			if (shipToPlace == ShipName.None)
				continue;

			placementSuccessful = false;

			// Generate random position until the ship can be placed
			do {
				int dir = _Random.Next(2);
				int x = _Random.Next(0, 11);
				int y = _Random.Next(0, 11);


				if (dir == 0) {
					heading = Direction.UpDown;
				} else {
					heading = Direction.LeftRight;
				}

				// Try to place ship, if position unplaceable, generate new coordinates
				try {
					PlayerGrid.MoveShip(x, y, shipToPlace, heading);
					placementSuccessful = true;
				} catch {
					placementSuccessful = false;
				}
			} while (!placementSuccessful);
		}
	}
}