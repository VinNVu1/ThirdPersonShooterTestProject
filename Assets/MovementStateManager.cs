using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementStateManager : MonoBehaviour
{

    public float moveSpeed = 3;
    [HideInInspector] public Vector3 direction;
    public CharacterController controller;
    InputSystemActions playerInput;
    InputAction moveAction;
    InputAction lookAction;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    public Transform cameraTransform;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new InputSystemActions();
        moveAction = playerInput.Player.Move;
        lookAction = playerInput.Player.Look;
    }

    void OnEnable()
    {
        moveAction.Enable();  
        lookAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();  
        lookAction.Disable();   
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetDirectionAndMove();
        HandleCameraRotation();
        Gravity();
    }

    void GetDirectionAndMove()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        direction = transform.forward * input.y + transform.right * input.x;

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }

    bool isGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false; 
    }

    void Gravity()
    {
        if (!isGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
    }

    void HandleCameraRotation()
    {
        Debug.Log("OnLook triggered");
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        float horizontal = lookInput.x;
        float vertical = lookInput.y;

        cameraTransform.RotateAround(transform.position, Vector3.up, horizontal);
        cameraTransform.RotateAround(transform.position, transform.right, -vertical);
    }

}
