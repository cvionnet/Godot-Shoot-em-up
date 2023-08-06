using BulletBallet.objects.items.classes;
using Timer = Godot.Timer;

namespace BulletBallet.actors.characters.pnj;

/// <summary>
/// Responsible for :
/// - displaying a random character
/// - initializing the character properties
/// - initializing the StateMachine
/// - setting new player's random direction (transition to Move)
/// - receiving actions from Items
/// </summary>
public partial class Pnj : CharacterBody2D
{
    public Entity Character { get; private set; }

    public states.StateMachine_Pnj StateMachine { get; private set; }
    public Label DebugLabel { get; private set; }
    public Label DebugLabel2 { get; private set; }
    public Timer TimerScore { get; private set; }
    public AnimationPlayer CharacterAnimation { get; private set; }
    public Sprite2D CharacterSprite { get; private set; }

    private Timer _timerItemActionDuration;
    private Timer _timerChangeDestination;

    private readonly float _minTimerNewDestination = 5.0f;
    private readonly float _maxTimerNewDestination = 10.0f;
    private readonly float _radiusMovement = 100.0f;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        StateMachine = GetNode<states.StateMachine_Pnj>("StateMachine");
        DebugLabel = GetNode<Label>("DebugLabel");
        DebugLabel2 = GetNode<Label>("DebugLabel2");
        CharacterAnimation = GetNode<AnimationPlayer>("CharacterAnimation");
        CharacterSprite = GetNode<Sprite2D>("CharacterSprite");

        TimerScore = GetNode<Timer>("CharacterTimers/TimerScore");
        _timerItemActionDuration = GetNode<Timer>("CharacterTimers/TimerItemActionDuration");
        _timerChangeDestination = GetNode<Timer>("CharacterTimers/TimerNewDestination");

        //_timerItemActionDuration.Connect("timeout", new Callable(this, nameof(onItemActionDurationTimer_Timeout)));
        _timerChangeDestination.Connect("timeout", new Callable(this, nameof(onChangeDestinationTimer_Timeout)));

        Initialize_Pnj();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// When action time of an item finish
    /// </summary>
    // private void onItemActionDurationTimer_Timeout() => CharacterProperties.ActionEnd_Item();

    /// <summary>
    /// Generate a new destination for the character
    /// </summary>
    private void onChangeDestinationTimer_Timeout()
    {
        Set_NewDestination();
        Initialize_TimerNewDestination();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Pnj()
    {
        Name = "PNJ";       // prefix name of nodes

        //Initialize_CharacterSprite();
        Initialize_Properties();
        Initialize_TimerNewDestination();

        // Initialize the StateMachine
        StateMachine.Init_StateMachine(this);
    }

    /// <summary>
    /// Generate a random character by adding a CustomCharacter node
    /// </summary>
    private void Initialize_CharacterSprite()
    {
        //PackedScene sceneCharacter = GD.Load<PackedScene>("res://src/actors/characters/customCharacter/CustomCharacter.tscn");
        //_spriteCharacter = (CustomCharacter)sceneCharacter.Instantiate();
        //AddChild(_spriteCharacter);
        //MoveChild(_spriteCharacter, 0);

        //CharacterAnimation = _spriteCharacter.GetNode<AnimationPlayer>("AnimationPlayer");
    }

    /// <summary>
    /// Initialize character properties
    /// </summary>
    private void Initialize_Properties()
    {
        Character = new Entity(Name, isControlledByPlayer: false);

        Character.Movement.Steering.LeaderToFollow = this;     // Steering AI - Set the player node as leader
        Character.Movement.MaxSpeed = new Vector2(Nucleus_Maths.Rnd.RandfRange(30.0f, 50.0f), Nucleus_Maths.Rnd.RandfRange(30.0f, 50.0f));
        //Character.Movement.Steering.TargetGlobalPosition = GlobalPosition;
        //Character.Movement.Steering.Speed = Character.Movement.MaxSpeed.x;
    }

    /// <summary>
    /// Send the character to a new destination
    /// </summary>
    private void Set_NewDestination()
    {
        // Move the character to another place around him
        Character.Movement.Steering.Set_TargetGlobalPosition(GlobalPosition, 10.0f, 
            Nucleus.ScreenWidth-10.0f, 10.0f, Nucleus.ScreenHeight-10.0f, _radiusMovement);
        
        if (Character.DebugMode) 
            DebugLabel2.Text = Mathf.Floor(Character.Movement.Steering.TargetGlobalPosition.X) + "/" + Mathf.Floor(Character.Movement.Steering.TargetGlobalPosition.Y);

        StateMachine.TransitionTo("Move");
    }

    /// <summary>
    /// Random timing before the character change destination
    /// </summary>
    private void Initialize_TimerNewDestination()
    {
        _timerChangeDestination.WaitTime = Nucleus_Maths.Rnd.RandfRange(_minTimerNewDestination, _maxTimerNewDestination);
        _timerChangeDestination.Start();
    }

    #region ACTIONS

    /// <summary>
    /// When an item has been touched (called by ItemGeneric)
    /// </summary>
    /// <param name="itemProperties">All properties of the item touched</param>
    /// <param name="itemTouchedBy">The name of the node that hits the item</param>
    public void Item_Action(Item itemProperties, string itemTouchedBy)
    {
        // Call the generic method
        // CharacterProperties.ActionFrom_Item(itemProperties, itemTouchedBy, _timerItemActionDuration);
    }

    #endregion ACTIONS

    #endregion
}