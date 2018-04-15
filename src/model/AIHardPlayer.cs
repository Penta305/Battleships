using System.Collections.Generic;
using SwinGameSDK;

// AIHardPlayer is the highest difficulty of AI. This AI will know the direction
// of a ships when it has found 2 ship tiles. If that ship is not destroyed after a miss,
// it will shoot the other way. If the ship is still not destroyed, then the AI
// knows it has hit multiple ships. It will then try to destoy all around tiles
// that have been hit.

namespace Battleship
{
    public class AIHardPlayer
    {
        // Target allows the AI to learn more things about the board,
        // for example the source of a shot target

        class Target
        {

            private Location _ShotAt;

            private Location _Source;

            public Location ShotAt
            {
                get
                {
                    return _ShotAt;
                }
            }

            public Location Source
            {
                get
                {
                    return _Source;
                }
            }

            internal Target(Location shootat, Location source)
            {
                _ShotAt = shootat;
                _Source = source;
            }

            // If the source shot and shootat shot are on the same row
            // and column, then return true

            public bool SameRow
            {
                get
                {
                    return;
                }
            }

            public bool SameColumn
            {
                get
                {
                    return;
                }
            }
        }

        // Private enumarator for AI states. Currently there are two states;
        // the AI can be searching for a ship, or if it has found a ship it will
        // target the same ship again.

        private enum AIStates
        {
            // The AI is searching for its next target
            Searching,

            // The AI is trying to target a ship
            TargetingShip,

            // The AI is locked onto a ship
            HittingShip,
        }

        private AIStates _CurrentState = AIStates.Searching;

        private Stack<Target> _Targets = new Stack<Target>();

        private List<Target> _LastHit = new List<Target>();

        private Target _CurrentTarget;

        public AIHardPlayer(BattleShipsGame game) :
                base(game)
        {
        }

        // GenerateCoords will call upon the right methods to generate the appropriate shooting
        // coordinates

        // CHECK
        // <param name="row">the row that will be shot at</param>
        // <param name="column">the column that will be shot at</param>
        protected override void GenerateCoords(ref int row, ref int column)
        {
            for (
            ; ((row < 0)
                        || ((column < 0)
                        || ((row >= EnemyGrid.Height)
                        || ((column >= EnemyGrid.Width)
                        || (EnemyGrid.Item[row, column] != TileView.Sea)))));
            )
            {
                _CurrentTarget = null;
                switch (_CurrentState)
                {
                    case AIStates.Searching:
                        this.SearchCoords(row, column);
                        break;
                    case AIStates.TargetingShip:
                    case AIStates.HittingShip:
                        this.TargetCoords(row, column);
                        break;
                    default:
                        throw new ApplicationException("AI has gone in an invalid state");
                        break;
                }
            }

        }

        // TargetCoords is used when a ship has been hit and it will continue
        // targeting to try and destroy this ship

        // CHECK
        // <param name="row">row generated around the hit tile</param>
        // <param name="column">column generated around the hit tile</param>
        private void TargetCoords(ref int row, ref int column)
        {
            Target t;
            t = _Targets.Pop();
            row = t.ShotAt.Row;
            column = t.ShotAt.Column;
            _CurrentTarget = t;
        }

        // SearchCoords will randomly generate shots within the grid as long as it
        // hasn't already hit that tile

        // CHECK
        // <param name="row">the generated row</param>
        // <param name="column">the generated column</param>
        private void SearchCoords(ref int row, ref int column)
        {
            row = _Random.Next(0, EnemyGrid.Height);
            column = _Random.Next(0, EnemyGrid.Width);
            _CurrentTarget = new Target(new Location(row, column), null);
        }

        // ProcessShot is able to process each shot that is made and call the right methods belonging
        // to that shot. For example, if its a miss = do nothing, if it's a hit = process that hit location

        // CHECK
        // <param name="row">the row that was shot at</param>
        // <param name="col">the column that was shot at</param>
        // <param name="result">the result from that hit</param>
        protected override void ProcessShot(int row, int col, AttackResult result)
        {
            switch (result.Value)
            {
                case ResultOfAttack.Miss:
                    _CurrentTarget = null;
                    break;
                case ResultOfAttack.Hit:
                    ProcessHit(row, col);
                    break;
                case ResultOfAttack.Destroyed:
                    this.ProcessDestroy(row, col, result.Ship);
                    break;
                case ResultOfAttack.ShotAlready:
                    throw new ApplicationException("Error in AI");
                    break;
            }
            if ((_Targets.Count == 0))
            {
                _CurrentState = AIStates.Searching;
            }

            // ProcessDetroy is able to process the destroyed ships targets and remove _LastHit targets.
            // It will also call RemoveShotsAround to remove targets that it was going to shoot at

            // <param name="row">the row that was shot at and destroyed</param>
            // <param name="col">the row that was shot at and destroyed</param>
            // <param name="ship">the row that was shot at and destroyed</param>
        }

