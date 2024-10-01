using System.Collections;
using UnityEngine;

/// <summary>
/// The AudioManager class manages audio playback in Unity.
/// It allows playing sounds from the Resources folder or AudioClip objects,
/// with options for volume, pitch, and looping. It also supports fade-out 
/// effects for smooth audio transitions.
/// </summary>
public static class AudioManager
{
    /// <summary>
    /// Plays an audio clip from the Resources folder with specified volume, pitch, and loop settings.
    /// </summary>
    /// <param name="soundFileName">Name of the sound file in the Resources folder.</param>
    /// <param name="volume">Volume level of the sound.</param>
    /// <param name="pitch">Pitch level of the sound.</param>
    /// <param name="loop">Whether the sound should loop.</param>
    /// <returns>Returns the AudioSource playing the sound.</returns>
    public static AudioSource PlayFromResources(string soundFileName, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        AudioClip soundClip = Resources.Load<AudioClip>($"Sound Effects/{soundFileName}");

        if (soundClip == null)
        {
            Debug.LogError($"Sound clip with name {soundFileName} not found in Resources folder.");
            return null;
        }

        GameObject soundGameObject = new GameObject(soundFileName);
        AudioSource soundSource = soundGameObject.AddComponent<AudioSource>();
        soundSource.clip = soundClip;
        soundSource.pitch = pitch;
        soundSource.volume = volume;
        soundSource.loop = loop;
        soundSource.Play();
        if (!loop) { Object.Destroy(soundGameObject, soundClip.length); }
        Debug.Log($"Sound {soundFileName} has been played.");
        return soundSource;
    }

    /// <summary>
    /// Stops an audio clip by fading out the volume over a specified duration.
    /// </summary>
    /// <param name="soundFileName">Name of the sound clip to stop.</param>
    /// <param name="fadeDuration">Duration of the fade-out effect.</param>
    public static void StopAudioClip(string soundFileName, float fadeDuration = 1.0f)
    {
        GameObject soundGameObject = GameObject.Find(soundFileName);
        if (soundGameObject == null || !soundGameObject.activeInHierarchy)
        {
            Debug.LogError($"Sound clip with name {soundFileName} not found or not active.");
            return;
        }

        AudioSource soundSource = soundGameObject.GetComponent<AudioSource>();
        if (soundSource != null)
        {
            soundGameObject.AddComponent<MonoBehaviourHelper>().StartCoroutine(FadeOutAndDestroy(soundSource, fadeDuration));
        }
    }

    /// <summary>
    /// Fades out the volume of the AudioSource and destroys the GameObject.
    /// </summary>
    /// <param name="soundSource">The AudioSource to fade out.</param>
    /// <param name="fadeDuration">Duration of the fade-out effect.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private static IEnumerator FadeOutAndDestroy(AudioSource soundSource, float fadeDuration)
    {
        float startVolume = soundSource.volume;

        while (soundSource.volume > 0)
        {
            soundSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        soundSource.Stop();
        Object.Destroy(soundSource.gameObject);
    }

    /// <summary>
    /// Plays a given AudioClip with specified volume and pitch settings.
    /// </summary>
    /// <param name="soundClipFile">The AudioClip to play.</param>
    /// <param name="volume">Volume level of the sound.</param>
    /// <param name="pitch">Pitch level of the sound.</param>
    public static void PlayAudioClip(AudioClip soundClipFile, float volume = 1.0f, float pitch = 1.0f)
    {
        if (soundClipFile == null)
        {
            Debug.LogError("Sound clip not found.");
            return;
        }

        GameObject soundGameObject = new GameObject(soundClipFile.name);
        AudioSource soundSource = soundGameObject.AddComponent<AudioSource>();
        soundSource.clip = soundClipFile;
        soundSource.pitch = pitch;
        soundSource.volume = volume;
        soundSource.Play();
        Debug.Log($"Sound {soundClipFile.name} has been played.");
        Object.Destroy(soundGameObject, soundClipFile.length);
    }
}

/// <summary>
/// Helper class to run coroutines in a static context.
/// </summary>
public class MonoBehaviourHelper : MonoBehaviour { }
