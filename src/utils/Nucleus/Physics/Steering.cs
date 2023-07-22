using BulletBallet.actors.characters.classes;

namespace BulletBallet.utils.NucleusFW.Physics;

public static class Nucleus_Steering
{
    public const float STEERING_DEFAULT_MASS = 2.0f;
    public const float STEERING_DEFAULT_MAXSPEED = 400.0f;
    public const float STEERING_CLOSE_DISTANCE = 3.0f;      // to stop the character to move if he is closed to the target (or it will have a kind of Parkinson movement)
    public const float STEERING_DEFAULT_FLEE = 200.0f;      // to run away

    public static Node2D LeaderToFollow { get; set; }    // to save the leader node to follow

    /// <summary>
    /// Calculate a velocity to move a character towards a destination (Follow)
    /// </summary>
    /// <param name="characterProperties">a Character object with all properties</param>
    /// <param name="position">the actual global position of the character</param>
    /// <param name="stopRadius">the circle radius around the target where the character stops</param>
    /// <returns>A vector2 to represent the destination velocity or a Vector2(0,0) if character is close to the target</returns>
    public static Vector2 Steering_Seek(Character characterProperties, Vector2 position, float stopRadius = STEERING_CLOSE_DISTANCE)
    {
//            return Steering_Seek(pCharacterProperties.Velocity, pPosition, pCharacterProperties.Steering.TargetGlobalPosition, pCharacterProperties.Steering.Speed,
//                            pCharacterProperties.Steering.Slow_Radius, pCharacterProperties.Steering.Mass, pStopRadius);

        return Steering_Seek(characterProperties.Velocity, position, characterProperties.Steering.TargetGlobalPosition, characterProperties.MaxSpeed.X,
                        characterProperties.Steering.Slow_Radius, characterProperties.Steering.Mass, stopRadius);
    }

    /// <summary>
    /// Calculate a velocity to move a character towards a destination (Follow)
    /// </summary>
    /// <param name="velocity">the actual velocity of the character</param>
    /// <param name="position">the actual global position of the character</param>
    /// <param name="targetPosition">the destination of the character</param>
    /// <param name="maxSpeed">the maximum speed the character can reach</param>
    /// <param name="slowRadius">(0.0f = no slow down) the circle radius around the target where the character starts to slow down</param>
    /// <param name="mass">to slow down the character</param>
    /// <param name="stopRadius">the circle radius around the target where the character stops</param>
    /// <returns>A vector2 to represent the destination velocity or a Vector2(0,0) if character is close to the target</returns>
    public static Vector2 Steering_Seek(Vector2 velocity, Vector2 position, Vector2 targetPosition,
                                        float maxSpeed = STEERING_DEFAULT_MAXSPEED, float slowRadius = 0.0f, float mass = STEERING_DEFAULT_MASS, float stopRadius = STEERING_CLOSE_DISTANCE)
    {
        // Check if we have enough distance between the character and the target
        if (position.DistanceTo(targetPosition) <= stopRadius)
            return Nucleus_Maths.VECTOR_0;

        // STEP 1 : use the formula to get the shortest path possible to the target
        Vector2 desire_velocity = (targetPosition - position).Normalized() * maxSpeed;

        // STEP 2 : slow down the character when he is closed to the target
        if (slowRadius != 0.0f)
        {
            float to_target = position.DistanceTo(targetPosition);

            // Reduce velocity if in the slow circle around the target
            if (to_target <= slowRadius)
                desire_velocity *= ((to_target / slowRadius) * 0.8f) + 0.2f;       // 0.8f + 0.2f : used to not slow down too much the character
        }

        // STEP 3 : apply the steering formula (use mass to slow down character's movement)
        return _CalculateSteering(desire_velocity, velocity, mass);
    }

    /// <summary>
    /// Calculate a velocity to move a character away from a destination (Run away)
    /// </summary>
    /// <param name="velocity">the actual velocity of the character</param>
    /// <param name="position">the actual global position of the character</param>
    /// <param name="targetPosition">the destination of the character</param>
    /// <param name="maxSpeed">the maximum speed the character can reach</param>
    /// <param name="fleeRadius">the circle radius where the character stop to flee</param>
    /// <param name="mass">to slow down the character</param>
    /// <returns>A vector2 to represent the destination velocity or a Vector2(0,0) if character is away to the target</returns>
    public static Vector2 Steering_Flee(Vector2 velocity, Vector2 position, Vector2 targetPosition,
                                        float maxSpeed = STEERING_DEFAULT_MAXSPEED, float fleeRadius = STEERING_DEFAULT_FLEE, float mass = STEERING_DEFAULT_MASS)
    {
        // If the target is outside the radius, do nothing
        if (position.DistanceTo(targetPosition) >= fleeRadius)
            return Nucleus_Maths.VECTOR_0;

        // Use the formula to get the shortest path possible to run away from the target, then apply steering
        Vector2 desire_velocity = (position - targetPosition).Normalized() * maxSpeed;
        return _CalculateSteering(desire_velocity, velocity, mass);
    }

    /// <summary>
    /// Calculate the steering
    /// </summary>
    /// <param name="desiredVelocity">Calculated from the Steering Behaviour formula (seek, flee ...)</param>
    /// <param name="velocity">the actual velocity of the character</param>
    /// <param name="mass">to slow down the character</param>
    /// <returns>A vector2 to represent the steering velocity</returns>
    private static Vector2 _CalculateSteering(Vector2 desiredVelocity,Vector2 velocity, float mass)
    {
        Vector2 steering = (desiredVelocity - velocity) / mass;
        return velocity + steering;
    }

    /// <summary>
    /// Calculate the velocity to keep distance behind the leader
    /// </summary>
    /// <param name="leaderPosition">the position of the node to follow</param>
    /// <param name="followerPosition">the position of the follower</param>
    /// <param name="followOffset">the distance to keep between the leader and follower node</param>
    /// <returns>A vector2 to represent the position to follow with the distance to keep</returns>
    public static Vector2 Steering_CalculateDistanceBetweenFollowers(Vector2 leaderPosition, Vector2 followerPosition, float followOffset)
    {
        // Get the vector direction to the leader (behind the leader)
        Vector2 direction = (followerPosition - leaderPosition).Normalized();
        Vector2 velocity = followerPosition - (direction * followOffset);

        // To avoid the follower to be too close to the leader (or it will have a kind of Parkinson movement)
        if (leaderPosition.DistanceTo(followerPosition) <= followOffset)
            velocity = followerPosition;

        return velocity;
    }

    /* OLD Steering_CalculateFlee (2020-01-19) - METHOD FROM GDSCRIPT

    /// /// <summary>
    /// Calculate a velocity to flee from a destination
    /// </summary>
    /// <param name="pPosition">the actual global position of the character</param>
    /// <param name="pTargetPosition">the destination of the character</param>
    /// <param name="pFleeRadius">the circle radius where the character starts to flee</param>
    /// <returns>A vector2 to represent the destination velocity</returns>
    public static Vector2 Steering_CalculateFlee(Vector2 pPosition, Vector2 pTargetPosition, float pFleeRadius = STEERING_DEFAULT_FLEE)
    {
        // If the target is outside the radius, do nothing
        if (pPosition.DistanceTo(pTargetPosition) > pFleeRadius)
            return pPosition;

        Vector2 flee_global_position = pTargetPosition - (pTargetPosition - pPosition).Normalized();
        Vector2 target_position = pPosition + (pPosition - flee_global_position).Normalized() * pFleeRadius;

        return target_position;
    }

    */
}
