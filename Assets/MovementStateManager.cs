using UnityEngine;
using UnityEngine.InputSystem;

public class MovementStateManager : MonoBehaviour
{

    public float moveSpeed = 3;
    [HideInInspector] public Vector3 direction;
    CharacterController controller;
    InputSystemActions playerInput;
    InputAction moveAction;

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
    }

    void GetDirectionAndMove()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        direction = transform.forward * input.y + transform.right * input.x;

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
}
