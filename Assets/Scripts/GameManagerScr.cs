using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Game
{
    public List<Card> EnemyDeck, PlayerDeck;
    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();
    }

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for(int i = 0; i < 10; i++){
            list.Add(CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)]);

        }
        return list;
    }
}

public class GameManagerScr : MonoBehaviour
{
    public Game CurrentGame;
    public Transform EnemyHand, PlayerHand, EnemyField, PlayerField;
    public GameObject CardPref;
    int Turn;

    public Hero PlayerH, EnemyH;
    public TextMeshProUGUI PlayerHPTxt, EnemyHPTxt;
    public GameManagerScr GameManager; //

    public List<CardInfoScr> PlayerHandCards = new List<CardInfoScr>(),
        PlayerFieldCards = new List<CardInfoScr>(),
        EnemyHandCards = new List<CardInfoScr>(),
        EnemyFieldCards = new List<CardInfoScr>();

    public GameObject WinScreen;
    public TextMeshProUGUI ResultTxt;

    public bool IsPlayerTurn{
        get{
            return Turn % 2 == 0;
        }
    }
    void Awake() //
    {
        GameManager = FindObjectOfType<GameManagerScr>();
    }

    void Start()
    {
        Turn = 0;
        PlayerH.HeroHp = 10;
        EnemyH.HeroHp = 10;
        PlayerH.Type = Hero.HeroType.PLAYER;
        EnemyH.Type = Hero.HeroType.ENEMY;

        CurrentGame = new Game();

        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand); 

    }

    void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 4){
            GiveCardToHand(deck, hand);
        }
    }

    void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if(deck.Count == 0)
            return;
        
        Card card = deck[0];
        GameObject cardGO = Instantiate(CardPref, hand, false);

        if (hand == EnemyHand){
            cardGO.GetComponent<CardInfoScr>().HideCardInfo(card);
            EnemyHandCards.Add(cardGO.GetComponent<CardInfoScr>());
        }else{
            cardGO.GetComponent<CardInfoScr>().ShowCardInfo(card);
            PlayerHandCards.Add(cardGO.GetComponent<CardInfoScr>());
        }
        deck.RemoveAt(0);
    }

    // void EnemyTurn(List<CardInfoScr> cards)
    IEnumerator EnemyTurn(List<CardInfoScr> cards)
    {        
        // DestroyCards(GameManager.PlayerFieldCards);  
        for(int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1);
            if(EnemyFieldCards.Count > 8 || EnemyHandCards.Count == 0){
                break;
            }

            cards[0].GetComponent<CardMovementScr>().MoveToField(EnemyField);
            yield return new WaitForSeconds(.51f);

            cards[0].ShowCardInfo(cards[0].SelfCard);
            cards[0].transform.SetParent(EnemyField);

            EnemyFieldCards.Add(cards[0]);
            EnemyHandCards.Remove(cards[0]);
        }
            for(int i = 0; i < EnemyFieldCards.Count; i++)
            {
                if(EnemyFieldCards[i].SelfCard.TimeCounters == 0)
                {
                    if(PlayerFieldCards.ElementAtOrDefault(i) != null)
                    {
                     CardsFight(EnemyFieldCards[i], PlayerFieldCards[i]);
                    }
                    else
                    {
                        DamageHero(EnemyFieldCards[i], PlayerH);
                    }
                }
                
                
                
            }
            foreach(var card in EnemyFieldCards)
            {
                if(card.SelfCard.TimeCounters > 0)
                {
                    card.SelfCard.TimeCounters--;
                }
                card.RefreshData();
            }
            DestroyCards(PlayerFieldCards);
        
        GameManager.ChangeTurn();
    }

    public void ChangeTurn()
    {
        Turn++;

        if(IsPlayerTurn)
        {
            GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
            GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
            
            DestroyCards(EnemyFieldCards);
        }
        else{
            // EnemyTurn(EnemyHandCards);
            StartCoroutine(EnemyTurn(EnemyHandCards));
        }
    }

    public void CardsFight(CardInfoScr attacker, CardInfoScr defender)
    {
        defender.SelfCard.GetDamage(attacker.SelfCard.Attack);
        defender.RefreshData();
    }

    public void DestroyCards(List<CardInfoScr> cards)
    {
        for(int i = 0; i < cards.Count; i++)
        {
            if(!cards[i].SelfCard.IsAlive)
            {
                // if(cards.Exists(x -> x == this))
                // {
                //     cards.Remove(this);
                // }
                Destroy(cards[i].gameObject);
                //cards.Remove(cards[i]); 
            }
        }
    }

    public void DamageHero(CardInfoScr card, Hero hero)
    {
        hero.HeroHp = Mathf.Clamp(hero.HeroHp - card.SelfCard.Attack, 0, int.MaxValue);
        ShowHP();
        CheckForResults();
    }

    void ShowHP()
    {
        EnemyHPTxt.text = EnemyH.HeroHp.ToString();
        PlayerHPTxt.text = PlayerH.HeroHp.ToString();
    }

    void CheckForResults()
    {
        if(PlayerH.HeroHp == 0)
        {
            WinScreen.SetActive(true);
            ResultTxt.text = "You lose";
        }
        else if(EnemyH.HeroHp == 0)
        {
            WinScreen.SetActive(true);
            ResultTxt.text = "You win";
        }
    }

}
