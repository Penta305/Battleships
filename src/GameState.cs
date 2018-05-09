namespace Battleship
{
    // The GameState represents the state of the Battleships game.
    // This is used to control the actions and the screen that's
    // displayed to the player.
    public enum GameState
    {
        ViewingMainMenu,
        ViewingGameMenu,
        ViewingHighScores,
        AlteringSettings,
        AlteringShipSettings,
        Deploying,
        Discovering,
        ReDiscovering,
        EndingGame,
        Quitting
    }
}