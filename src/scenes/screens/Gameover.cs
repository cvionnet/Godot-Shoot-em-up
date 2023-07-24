namespace BulletBallet.scenes.screens;

public partial class Gameover : Node
{
    private Button _buttonStart;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _buttonStart = GetNode<Button>("Button");
        _buttonStart.Connect("pressed", new Callable(this, nameof(_onButtonStart_Pressed)));

        Initialize_Gameover();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// Button "Play again"
    /// </summary>
    private void _onButtonStart_Pressed()
    {
        Nucleus.SignalManager.EmitSignal(SignalManager.SignalName.Generic_TransitionScene, "screens/Menu");    // (to SceneManager) Restart a new game
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Gameover()
    { }

    #endregion
}