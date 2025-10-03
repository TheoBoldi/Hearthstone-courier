using UnityEngine;

public class QuestReceiver : InteractableCharacter
{
    [Header("Quest Receiver Settings")]
    public string[] acceptableQuestIds; // Which quests can be delivered here
    
    protected override void HandleInteract()
    {
        // Check if player has a quest to deliver here
        QuestLog playerQuestLog = FindFirstObjectByType<QuestLog>();
        if (playerQuestLog != null)
        {
            DeliveryQuest questToDeliver = playerQuestLog.GetQuestForReceiver(characterName);
            if (questToDeliver != null)
            {
                Debug.Log($"=== DELIVERY COMPLETE ===");
                Debug.Log($"{characterName}: Thank you for the delivery!");
                Debug.Log($"You received {questToDeliver.rewardGold} gold!");
                playerQuestLog.CompleteQuest(questToDeliver.questId);
            }
            else
            {
                Debug.Log($"{characterName}: I'm not expecting any deliveries right now.");
            }
        }
    }

    protected override void OnPlayerEnterRange()
    {
        // Check if player has a quest for this receiver
        QuestLog playerQuestLog = FindFirstObjectByType<QuestLog>();
        if (playerQuestLog != null && playerQuestLog.HasQuestForReceiver(characterName))
        {
            Debug.Log($"Deliver package to {characterName}");
        }
        else
        {
            base.OnPlayerEnterRange(); // Use default prompt
        }
    }
}