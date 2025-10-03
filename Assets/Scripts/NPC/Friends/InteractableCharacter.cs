using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableCharacter : MonoBehaviour
{
    [Header("Character Settings")]
    public string characterName = "Character";
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

    // Common interaction method
    public virtual void Interact()
    {
        if (canInteract)
        {
            Debug.Log($"{characterName}: {interactionMessage}");
            onInteract?.Invoke();
            HandleInteract();
        }
    }

    // Methods that child classes can override for specific behavior
    protected virtual void OnPlayerEnterRange()
    {
        //Debug.Log($"Press E to talk to {characterName}");
    }

    protected virtual void OnPlayerExitRange()
    {
        //Debug.Log($"Left {characterName}'s range");
    }
    
    protected virtual void HandleInteract()
    {
        // Empty in base class - child classes override this
    }
}