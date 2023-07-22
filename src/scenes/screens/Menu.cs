namespace BulletBallet.scenes.screens;

/// <summary>
/// Responsible for :
/// - selecting and launching the level to play
/// </summary>
public partial class Menu : Node
{
    private Button _buttonStart;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _buttonStart = GetNode<Button>("Button");
        _buttonStart.Connect("pressed", new Callable(this, nameof(_onButtonStart_Pressed)));

        Initialize_Menu();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// Button "Start"
    /// </summary>
    private void _onButtonStart_Pressed()
    {
        // Go to 1st level
        Nucleus.GameManager.LevelActive = Nucleus.GameManager.LevelList[0];
        Nucleus.SignalManager.EmitSignal(SignalManager.SignalName.Generic_TransitionScene, $"levels/Level{Nucleus.GameManager.LevelActive.LevelId}");    // (to SceneManager)
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Menu()
    { }

    #endregion
}