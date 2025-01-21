using Unity.Cinemachine;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementStateManager : MonoBehaviour
{

    public float moveSpeed = 3;
    private InputAction moveAction;
    [HideInInspector] public Vector3 direction;
    CharacterController controller;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    private Camera mainCamera;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        var playerInput = new InputSystemActions();
        moveAction = playerInput.Player.Move;

        
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        moveAction.Enable();  
    }

    void OnDisable()
    {
        moveAction.Disable();  
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
        Gravity();
    }

    void GetDirectionAndMove()
{
    Vector2 input = moveAction.ReadValue<Vector2>();
    direction = new Vector3(input.x, 0, input.y).normalized; 

    if (direction != Vector3.zero) 
    {
        
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        
        cameraForward.y = 0;
        cameraForward.Normalize();

        
        Vector3 moveDirection = cameraForward * direction.z + cameraRight * direction.x;

        
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

       
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); 
    }
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


}


// No Comment Demon - Adrian