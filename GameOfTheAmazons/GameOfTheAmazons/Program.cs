namespace GameOfTheAmazons
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            GameOfTheAmazons game = new GameOfTheAmazons();

            game.startGame();
        }
    }
}