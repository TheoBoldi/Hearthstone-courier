using UnityEngine;

public class QuestGiver : InteractableCharacter
{
    [Header("Quest Giver Specific")]
    public DeliveryQuest[] availableQuests;
    
    [Header("Dialogue Messages")]
    [Tooltip("Message shown when NPC has quests available")]
    public string hasQuestMessage = "I have an urgent delivery for you!";
    
    [Tooltip("Message shown when NPC has no quests available")]
    public string noQuestMessage = "I don't have any work for you right now. Check back later!";
    
    private DeliveryQuest pendingQuest; // Quest waiting for acceptance
    private bool hasPendingOffer = false;
    
    protected override void HandleInteract()
    {
        if (hasPendingOffer)
        {
            // Player is confirming the pending quest
            AcceptPendingQuest();
        }
        else
        {
            // Offer a new quest
            OfferNewQuest();
        }
    }

    private void OfferNewQuest()
    {
        QuestLog playerQuestLog = FindFirstObjectByType<QuestLog>();
        if (playerQuestLog != null && availableQuests.Length > 0)
        {
            // Check if player already has a quest from this NPC
            if (playerQuestLog.HasQuestFromNPC(characterName))
            {
                // Use the no quest message when player already has a quest
                Debug.Log($"{characterName}: {noQuestMessage}");
                return;
            }
            
            // Find the first available quest that player doesn't already have
            DeliveryQuest questToOffer = null;
            foreach (var quest in availableQuests)
            {
                if (!playerQuestLog.HasQuest(quest.questId))
                {
                    questToOffer = quest;
                    break;
                }
            }
            
            if (questToOffer != null)
            {
                pendingQuest = questToOffer;
                hasPendingOffer = true;
                
                Debug.Log($"=== QUEST OFFERED ===");
                Debug.Log($"{characterName}: {hasQuestMessage}");
                Debug.Log($"{pendingQuest.questName}");
                Debug.Log($"{pendingQuest.description}");
                Debug.Log($"Deliver to: {pendingQuest.toNpcId}");
                Debug.Log($"Reward: {pendingQuest.rewardGold} gold");
                Debug.Log($"Press E again to accept, or walk away to decline.");
            }
            else
            {
                // Use the no quest message when no quests are available
                Debug.Log($"{characterName}: {noQuestMessage}");
            }
        }
        else
        {
            // Use the no quest message when no quests are available
            Debug.Log($"{characterName}: {noQuestMessage}");
        }
    }

    private void AcceptPendingQuest()
    {
        QuestLog playerQuestLog = FindFirstObjectByType<QuestLog>();
        PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
        
        if (playerQuestLog != null && playerInventory != null && pendingQuest != null)
        {
            // Double-check that player doesn't already have this quest
            if (!playerQuestLog.HasQuest(pendingQuest.questId))
            {
                // Try to add the package to inventory
                if (pendingQuest.questPackage != null)
                {
                    if (playerInventory.AddPackage(pendingQuest.questPackage))
                    {
                        playerQuestLog.AddQuest(pendingQuest);
                        Debug.Log($"Now carrying: {pendingQuest.questPackage.itemName}");
                    }
                    else
                    {
                        Debug.Log($"Cannot accept quest - inventory is full!");
                        return;
                    }
                }
                else
                {
                    // Quest without physical package (for now)
                    playerQuestLog.AddQuest(pendingQuest);
                }
            }
            else
            {
                Debug.Log($"You already have this quest: {pendingQuest.questName}");
            }
            
            // Reset pending state
            pendingQuest = null;
            hasPendingOffer = false;
        }
    }

    protected override void OnPlayerEnterRange()
    {
        base.OnPlayerEnterRange();
        
        QuestLog playerQuestLog = FindFirstObjectByType<QuestLog>();
        bool hasQuestFromMe = playerQuestLog != null && playerQuestLog.HasQuestFromNPC(characterName);
        
        if (hasPendingOffer)
        {
            Debug.Log($"Still waiting for your decision on {pendingQuest.questName}...");
        }
        else if (hasQuestFromMe)
        {
            Debug.Log($"{characterName}: Come back after you've completed my delivery!");
        }
        else
        {
            Debug.Log($"Quest Available! Talk to {characterName} for work");
        }
    }

    protected override void OnPlayerExitRange()
    {
        base.OnPlayerExitRange();
        
        // Cancel pending offer if player walks away
        if (hasPendingOffer)
        {
            Debug.Log($"Quest offer for {pendingQuest.questName} cancelled.");
            pendingQuest = null;
            hasPendingOffer = false;
        }
    }
}