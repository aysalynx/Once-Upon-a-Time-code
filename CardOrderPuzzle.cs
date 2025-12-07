using UnityEngine;
using TMPro;

public class CardOrderPuzzle : MonoBehaviour
{
    [Header("Unlock")]
    public bool isUnlocked = false;   

    [Header("Sequence")]
    [Tooltip("IDs карт: 0,1,2,3")]
    public int[] sequence;            

    [Header("UI & Reward")]
    public TextMeshProUGUI infoText;
    public ItemSO rewardHat;          
    public string rewardItemId = "hat";

    int index = 0;
    bool completed = false;

    public void UnlockPuzzle()
    {
        isUnlocked = true;
        if (infoText) infoText.text = "The cards seem important now...";
    }

    public void OnCardClicked(int cardId)
    {
        if (!isUnlocked || completed) return;

        
        if (cardId == sequence[index])
        {
            index++;

            if (index >= sequence.Length)
            {
                
                completed = true;
                index = 0;

                if (infoText) infoText.text = "You found the right order!";

                if (rewardHat != null && InventoryService.Instance != null)
                    InventoryService.Instance.Add(rewardHat);
            }
            else
            {
                if (infoText) infoText.text = "Good, keep going...";
            }
        }
        else
        {
            
            index = 0;
            if (infoText) infoText.text = "Wrong order. Try again.";
        }
    }
}
