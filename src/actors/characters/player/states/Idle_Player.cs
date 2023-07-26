using System;
using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using Godot.Collections;

namespace BulletBallet.actors.characters.player.states;

/// <summary>
/// Responsible for :
/// - 
/// </summary>
public partial class Idle_Player : Node, IState
{
    private Player _rootNode;
    private Move_Player _moveNode;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _moveNode = GetParent<Move_Player>();

        Initialize_Idle();
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

        _rootNode.CharacterAnimation.Play("idle");

        _moveNode.Enter_State(rootNode, param);
    }

    public void Exit_State() => _moveNode.Exit_State();
    public void Update(double delta) => _moveNode.Update(delta);

    public void Physics_Update(double delta)
    {
        _moveNode.Physics_Update(delta);

        if(!_rootNode.Character.Movement.IsMoving)
            Play_Idle();

        /*
    // Conditions of transition to Run or Air states
    if (Nucleus_Utils.StateMachine_Player.RootNode.IsOnFloor() && _moveNode.isMoving)
        Nucleus_Utils.StateMachine_Player.TransitionTo("Move/Run");
    else if (!Nucleus_Utils.StateMachine_Player.RootNode.IsOnFloor())
        Nucleus_Utils.StateMachine_Player.TransitionTo("Move/Air");
    */
    }

    public void Input_State(InputEvent @event) => _moveNode.Input_State(@event);
    public string GetStateName() => Name;

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Idle()
    { }

    private void Play_Idle()
    { }

    #endregion
}