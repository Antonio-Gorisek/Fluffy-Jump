using System.Collections;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlatformAnimation : MonoBehaviour
{
    // How far the platform should move down each time it's hit
    [SerializeField] private float moveDistance = 3f;
    // How fast the platform moves
    [SerializeField] private float moveSpeed = 15f;

    // Original position of the platform
    private Vector3 originalPosition;

    // Track whether the object has been inside the screen
    private bool hasEnteredScreen = false;

    private void Start()
    {
        originalPosition = transform.position; // Save the original position at start
    }
    Coroutine cor;
    // Detect when player collides with platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object that collided has the "Player" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Start the animation to move the platform down
            if (cor != null) { StopCoroutine(cor); }
            cor = StartCoroutine(MovePlatformDown());
        }
    }

    // Coroutine to handle moving the platform down and trying to return to its original position
    private IEnumerator MovePlatformDown()
    {
        // Move the platform down
        Vector3 targetPosition = transform.position - new Vector3(0, moveDistance, 0);

        // Move down smoothly
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Ensure the platform reaches the target position
        transform.position = targetPosition;

        // Start trying to return to the original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Ensure the platform is exactly at its original position at the end
        transform.position = originalPosition;
    }

    // Called when the object becomes visible to any camera
    private void OnBecameVisible()
    {
        hasEnteredScreen = true; // Mark that the object has been inside the screen
    }

    // Called when the object is no longer visible to any camera
    private void OnBecameInvisible()
    {
        // If the object has already been visible once, destroy it when it goes out of view
        if (hasEnteredScreen)
        {
            Destroy(gameObject);
        }
    }
}
