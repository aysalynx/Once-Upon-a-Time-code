using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    public GameObject panel;  

    public void Toggle()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }
}
