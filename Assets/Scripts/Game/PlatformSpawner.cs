using System.Collections;
using UnityEngine;

/// <summary>
/// The PlatformSpawner class manages the instantiation and movement of platforms in the game.
/// It spawns platforms at random intervals and moves them across the screen.
/// </summary>
public class PlatformSpawner : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 20;  // Speed at which platforms move
    [HideInInspector] public float minTimeToSpawn = 1; // Minimum time interval for spawning platforms
    [HideInInspector] public float maxTimeToSpawn = 2; // Maximum time interval for spawning platforms
    private int spawnCount; // Counter for the number of spawned platforms

    private Transform _parentPlatforms; // Parent object to which platforms will be attached

    /// <summary>
    /// Initializes the parent transform for platforms when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        _parentPlatforms = GameObject.Find("Platforms").transform;
    }

    /// <summary>
    /// Starts the platform spawning process and initial platform instantiation.
    /// </summary>
    private void Start()
    {
        InstantiatePlatform();
        StartCoroutine(SpawnPlatform());
    }

    /// <summary>
    /// Moves the spawner object continuously to the right.
    /// </summary>
    private void Update()
        => transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0), Space.World);

    /// <summary>
    /// Coroutine that spawns platforms at random intervals.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator SpawnPlatform()
    {
        while (true) // Infinite loop for continuous spawning
        {
            yield return new WaitForSeconds(Random.Range(minTimeToSpawn, maxTimeToSpawn)); // Wait for a random time
            InstantiatePlatform();
        }
    }

    /// <summary>
    /// Instantiates a new platform at the spawner's position.
    /// </summary>
    private void InstantiatePlatform()
    {
        spawnCount++;
        GameObject platform = Instantiate(Resources.Load<GameObject>("Prefab/Platform"), transform.position, Quaternion.identity, _parentPlatforms);
        platform.name = spawnCount.ToString();
    }
}