using UnityEngine;

/// <summary>
/// The ControlBackground class manages background layers in a Unity scene.
/// It assigns sprites to specified GameObjects representing different layers,
/// allowing for dynamic background changes at the start of the game.
/// </summary>
public class ControlBackground : MonoBehaviour
{
    public Sprite[] LayerSprites; // Array of sprites to assign to the background layers
    private GameObject[] _layerObject = new GameObject[5]; // Array to hold references to layer GameObjects

    /// <summary>
    /// Initializes the background layers by finding the corresponding GameObjects
    /// and changing their sprites at the start of the game.
    /// </summary>
    void Start()
    {
        for (int i = 0; i < _layerObject.Length; i++)
        {
            _layerObject[i] = GameObject.Find("Layer_" + i);
        }

        ChangeSprite();
    }

    /// <summary>
    /// Changes the sprites of the background layers to the ones specified in LayerSprites.
    /// Each layer's sprite is updated for both the main sprite and its child sprites.
    /// </summary>
    void ChangeSprite()
    {
        _layerObject[0].GetComponent<SpriteRenderer>().sprite = LayerSprites[0];
        for (int i = 1; i < _layerObject.Length; i++)
        {
            Sprite changeSprite = LayerSprites[i];
            _layerObject[i].GetComponent<SpriteRenderer>().sprite = changeSprite;
            _layerObject[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = changeSprite;
            _layerObject[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = changeSprite;
        }
    }
}
