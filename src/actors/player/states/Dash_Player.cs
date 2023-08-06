using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using Collections = Godot.Collections;

namespace BulletBallet.actors.characters.player.states;

/// <summary>
/// Responsible for :
/// - setting the maxspeed and velocity to a boost value (within a limited timing)
/// </summary>
public partial class Dash_Player : Node, IState
{
    private Player _rootNode;
    private Move_Player _moveNode;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _moveNode = GetParent<Move_Player>();

        Initialize_Dash();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region INTERFACE IMPLEMENTATION

    public void Enter_State<T>(T rootNode, Collections.Dictionary<string, GodotObject> param = null)
    {
        if (rootNode == null || rootNode.GetType() != typeof(Player))
        {
            Nucleus.Logs.Error($"State Machine root node is null or type not expected ({rootNode.GetType()})", new NullReferenceException(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            return;
        }
        if (_rootNode == null)
        {
            _rootNode = rootNode as Player;
            _rootNode.TimerDashDuration.Connect("timeout", new Callable(this, nameof(onTimerDash_Timeout)));
        }

        _moveNode.Enter_State(rootNode, param);

        Play_Dash();
    }

    public void Exit_State() => _moveNode.Exit_State();
    public void Update(double delta) => _moveNode.Update(delta);
    public void Physics_Update(double delta) => _moveNode.Physics_Update(delta);
    public void Input_State(InputEvent @event) => _moveNode.Input_State(@event);
    public string GetStateName() => Name;

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    // Reset values after a dash
    public void onTimerDash_Timeout()
    {
        _rootNode.Character.Movement.MaxSpeed *= 1/_rootNode.Character.Movement.Dash_SpeedBoost;
        _rootNode.Character.Movement.Velocity *= 1/_rootNode.Character.Movement.Dash_SpeedBoost;
        _rootNode.Character.Movement.IsDashing = false;
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Dash()
    { }

    /// <summary>
    /// Make the player run faster
    /// </summary>
    private void Play_Dash()
    {
        _rootNode.Character.Movement.IsDashing = true;
        _rootNode.TimerDashDuration.Start();

        //TODO:   play animation

        _rootNode.Character.Movement.MaxSpeed *= _rootNode.Character.Movement.Dash_SpeedBoost;
        _rootNode.Character.Movement.Velocity *= _rootNode.Character.Movement.Dash_SpeedBoost;

        /*
    if (Nucleus_Utils.StateMachine_Player.RootNode.IsOnFloor() && Input.IsActionPressed("button_X"))
        _moveNode.MaxSpeed.x = _moveNode.MaxSpeed_Default.x + SpeedBoost;
    else
        _moveNode.MaxSpeed = _moveNode.MaxSpeed_Default;
    */
    }

    #endregion
}