namespace BulletBallet.utils.NucleusFW.SpawnFactory;

/// <summary>
/// A class to define timing options to apply to spawn an instance
/// </summary>
public sealed class Spawn_Timing
{
    public bool IsTimed { get; private set; }
    public bool IsRandomTime { get; private set; }
    //public bool IsRandomTimePerSpawn { get; private set; }
    public float MinTime { get; private set; }
    public float MaxTime { get; private set; }

    /// <summary>
    /// Constructor (empty constructor : timing options are disabled)
    /// </summary>
    /// <param name="isTimed">Set to true to create a Spawn_Timing</param>
    /// <param name="isRandomTime">Use a random spawn timing ?</param>
    /// <param name="isRandomTimePerSpawn">Set a random spawn timing on each instance ?</param>
    /// <param name="minTime">Minimum time to wait before creating a new instance</param>
    /// <param name="maxTime">Maximum time to wait before creating a new instance (used if pIsRandomTime or pIsRandomTimePerSpawn=true)</param>
    public Spawn_Timing(bool isTimed=false, bool isRandomTime=false, bool isRandomTimePerSpawn=false, float minTime=0.0f, float maxTime=1.0f)
    {
        IsTimed = isTimed;
        IsRandomTime = isRandomTime;
        //IsRandomTimePerSpawn = pIsRandomTimePerSpawn;
        MinTime = minTime;
        MaxTime = maxTime;
    }

    /// <summary>
    /// Return the amount of time to wait
    ///     - if IsTimed = false                        => return 0.0f
    ///     - if IsTimed = true                         => return MinTime
    ///     - if IsTimed = true and IsRandomTime = true => return a random value between MinTime and MaxTime
    /// </summary>
    /// <returns>A float to represent the amount of time</returns>
    public float GetTiming()
    {
        float spawnTime = 0.0f;

        if(IsTimed)
        {
            spawnTime = MinTime;

            if (IsRandomTime)
                spawnTime = Nucleus_Maths.Rnd.RandfRange(MinTime, MaxTime);
        }

        return spawnTime;
    }
}