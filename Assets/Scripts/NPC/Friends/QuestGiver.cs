using UnityEngine;

public class QuestGiver : InteractableCharacter
{
    [Header("Quest Giver Specific")]
    public DeliveryQuest[] availableQuests;
    
    [Header("Dialogue Messages")]
    public string hasQuestMessage = "I have an urgent delivery for you!";
    public string noQuestMessage = "I don't have any work for you right now. Check back later!";
    public string questCompletedMessage = "Thanks for completing that delivery! Come back later for more work.";
    
    private DeliveryQuest pendingQuest;
    private bool hasPendingOffer = false;
    
    protected override void HandleInteract()
    {
        if (hasPendingOffer)
        {
            // Play QUEST ACCEPT sound when accepting the quest
            AudioManager.Instance.PlayQuestAcceptSound();
            AcceptPendingQuest();
        }
        else
        {
            // Play INTERACTION sound for initial interaction
            AudioManager.Instance.PlayInteractionSound();
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
                Debug.Log($"{characterName}: You already have a quest from me. Complete it first!");
                return;
            }
            
            // Find the first available quest that player doesn't already have AND hasn't completed
            DeliveryQuest questToOffer = null;
            foreach (var quest in availableQuests)
            {
                if (!playerQuestLog.HasQuest(quest.questId) && !playerQuestLog.HasCompletedQuest(quest.questId))
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
                // Use different message if all quests are completed
                bool hasCompletedQuests = false;
                foreach (var quest in availableQuests)
                {
                    if (playerQuestLog.HasCompletedQuest(quest.questId))
                    {
                        hasCompletedQuests = true;
                        break;
                    }
                }
                
                if (hasCompletedQuests)
                {
                    Debug.Log($"{characterName}: {questCompletedMessage}");
                }
                else
                {
                    Debug.Log($"{characterName}: {noQuestMessage}");
                }
            }
        }
        else
        {
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
            if (!playerQuestLog.HasQuest(pendingQuest.questId) && !playerQuestLog.HasCompletedQuest(pendingQuest.questId))
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
                    // Quest without physical package
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
        bool hasCompletedMyQuests = false;
        
        // Check if player has completed all available quests
        if (playerQuestLog != null)
        {
            foreach (var quest in availableQuests)
            {
                if (playerQuestLog.HasCompletedQuest(quest.questId))
                {
                    hasCompletedMyQuests = true;
                    break;
                }
            }
        }
        
        if (hasPendingOffer)
        {
            Debug.Log($"Still waiting for your decision on {pendingQuest.questName}...");
        }
        else if (hasQuestFromMe)
        {
            Debug.Log($"You already have a quest from {characterName}. Complete it first!");
        }
        else if (hasCompletedMyQuests)
        {
            Debug.Log($"{characterName}: Come back later for more deliveries!");
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