using System;
using TMPro;
using UnityEngine;

/// <summary>
/// The PlatformHit class manages the number of hits required for a platform to be destroyed.
/// It updates the visual representation of remaining hits and handles player collisions.
/// </summary>
public class PlatformHit : MonoBehaviour
{
    private int _mustHitTimes = 1; // Total hits required to destroy the platform
    private int _minMustTimes = 1;  // Minimum hits required
    private int _maxMustTimes = 2;  // Maximum hits required
    [SerializeField] private TMP_Text _textHitTimes; // UI element displaying the hit count

    /// <summary>
    /// Initializes the platform hit count based on the platform's name.
    /// </summary>
    private void Start()
    {
        // Check if the platform's name indicates a certain type
        if (Int16.Parse(gameObject.name) < 5)
            return;

        // Set random hit count between minimum and maximum
        _mustHitTimes = UnityEngine.Random.Range(_minMustTimes, _maxMustTimes + 1);
        _textHitTimes.text = $"X {_mustHitTimes}";
    }

    /// <summary>
    /// Returns the number of hits left to destroy the platform.
    /// </summary>
    /// <returns>Number of remaining hits.</returns>
    public int GetLeftHitTimes()
    {
        return _mustHitTimes;
    }

    /// <summary>
    /// Handles collision with the player and updates the hit count.
    /// </summary>
    /// <param name="collision">The collision details.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object that collided is the player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Play bounce sound effect
            AudioManager.PlayFromResources("Bounce", 0.1f, UnityEngine.Random.Range(0.8f, 1.2f));
            _mustHitTimes -= 1; // Decrement the hit count
            _textHitTimes.text = $"X {_mustHitTimes}"; // Update UI text

            // Check if the platform is destroyed
            if (_mustHitTimes == 0)
            {
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.4f); // Fade the platform
                _textHitTimes.color = new Color(0, 0, 0, 0.7f); // Fade the hit text
                GetComponent<BoxCollider2D>().enabled = false; // Disable the collider
            }
        }
    }
}
