using UnityEngine;

public class QuestReceiver : InteractableCharacter
{
    [Header("Quest Receiver Settings")]
    public string[] acceptableQuestIds;
    
    [Header("Delivery Messages")]
    public string deliveryPrompt = "Deliver the package to me?";
    public string thanksMessage = "Thank you for the delivery!";
    
    private DeliveryQuest pendingDelivery; // Delivery waiting for confirmation
    private bool hasPendingDelivery = false;
    
    protected override void HandleInteract()
    {
        if (hasPendingDelivery)
        {
            // Play DELIVERY COMPLETE sound when confirming delivery
            AudioManager.Instance.PlayDeliveryCompleteSound();
            // Player is confirming the delivery
            CompletePendingDelivery();
        }
        else
        {
            // Play INTERACTION sound for initial interaction
            AudioManager.Instance.PlayInteractionSound();
            // Check if player has a quest to deliver here
            OfferDelivery();
        }
    }

    private void OfferDelivery()
    {
        QuestLog playerQuestLog = FindFirstObjectByType<QuestLog>();
        PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
        
        if (playerQuestLog != null && playerInventory != null)
        {
            DeliveryQuest questToDeliver = playerQuestLog.GetQuestForReceiver(characterName);
            if (questToDeliver != null && questToDeliver.questPackage != null)
            {
                pendingDelivery = questToDeliver;
                hasPendingDelivery = true;
                
                Debug.Log($"=== DELIVERY CONFIRMATION ===");
                Debug.Log($"{characterName}: {deliveryPrompt}");
                Debug.Log($"Package: {questToDeliver.questPackage.itemName}");
                Debug.Log($"Reward: {questToDeliver.rewardGold} gold");
                Debug.Log($"Press E again to deliver, or walk away to cancel.");
            }
            else
            {
                Debug.Log($"{characterName}: I'm not expecting any deliveries right now.");
            }
        }
    }

    private void CompletePendingDelivery()
    {
        QuestLog playerQuestLog = FindFirstObjectByType<QuestLog>();
        PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
        
        if (playerQuestLog != null && playerInventory != null && pendingDelivery != null)
        {
            // Remove the package from inventory
            if (playerInventory.RemovePackageByQuestId(pendingDelivery.questId))
            {
                Debug.Log($"=== DELIVERY COMPLETE ===");
                Debug.Log($"{characterName}: {thanksMessage}");
                Debug.Log($"Removed from inventory: {pendingDelivery.questPackage.itemName}");
                Debug.Log($"You received {pendingDelivery.rewardGold} gold!");
                playerQuestLog.CompleteQuest(pendingDelivery.questId);
            }
            else
            {
                Debug.Log($"Error: Could not remove package from inventory.");
            }
            
            // Reset pending state
            pendingDelivery = null;
            hasPendingDelivery = false;
        }
    }

    protected override void OnPlayerEnterRange()
    {
        // Check if player has a quest for this receiver
        QuestLog playerQuestLog = FindFirstObjectByType<QuestLog>();
        if (playerQuestLog != null && playerQuestLog.HasQuestForReceiver(characterName))
        {
            if (hasPendingDelivery)
            {
                Debug.Log($"Ready to deliver {pendingDelivery.questPackage.itemName}? Press E to confirm.");
            }
            else
            {
                Debug.Log($"Deliver package to {characterName}");
            }
        }
        else
        {
            base.OnPlayerEnterRange(); // Use default prompt
        }
    }

    protected override void OnPlayerExitRange()
    {
        base.OnPlayerExitRange();
        
        // Cancel pending delivery if player walks away
        if (hasPendingDelivery)
        {
            Debug.Log($"Delivery to {characterName} cancelled.");
            pendingDelivery = null;
            hasPendingDelivery = false;
        }
    }
}