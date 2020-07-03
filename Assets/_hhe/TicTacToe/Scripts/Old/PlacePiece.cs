using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlacePiece : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    UnityEvent GameFieldClicked;

    public bool Taken = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(Taken == false)
        {
            AppManager.GameFieldInPlay = this.gameObject;
            GameFieldClicked.Invoke();
        }
    }
}