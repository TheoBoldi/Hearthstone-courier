using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    private Dictionary<string, DeliveryQuest> activeQuests = new Dictionary<string, DeliveryQuest>();
    private HashSet<string> completedQuestIds = new HashSet<string>(); // NEW: Track completed quests
    
    public void AddQuest(DeliveryQuest quest)
    {
        if (!activeQuests.ContainsKey(quest.questId) && !completedQuestIds.Contains(quest.questId))
        {
            activeQuests.Add(quest.questId, quest);
            Debug.Log($"=== QUEST ACCEPTED ==="); // NEW: Header
            Debug.Log($"{quest.questName}");
            Debug.Log($"Deliver to: {quest.toNpcId}");
        }
        else
        {
            Debug.Log($"You already have this quest: {quest.questName}");
        }
    }
    
    public void CompleteQuest(string questId)
    {
        if (activeQuests.ContainsKey(questId))
        {
            DeliveryQuest quest = activeQuests[questId];
            quest.isCompleted = true;
            activeQuests.Remove(questId);
            completedQuestIds.Add(questId); // NEW: Mark as completed
            Debug.Log($"Quest completed: {quest.questName}");
            Debug.Log($"Reward: {quest.rewardGold} gold");
        }
    }
    
    public DeliveryQuest GetQuestForReceiver(string receiverName)
    {
        foreach (var quest in activeQuests.Values)
        {
            if (quest.toNpcId == receiverName && !quest.isCompleted)
            {
                return quest;
            }
        }
        return null;
    }
    
    public bool HasQuestForReceiver(string receiverName)
    {
        return GetQuestForReceiver(receiverName) != null;
    }
    
    // Check if player already has a specific quest
    public bool HasQuest(string questId)
    {
        return activeQuests.ContainsKey(questId);
    }
    
    // Check if player has any quest from this NPC
    public bool HasQuestFromNPC(string npcName)
    {
        foreach (var quest in activeQuests.Values)
        {
            if (quest.fromNpcId == npcName && !quest.isCompleted)
            {
                return true;
            }
        }
        return false;
    }
    
    // NEW: Check if player has completed a specific quest
    public bool HasCompletedQuest(string questId)
    {
        return completedQuestIds.Contains(questId);
    }
    
    // NEW: Check if player has completed any quest from this NPC
    public bool HasCompletedQuestFromNPC(string npcName)
    {
        // We'd need to store more info to implement this properly
        // For now, we'll handle this in QuestGiver by checking individual quests
        return false;
    }
    
    // Helper method to see current quests (for debugging)
    public void PrintActiveQuests()
    {
        Debug.Log("=== Active Quests ===");
        foreach (var quest in activeQuests.Values)
        {
            Debug.Log($"{quest.questName} -> Deliver to {quest.toNpcId}");
        }
    }
}