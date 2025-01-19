using UnityEngine;
using UnityEngine.InputSystem;

public class AimStateManager : MonoBehaviour
{

    public Transform playerTransform;
    public float sensitivity = 20f;
    public float minPitch = -30f;
    public float maxPitch = 60f;
    public float collisionOffset = 0.2f;
    private InputAction lookAction;
    private float currentPitch = 0f;


    // Update is called once per frame
    void Update()
    {
        HandleCameraRotation();
    }

    void Awake()
    {
        var playerInput = new InputSystemActions();
        lookAction = playerInput.Player.Look;
    }

    void OnEnable()
    {
        lookAction.Enable();
    }

    void OnDisable()
    {
        lookAction.Disable();
    }

    void HandleCameraRotation()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        float horizontal = lookInput.x * sensitivity * Time.deltaTime;
        float vertical = lookInput.y * sensitivity * Time.deltaTime;

        currentPitch -= vertical;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // Rotate player horizontally, clamp camera pitch independently
        playerTransform.localRotation = Quaternion.Euler(0, playerTransform.rotation.eulerAngles.y + horizontal, 0);

        // Sync camera position and rotation separately from player tilt
        transform.rotation = Quaternion.Euler(currentPitch, playerTransform.rotation.eulerAngles.y, 0);
    }

}
