using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameManager class controls the overall game flow, including game state,
/// difficulty management, and interactions between various game components.
/// It also handles audio playback and player input for starting and exiting the game.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Header("ParallaxBackground options:")]
    [SerializeField] private bool _cameraMove; // Indicates if the camera should move
    [SerializeField] private float _cameraMoveSpeed = 20f; // Speed of the camera movement

    [Space(2f)]
    [Header("PlatformSpawner options:")]
    [SerializeField] private float _minTimeToSpawn = 1f; // Minimum time interval for spawning platforms
    [SerializeField] private float _maxTimeToSpawn = 2f; // Maximum time interval for spawning platforms
    [SerializeField] private float _moveSpeed = 20f; // Speed of platform movement

    [Space(2f)]
    [Header("PlayerController options:")]
    [SerializeField] private float _jumpForce = 30f; // Jump force for the player

    private bool _isGameOver = false; // Flag to check if the game is over
    private float _playTime; // Total playtime
    private int _platformHits; // Count of platform hits

    [Space(10f)]
    [SerializeField] private ParallaxBackground _parallaxBackground;
    [SerializeField] private PlatformSpawner _platformSpawner;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Animator _startGameAnim;
    [SerializeField] private Animator _gameOverAnim;
    [SerializeField] private TMP_Text _gameOverReason;

    /// <summary>
    /// Sets the game state to inactive when the script is enabled.
    /// </summary>
    private void OnEnable() => SetGameActive(false);


    private void Awake() => SetFrameRate();


    /// <summary>
    /// Starts playing the background melody at the beginning of the game.
    /// </summary>
    private void Start()
        => AudioManager.PlayFromResources("Melody", 0.2f, UnityEngine.Random.Range(0.8f, 1.2f), true);

    /// <summary>
    /// Updates the game state every frame, handling player inputs and time tracking.
    /// </summary>
    private void Update()
    {
        if (isGameOver())
            return;

        _playTime += Time.deltaTime; // Increment play time

        // Handle exit game input
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            ExitGame();
        }

        // Handle start game input
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButton(0))
        {
            StartGame();
        }
    }

    private void SetFrameRate()
    {
        // Lock the FPS based on the platform
#if UNITY_STANDALONE
            // For PC, typically 60 FPS
            Application.targetFrameRate = 60;
#elif UNITY_IOS || UNITY_ANDROID
        // For mobile devices, it can be 30 or 60 FPS
        Application.targetFrameRate = 60; // or 30 if needed
#elif UNITY_CONSOLE
            // For consoles, typically 30 or 60 FPS
            Application.targetFrameRate = 60; // or 30 if needed
#else
            // Default value for other platforms
            Application.targetFrameRate = 60;
#endif

        // Log the set target frame rate to the console for debugging
        Debug.Log("Target Frame Rate set to: " + Application.targetFrameRate);
    }

    /// <summary>
    /// Gets the formatted playtime as a string in mm:ss format.
    /// </summary>
    /// <returns>Formatted playtime string.</returns>
    public string GetPlayTime()
    {
        int minutes = Mathf.FloorToInt(_playTime / 60f);
        int seconds = Mathf.FloorToInt(_playTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Increments the count of platform hits by one.
    /// </summary>
    public void AddPlatfromHit() => _platformHits++;

    /// <summary>
    /// Gets the current count of platform hits.
    /// </summary>
    /// <returns>Number of platform hits.</returns>
    public int GetPlatfromHits() { return _platformHits; }

    /// <summary>
    /// Changes the game difficulty by adjusting speed and spawn times.
    /// </summary>
    public void ChangeDifficulty()
    {
        // Increase speeds by 1%
        _cameraMoveSpeed *= 1.01f;
        _moveSpeed *= 1.01f;

        // Decrease spawn times by 1%
        _minTimeToSpawn *= 0.99f;
        _maxTimeToSpawn *= 0.99f;

        SetupGame(); // Setup the game with new difficulty settings
    }

    /// <summary>
    /// Sets up the game environment by applying current settings to game components.
    /// </summary>
    public void SetupGame()
    {
        if (isGameOver())
            return;

        _parallaxBackground.CameraMoveSpeed = _cameraMoveSpeed;
        _parallaxBackground.CameraMove = _cameraMove;
        _platformSpawner.moveSpeed = _moveSpeed;
        _platformSpawner.minTimeToSpawn = _minTimeToSpawn;
        _platformSpawner.maxTimeToSpawn = _maxTimeToSpawn;
        _playerController.JumpForce = _jumpForce;
    }

    /// <summary>
    /// Activates or deactivates game components based on the game state.
    /// </summary>
    /// <param name="value">True to activate, false to deactivate.</param>
    public void SetGameActive(bool value)
    {
        _parallaxBackground?.gameObject?.SetActive(value);
        _platformSpawner?.gameObject?.SetActive(value);
        _playerController?.gameObject?.SetActive(value);
        canvas?.gameObject?.SetActive(!value);
    }

    /// <summary>
    /// Checks if the game is currently over.
    /// </summary>
    /// <returns>True if the game is over; otherwise, false.</returns>
    public bool isGameOver() { return _isGameOver; }

    /// <summary>
    /// Triggers the start game animation.
    /// </summary>
    public void StartGame() => _startGameAnim.Play("StartGame");

    /// <summary>
    /// Handles game over logic, including displaying the reason and stopping the background music.
    /// </summary>
    /// <param name="reason">Reason for the game over.</param>
    public void GameOver(string reason)
    {
        if (_isGameOver)
            return;

        _gameOverReason.text = reason;

        AudioManager.StopAudioClip("Melody");
        _isGameOver = true;
        SetGameActive(false);
        _gameOverAnim?.Play("GameOver");
        AudioManager.PlayFromResources("Fail", 0.2f, 0.6f);
    }

    /// <summary>
    /// Exits the game application.
    /// </summary>
    public void ExitGame()
        => Application.Quit();
}
