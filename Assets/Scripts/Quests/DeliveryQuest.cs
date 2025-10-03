using UnityEngine;

[System.Serializable]
public class DeliveryQuest
{
    public string questId;
    public string questName;
    public string description;
    public string fromNpcId;
    public string toNpcId;
    public float rewardGold;
    public bool isCompleted;
    
    // NEW: Package information
    public Package questPackage;
    
    public DeliveryQuest(string id, string name, string desc, string from, string to, float reward, Package package = null)
    {
        questId = id;
        questName = name;
        description = desc;
        fromNpcId = from;
        toNpcId = to;
        rewardGold = reward;
        isCompleted = false;
        questPackage = package;
    }
}