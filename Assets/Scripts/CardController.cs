using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    // public Card card;
    // public bool IsPlayerCard;
    // public CardInfoScr Info;
    // public CardMovementScr Movement;
    // GameManagerScr gameManager;

    // public void Init(Card card, bool IsPlayerCard)
    // {
    //     this.card = card;
    //     gameManager = GameManagerScr.Instance;
    //     IsPlayerCard = IsPlayerCard;
    //     if(IsPlayerCard)
    //     {
    //         Info.ShowCardInfo();
    //     }
    //     else
    //     {
    //         Info.HideCardInfo();
    //     }
    // }

    // public void OnCast()
    // {
    //     if(IsPlayerCard)
    //     {
    //         gameManager.PlayerHandCards.Remove(this);
    //         gameManager.PlayerFieldCards.Add(this);
    //     }
    //     else
    //     {
    //         gameManager.EnemyHandCards.Remove(this);
    //         gameManager.EnemyFieldCards.Add(this);
    //     }
    // }

    // public void OnAttack()
    // {

    // }

    // public void CheckForAlive()
    // {
    //     if(Card.IsAlive)
    //     {
    //         Info.RefreshData();
    //     }
    //     else{
    //         DestroyCard();
    //     }
    // }

    // public void DestroyCard()
    // {
    //     Movement.OnEndDrag(null);

    //     RemoveCardFromList(gameManager.EnemyFieldCards);
    //     RemoveCardFromList(gameManager.EnemyHandCards);
    //     RemoveCardFromList(gameManager.PlayerFieldCards);
    //     RemoveCardFromList(gameManager.PlayerHandCards);
    // }

    // void RemoveCardFromList(List<CardController> list)
    // {
    //     if(list.Exists(x -> x == this))
    //         list.Remove(this);
    // }
}
