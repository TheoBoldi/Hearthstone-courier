using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private PlayerController player;
    
    private PlayerControls playerControls;
    private Vector2 moveInput;

    public Vector2 MoveInput => moveInput;
    public bool IsMoving => moveInput.magnitude > 0.1f;

    private void Awake()
    {
        // Initialize the input system
        playerControls = new PlayerControls();
        
        if (player == null)
            player = FindFirstObjectByType<PlayerController>();
    }

    private void OnEnable()
    {
        // Enable our input actions and subscribe to events
        playerControls.Player.Enable();
        
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
        
        playerControls.Player.Interact.performed += OnInteract;
        playerControls.Player.Sprint.performed += OnSprint;
        playerControls.Player.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        // Clean up when disabled
        playerControls.Player.Disable();
        
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;
        
        playerControls.Player.Interact.performed -= OnInteract;
        playerControls.Player.Sprint.performed -= OnSprint;
        playerControls.Player.Sprint.canceled -= OnSprint;
    }

    // Input event handlers
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
        if (player != null)
            player.HandleMovement(moveInput);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (player != null)
            player.HandleInteract();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        bool isSprinting = context.phase == InputActionPhase.Performed;
        if (player != null)
            player.HandleSprint(isSprinting);
    }
}