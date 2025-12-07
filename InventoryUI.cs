using System.Collections;          
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform slotsParent;   
    public GameObject slotPrefab;   

    bool started = false;           

    void Start()
    {
        started = true;
        
        StartCoroutine(DelayedRefresh());
    }

    IEnumerator DelayedRefresh()
    {
        yield return null;
        Refresh();
    }

    void OnEnable()
    {
        
        if (started)
            Refresh();
    }

    public void Refresh()
    {
        
        if (slotsParent == null)
        {
            Debug.LogError("InventoryUI: slotsParent is NOT assigned in Inspector.");
            return;
        }
        if (slotPrefab == null)
        {
            Debug.LogError("InventoryUI: slotPrefab is NOT assigned in Inspector.");
            return;
        }
        if (InventoryService.Instance == null)
        {
            Debug.LogError("InventoryUI: InventoryService.Instance is null. No InventoryService in scene?");
            return;
        }

        
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        
        foreach (var item in InventoryService.Instance.items)
        {
            var slotGO = Instantiate(slotPrefab, slotsParent);
            var ui = slotGO.GetComponent<InventorySlotUI>();
            if (ui != null)
                ui.Set(item);
        }
    }
}
