using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using Collections = Godot.Collections;

namespace BulletBallet.actors.characters.pnj.states;

/// <summary>
/// Responsible for :
/// - transitioning to Idle
/// </summary>
public partial class Spawn_Pnj : Node, IState
{
    private Pnj _rootNode;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    // Called when the node enters the scene tree for the first time
    public override void _Ready()
    {
        _SceneReady();
        Initialize_Spawn();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region INTERFACE IMPLEMENTATION

    public void Enter_State<T>(T rootNode, Collections.Dictionary<string, GodotObject> param = null)
    {
        if (rootNode == null || rootNode.GetType() != typeof(Pnj))
        {
            Nucleus.Logs.Error($"State Machine root node is null or type not expected ({rootNode.GetType()})", new NullReferenceException(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            return;
        }
        if (_rootNode == null) _rootNode = rootNode as Pnj;
        if (_rootNode.Character.DebugMode) _rootNode.DebugLabel.Text = _rootNode.StateMachine.ActiveState.GetStateName();

        Enter_CharacterEntrance();








        // TODO: ajouter une animation pour apparaitre et utiliser uniquement  _on_Spawn_AnimationFinished   (supprimer la ligne TransitionTo dans Enter_State)
        _rootNode.StateMachine.TransitionTo("Idle");

        /*
    if (_player.Skin != null)
    {
        _player.Skin.PlayAnimation("spawn");
        _player.Skin.Connect("AnimationFinished", this, nameof(_on_Spawn_AnimationFinished));
    }
    else
    {
        _on_Spawn_AnimationFinished("");
    }
    */
    }

    public void Exit_State()
    {
        /*
    if (_player.Skin != null)
    {
        _player.Skin.Disconnect("AnimationFinished", this, nameof(_on_Spawn_AnimationFinished));
    }
    */
    }

    public void Update(double delta) { }
    public void Physics_Update(double delta) { }
    public void Input_State(InputEvent @event) { }
    public string GetStateName() => Name;

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    public void _on_Spawn_AnimationFinished(string animName)
    {
        _rootNode.StateMachine.TransitionTo("Idle");
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    /// <summary>
    /// Wait for the owner to be ready (owner = Node at the top of the scene), to be sure to access safely to nodes
    /// </summary>
    async private void _SceneReady() => await ToSignal(Owner, "ready");

    private void Initialize_Spawn()
    { }

    private void Enter_CharacterEntrance()
    {
        _rootNode.Visible = true;
    }

    #endregion
}