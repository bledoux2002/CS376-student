using UnityEngine;

/// <summary>
/// Periodically spawns the specified prefab in a random location.
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Object to spawn
    /// </summary>
    public GameObject Prefab;

    /// <summary>
    /// Seconds between spawn operations
    /// </summary>
    public float SpawnInterval = 20;

    /// <summary>
    /// Time of next spawn
    /// </summary>
    public float spawnTime = 0;

    /// <summary>
    /// How many units of free space to try to find around the spawned object
    /// </summary>
    public float FreeRadius = 10;

    /// <summary>
    /// 
    /// </summary>

    /// <summary>
    /// Check if we need to spawn and if so, do so.
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void Update()
    {
        float time = Time.time;
        if (time > spawnTime)
        {
            Instantiate(Prefab, SpawnUtilities.RandomFreePoint(FreeRadius), Quaternion.identity);
            spawnTime = spawnTime + SpawnInterval;
        }
    }
}