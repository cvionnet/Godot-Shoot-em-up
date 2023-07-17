using System;
using Godot;

namespace Nucleus.Physics
{
    public static class Nucleus_Movement
    {
        /// <summary>
        /// Get the moving direction of a character according of the inputs
        /// </summary>
        /// <param name="pMove_Left">the name of the action used in the Input Map</param>
        /// <param name="pMove_Right">the name of the action used in the Input Map</param>
        /// <param name="pNormalize">True to return a normalized vector</param>
        /// <param name="pMove_Up">(optional) the name of the action used in the Input Map</param>
        /// <param name="pMove_Down">(optional) the name of the action used in the Input Map</param>
        /// <returns>A vector2 to represent the direction of the character</returns>
        public static Vector2 GetMovingDirection(string pMove_Left, string pMove_Right, bool pNormalize, string pMove_Up = "", string pMove_Down = "")
        {
            Vector2 new_velocity;

            // Get the direction
            if(pMove_Up != "")
            {
                // Top-down
                new_velocity = new Vector2(
                    Input.GetActionStrength(pMove_Right) - Input.GetActionStrength(pMove_Left),
                    Input.GetActionStrength(pMove_Down) - Input.GetActionStrength(pMove_Up)
                );
            }
            else
            {
                // Plateformer
                new_velocity = new Vector2(
                    Input.GetActionStrength(pMove_Right) - Input.GetActionStrength(pMove_Left),
                    1.0f
                );
            }

            // Normalize the vector (to not have a character that move quicker with an angled direction)
            new_velocity = (pNormalize) ? new_velocity.Normalized() : new_velocity;

            return new_velocity;
        }

        /// <summary>
        /// Calculate the velocity of a character
        /// </summary>
        /// <param name="pCharacterProperties">a Character object with all properties</param>
        /// <param name="delta">delta time</param>
        /// <returns>A vector2 to represent the new velocity</returns>
        public static Vector2 CalculateVelocity(Character pCharacterProperties, double delta)
        {
            // Formula in CalculateVelocity() use the value of MaxFall_Speed to limit y axis upper speed (with Mathf.Clamp). If topdown game, set this value to MaxSpeed.y
            float maxspeed = pCharacterProperties.IsPlateformer ? pCharacterProperties.MaxFall_Speed : pCharacterProperties.MaxSpeed.Y;

            return CalculateVelocity(pCharacterProperties.Velocity, pCharacterProperties.MaxSpeed, pCharacterProperties.Acceleration, pCharacterProperties.Deceleration
                                    , pCharacterProperties.Direction, delta, maxspeed);
        }

        /// <summary>
        /// Calculate the velocity of a character
        /// </summary>
        /// <param name="pActualVelocity">the actual velocity of the character</param>
        /// <param name="pMax_Speed">to limit the new velocity</param>
        /// <param name="pAcceleration">the acceleration of the character</param>
        /// <param name="pDecceleration">the decceleration of the character</param>
        /// <param name="pDirection">the direction of the character</param>
        /// <param name="delta">delta time</param>
        /// <param name="pMaxFallSpeed">the maximum speed the character can reach when he is falling</param>
        /// <returns>A vector2 to represent the new velocity</returns>
        public static Vector2 CalculateVelocity(Vector2 pActualVelocity, Vector2 pMax_Speed, Vector2 pAcceleration, Vector2 pDecceleration, Vector2 pDirection, double delta, float pMaxFallSpeed = 1500.0f)
        {
            Vector2 new_velocity;

            // Calculate the new velocity on both axis (acceleration and decceleration)
            new_velocity.X = (float)_CalculateNewVelocity(pDirection.X, pActualVelocity.X, pAcceleration.X, pDecceleration.X, delta);
            new_velocity.Y = (float)_CalculateNewVelocity(pDirection.Y, pActualVelocity.Y, pAcceleration.Y, pDecceleration.Y, delta);

            // Limit the velocity on both axis to the max speed
            new_velocity.X = Mathf.Clamp(new_velocity.X, -pMax_Speed.X, pMax_Speed.X);
            new_velocity.Y = Mathf.Clamp(new_velocity.Y, -pMax_Speed.Y, pMaxFallSpeed);

            return new_velocity;
        }

        /// <summary>
        /// Calculate the new velocity for an axis (with acceleration and decceleration)
        /// </summary>
        /// <param name="pDirection">the direction of the character</param>
        /// <param name="pActualVelocity">the actual velocity of the character</param>
        /// <param name="pAcceleration">the acceleration of the character</param>
        /// <param name="pDecceleration">he decceleration of the character</param>
        /// <param name="delta">delta time</param>
        /// <returns></returns>
        private static double _CalculateNewVelocity(float pDirection, float pActualVelocity, float pAcceleration, float pDecceleration, double delta)
        {
            double newVelocity = 0.0d;

            // If the character has a direction (it means that the player is pressing keys), we calculate the velocity
            if (pDirection != 0.0f)
            {
                newVelocity = pActualVelocity + (pDirection * pAcceleration * delta);
            }
            // Else we calculate the decceleration
            else if (pDirection == 0.0f && Mathf.Abs(pActualVelocity) > 0.2f)
            {
                // Read the direction from velocity (can be positive or negative)
                int direction = Mathf.Sign(pActualVelocity);

                newVelocity = pActualVelocity - (direction * pDecceleration * delta);

                // To avoid a bug : if the Inertia_Stop is a large value, the result vector will be greater than 0.2f and in the opposite direction (the player moves by himself)
                if (Mathf.Sign(newVelocity) != Mathf.Sign(pActualVelocity))
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
        /// <param name="pVelocity">the actual velocity of the character</param>
        /// <param name="pMax_Speed">to limit the new velocity</param>
        /// <param name="pImpulse">a value to represent the high of the jump</param>
        /// <returns>A vector2 to represent the jump velocity</returns>
        public static Vector2 CalculateJumpVelocity(Vector2 pVelocity, Vector2 pMax_Speed, float pImpulse)
        {
            return CalculateVelocity(pVelocity, pMax_Speed, new Vector2(0.0f, pImpulse), Nucleus_Utils.VECTOR_0, Nucleus_Utils.VECTOR_FLOOR, 1.0f);
        }
    }
}