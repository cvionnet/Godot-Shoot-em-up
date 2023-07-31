using BulletBallet.scenes.classes;
using BulletBallet.utils.NucleusFW.SceneManager;

namespace BulletBallet.scenes;

/// <summary>
/// Responsible for :
/// - loading the SceneManager
/// - initializing levels
/// - transitioning between screens (Menu, GameOver ...)
/// - checking for GameOver
/// </summary>
public partial class GameBrain : Node
{
    #region GODOT METHODS

    public override void _Ready()
    {
        Initialize_GameBrain();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_restart"))
            GetTree().ReloadCurrentScene();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    private void onLevel_Timeout()
    {
        Display_EndGame();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_GameBrain()
    {
        Nucleus.Initialize_Nucleus(GetViewport());
        Nucleus.GameManager = GetNode<GameManager>("/root/GameManager");
        Nucleus.SignalManager = GetNode<SignalManager>("/root/SignalManager");
        
        // Connect SceneManager after Nucleus_Utils.State_Manager initialization
        GetParent().GetNode<SceneManager>("SceneManager").Initialize_SceneManager();
        Nucleus.SignalManager.Connect(SignalManager.SignalName.UiPlayer_GameBrain_LevelTimeout, new Callable(this, nameof(onLevel_Timeout)));   // emitted from Player

        Initialize_LevelsList();
    }

    /// <summary>
    /// To initialize all levels properties
    /// </summary>
    private void Initialize_LevelsList()
    {
        Nucleus.GameManager.LevelList.Add(new Level() { 
            LevelId = 1, 
            RoundTime = 60, 
            PnjNumberToDisplay = 5
        });
    }

    /// <summary>
    /// Gameover screen
    /// </summary>
    private void Display_EndGame()
    {
        Nucleus.SignalManager.EmitSignal(SignalManager.SignalName.Generic_TransitionScene, "screens/Gameover");    // (to SceneManager)
    }

    #endregion
}