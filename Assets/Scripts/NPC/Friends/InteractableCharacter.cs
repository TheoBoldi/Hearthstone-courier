using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableCharacter : MonoBehaviour
{
    [Header("Character Settings")]
    public string characterName = "Character";
    
    [Header("Basic Interaction (Optional)")]
    [Tooltip("Simple message shown if no custom interaction is implemented. Leave empty for no message.")]
    public string interactionMessage = "Hello!";
    
    [Header("Interaction Events")]
    public UnityEvent onInteract;

    protected bool canInteract = false;

    // Common interaction detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            OnPlayerEnterRange();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            OnPlayerExitRange();
        }
    }

    // Common interaction method - called by PlayerController
    public void Interact()
    {
        if (canInteract)
        {
            // Only show basic message if child class doesn't override behavior
            // We'll check this by seeing if HandleInteract does anything meaningful
            onInteract?.Invoke();
            HandleInteract();
        }
    }

    // Methods that child classes can override for specific behavior
    protected virtual void OnPlayerEnterRange()
    {
        Debug.Log($"Press E to talk to {characterName}");
    }

    protected virtual void OnPlayerExitRange()
    {
        Debug.Log($"Left {characterName}'s range");
    }

    // This is the method child classes override for their specific interaction logic
    protected virtual void HandleInteract()
    {
        // Base implementation - show simple message if provided
        if (!string.IsNullOrEmpty(interactionMessage))
        {
            Debug.Log($"{characterName}: {interactionMessage}");
        }
    }
}