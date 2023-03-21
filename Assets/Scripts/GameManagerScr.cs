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
        List<Card> list = new List<Card>{
            new SwormSummoner("SwormSummoner", "Sprites/Cards/SwormSummoner", 1, 2, 1),
            new SomeCard("spirit", "Sprites/Cards/spirit", 1, 2, 0),
            new Archer("archer", "Sprites/Cards/archer", 1, 3, 1),
            new SomeCard("spirit", "Sprites/Cards/berserker", 3, 4, 2),
            new Druid("druid", "Sprites/Cards/druid", 1, 2, 0),
            new SomeCard("spirit", "Sprites/Cards/spirit", 1, 2, 0),
            new Archer("archer", "Sprites/Cards/archer", 1, 3, 1),
            new SomeCard("spirit", "Sprites/Cards/berserker", 3, 4, 2),
            new Druid("druid", "Sprites/Cards/druid", 1, 2, 0),
            // new SwormSummoner("SwormSummoner", "Sprites/Cards/SwormSummoner", 1, 2, 1)


            // new Card("archer", "Sprites/Cards/archer_1", 2, 3, 1),
            // new Card("berserker", "Sprites/Cards/berserker_1", 3, 4, 2),
            // new Card("spirit", "Sprites/Cards/spirit_1", 1, 2, 0),
            // new Card("archer", "Sprites/Cards/archer_1", 2, 3, 1),
            // new Card("berserker", "Sprites/Cards/berserker_1", 3, 4, 2),
            // new Card("spirit", "Sprites/Cards/spirit_1", 1, 2, 0),
            // new Card("archer", "Sprites/Cards/archer_1", 2, 3, 1),
            // new Card("berserker", "Sprites/Cards/berserker_1", 3, 4, 2)
        };
        // for(int i = 0; i < 10; i++){
        //     list.Add(CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)]);

        // }
        return list;
    }
}

public class GameManagerScr : MonoBehaviour
{
    //public static GameManagerScr Instance;
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

    //public Animation anim; //

    public bool IsPlayerTurn{
        get{
            return Turn % 2 == 0;
        }
    }
    void Awake()
    {
        //anim = GetComponent<Animation>(); //
        GameManager = FindObjectOfType<GameManagerScr>();
        // if(Instance == null)
        // {
        //     Instance = this;
        // }
    }

    void Start()
    {
        StartGame();
    }

    public void RestartGame()
    {
        foreach(var card in PlayerFieldCards)
            Destroy(card.gameObject);
        foreach(var card in PlayerHandCards)
            Destroy(card.gameObject);
        foreach(var card in EnemyFieldCards)
            Destroy(card.gameObject);
        foreach(var card in EnemyHandCards)
            Destroy(card.gameObject);

        PlayerFieldCards.Clear();
        PlayerHandCards.Clear();
        EnemyHandCards.Clear();
        EnemyFieldCards.Clear();

        StartGame();
    }
    
    void StartGame()
    {
        Turn = new System.Random().Next( -1, 1);
        PlayerH.HeroHp = 10;
        EnemyH.HeroHp = 10;
        PlayerH.Type = Hero.HeroType.PLAYER;
        EnemyH.Type = Hero.HeroType.ENEMY;

        CurrentGame = new Game();
        WinScreen.SetActive(false);
        ShowHP();
        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand); 
        ChangeTurn();
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
        
        //CreateCardPref(deck[0], hand);
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

