using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private int maxCapacity = 3;
    
    [Header("Debug - Current Packages")]
    [SerializeField] private List<Package> carriedPackages = new List<Package>(); // Now serialized
    
    // Public properties for other systems to check
    public bool CanCarryMore => carriedPackages.Count < maxCapacity;
    public int CurrentPackages => carriedPackages.Count;
    public int MaxCapacity => maxCapacity;
    
    // Event for UI updates
    public System.Action<Package> onPackageAdded;
    public System.Action<Package> onPackageRemoved;
    
    public bool AddPackage(Package package)
    {
        if (CanCarryMore && package != null)
        {
            carriedPackages.Add(package);
            Debug.Log($"Picked up: {package.itemName}");
            onPackageAdded?.Invoke(package);
            return true;
        }
        else
        {
            Debug.Log($"Inventory full! Cannot carry {package.itemName}");
            return false;
        }
    }
    
    public Package GetPackageForQuest(string questId)
    {
        return carriedPackages.Find(p => p.questId == questId);
    }
    
    public bool RemovePackage(Package package)
    {
        if (package != null && carriedPackages.Contains(package))
        {
            carriedPackages.Remove(package);
            Debug.Log($"Removed from inventory: {package.itemName}");
            onPackageRemoved?.Invoke(package);
            return true;
        }
        return false;
    }
    
    public bool RemovePackageByQuestId(string questId)
    {
        Package package = GetPackageForQuest(questId);
        if (package != null)
        {
            return RemovePackage(package);
        }
        return false;
    }
    
    // Get all carried packages (for UI)
    public List<Package> GetAllPackages()
    {
        return new List<Package>(carriedPackages);
    }
    
    // Clear all packages (if needed)
    public void ClearInventory()
    {
        carriedPackages.Clear();
        Debug.Log("Inventory cleared");
    }
}