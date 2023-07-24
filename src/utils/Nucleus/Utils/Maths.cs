namespace BulletBallet.utils.NucleusFW.Utils;

public static class Nucleus_Maths
{
    public static RandomNumberGenerator Rnd { get; } = new RandomNumberGenerator();
    private const float _zero = 0.0f;

    public static Vector2 VECTOR_0 { get; } = new Vector2(0.0f,0.0f);         // (=Vector2.ZERO in GDScript)
    public static Vector2 VECTOR_1 { get; } = new Vector2(1.0f,1.0f);
    public static Vector2 VECTOR_INF { get; } = new Vector2(1.0f/_zero,1.0f/_zero);    // infinite vector  (=Vector2.INF in GDScript)
    public static Vector2 VECTOR_FLOOR { get; } = new Vector2(0,-1);          // (=Vector2.UP in GDScript) Use it for plateformer
    
    //*-------------------------------------------------------------------------*//

    #region METHODS - RANDOM

    /// <summary>
    /// Return a int random value, avoiding 0
    /// </summary>
    /// <param name="min">min int value</param>
    /// <param name="max">max int value</param>
    /// <returns>int random value</returns>
    public static int RndIntAvoidZero(int min = -1, int max = 1)
    {
        int number;
        do
        {
            number = Rnd.RandiRange(min, max);
        } while (number == 0);
        return number;
    }

    /// <summary>
    /// Return a float random value, avoiding 0.0f
    /// </summary>
    /// <param name="min">min float value</param>
    /// <param name="max">max float value</param>
    /// <returns>float random value</returns>
    public static float RndFloatAvoidZero(float min = -1.0f, float max = 1.0f)
    {
        float number;
        do
        {
            number = Rnd.RandfRange(min, max);
        } while (number == 0.0f);
        return number;
    }

    #endregion

    #region METHODS - GEOMETRY

    /// <summary>
    /// Get the direction vector between 2 objects
    /// </summary>
    /// <param name="actualPosition">A vector2 representing the 1st object (eg : the player.GlobalPosition)</param>
    /// <param name="targetPosition">A vector2 representing the 2nd object (eg : the enemy.GlobalPosition)</param>
    /// <param name="normalize">set to True to return a normalized vector</param>
    /// <returns>A Vector2 to represent the direction</returns>
    public static Vector2 GetDirectionBetween_2_Objects(Vector2 actualPosition, Vector2 targetPosition, bool normalize = true)
    {
        //Vector2 direction = VECTOR_0;
        Vector2 direction = normalize ? (targetPosition - actualPosition).Normalized() : (targetPosition - actualPosition);
        return direction;
    }

    /// <summary>
    /// Get the distance between 2 objects
    /// </summary>
    /// <param name="actualPosition">A vector2 representing the 1st object (eg : the player.GlobalPosition)</param>
    /// <param name="targetPosition">A vector2 representing the 2nd object (eg : the enemy.GlobalPosition)</param>
    /// <returns>A float to represent the distance</returns>
    public static float GetDistanceBetween_2_Objects(Vector2 actualPosition, Vector2 targetPosition)
    {
        return (targetPosition - actualPosition).Length();
    }

    /// <summary>
    /// Get the angle between 2 objects
    /// </summary>
    /// <param name="actualPosition">A vector2 representing the 1st object (eg : the player.GlobalPosition)</param>
    /// <param name="targetPosition">A vector2 representing the 2nd object (eg : the enemy.GlobalPosition)</param>
    /// <returns>A float to represent the angle in Radians</returns>
    public static float GetAngleTo(Vector2 actualPosition, Vector2 targetPosition)
    {
        // Get the destination position, then the angle to the destination position
        return (targetPosition - actualPosition).Angle();
    }

    /// <summary>
    /// Get the centered position of any set of sprites     Eg : get the center position of a grid made from multiple cells
    /// </summary>
    /// <param name="totalWidth">The total width of the set</param>
    /// <param name="totalHeight">The total height of the set</param>
    /// <param name="uniqueElementSize">the size of an unique element of the set</param>
    /// <param name="destinationPosition">the point where the set will be displayed (typically Utils.VECTOR_0   or  GlobalPosition)</param>
    /// <returns>A vector to represent the centered coordinates</returns>
    public static Vector2 Get_CenteredPosition(float totalWidth, float totalHeight, float uniqueElementSize, Vector2 destinationPosition)
    {
        // Get the centered position
        float x = uniqueElementSize - (uniqueElementSize / 2) - (totalWidth / 2);
        float y = uniqueElementSize - (totalHeight / 2);

        // Add the destination point
        return new Vector2(x + destinationPosition.X, y + destinationPosition.Y);
    }

    #endregion
}
