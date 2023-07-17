using Godot;
using Nucleus;

/// <summary>
/// Contains all generic properties shared between all characters (max speed, acceleration ...)
/// </summary>
public partial class Character
{
    public Character(bool isPlateformer, string pName)
    {
        IsPlateformer = isPlateformer;
        Name = pName;
    }

#region INITIAL SETUP

    // To set default values according to the game type (plateformer or top-down)
    public bool IsPlateformer {
        get => _isPlateformer;
        set {
            _isPlateformer = value;

            Gravity = _isPlateformer ? 3000.0f : 0.0f;
            MaxFall_Speed = _isPlateformer ? 1500.0f : 0.0f;
            Deceleration = _isPlateformer ? new Vector2(Inertia_Stop, 0.0f) : new Vector2(Inertia_Stop, Inertia_Stop) ;
        }
    }
    private bool _isPlateformer;

    public string Name { get; set; }

    public bool IsControlledByPlayer {
        get => _isControlledByPlayer;
        set {
            _isControlledByPlayer = value;
            
            // Initialize characters controlled by AI
            if (!_isControlledByPlayer)
                Velocity = Nucleus_Utils.VECTOR_0;
        }
    }
    private bool _isControlledByPlayer;
    
    public bool DebugMode { get; set; } = Nucleus_Utils.DEBUG_MODE;    // to activate or no debug options on a character

#endregion

#region CHARACTER PROPERTIES

    public int Score { get; set; } = 0;

#endregion
}