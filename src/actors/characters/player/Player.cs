namespace BulletBallet.actors.characters.player;

/// <summary>
/// Responsible for :
/// - initializing the character properties
/// - initializing the StateMachine
/// - receiving actions from Items
/// </summary>
public partial class Player : CharacterBody2D
{
    public Character CharacterProperties { get; private set; }

    public StateMachine_Player StateMachine { get; private set; }
    public camera_shake.CameraShake Camera { get; private set; }
    public Timer TimerDashDuration { get; private set; }
    public Timer TimerScore { get; private set; }
    public AnimationPlayer CharacterAnimation { get; private set; }
    public Sprite2D CharacterSprite { get; private set; }

    private Timer _timerItemActionDuration;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        StateMachine = GetNode<StateMachine_Player>("StateMachine");
        Camera = GetNode<camera_shake.CameraShake>("CameraShake");
        CharacterAnimation = GetNode<AnimationPlayer>("CharacterAnimation");
        CharacterSprite = GetNode<Sprite2D>("CharacterSprite");

        TimerDashDuration = GetNode<Timer>("CharacterTimers/TimerDashDuration");
        TimerScore = GetNode<Timer>("CharacterTimers/TimerScore");
        _timerItemActionDuration = GetNode<Timer>("CharacterTimers/TimerItemActionDuration");

        Initialize_Player();
    }

    public override void _Process(double delta) { }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Player()
    {
        Name = "Player";       // prefix name of nodes

        Initialize_Properties();
        StateMachine.Init_StateMachine(this);
    }
    
    /// <summary>
    /// Initialize character properties
    /// </summary>
    private void Initialize_Properties()
    {
        CharacterProperties = new Character(isPlateformer: false, Name);

        CharacterProperties.IsControlledByPlayer = true;
        CharacterProperties.Inertia_Start = 800.0f;     //400.0f for ice effect
        CharacterProperties.Inertia_Stop = 800.0f;
        CharacterProperties.MaxSpeed_Default = new Vector2(300.0f, 300.0f);

        // Dash
        CharacterProperties.Dash_SpeedBoost = 1.5f;
        TimerDashDuration.WaitTime = 0.3f;
    }

    #region ACTIONS

    /// <summary>
    /// When an item has been touched (called by ItemGeneric)
    /// </summary>
    /// <param name="itemProperties">All properties of the item touched</param>
    /// <param name="itemTouchedBy">The name of the node that hits the item</param>
    public void Item_Action(items.classes.Item itemProperties, string itemTouchedBy)
    {
        // Call the generic method
        CharacterProperties.ActionFrom_Item(itemProperties, itemTouchedBy, _timerItemActionDuration);
    }

    #endregion ACTIONS

    #endregion
}