using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using BulletBallet.utils.NucleusFW.Physics;
using Collections = Godot.Collections;

namespace BulletBallet.actors.characters.player.states;

/// <summary>
/// Responsible for :
/// - moving the character
/// - transitioning to Dash
/// - updating the score
/// - zooming/dezooming the camera
/// </summary>
public partial class Move_Player : Node, IState
{
    private Player _rootNode;

    public Vector2 Acceleration_Default { get; private set; }
    public Vector2 Decceleration_Default { get; private set; }

    private bool _zoomOut = false;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        Initialize_Move();
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
        if (_rootNode == null) {
            _rootNode = rootNode as Player;

            _rootNode.TimerScore.Connect("timeout", new Callable(this, nameof(onTimerScore_Timeout)));
        }
    }

    public void Exit_State() { }

    public void Update(double delta)
    {
        // Solution 1 (by code) : move the player on a sin wave when he runs (generate an effect that the player is moving up and down)
        // Solution 2 (on AnimationPlayer) : change the y position along the animation  
        //if(_rootNode.CharacterProperties.IsMoving)
        //    _rootNode.CharacterSprite.Position = new Vector2(_rootNode.CharacterSprite.Position.x, Mathf.Sin(OS.GetTicksMsec() / (_rootNode.CharacterProperties.MaxSpeed.y/10)) * 1.5f);
    }

    public void Physics_Update(double delta)
    {
        Movement_isPlayerMoving();

        if(_rootNode.Character.Movement.IsMoving)
        {
            if (_rootNode.CharacterAnimation.CurrentAnimation != "run")
                _rootNode.CharacterAnimation.Play("run");
            Movement_UpdateVelocity(delta);
        }
        else
        {
            if (_rootNode.CharacterAnimation.CurrentAnimation != "idle")
                _rootNode.CharacterAnimation.Play("idle");
        }
    }

    public void Input_State(InputEvent @event)
    {
        // Detect a dash command
        Movement_Dash(@event);

        // Detect a zoom command
        //Zoom_Camera(@event);
    }

    public string GetStateName() => Name;

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// Update the score
    /// </summary>
    private void onTimerScore_Timeout() => _rootNode.Update_Score(1);

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Move()
    { }

    /// <summary>
    /// Check if the player is moving (direction (joypad) or velocity (acceleration/decceleration))
    /// </summary>
    private void Movement_isPlayerMoving()
    {
        if (Nucleus.Genre == GameManager.Genre.PLATEFORMER)
            _rootNode.Character.Movement.Direction = Nucleus_Movement.GetMovingDirection("L_left", "L_right", false);
        else
            _rootNode.Character.Movement.Direction = Nucleus_Movement.GetMovingDirection("L_left", "L_right", false, "L_up", "L_down");

        // Check if the player is moving : he has a direction (joypad) or a velocity (acceleration/decceleration)
        _rootNode.Character.Movement.IsMoving = _rootNode.Character.Movement.Velocity.X != 0.0f || _rootNode.Character.Movement.Velocity.Y != 0.0f || _rootNode.Character.Movement.Direction.X != 0.0f || _rootNode.Character.Movement.Direction.Y != 0.0f;

        // Flip sprite on left or right
        _rootNode.CharacterSprite.FlipH = _rootNode.Character.Movement.IsOrientationHorizontalInverted;
    }

    /// <summary>
    /// Calculate velocity and move the player on axis
    /// </summary>
    /// <param name="delta">delta time</param>
    private void Movement_UpdateVelocity(double delta)
    {
        _rootNode.Character.Movement.Velocity = Nucleus_Movement.CalculateVelocity(_rootNode.Character, delta);

        _rootNode.Velocity = _rootNode.Character.Movement.Velocity;
        if (Nucleus.Genre == GameManager.Genre.PLATEFORMER)
            _rootNode.UpDirection = Nucleus_Maths.VECTOR_FLOOR;
        
        //_rootNode.CharacterProperties.Velocity = _rootNode.MoveAndSlide();
        _rootNode.MoveAndSlide();
    }

    /// <summary>
    /// Do a dash when a button is pressed
    /// </summary>
    private void Movement_Dash(InputEvent @event)
    {
        if (_rootNode.Character.Movement.IsMoving && !_rootNode.Character.Movement.IsDashing && @event.IsActionPressed("button_A"))
        {
            //_moveNode.DashCount++;

            //Godot.Collections.Dictionary<string,object> param = new Godot.Collections.Dictionary<string,object>();
            //param.Add("direction", _moveNode.Hook.Raycast.CastTo.Normalized());
            //Utils.StateMachine_Player.TransitionTo("Move/Dash", param);
            _rootNode.StateMachine.TransitionTo("Move/Dash");
        }
    }

    /// <summary>
    /// Zoom or dezoom camera when a button is pressed
    /// </summary>
    // private void Zoom_Camera(InputEvent @event)
    // {
    //     if (@event.IsActionPressed("button_Y") && !_zoomOut)
    //     {
    //         _rootNode.Camera.Zoom_Camera(Nucleus.GameManager.ZoomLevelGame);
    //         _zoomOut = true;
    //     }
    //     else if (@event.IsActionPressed("button_Y") && _zoomOut)
    //     {
    //         _rootNode.Camera.Zoom_Camera(Nucleus.GameManager.ZoomLevelZoomOut);
    //         _zoomOut = false;
    //     }
    // }

    /// <summary>
    /// Start the timer to update the score if conditions are filled
    /// </summary>
    private void Check_UpdateScore()
    { }

    #endregion
}