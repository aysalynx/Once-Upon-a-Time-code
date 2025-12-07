using UnityEngine;
using TMPro;

public class BunnyGiveHint : MonoBehaviour
{
    [Header("Requirement")]
    public string requiredItemId = "clock";   
    public bool consumeRequiredItem = true;

    [Header("UI")]
    public TextMeshProUGUI infoText;
    [TextArea]
    public string messageNoItem =
        "The bunny looks worried. Maybe he needs something...";
    [TextArea]
    public string messageGotClock =
        "Thank you! Remember this order: ♥ → ♦ → ♣ → ♠";

    [Header("Puzzle")]
    public CardOrderPuzzle cardPuzzle; 

    bool used = false;

    void OnMouseDown()
    {
        var inv = InventoryService.Instance;
        if (inv == null) return;

        if (used)
        {
            Show(messageGotClock); 
            return;
        }

        if (!inv.HasItem(requiredItemId))
        {
            Show(messageNoItem);
            return;
        }

        
        if (consumeRequiredItem)
            inv.Remove(requiredItemId);

        used = true;
        Show(messageGotClock);

        
        if (cardPuzzle != null)
            cardPuzzle.UnlockPuzzle();
    }

    void Show(string msg)
    {
        if (infoText != null && !string.IsNullOrEmpty(msg))
            infoText.text = msg;
    }
}
