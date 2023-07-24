using System;
using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using Godot.Collections;

namespace BulletBallet.actors.characters.pnj.states;

/// <summary>
/// Responsible for :
/// - deleting the character
/// </summary>
public partial class Fall_Pnj : Node, IState
{
    private Pnj _rootNode;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        Initialize_Fall();
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
        if (_rootNode == null) {
            _rootNode = rootNode as Pnj;
        }
        if (_rootNode.CharacterProperties.DebugMode) _rootNode.DebugLabel.Text = _rootNode.StateMachine.ActiveState.GetStateName();

        Make_CharacterFall();
    }

    public void Exit_State() { }
    public void Update(double delta) { }
    public void Physics_Update(double delta) { }
    public void Input_State(InputEvent @event) { }
    public string GetStateName() => Name;

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// Make the character respawn
    /// </summary>
    private void onTimerFall_Timeout()
    {
        _rootNode.Position = new Vector2(100, 100);
        _rootNode.StateMachine.TransitionTo("Spawn");
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Fall()
    { }

    private void Make_CharacterFall()
    {
        _rootNode.CharacterProperties.Update_Score(-5);
        _rootNode.CharacterProperties.Reset_Movement();
        _rootNode.Visible = false;
        _rootNode.Position = Nucleus_Maths.VECTOR_0;

//        _rootNode.CallDeferred("queue_free");
    }

    #endregion
}