using UnityEngine;

[System.Serializable]
public class Package
{
    public string packageId;
    public string questId; // Which quest this package belongs to
    public string itemName;
    public string description;
    public float weight;
    public bool isFragile;
    public int value;
    
    // Constructor for easy creation
    public Package(string id, string qId, string name, string desc, float pkgWeight = 1.0f, bool fragile = false, int pkgValue = 0)
    {
        packageId = id;
        questId = qId;
        itemName = name;
        description = desc;
        weight = pkgWeight;
        isFragile = fragile;
        value = pkgValue;
    }
}