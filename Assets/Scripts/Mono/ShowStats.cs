using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The ShowStats class is responsible for displaying game statistics such as playtime and platform hits.
/// It allows the player to skip to the main menu after the game is over.
/// </summary>
public class ShowStats : MonoBehaviour
{
    [SerializeField] private TMP_Text _TxtPlayTime;
    [SerializeField] private TMP_Text _TxtPlatformHits;
    private bool _allowToSkip = false;

    /// <summary>
    /// Displays the playtime and platform hit statistics on the UI.
    /// </summary>
    private void Show()
    {
        _TxtPlayTime.text += GameManager.Instance.GetPlayTime();
        _TxtPlatformHits.text += GameManager.Instance.GetPlatfromHits();
    }

    /// <summary>
    /// Sets the flag to allow skipping to the main menu.
    /// It is used in Animation (after game over animation)
    /// </summary>
    private void AllowToSkip() => _allowToSkip = true;

    void Update()
    {
        // Check for user input to skip to the main menu
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButton(0))
        {
            // Only allow skipping if the game is over and skipping is permitted
            if (!GameManager.Instance.isGameOver() || !_allowToSkip)
                return;

            SceneManager.LoadScene(0);
        }
    }
}