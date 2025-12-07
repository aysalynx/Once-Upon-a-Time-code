using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CupClick : MonoBehaviour
{
    
    CupGameManager mainMgr;

    
#if PLAYABLE_AD
    CupGameAdManager adMgr;
#endif

    void Awake()
    {
        
        mainMgr = Object.FindFirstObjectByType<CupGameManager>();

#if PLAYABLE_AD
        adMgr = Object.FindFirstObjectByType<CupGameAdManager>();
#endif
    }

    void OnMouseDown()
    {
        
        if (mainMgr != null)
        {
            mainMgr.OnCupClicked(transform);
            return;
        }

#if PLAYABLE_AD
        // Рекламная версия
        if (adMgr != null)
        {
            adMgr.OnCupClicked(transform);
            return;
        }
#endif

        Debug.LogError("CupClick: no manager found in scene.");
    }
}
