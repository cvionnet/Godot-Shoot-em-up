using BulletBallet.utils.NucleusFW.AI;

namespace BulletBallet.actors.characters.classes;

public class Entity_Movement2D
{
    public Nucleus_Steering Steering { get; } = new Nucleus_Steering();
    public bool IsMoving { get; set; } = false;

    public Vector2 Acceleration { get; set; }
    public Vector2 Deceleration { get; set; }

    public Vector2 MaxSpeed { get; set; }
    public float MaxFallSpeed { get; private set; }        // to make the character fall quicker (add more mass)
    
    public bool IsOrientationHorizontalInverted { get; private set; }      // Left or Right
    public bool IsOrientationVerticalInverted { get; private set; }        // Up or Down

    public Vector2 Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            // To flip Sprite when moving
            IsOrientationHorizontalInverted = Detect_SpriteOrientation(_direction.X, IsOrientationHorizontalInverted);
            IsOrientationVerticalInverted = Detect_SpriteOrientation(_direction.Y, IsOrientationVerticalInverted);
        }
    }
    private Vector2 _direction;

    public Vector2 Velocity
    {
        get => _velocity;
        set
        {
            _velocity = value;
            // To flip Sprite when moving
            IsOrientationHorizontalInverted = Detect_SpriteOrientation(_velocity.X, IsOrientationHorizontalInverted);
            IsOrientationVerticalInverted = Detect_SpriteOrientation(_velocity.Y, IsOrientationVerticalInverted);
        }
    }
    private Vector2 _velocity;

    public float Inertia_Start {
        get => _inertia_Start;
        set {
            _inertia_Start = value;
            Acceleration = (Nucleus.Genre == GameManager.Genre.PLATEFORMER) ? new Vector2(Inertia_Start, Gravity) : new Vector2(Inertia_Start, Inertia_Start);
        }
    }
    private float _inertia_Start;

    public float Inertia_Stop {
        get => _inertia_Stop;
        set {
            _inertia_Stop = value;
            Deceleration = (Nucleus.Genre == GameManager.Genre.PLATEFORMER) ? new Vector2(Inertia_Stop, 0.0f) : new Vector2(Inertia_Stop, Inertia_Stop) ;
        }
    }
    private float _inertia_Stop;    
    
    public float Gravity {
        get => _gravity;
        set {
            _gravity = value;
            Acceleration = (Nucleus.Genre == GameManager.Genre.PLATEFORMER) ? new Vector2(Inertia_Start, Gravity) : new Vector2(Inertia_Start, Inertia_Start);
        }
    }
    private float _gravity;    
    
    #region DASH
    public float Dash_SpeedBoost { get; set; }      // default percentage to boost the character maxspeed
    public bool IsDashing { get; set; } = false;

    // Save the max speed the character can raise (a dash will overcome this value)
    public Vector2 MaxSpeed_Default {
        get => _maxSpeed_Default;
        set {
            _maxSpeed_Default = value;
            MaxSpeed = _maxSpeed_Default;
        }
    }
    private Vector2 _maxSpeed_Default;
    #endregion      
    
    //*-------------------------------------------------------------------------*//
    
    public Entity_Movement2D(bool isControlledByPlayer)
    {
        if (!isControlledByPlayer)
            Velocity = Nucleus_Maths.VECTOR_0;

        Set_DefaultValues();
    }

    private void Set_DefaultValues()
    {
        Inertia_Start = 800.0f;     //400.0f for ice effect
        Inertia_Stop = 800.0f;
        MaxSpeed_Default = new Vector2(300.0f, 300.0f);
        
        Gravity = (Nucleus.Genre == GameManager.Genre.PLATEFORMER) ? 3000.0f : 0.0f;
        MaxFallSpeed = (Nucleus.Genre == GameManager.Genre.PLATEFORMER) ? 1500.0f : 0.0f;
        Deceleration = (Nucleus.Genre == GameManager.Genre.PLATEFORMER) ? new Vector2(Inertia_Stop, 0.0f) : new Vector2(Inertia_Stop, Inertia_Stop);

        // Dash
        Dash_SpeedBoost = 1.5f;
        MaxSpeed_Default = new Vector2(300.0f, 300.0f);
    }
    
    /// <summary>
    /// Check if orientation must be inverted (horizontal or vertical)
    /// Used to flip Sprite in the correct orientation when they are moving
    /// </summary>
    /// <param name="direction">The direction of the character</param>
    /// <param name="previousOrientationStatus">The previous </param>
    /// <returns>True if the orientation must be inverted</returns>
    private bool Detect_SpriteOrientation(float direction, bool previousOrientationStatus)
    {
        bool newOrientationStatus = previousOrientationStatus;      // to keep the same orientation if the character is not moving 

        if (direction > 0 && previousOrientationStatus)
            newOrientationStatus = false;
        else if (direction < 0 && !previousOrientationStatus)
            newOrientationStatus = true;

        return newOrientationStatus;
    }    
}
