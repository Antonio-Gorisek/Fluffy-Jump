using UnityEngine;

/// <summary>
/// The PlayerController class manages player movement, jumping, and interactions with platforms.
/// It handles player input, collision detection, and game state updates.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public float JumpForce = 10f; // Jump force for the player
    private Rigidbody2D _rigidbody2D; // Reference to the player's Rigidbody2D
    private Animator _animator; // Reference to the player's Animator
    private bool _isGrounded = false; // Checks if the player is on the ground
    private bool hasEnteredScreen = false; // Checks if the player has entered the screen
    private bool hasHitFirstPlatform = false; // Checks if the player has hit the first platform
    private int lastHit = 0; // Last platform hit by the player

    void Start()
    {
        // Cache the Rigidbody2D and Animator components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called when the object becomes visible to any camera.
    /// </summary>
    private void OnBecameVisible() => hasEnteredScreen = true;

    /// <summary>
    /// Called when the object is no longer visible to any camera.
    /// Displays game over message if the player has previously entered the screen.
    /// </summary>
    private void OnBecameInvisible()
    {
        if (hasEnteredScreen)
        {
            GameManager.Instance.GameOver("You fell off the map.");
        }
    }

    private void Update()
    {
        // Prevents movement until the player has hit the first platform
        if (!hasHitFirstPlatform)
            return;

        // Allow player to fall immediately when the space key or touch screen is pressed
        if (!_isGrounded && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -JumpForce * 2);
        }
    }

    /// <summary>
    /// Handles the jump logic and marks the first platform hit.
    /// </summary>
    private void Jump()
    {
        hasHitFirstPlatform = true;
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, JumpForce);
    }

    void FixedUpdate()
    {
        // Check if the player is grounded using an OverlapCircle
        _isGrounded = Physics2D.OverlapCircle(transform.position + new Vector3(0, -0.77f, 0), 0.15f, 1 << LayerMask.NameToLayer("Ground"));

        // Update animator parameters
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetFloat("vSpeed", _rigidbody2D.velocity.y);
    }

    /// <summary>
    /// Handles collisions with platforms and updates game state.
    /// </summary>
    /// <param name="collision2D">The collision information.</param>
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.layer != LayerMask.NameToLayer("Platform")) return;

        HasUserMissedPlatform(collision2D);

        Jump();
        GameManager.Instance.AddPlatfromHit();
        GameManager.Instance.ChangeDifficulty();

        // Spawn a cloud prefab if collision velocity is high
        if (collision2D.relativeVelocity.magnitude > 20)
            Instantiate(Resources.Load("Prefabs/Cloud"), transform.position - new Vector3(-1, 3, 0), transform.rotation);
    }

    /// <summary>
    /// Checks if the player has missed the previous platform.
    /// </summary>
    /// <param name="collision2D">The collision information.</param>
    private void HasUserMissedPlatform(Collision2D collision2D)
    {
        // Check if the platform hit is the next in sequence
        if (System.Int16.Parse(collision2D.gameObject.name) != lastHit + 1)
        {
            GameManager.Instance.GameOver("You missed the\nprevious platform.");
            return;
        }

        // Check if all hits for the current platform are completed
        if (collision2D.gameObject.GetComponent<PlatformHit>().GetLeftHitTimes() == 0)
        {
            lastHit = System.Int16.Parse(collision2D.gameObject.name); // Update the last hit platform
        }
    }
}
