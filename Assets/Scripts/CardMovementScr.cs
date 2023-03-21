using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;
public class CardMovementScr : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera MainCamera;
    Vector3 offset;
    public Transform DefaultParent, DefaultTempCardParent;
    public GameManagerScr GameManager;
    public bool IsDraggable;
    // CardController CC;
    
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
        {
             return;
            //break;
        }
        if(DefaultParent.GetComponent<DropPlaceScript>().Type == FieldType.SELF_FIELD)
        {
            StartCoroutine(GameManager.PlayerTurn()); 
        //     for(int i = 0; i < GameManager.PlayerFieldCards.Count; i++)
        //     {   
        //         if(GameManager.PlayerFieldCards[i].SelfCard.TimeCounters == 0)
        //         {
        //             if(GameManager.EnemyFieldCards.ElementAtOrDefault(i) != null)
        //             {
        //                 GameManager.PlayerFieldCards[i].GetComponent<CardMovementScr>().MoveToTarget(GameManager.EnemyFieldCards[i].transform);
        //                 //yield return new WaitForSeconds(.75f);
        //                 GameManager.CardsFight(GameManager.PlayerFieldCards[i], GameManager.EnemyFieldCards[i]);
        //             }
        //             else
        //             {
        //                 GameManager.PlayerFieldCards[i].GetComponent<CardMovementScr>().MoveToTarget(GameManager.EnemyH.transform);
        //                 GameManager.DamageHero(GameManager.PlayerFieldCards[i], GameManager.EnemyH);
        //             }
        //         }
                
        //     }
                
        //     foreach(var card in GameManager.PlayerFieldCards)
        //     {
        //         if(card.SelfCard.TimeCounters > 0)
        //         {
        //             card.SelfCard.TimeCounters--;
        //         }
        //         card.RefreshData();
        //     }
        //     GameManager.DestroyCards(GameManager.EnemyFieldCards);
        //     GameManager.ChangeTurn();
        }
        

        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void MoveToField(Transform field)
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.DOMove(field.position, .5f);
    }

    public void MoveToTarget(Transform target)
    {
        // transform.SetParent(GameObject.Find("Canvas").transform);
        StartCoroutine(MoveToTargetCor(target));
    }

    IEnumerator MoveToTargetCor(Transform target)
    {
        Vector3 pos = transform.position;
        // Transform parent = transform.parent;
        // int index = transform.GetSiblingIndex();

        // transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;

        // transform.SetParent(GameObject.Find("Canvas").transform);

        transform.DOMove(target.position, .25f);
        yield return new WaitForSeconds(.25f);

        transform.DOMove(pos, .25f);
        yield return new WaitForSeconds(.25f);

        // transform.SetParent(parent);
        // transform.SetSiblingIndex(index);
    }

}
