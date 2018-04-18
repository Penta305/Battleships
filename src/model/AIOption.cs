namespace Battleship
{
    // The different levels of AI difficulty
    public enum AIOption
    {
        // Completely random shooting
        Easy,

        // Marks squares around hits
        Medium,

        // Same as medium, but removes shots once it misses
        Hard
    }
}