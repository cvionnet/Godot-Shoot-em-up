namespace BulletBallet.actors.characters.classes;

/// <summary>
/// Properties used to move the character using steering methods from Nucleus.Steering (eg : for PNJ's statemachine)
/// </summary>
public class Steering
{
    public float Mass { get; }
    public float Slow_Radius { get; }       // radius the node will start to slow down

    public float Speed { get; set; }

    public Vector2 TargetGlobalPosition { get; set; }

    public Steering(float mass = 10.0f, float slowRadius = 80.0f)
    {
        Mass = mass;
        Slow_Radius = slowRadius;

        TargetGlobalPosition = Nucleus_Maths.VECTOR_0;
//        Speed = 0.0f;
    }
}