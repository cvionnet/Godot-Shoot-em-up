namespace BulletBallet.utils.NucleusFW;

/// <summary>
/// Class that contains all variables and methods useful between projects
/// </summary>
public static class Nucleus
{
    public static bool DEBUG_MODE { get; } = true;

    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }
    public static GameManager.Genre Genre { get; set; }
    
    public static GameManager GameManager { get; set; } // = new GameManager();
    public static SignalManager SignalManager { get; set; }
    public static Nucleus_Logs Logs { get; set; } = new Nucleus_Logs();

    /// <summary>
    /// Call this method in the 1st scene of the game
    /// </summary>
    /// <param name="game">The viewport of the scene</param>
    public static void Initialize_Nucleus(Viewport game)
    {
        Nucleus_Maths.Rnd.Randomize();

        Genre = GameManager.Genre.TOPDOWN;

        ScreenWidth = DisplayServer.WindowGetSize().X;
        ScreenHeight = DisplayServer.WindowGetSize().Y;
    }

    /// <summary>
    /// Actions to perform when the game is exited
    /// </summary>
    public static void Finalize_Nucleus()
    {
        Logs.Info("User has quit the game");
    }
}
