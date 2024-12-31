using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementStateManager : MonoBehaviour
{

    public float moveSpeed = 3;
    [HideInInspector] public Vector3 direction;
    CharacterController controller;
    InputSystemActions playerInput;
    InputAction moveAction;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new InputSystemActions();
        moveAction = playerInput.Player.Move;
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
        direction = transform.forward * input.y + transform.right * input.x;

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }

    bool isGrounded()
    {
        spherePos = new Vector3(transform.position.x - groundYOffset, transform.position.z);
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
