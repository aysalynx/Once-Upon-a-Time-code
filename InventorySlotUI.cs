using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI label;

    public void Set(ItemSO item)
    {
        icon.sprite = item.icon;
        label.text = item.itemName;
    }
}
