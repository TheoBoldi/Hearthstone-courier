using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    private Rigidbody2D rb;
    private Vector2 currentMovement;
    private bool isSprinting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // These methods are now called by the InputHandler events
    public void HandleMovement(Vector2 direction)
    {
        currentMovement = direction;
    }

    public void HandleSprint(bool sprinting)
    {
        isSprinting = sprinting;
         Debug.Log($"Sprint state: {isSprinting}");
    }

    public void HandleInteract()
    {
        Debug.Log("Interacting with something!");
        // We'll implement actual interaction logic later
    }

    private void FixedUpdate()
    {
        // Apply movement in FixedUpdate for physics
        if (currentMovement.magnitude > 0.1f)
        {
            float currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
            Vector2 movement = currentMovement * currentSpeed;
            rb.linearVelocity = movement;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}