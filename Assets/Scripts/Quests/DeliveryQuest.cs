using UnityEngine;

[System.Serializable]
public class DeliveryQuest
{
    public string questId;
    public string questName;
    public string description;
    public string fromNpcId;    // Who gives the quest
    public string toNpcId;      // Who receives the delivery
    public float rewardGold;
    public bool isCompleted;
    
    public DeliveryQuest(string id, string name, string desc, string from, string to, float reward)
    {
        questId = id;
        questName = name;
        description = desc;
        fromNpcId = from;
        toNpcId = to;
        rewardGold = reward;
        isCompleted = false;
    }
}