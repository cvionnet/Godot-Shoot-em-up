using System;
using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using Godot.Collections;

namespace BulletBallet.actors.characters.pnj.states;

/// <summary>
/// Responsible for :
/// - playing the move animation
/// - moving the character using Steering
/// - transitioning to Idle
/// </summary>
public partial class Move_Pnj : Node, IState
{
    private Pnj _rootNode;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        Initialize_Move();
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

            _rootNode.TimerScore.Connect("timeout", new Callable(this, nameof(onTimerScore_Timeout)));
        }
        if (_rootNode.Character.DebugMode) _rootNode.DebugLabel.Text = _rootNode.StateMachine.ActiveState.GetStateName();

        _rootNode.Character.Movement.IsMoving = true;
    }

    public void Exit_State() { }
    public void Update(double delta) { }

    public void Physics_Update(double delta) => Move_Character();

    public void Input_State(InputEvent @event) { }
    public string GetStateName() => Name;

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// When the character walk on a platform or on borders (two Area2D collides)
    /// </summary>
    private void onAreaShapeEntered(int areaId, Area2D area, int areaShape, int localShape)
    {
        if(area != null && area.Name.ToString().StartsWith("@Platform"))
        {
            Check_UpdateScore();
        }
        else if(area != null && area.Name.ToString().StartsWith("Border"))
        {
            _rootNode.StateMachine.TransitionTo("Fall");
        }
    }

    /// <summary>
    /// Update the score
    /// </summary>
    private void onTimerScore_Timeout()
    {
        //_rootNode.CharacterProperties.Update_Score(1);
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Move()
    { }

    /// <summary>
    /// Calculate velocity between the character and the target (cursor), then move the character
    /// </summary>
    private void Move_Character()
    {
        // Perform calcul only if the node have to move
        if (_rootNode.Character.Movement.Steering.TargetGlobalPosition != _rootNode.GlobalPosition)
        {
            _rootNode.Character.Movement.Velocity = _rootNode.Character.Movement.Steering.Steering_Seek(_rootNode.Character, _rootNode.GlobalPosition);

            // Move the character
            if (_rootNode.Character.Movement.Velocity.Abs() >= Nucleus_Maths.VECTOR_1)
            {
                if (_rootNode.CharacterAnimation.CurrentAnimation != "run")
                    _rootNode.CharacterAnimation.Play("run");

                //_rootNode.CharacterProperties.Velocity = _rootNode.MoveAndSlide(_rootNode.CharacterProperties.Velocity);
                _rootNode.Velocity = _rootNode.Character.Movement.Velocity;
                _rootNode.MoveAndSlide();

                // Flip sprite on left or right
                _rootNode.CharacterSprite.FlipH = _rootNode.Character.Movement.IsOrientationHorizontalInverted;

                //_animatedSprite.Rotation = _velocity.Angle();   // point the character direction towards the destination
            }
            else
            {
                _rootNode.Character.Movement.IsMoving = false;
                _rootNode.StateMachine.TransitionTo("Idle");
            }
        }
    }

    /// <summary>
    /// Start the timer to update the score if conditions are filled
    /// </summary>
    private void Check_UpdateScore()
    {
    }

    #endregion
}