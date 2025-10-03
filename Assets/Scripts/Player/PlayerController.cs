using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    private Rigidbody2D rb;
    private Vector2 currentMovement;
    private bool isSprinting = false;
    private InteractableCharacter currentInteractable;

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
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
        else
        {
            Debug.Log("No one nearby to interact with");
        }
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
    
     // Detect when we enter ANY InteractableCharacter's range
    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractableCharacter character = other.GetComponent<InteractableCharacter>();
        if (character != null)
        {
            currentInteractable = character;
            Debug.Log($"Now near {character.characterName} - ready to interact");
        }
    }

    // Detect when we leave the current InteractableCharacter's range
    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableCharacter character = other.GetComponent<InteractableCharacter>();
        if (character != null && character == currentInteractable)
        {
            Debug.Log($"Left {character.characterName}'s range");
            currentInteractable = null;
        }
    }

    // Optional: Visualize interaction range in Scene view
    private void OnDrawGizmosSelected()
    {
        if (currentInteractable != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, currentInteractable.transform.position);
        }
    }
}