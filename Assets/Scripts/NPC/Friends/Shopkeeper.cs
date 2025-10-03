using UnityEngine;

public class Shopkeeper : InteractableCharacter
{
    [Header("Shopkeeper Specific")]
    public string[] itemsForSale = { "Health Potion", "Stamina Potion" };

    protected override void HandleInteract()
    {
        // Play interaction sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayInteractionSound();
        
        // Shop-specific logic here
        Debug.Log($"{characterName}: Welcome to my shop! I sell: {string.Join(", ", itemsForSale)}");
        // We'll hook this up to a shop UI later
    }

    protected override void OnPlayerEnterRange()
    {
        base.OnPlayerEnterRange(); // Show the base prompt
        Debug.Log($"Shop open! Talk to {characterName} to browse items");
    }
}