using System;
using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using Godot.Collections;

namespace BulletBallet.actors.characters.pnj.states;

/// <summary>
/// Responsible for :
/// - playing the idle animation
/// </summary>
public partial class Idle_Pnj : Node, IState
{
    private Pnj _rootNode;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        Initialize_Idle();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region INTERFACE IMPLEMENTATION

    public void Enter_State<T>(T rootNode, Dictionary<string, GodotObject> param = null)
    {
        if (rootNode == null || rootNode.GetType() != typeof(Pnj))
        {
            Nucleus.Logs.Error($"State Machine root node is null or type not expected ({rootNode.GetType()})", new NullReferenceException(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            return;
        }
        if (_rootNode == null) _rootNode = rootNode as Pnj;
        if (_rootNode.CharacterProperties.DebugMode)
        {
            _rootNode.DebugLabel.Text = _rootNode.StateMachine.ActiveState.GetStateName();
            _rootNode.DebugLabel2.Text = "";
        }

        _rootNode.CharacterAnimation.Play("idle");

//        _moveNode.MaxSpeed = _moveNode.MaxSpeed_Default;
        //_moveNode.Velocity = Utils.VECTOR_0;      // not needed with the use of decceleration in Move.cs
    }

    public void Exit_State() { }
    public void Update(double delta) { }
    public void Physics_Update(double delta) { }
    public void Input_State(InputEvent @event) { }
    public string GetStateName() => Name;

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Idle()
    { }

    #endregion
}