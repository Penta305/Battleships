using System;

namespace Battleship
{
    // The ISeaGrid defines the read only interface of a Grid. This
    // allows each player to see and attack their opponents grid.
    public interface ISeaGrid
    {
        int Width
        {
            get;
        }

        int Height
        {
            get;
        }

        // Indicates that the grid has changed.
        event EventHandler Changed;


        // Provides access to the given row/column
        // Dylan's Implementation
		
        // TileView Item
        // {
            // get;
        // }
		
        // Andrew's Implementation
        TileView Item(int row, int col);

        // CHECK: Properties don't accept parameters in C#, Change to method?
        // TileView Item2(int row, int col);

        // Mark the indicated tile as shot.
        AttackResult HitTile(int row, int col);
    }
}