    IEnumerator EnemyTurn(List<CardInfoScr> cards)
    {         
        for(int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1);
            if(EnemyFieldCards.Count > 8 || EnemyHandCards.Count == 0){
                break;
            }

            cards[0].GetComponent<CardMovementScr>().MoveToField(EnemyField);
            yield return new WaitForSeconds(.75f);

            cards[0].ShowCardInfo(cards[0].SelfCard);
            cards[0].transform.SetParent(EnemyField);
            //cards[0].transform.SetAsLastSibling();

            EnemyFieldCards.Add(cards[0]);
            EnemyHandCards.Remove(cards[0]);

            EnemyFieldCards[EnemyFieldCards.Count - 1].SelfCard.EnterTheBattlefield(EnemyFieldCards, EnemyField);

            yield return new WaitForSeconds(.50f);
        }
            for(int i = 0; i < EnemyFieldCards.Count; i++)
            {
                if(EnemyFieldCards[i].SelfCard.TimeCounters == 0)
                {

                    if(PlayerFieldCards.ElementAtOrDefault(i) != null)
                    {
                        EnemyFieldCards[i].SelfCard.OnAttack(PlayerFieldCards);
                        EnemyFieldCards[i].GetComponent<CardMovementScr>().MoveToTarget(PlayerFieldCards[i].transform);
                        yield return new WaitForSeconds(.75f);
                        CardsFight(EnemyFieldCards[i], PlayerFieldCards[i]);
                    }
                    else
                    {
                        EnemyFieldCards[i].GetComponent<CardMovementScr>().MoveToTarget(PlayerH.transform);
                        yield return new WaitForSeconds(.75f);
                        DamageHero(EnemyFieldCards[i], PlayerH);
                    }
                }
            }
            foreach(var card in EnemyFieldCards)
            {
                if(card.SelfCard.TimeCounters > 0)
                {
                    card.SelfCard.TimeCounters--;
                    yield return new WaitForSeconds(.75f);
                }
                card.RefreshData();
            }
            DestroyCards(PlayerFieldCards);
            ResultTxt.text = CheckForResults();
            if(ResultTxt.text == "")
            {
                GameManager.ChangeTurn();
            }
    }

    public IEnumerator PlayerTurn()
    {
        //Debug.Log(PlayerFieldCards.IndexOf(PlayerFieldCards[PlayerFieldCards.Count - 1]) + " Last element");
        PlayerFieldCards[PlayerFieldCards.Count - 1].SelfCard.EnterTheBattlefield(PlayerFieldCards, PlayerField);
        //Debug.Log(PlayerFieldCards.IndexOf(PlayerFieldCards[PlayerFieldCards.Count - 1]) + " Last element");
        yield return new WaitForSeconds(.75f);
        for(int i = 0; i < PlayerFieldCards.Count; i++)
        {   
                if(PlayerFieldCards[i].SelfCard.TimeCounters == 0)
                {
                    if(EnemyFieldCards.ElementAtOrDefault(i) != null)
                    {
                        PlayerFieldCards[i].SelfCard.OnAttack(EnemyFieldCards);
                        PlayerFieldCards[i].GetComponent<CardMovementScr>().MoveToTarget(EnemyFieldCards[i].transform);
                        yield return new WaitForSeconds(.75f);
                        CardsFight(PlayerFieldCards[i], EnemyFieldCards[i]);
                    }
                    else
                    {
                        PlayerFieldCards[i].GetComponent<CardMovementScr>().MoveToTarget(EnemyH.transform);
                        yield return new WaitForSeconds(.75f);
                        DamageHero(PlayerFieldCards[i], EnemyH);
                    }
                }
                
        }
                
            foreach(var card in PlayerFieldCards)
            {
                if(card.SelfCard.TimeCounters > 0)
                {
                    card.SelfCard.TimeCounters--;
                }
                card.RefreshData();
            }
            DestroyCards(EnemyFieldCards);
        ResultTxt.text = CheckForResults();
        if(ResultTxt.text == "")
        {
            GameManager.ChangeTurn();
        }     
    }

    public void ChangeTurn()
    {
        Turn++;
        // StopAllCoroutines();
        if(IsPlayerTurn)
        {
            GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
            DestroyCards(EnemyFieldCards);
        }
        else{
            GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
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
            if(!(cards[i].SelfCard.IsAlive))
            {
                // if(cards.Exists(x -> x == this))
                // {
                //     cards[i].Remove(this);
                // }
                Destroy(cards[i].gameObject);
                cards.Remove(cards[i]); 
            }
        }
    }

    public void DamageHero(CardInfoScr card, Hero hero)
    {
        hero.HeroHp = Mathf.Clamp(hero.HeroHp - card.SelfCard.Attack, 0, int.MaxValue);
        ShowHP();
        // ResultTxt.text = CheckForResults();
    }

    void ShowHP()
    {
        EnemyHPTxt.text = EnemyH.HeroHp.ToString();
        PlayerHPTxt.text = PlayerH.HeroHp.ToString();
    }

    string CheckForResults()
    {
        if(PlayerH.HeroHp == 0)
        {
            WinScreen.SetActive(true);
            return "You lose";
        }
        else if(EnemyH.HeroHp == 0)
        {
            WinScreen.SetActive(true);
            return "You win";
        }
        else{
            return "";
        }
    }

    // void CreateCardPref(Card card, Transform hand)
    // {
    //     GameObject cardGO = Instantiate(CardPref, hand, false);
    //     CardController cardC = cardGO.GetComponent<CardController>();

    //     cardC.Init(card, hand == PlayerHand);
    //     if(cardC.IsPlayerCard)
    //     {
    //         PlayerHandCards.Add(cardC);
    //     }
    //     else{
    //         EnemyHandCards.Add(cardC);
    //     }
    // }
}
