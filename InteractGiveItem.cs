using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class InteractGiveItem : MonoBehaviour
{
    [Header("Requirement")]
    [Tooltip("ID item")]
    public string requiredItemId;

    [Tooltip("remove item after use")]
    public bool consumeRequiredItem = true;

    [Header("Reward")]
    [Tooltip("Item given")]
    public ItemSO rewardItem;

    [Header("UI message (optional)")]
    public TextMeshProUGUI infoText;
    [TextArea] public string messageNoItem = "I need something here...";
    [TextArea] public string messageSuccess = "Something happened!";
    [TextArea] public string messageAfterUsed = "Nothing else here.";

    bool used = false;

    void OnMouseDown()
    {
        if (used)       
        {
            Show(messageAfterUsed);
            return;
        }

        var inv = InventoryService.Instance;
        if (inv == null) return;

        
        if (!string.IsNullOrEmpty(requiredItemId) && !inv.HasItem(requiredItemId))
        {
            Show(messageNoItem);
            return;
        }

        
        if (!string.IsNullOrEmpty(requiredItemId) && consumeRequiredItem)
        {
            inv.Remove(requiredItemId);
        }

        
        if (rewardItem != null)
        {
            inv.Add(rewardItem);
        }

        used = true;
        Show(messageSuccess);
    }

    void Show(string msg)
    {
        if (infoText != null && !string.IsNullOrEmpty(msg))
            infoText.text = msg;
    }
}
