using System.Collections.Generic;
using UnityEngine;

public class InventoryService : MonoBehaviour
{
    public static InventoryService Instance;

    public List<ItemSO> items = new List<ItemSO>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Add(ItemSO item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log("Added: " + item.itemName);

            
            var ui = FindObjectOfType<InventoryUI>();
            if (ui != null)
            {
                ui.Refresh();
            }
        }
    }


    public bool HasItem(string id)
    {
        return items.Exists(i => i.id == id);
    }

    public bool Remove(string id)
    {
        int index = items.FindIndex(i => i.id == id);
        if (index < 0) return false;
        items.RemoveAt(index);
        Debug.Log("Removed: " + id);
        return true;
    }
}
