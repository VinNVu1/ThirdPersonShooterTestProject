using UnityEngine;
using UnityEngine.InputSystem;

public class AimStateManager : MonoBehaviour
{

    public Transform playerTransform;
    private InputAction lookAction;
    public float sensitivity = 20f;
    public float minPitch = -30f;
    public float maxPitch = 60f;
    float currentPitch = 0f;
    public float collisionOffset = 0.2f;
    public LayerMask groundLayer;

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

        Quaternion targetRotation = Quaternion.Euler(currentPitch, transform.eulerAngles.y + horizontal, 0);
        transform.position = playerTransform.position;
        transform.rotation = targetRotation;

        Vector3 directionToCamera = transform.position - playerTransform.position;
        RaycastHit hit;
        if(Physics.Raycast(playerTransform.position, directionToCamera.normalized, out hit, directionToCamera.magnitude, groundLayer))
        {
            transform.position = hit.point - directionToCamera.normalized * collisionOffset;
        }
        else
        {
            transform.position = playerTransform.position;
        }

        //transform.RotateAround(playerTransform.position, Vector3.up, horizontal);
        //transform.localEulerAngles = new Vector3(currentPitch, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

}
