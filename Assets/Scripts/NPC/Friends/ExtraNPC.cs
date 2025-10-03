using UnityEngine;

public class ExtraNPC : InteractableCharacter
{
    [Header("Extra NPC Settings")]
    public bool hasRandomDialogue = true;
    private string[] randomMessages = {
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
        if (hasRandomDialogue)
        {
            string randomMessage = randomMessages[Random.Range(0, randomMessages.Length)];
            Debug.Log($"{characterName}: {randomMessage}");
        }
        // If no random dialogue, the base HandleInteract is empty anyway
    }

    protected override void OnPlayerEnterRange()
    {
        if (hasRandomDialogue)
        {
            Debug.Log($"Talk to {characterName} for a chat");
        }
        else
        {
            base.OnPlayerEnterRange(); // Use base prompt
        }
    }
}