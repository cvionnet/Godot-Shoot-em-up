using BulletBallet.actors.characters.classes;

namespace BulletBallet.utils.NucleusFW.Physics;

public static class Nucleus_Movement
{
    /// <summary>
    /// Get the moving direction of a character according of the inputs
    /// </summary>
    /// <param name="moveLeft">the name of the action used in the Input Map</param>
    /// <param name="moveRight">the name of the action used in the Input Map</param>
    /// <param name="normalize">True to return a normalized vector</param>
    /// <param name="moveUp">(optional) the name of the action used in the Input Map</param>
    /// <param name="moveDown">(optional) the name of the action used in the Input Map</param>
    /// <returns>A vector2 to represent the direction of the character</returns>
    public static Vector2 GetMovingDirection(string moveLeft, string moveRight, bool normalize, string moveUp = "", string moveDown = "")
    {
        Vector2 new_velocity;

        // Get the direction
        if(moveUp != "")
        {
            // Top-down
            new_velocity = new Vector2(
                Input.GetActionStrength(moveRight) - Input.GetActionStrength(moveLeft),
                Input.GetActionStrength(moveDown) - Input.GetActionStrength(moveUp)
            );
        }
        else
        {
            // Plateformer
            new_velocity = new Vector2(
                Input.GetActionStrength(moveRight) - Input.GetActionStrength(moveLeft),
                1.0f
            );
        }

        // Normalize the vector (to not have a character that move quicker with an angled direction)
        new_velocity = (normalize) ? new_velocity.Normalized() : new_velocity;

        return new_velocity;
    }

    /// <summary>
    /// Calculate the velocity of a character
    /// </summary>
    /// <param name="characterProperties">a Character object with all properties</param>
    /// <param name="delta">delta time</param>
    /// <returns>A vector2 to represent the new velocity</returns>
    public static Vector2 CalculateVelocity(Character characterProperties, double delta)
    {
        // Formula in CalculateVelocity() use the value of MaxFall_Speed to limit y axis upper speed (with Mathf.Clamp). If topdown game, set this value to MaxSpeed.y
        float maxspeed = characterProperties.IsPlateformer ? characterProperties.MaxFall_Speed : characterProperties.MaxSpeed.Y;

        return CalculateVelocity(characterProperties.Velocity, characterProperties.MaxSpeed, characterProperties.Acceleration, characterProperties.Deceleration
                                , characterProperties.Direction, delta, maxspeed);
    }

    /// <summary>
    /// Calculate the velocity of a character
    /// </summary>
    /// <param name="actualVelocity">the actual velocity of the character</param>
    /// <param name="maxSpeed">to limit the new velocity</param>
    /// <param name="acceleration">the acceleration of the character</param>
    /// <param name="deceleration">the deceleration of the character</param>
    /// <param name="direction">the direction of the character</param>
    /// <param name="delta">delta time</param>
    /// <param name="maxFallSpeed">the maximum speed the character can reach when he is falling</param>
    /// <returns>A vector2 to represent the new velocity</returns>
    public static Vector2 CalculateVelocity(Vector2 actualVelocity, Vector2 maxSpeed, Vector2 acceleration, Vector2 deceleration, Vector2 direction, double delta, float maxFallSpeed = 1500.0f)
    {
        Vector2 new_velocity;

        // Calculate the new velocity on both axis (acceleration and decceleration)
        new_velocity.X = (float)_CalculateNewVelocity(direction.X, actualVelocity.X, acceleration.X, deceleration.X, delta);
        new_velocity.Y = (float)_CalculateNewVelocity(direction.Y, actualVelocity.Y, acceleration.Y, deceleration.Y, delta);

        // Limit the velocity on both axis to the max speed
        new_velocity.X = Mathf.Clamp(new_velocity.X, -maxSpeed.X, maxSpeed.X);
        new_velocity.Y = Mathf.Clamp(new_velocity.Y, -maxSpeed.Y, maxFallSpeed);

        return new_velocity;
    }

    /// <summary>
    /// Calculate the new velocity for an axis (with acceleration and decceleration)
    /// </summary>
    /// <param name="direction">the direction of the character</param>
    /// <param name="actualVelocity">the actual velocity of the character</param>
    /// <param name="acceleration">the acceleration of the character</param>
    /// <param name="deceleration">he deceleration of the character</param>
    /// <param name="delta">delta time</param>
    /// <returns></returns>
    private static double _CalculateNewVelocity(float direction, float actualVelocity, float acceleration, float deceleration, double delta)
    {
        double newVelocity = 0.0d;

        // If the character has a direction (it means that the player is pressing keys), we calculate the velocity
        if (direction != 0.0f)
        {
            newVelocity = actualVelocity + (direction * acceleration * delta);
        }
        // Else we calculate the deceleration
        else if (direction == 0.0f && Mathf.Abs(actualVelocity) > 0.2f)
        {
            // Read the direction from velocity (can be positive or negative)
            int directionVelocity = Mathf.Sign(actualVelocity);

            newVelocity = actualVelocity - (directionVelocity * deceleration * delta);

            // To avoid a bug : if the Inertia_Stop is a large value, the result vector will be greater than 0.2f and in the opposite direction (the player moves by himself)
            if (Mathf.Sign(newVelocity) != Mathf.Sign(actualVelocity))
                newVelocity = 0.0f;
        }
        else
        {
            newVelocity = 0.0f;
        }

        return newVelocity;
    }

    /// <summary>
    /// Calculate the jump velocity of a character
    /// </summary>
    /// <param name="velocity">the actual velocity of the character</param>
    /// <param name="maxSpeed">to limit the new velocity</param>
    /// <param name="impulse">a value to represent the high of the jump</param>
    /// <returns>A vector2 to represent the jump velocity</returns>
    public static Vector2 CalculateJumpVelocity(Vector2 velocity, Vector2 maxSpeed, float impulse)
    {
        return CalculateVelocity(velocity, maxSpeed, new Vector2(0.0f, impulse), Nucleus_Maths.VECTOR_0, Nucleus_Maths.VECTOR_FLOOR, 1.0f);
    }
}
