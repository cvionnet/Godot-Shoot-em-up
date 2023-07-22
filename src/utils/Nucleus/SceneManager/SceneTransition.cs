namespace BulletBallet.utils.NucleusFW.SceneManager;

/// <summary>
/// Responsible for :
/// - playing a fadein/out animation (emit a signal when finished)
/// </summary>
public partial class SceneTransition : Node
{
    private AnimationPlayer _animation;

    //*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _animation = GetNode<AnimationPlayer>("AnimationPlayer");
        _animation.Connect("animation_finished", new Callable(this, nameof(_onAnimation_Finished)));
    }

    #endregion

    //*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    private void _onAnimation_Finished(string animName)
    {
        if (animName == "fadeToBlack")
        {
            // (send to SceneManager)
            Nucleus.SignalManager.EmitSignal(SignalManager.SignalName.SceneTransition_AnimationFinished);
            _animation.Play("fadeToNormal");
        }
    }

    #endregion

    //*-------------------------------------------------------------------------*//

    #region USER METHODS

    /// <summary>
    /// To activate the fade to black transition
    /// </summary>
    public void Transition_Scene()
        => _animation.Play("fadeToBlack");

    #endregion
}