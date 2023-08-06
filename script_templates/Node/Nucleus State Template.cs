using Godot;
using System;

/// <summary>
/// Responsible for :
/// - 
/// </summary>
public class %CLASS% : Node, IState
{
#region HEADER

    //[Export] private int Value = 0;

    //👉 case 1 - local signal (timeout...) : as usual (eg :   _shakeLength.Connect("timeout", this, nameof(onShakeLength_Timeout));)
    //👉 case 2 - signal to another scene : use the global signal Bus   (https://www.gdquest.com/tutorial/godot/design-patterns/event-bus-singleton/)
    //      1. cut-paste the [Signal] declaration to StateManager.cs  (⚠ make it public)
    //      2. to emit                                       : Nucleus_Utils.State_Manager.EmitSignal("Signal_Name", params);
    //      3. to connect (in the _Ready of the other scene) : Nucleus_Utils.State_Manager.Connect("Signal_Name", this, nameof(method-to-execute));
    //[Signal] private delegate void %CLASS%_DESTINATION_MySignal(bool value1, int value2);

    //private int _value2 = 0;

    //public int value1 = 0;

    #endregion

    //*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    // Called when the node enters the scene tree for the first time
    //public override void _Ready()
    //{}

    #endregion

    //*-------------------------------------------------------------------------*//

    #region INTERFACE IMPLEMENTATION

    public void Enter_State(Godot.Collections.Dictionary<string, object> pParam)
    {}

    public void Exit_State()
    {}

    public void Update(float delta)
    {}

    public void Physics_Update(float delta)
    {}

    public void Input_State(InputEvent @event)
    {}

    public string GetStateName()
    {
        return this.Name;
    }

    #endregion

    //*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    #endregion

    //*-------------------------------------------------------------------------*//

    #region USER METHODS

    #endregion
}