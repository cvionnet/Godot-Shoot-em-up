/// <summary>
/// Responsible for :
/// - 
/// </summary>
public partial class Bullet1 : Node2D
{
    //[Export] private int Value = 0;

    //👉 case 1 - local signal (timeout...) : as usual (eg :   _shakeLength.Connect("timeout", this, nameof(onShakeLength_Timeout));)
    //👉 case 2 - signal to another scene : use the global signal Bus   (https://www.gdquest.com/tutorial/godot/design-patterns/event-bus-singleton/)
    //      1. cut-paste the [Signal] declaration to StateManager.cs  (⚠ make it public)
    //      2. to emit                                       : Nucleus_Utils.State_Manager.EmitSignal("Signal_Name", params);
    //      3. to connect (in the _Ready of the other scene) : Nucleus_Utils.State_Manager.Connect("Signal_Name", this, nameof(method-to-execute));
    //[Signal] private delegate void %CLASS%_DESTINATION_MySignal(bool value1, int value2);

    //private int _value2 = 0;

    //public int value1 = 0;

    //*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    // A constructor replace the _init() method in GDScript ("Called when the engine creates object in memory")
    //public %CLASS%()
    //{}

    // Called when the node enters the scene tree and before children
    //public override void _EnterTree()
    //{}

    // Called when the node and its children have entered the scene tree
    public override void _Ready()
    {
        Initialize_Bullet1();
    }

    // To draw custom nodes (primitives ...). Called once, then draw commands are cached.
    // Use Update(); in _Process() to call _Draw() every frame
    //   All draw* shapes : https://docs.godotengine.org/en/stable/classes/class_canvasitem.html#class-canvasitem
    //public override void _Draw()
    //{}

    //public override void _Process(double delta)
    //{}

    //public override void _PhysicsProcess(double delta)
    //{}

    // Use to add warning in the Editor   (must add the [Tool] attribute on the class)
    //public override string _GetConfigurationWarning()
    //{ return (MyObject == null) ? "The object XXXX must not be empty !" : ""; }    

    // Use to detect a key not defined in the Input Manager  (called only when a touch is pressed or released - not suitable for long press like run button)
    // Note : it's cleaner to define key in the Input Manager and use  Input.IsActionPressed("myaction")   in  _Process
    /*public override void _UnhandledInput(InputEvent @event)
    {
        // To restart the scene using Alt+R  (key combination need to be define in Input Map)
        if (@event.IsActionPressed("debug_restart"))
            GetTree().ReloadCurrentScene();


        if (@event is InputEventKey eventKey)
        {
            // Close game if press Escape
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
            {
                GetTree().Quit();
                //_sceneTree.SetInputAsHandled();   // If uncommented, all eventKey conditions below will not be tested (usefull for a Pause)
            }
        }
    }*/

    #endregion

    //*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    #endregion

    //*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_Bullet1()
    {}

    #endregion
}