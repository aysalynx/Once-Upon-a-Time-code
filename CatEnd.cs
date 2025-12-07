using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class CatEnd : MonoBehaviour
{
    [Header("Requirement")]
    public string requiredItemId = "hat";  
    public bool consumeItem = false;        

    [Header("UI")]
    public TextMeshProUGUI infoText;
    [TextArea]
    public string messageNoHat =
        "The cat seems unimpressed. Maybe you need something on your head...";
    [TextArea]
    public string messageWithHat =
        "Oh, stylish! Let me show you the way out.";

    [Header("End")]
    public string endSceneName = "TheEnd";  

    bool used = false;

    void OnMouseDown()
    {
        var inv = InventoryService.Instance;
        if (inv == null) return;

        if (used) return; 

        if (!inv.HasItem(requiredItemId))
        {
            Show(messageNoHat);
            return;
        }

       
        if (consumeItem)
            inv.Remove(requiredItemId);

        used = true;
        Show(messageWithHat);

        StartCoroutine(GoToEnd());
    }

    System.Collections.IEnumerator GoToEnd()
    {
        yield return new WaitForSeconds(1.2f); 
        if (!string.IsNullOrEmpty(endSceneName))
            SceneManager.LoadScene(endSceneName);
    }

    void Show(string msg)
    {
        if (infoText != null && !string.IsNullOrEmpty(msg))
            infoText.text = msg;
    }
}
