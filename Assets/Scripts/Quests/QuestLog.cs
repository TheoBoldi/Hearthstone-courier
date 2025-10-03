using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    private Dictionary<string, DeliveryQuest> activeQuests = new Dictionary<string, DeliveryQuest>();
    
    public void AddQuest(DeliveryQuest quest)
    {
        if (!activeQuests.ContainsKey(quest.questId))
        {
            activeQuests.Add(quest.questId, quest);
            Debug.Log($"Quest accepted: {quest.questName}");
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
            Debug.Log($"Quest completed: {quest.questName}");
            Debug.Log($"Reward: {quest.rewardGold} gold");
            // TODO: Add gold to player inventory later
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
    
    // NEW: Check if player already has a specific quest
    public bool HasQuest(string questId)
    {
        return activeQuests.ContainsKey(questId);
    }
    
    // NEW: Check if player has any quest from this NPC
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