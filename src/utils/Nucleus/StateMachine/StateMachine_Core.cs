using Godot.Collections;

namespace BulletBallet.utils.NucleusFW.StateMachine;

/// <summary>
/// Responsible for :
/// - storing the active state
/// - calling Updates() + Input() of the active state
/// - transitioning to a new state
/// </summary>
public partial class StateMachine_Core<T> : Node
{
    public IState ActiveState { get; set; }

    private T _rootNode;

    //*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public void Initialize_StateMachine_Core(NodePath initialState, T rootNode)
    {
        _rootNode = rootNode;

        // Set the initial state
        ActiveState = GetNode<IState>(initialState);
        ActiveState.Enter_State<T>(rootNode);
    }

    public void Update(double delta)           => ActiveState.Update(delta);
    public void Physics_Update(double delta)   => ActiveState.Physics_Update(delta);
    public void Input_State(InputEvent @event) => ActiveState.Input_State(@event);

    #endregion

    //*-------------------------------------------------------------------------*//

    #region USER METHODS

    /// <summary>
    /// Move from the previous state to a new state
    /// </summary>
    /// <param name="targetStatePath">The node path from the "StateMachine" node to the state (because we could have a hierarchy like "StateMachine" > "Move" > "Idle" </param>
    /// <param name="param">A dictionary to pass parameters from a state to the next one </param>
    public void TransitionTo(string targetStatePath, Dictionary<string, GodotObject> param = null)
    {
        // Check if the path exists
        if (!HasNode(targetStatePath))
            return;

        // Get the new state, exit the previous one, set the new one as active and enter it
        var newState = GetNode<IState>(targetStatePath);
        ActiveState.Exit_State();
        ActiveState = newState;
        ActiveState.Enter_State<T>(_rootNode, param);
    }

    #endregion
}