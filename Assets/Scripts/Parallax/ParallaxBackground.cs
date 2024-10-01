using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    // Flag that determines if the camera should move
    [HideInInspector] public bool CameraMove;

    // Target speed for camera movement
    [HideInInspector] public float CameraMoveSpeed = 1.5f;

    // Time it takes for the camera to reach full speed (0.3 seconds)
    public float AccelerationTime = 0.3f;

    // The current speed at which the camera is moving, starting from 0
    private float currentSpeed = 0f;

    // Reference velocity for Mathf.SmoothDamp, required to track smoothness
    private float speedSmoothVelocity = 0f;

    [Header("Layer Setting")]
    // Array that holds the speed factor for each layer (used for parallax effect)
    public float[] LayerSpeed = new float[7];

    // Array that holds references to the layer objects (background elements)
    public GameObject[] LayerObjects = new GameObject[7];

    // Reference to the camera's transform
    private Transform _camera;

    // Array to store the initial x position of each layer
    private float[] _startPos = new float[7];

    // The width of the background sprite (x-axis bounds)
    private float _boundSizeX;

    // The scale factor of the background sprite on the x-axis
    private float _sizeX;

    void Start()
    {
        _camera = Camera.main.transform;

        // Get the x-scale of the first layer object to understand background size
        _sizeX = LayerObjects[0].transform.localScale.x;

        // Get the width of the sprite based on its bounds
        _boundSizeX = LayerObjects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // Initialize starting positions of each layer with respect to the camera's initial position
        for (int i = 0; i < 5; i++)
        {
            _startPos[i] = _camera.position.x;
        }
    }

    void Update()
    {
        if (CameraMove)
        {
            // Smoothly transition the current speed to the target speed over time (0.3 seconds)
            currentSpeed = Mathf.SmoothDamp(currentSpeed, CameraMoveSpeed, ref speedSmoothVelocity, AccelerationTime);

            // Move the camera horizontally by the current speed (taking deltaTime into account for smooth movement)
            _camera.position += Vector3.right * Time.deltaTime * currentSpeed;
        }

        for (int i = 0; i < 5; i++)
        {
            // Calculate the temporary position offset for parallax (1 - layer speed for background effect)
            float temp = (_camera.position.x * (1 - LayerSpeed[i]));

            // Calculate the distance the layer should move relative to the camera's movement
            float distance = _camera.position.x * LayerSpeed[i];

            // Set the new position of the layer, keeping its y position the same as the camera's
            LayerObjects[i].transform.position = new Vector2(_startPos[i] + distance, _camera.position.y);

            // If the background has moved past its boundary (right), loop the background to the start position
            if (temp > _startPos[i] + _boundSizeX * _sizeX)
            {
                _startPos[i] += _boundSizeX * _sizeX;
            }
            // If the background has moved past its boundary (left), loop the background to the start position
            else if (temp < _startPos[i] - _boundSizeX * _sizeX)
            {
                _startPos[i] -= _boundSizeX * _sizeX;
            }
        }
    }
}
