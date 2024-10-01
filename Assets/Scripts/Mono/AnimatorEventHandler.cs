using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class handles events triggered from an animation.
/// It allows the Unity Animator to call functions at specific points in the animation timeline.
/// The OnAnimationEvent method is linked to UnityEvents, enabling custom behavior
/// to be invoked when the animation event is triggered.
/// </summary>
public class AnimatorEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAnimation;

    // This method is called from the animator event and triggers the UnityEvent.
    public void OnAnimationEvent() => OnAnimation.Invoke();

    // Called when it is necessary to destroy the animation game object.
    public void DestroyThis() => Destroy(gameObject);
}
