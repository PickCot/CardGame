using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using DG.Tweening;
public class CardMovementScr : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera MainCamera;
    Vector3 offset;
    public Transform DefaultParent, DefaultTempCardParent;
    public GameManagerScr GameManager;
    public bool IsDraggable;
    
    void Awake()
    {
        MainCamera = Camera.allCameras[0];
        GameManager = FindObjectOfType<GameManagerScr>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        DefaultParent = transform.parent;

        IsDraggable = DefaultParent.GetComponent<DropPlaceScript>().Type == FieldType.SELF_HAND &&
                      GameManager.IsPlayerTurn;


        if (!IsDraggable)
            return;

        transform.SetParent(DefaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDraggable)
            return;
        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDraggable)
            return;
        
        if(DefaultParent.GetComponent<DropPlaceScript>().Type == FieldType.SELF_FIELD)
        {
            //Destroy(GameManager.EnemyFieldCards);
            for(int i = 0; i < GameManager.PlayerFieldCards.Count; i++)
            {   
                if(GameManager.PlayerFieldCards[i].SelfCard.TimeCounters == 0)
                {
                    if(GameManager.EnemyFieldCards.ElementAtOrDefault(i) != null)
                    {
                        GameManager.CardsFight(GameManager.PlayerFieldCards[i], GameManager.EnemyFieldCards[i]);
                    }
                    else
                    {
                        GameManager.DamageHero(GameManager.PlayerFieldCards[i], GameManager.EnemyH);
                    }
                }
                
            }
            
            foreach(var card in GameManager.PlayerFieldCards)
            {
                if(card.SelfCard.TimeCounters > 0)
                {
                    card.SelfCard.TimeCounters--;
                }
                card.RefreshData();
            }
            GameManager.ChangeTurn();
        }
        

        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void MoveToField(Transform field)
    {
        transform.DOMove(field.position, .5f);
    }

}
