using System;
using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using Godot.Collections;

namespace BulletBallet.actors.characters.player.states;

/// <summary>
/// Responsible for :
/// - transitioning to Idle
/// - initializing Camera default zoom
/// </summary>
public partial class Spawn_Player : Node, IState
{
    private Player _rootNode;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _SceneReady();
        Initialize_Spawn();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region INTERFACE IMPLEMENTATION

    public void Enter_State<T>(T rootNode, Dictionary<string, GodotObject> param = null)
    {
        if (rootNode == null || rootNode.GetType() != typeof(Player))
        {
            Nucleus.Logs.Error($"State Machine root node is null or type not expected ({rootNode.GetType()})", new NullReferenceException(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            return;
        }
        if (_rootNode == null) _rootNode = rootNode as Player;

        Enter_CharacterEntrance();






        // TODO: ajouter une animation pour apparaitre et utiliser  _on_Spawn_AnimationFinished
        _rootNode.StateMachine.TransitionTo("Move/Idle");




        /*
    if (_player.Skin != null)
    {
        _player.Skin.PlayAnimation("spawn");
        _player.Skin.Connect("AnimationFinished", this, nameof(_on_Spawn_AnimationFinished));
    }
    else
    {
        // Force to display the Idle state
        _on_Spawn_AnimationFinished("");
    }
    */
    }

    public void Exit_State() { }
    public void Update(double delta) { }
    public void Physics_Update(double delta) { }
    public void Input_State(InputEvent @event) { }
    public string GetStateName() => Name;

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    public void _on_Spawn_AnimationFinished(string animName)
    {
        _rootNode.StateMachine.TransitionTo("Move/Idle", null);
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
        // Zoom effect when the player appears
        _rootNode.Camera.Zoom_Camera(Nucleus.GameManager.ZoomLevelGame, 0.4f);
        _rootNode.Visible = true;
    }

    #endregion
}