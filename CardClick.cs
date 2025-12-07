using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CardClick : MonoBehaviour
{
    public int cardId;                 
    public CardOrderPuzzle puzzle;     

    void OnMouseDown()
    {
        if (puzzle != null)
            puzzle.OnCardClicked(cardId);
    }
}
