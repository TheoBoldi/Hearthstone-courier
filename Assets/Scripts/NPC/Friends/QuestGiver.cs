using UnityEngine;

public class QuestGiver : InteractableCharacter
{
    [Header("Quest Giver Specific")]
    public string questTitle = "Delivery Quest";
    public string questDescription = "Deliver this package to the village elder.";

    protected override void HandleInteract()
    {
        // Quest-specific logic here
        Debug.Log($"Quest Offered: {questTitle} - {questDescription}");
        // We'll hook this up to our quest system later
    }

    protected override void OnPlayerEnterRange()
    {
        base.OnPlayerEnterRange(); // Show the base prompt
        Debug.Log($"Quest Available! Talk to {characterName} about '{questTitle}'");
    }
}