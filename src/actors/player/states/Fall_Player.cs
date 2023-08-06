using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;
using Collections = Godot.Collections;

namespace BulletBallet.actors.characters.player.states;

/// <summary>
/// Responsible for :
/// - counting time before respawning
/// - updating score (negative)
/// - positioning character on a safe platform (to respawn)
/// </summary>
public partial class Fall_Player : Node, IState
{
    private Player _rootNode;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        Initialize_Fall();
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
        }

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
        _rootNode.Update_Score(-5);
        //_rootNode.CharacterProperties.Reset_Movement();
        _rootNode.Visible = false;
        _rootNode.Camera.Zoom_Camera(Nucleus.GameManager.ZoomLevelZoomOut, 0.5f);

//        _rootNode.CallDeferred("queue_free");
    }

    #endregion
}