        void ProcessDestroy(int row, int col, Ship ship)
        {
            bool foundOriginal;
            Location source;
            Target current;
            current = _CurrentTarget;
            foundOriginal = false;
            int i;
            for (i = 1; (i
                        <= (ship.Hits - 1)); i++)
            {
                if (!foundOriginal)
                {
                    source = current.Source;

                    // Source is nothing if the ship was originally hit in
                    // the middle. This then searches forward rather than
                    // backward through the list of targets
                    if ((source == null))
                    {
                        source = current.ShotAt;
                        foundOriginal = true;
                    }

                }
                else
                {
                    source = current.ShotAt;
                }

                // Find the source in _LastHit
                foreach (Target t in _LastHit)
                {
                    if (((!foundOriginal
                                && (t.ShotAt == source))
                                || (foundOriginal
                                && (t.Source == source))))
                    {
                        current = t;
                        _LastHit.Remove(t);
                        break;
                    }

                }

                this.RemoveShotsAround(current.ShotAt);
            }

        }

        // RemoveShotsAround will remove targets that belong to the destroyed ship by checking if 
        // the source of the targets belong to the destroyed ship. If they don't, put them on a new stack.
        // Then clear the targets stack and move all the targets that still need to be shot at back 
        // onto the targets stack.

        // CHECK
        // <param name="toRemove"></param>
        private void RemoveShotsAround(Location toRemove)
        {
            Stack<Target> newStack = new Stack<Target>();

            // Create a new stack
            // Check all targets in the _Targets stack
            foreach (Target t in _Targets)
            {
                // If the source of the target does not belong to the destroyed ship put them on the newStack
                if (t.Source)
                {
                    IsNot;
                    toRemove;
                    newStack.Push(t);
                    _Targets.Clear();

                    // Clear the _Targets stack
                    // For all the targets in the newStack, move them back onto the _Targets stack
                    foreach (Target t in newStack)
                    {
                        _Targets.Push(t);
                    }

                    // Ff the _Targets stack is 0 then change the AI's state back to searching
                    if ((_Targets.Count == 0))
                    {
                        _CurrentState = AIStates.Searching;
                    }

                    // ProcessHit gets the last hit location coordinates and will ask AddTarget to
                    // create targets around that location by calling the method four times each time with
                    // a new location around the last hit location.
                    // It will then set the state of the AI and if it's not Searching or targetingShip then 
                    // start ReOrderTargets.

                    // CHECK
                    // <param name="row"></param>
                    // <param name="col"></param>
                    ProcessHit(((int)(row)), ((int)(col)));
                    _LastHit.Add(_CurrentTarget);

                    // Uses _CurrentTarget as the source
                    AddTarget((row - 1), col);
                    AddTarget(row, (col - 1));
                    AddTarget((row + 1), col);
                    AddTarget(row, (col + 1));
                    if ((_CurrentState == AIStates.Searching))
                    {
                        _CurrentState = AIStates.TargetingShip;
                    }
                    else
                    {
                        // Either targetting or hitting, both are the same here
                        _CurrentState = AIStates.HittingShip;
                        ReOrderTargets();
                    }

                }

                // ReOrderTargets will optimise the targeting by re-ordering the stack that
                // the targets are in. By putting the most important targets at the top,
                // they are the ones that will be shot at first.

                ReOrderTargets();

                // If the ship is lying on the same row, call MoveToTopOfStack to optimise the row
                if (_CurrentTarget.SameRow)
                {
                    MoveToTopOfStack(_CurrentTarget.ShotAt.Row, -1);
                }
                else if (_CurrentTarget.SameColumn)
                {
                    // Else if the ship is lying on the same column, call MoveToTopOfStack to optimise the column
                    MoveToTopOfStack(-1, _CurrentTarget.ShotAt.Column);
                }

                // MoveToTopOfStack will re-order the stack by checking the coordinates of each target.
                // If they have the correct column or row values, it will be moved to the _Match stack, otherwise 
                // put it on the _NoMatch stack. Then move all the targets from the _NoMatch stack back onto the 
                // _Targets stack, these will be at the bottom making them less important. Then move all the
                // targets from the _Match stack onto the _Targets stack, these will be on the top and will
                // therefor be shot at first.

                // CHECK
                // <param name="row">the row of the optimisation</param>
                // <param name="column">the column of the optimisation</param>
                MoveToTopOfStack(((int)(row)), ((int)(column)));
                Stack<Target> _NoMatch = new Stack<Target>();
                Stack<Target> _Match = new Stack<Target>();
                Target current;
                while ((_Targets.Count > 0))
                {
                    current = _Targets.Pop();
                    if (((current.ShotAt.Row == row)
                                || (current.ShotAt.Column == column)))
                    {
                        _Match.Push(current);
                    }
                    else
                    {
                        _NoMatch.Push(current);
                    }

                }

                foreach (Target t in _NoMatch)
                {
                    _Targets.Push(t);
                }

                foreach (Target t in _Match)
                {
                    _Targets.Push(t);
                }

                // AddTarget will add the targets it will shoot onto a stack

                // CHECK
                // <param name="row">the row of the targets location</param>
                // <param name="column">the column of the targets location</param>
                AddTarget(((int)(row)), ((int)(column)));
                if (((row >= 0)
                            && ((column >= 0)
                            && ((row < EnemyGrid.Height)
                            && ((column < EnemyGrid.Width)
                            && (EnemyGrid.Item[row, column] == TileView.Sea))))))
                {
                    _Targets.Push(new Target(new Location(row, column), _CurrentTarget.ShotAt));
                }

            }

        }
    }
}