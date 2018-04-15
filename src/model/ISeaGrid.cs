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

        /*
        Indicates that the grid has changed.
        ''' </summary>
        Event Changed As EventHandler
         */
        event EventHandler Changed;

        /*
        ''' <summary>
        ''' Provides access to the given row/column
        ''' </summary>
        ''' <param name="row">the row to access</param>
        ''' <param name="column">the column to access</param>
        ''' <value>what the player can see at that location</value>
        ''' <returns>what the player can see at that location</returns>
        */

        TileView Item
        {
            get;
        }

        // CHECK: Properties don't accept parameters in C#, Change to method?
        // TileView Item2(int row, int col);

        /*
        ''' <summary>
        ''' Mark the indicated tile as shot.
        ''' </summary>
        ''' <param name="row">the row of the tile</param>
        ''' <param name="col">the column of the tile</param>
        ''' <returns>the result of the attack</returns>
        */

        AttackResult HitTile(int row, int col);
    }
}