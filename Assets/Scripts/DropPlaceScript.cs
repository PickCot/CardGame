using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}

public class DropPlaceScript : MonoBehaviour, IDropHandler
{
    public FieldType Type;
    public bool insertion = false;
    // public GameManagerScr GameManager;
    
    public void OnDrop(PointerEventData eventData)
    {
        if(Type != FieldType.SELF_FIELD){
            return;
        }
            
        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();
        if(card && card.GameManager.PlayerFieldCards.Count < 9)
        {
            card.GameManager.PlayerHandCards.Remove(card.GetComponent<CardInfoScr>());
            card.GameManager.PlayerFieldCards.Add(card.GetComponent<CardInfoScr>());
            card.DefaultParent = transform;
            // card.OnCast();
            
        }
       
    }

    // public void OnPointEnter(PointerEventData eventData)
    // {
    //     if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD)
    //     CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();
    //     // if(card)
    //     // {
    //     //     card.DefaultTempCardParent = transform;
    //     // }
    // }
}
