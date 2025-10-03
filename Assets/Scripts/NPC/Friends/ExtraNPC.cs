using UnityEngine;
using System.Collections.Generic;

public class ExtraNPC : InteractableCharacter
{
    [Header("Extra NPC Settings")]
    public bool hasRandomDialogue = true;
    
    [Header("Random Dialogue Options")]
    [Tooltip("Add the random messages this NPC can say. If empty, will use basic interaction message.")]
    [SerializeField] private List<string> randomMessages = new List<string>
    {
        "Lovely weather today!",
        "The crops are growing well this season.",
        "Have you seen my cat?",
        "Busy day for deliveries!",
        "The harvest festival is coming up soon!",
        "I heard there are wolves in the forest lately.",
        "That's a fine delivery bag you have there."
    };

    protected override void HandleInteract()
    {
        // Play interaction sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayInteractionSound();
        
        if (hasRandomDialogue && randomMessages.Count > 0)
        {
            string randomMessage = randomMessages[Random.Range(0, randomMessages.Count)];
            Debug.Log($"{characterName}: {randomMessage}");
        }
        else
        {
            // If no random dialogue or no messages, fall back to basic interaction message
            base.HandleInteract();
        }
    }

    protected override void OnPlayerEnterRange()
    {
        if (hasRandomDialogue && randomMessages.Count > 0)
        {
            Debug.Log($"Talk to {characterName} for a chat");
        }
        else
        {
            base.OnPlayerEnterRange(); // Use base prompt
        }
    }

    // Optional: Public methods to manage messages at runtime if needed
    public void AddMessage(string newMessage)
    {
        if (!randomMessages.Contains(newMessage))
        {
            randomMessages.Add(newMessage);
        }
    }

    public void RemoveMessage(string messageToRemove)
    {
        randomMessages.Remove(messageToRemove);
    }

    public List<string> GetMessages()
    {
        return new List<string>(randomMessages);
    }
}