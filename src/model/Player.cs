using System;
using System.Collections;
using System.Collections.Generic;
using SwinGameSDK;

// Player has its own _PlayerGrid, and can see an _EnemyGrid, it can also check if
// all ships are deployed and if all ships are detroyed. A Player can also attach.

namespace Battleship
{
    public class Player
    {

        protected static Random _Random = new Random();

        private Dictionary<ShipName, Ship> _Ships = new Dictionary<ShipName, Ship>();

        private SeaGrid _playerGrid = new SeaGrid(_Ships);

        private ISeaGrid _enemyGrid;

        protected BattleShipsGame _game;

        private int _shots;

        private int _hits;

        private int _misses;

        // Returns the game that the player is a part of.

        public BattleShipsGame Game
        {
            get
            {
                return _game;
            }
            set
            {
                _game = value;
            }
        }

        public ISeaGrid Enemy
        {
            set
            {
                _enemyGrid = value;
            }
        }

        public Player(BattleShipsGame controller)
        {
            _game = controller;

            // For each ship, add the ship's name so the seagrid knows which one it is
            foreach (ShipName name in Enum.GetValues(typeof(ShipName)))
            {
                if ((name != ShipName.None))
                {
                    _Ships.Add(name, new Ship(name));
                }

            }

            this.RandomizeDeployment();
        }

        // The EnemyGrid is a ISeaGrid because you shouldn't be allowed to see the enemies ships

        public ISeaGrid EnemyGrid
        {
            get
            {
                return _enemyGrid;
            }
            set
            {
                _enemyGrid = value;
            }
        }

        public SeaGrid PlayerGrid
        {
            get
            {
                return _playerGrid;
            }
        }

        public bool ReadyToDeploy
        {
            get
            {
                return _playerGrid.AllDeployed;
            }
        }

        public bool IsDestroyed
        {
            get
            {
                // Check if all ships are destroyed... -1 for the none ship
                return;
            }
        }

        public Ship Ship
        {
            get
            {
                if ((name == ShipName.None))
                {
                    return null;
                }

                return _Ships.Item[name];
            }
        }

        public int Shots
        {
            get
            {
                return _shots;
            }
        }

        public int Hits
        {
            get
            {
                return _hits;
            }
        }

        public int Missed
        {
            get
            {
                return _misses;
            }
        }

        public int Score
        {
            get
            {
                if (IsDestroyed)
                {
                    return 0;
                }
                else
                {
                    return ((Hits * 12)
                                - (Shots
                                - (PlayerGrid.ShipsKilled * 20)));
                }

            }
        }

        public IEnumerator<Ship> GetShipEnumerator()
        {
            Ship[,] result;
            _Ships.Values.CopyTo(result, 0);
            List<Ship> lst = new List<Ship>();
            lst.AddRange(result);
            return lst.GetEnumerator();
        }

        // Makes it possible to enumerate over the ships the player has.

        public IEnumerator GetEnumerator()
        {
            Ship[,] result;
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

        internal AttackResult Shoot(int row, int col)
        {
            _shots++;
            AttackResult result;
            result = EnemyGrid.HitTile(row, col);
            switch (result.Value)
            {
                case ResultOfAttack.Destroyed:
                case ResultOfAttack.Hit:
                    _hits++;
                    break;
                case ResultOfAttack.Miss:
                    _misses++;
                    break;
            }
            return result;
        }

        public virtual void RandomizeDeployment()
        {
            bool placementSuccessful;
            Direction heading;

            // For each ship to deploy in shiplist
            foreach (ShipName shipToPlace in Enum.GetValues(typeof(ShipName)))
            {
                if ((shipToPlace == ShipName.None))
                {
                    // TODO: Continue For... Warning!!! not translated
                }

                placementSuccessful = false;
                for (
                ; !placementSuccessful;
                )
                {
                    int dir = _Random.Next(2);
                    int x = _Random.Next(0, 11);
                    int y = _Random.Next(0, 11);
                    if ((dir == 0))
                    {
                        heading = Direction.UpDown;
                    }
                    else
                    {
                        heading = Direction.LeftRight;
                    }

                    // Try to place ship, if position unplaceable, generate new coordinates
                    try
                    {
                        PlayerGrid.MoveShip(x, y, shipToPlace, heading);
                        placementSuccessful = true;
                    }
                    catch (System.Exception placementSuccessful)
                    {
                        false;
                    }

                }

            }

        }
    }
}