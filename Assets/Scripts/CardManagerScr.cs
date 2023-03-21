using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, HP;
    public int MaxHP;
    public int TimeCounters;

    public bool IsAlive
    {
        get{return HP > 0;}
    }

    // public Card(string name, string logoPath, int attack, int hp, int timeCounters)
    // {
    //     Name = name;
    //     Logo = Resources.Load<Sprite>(logoPath);
    //     Attack = attack;
    //     HP = hp;
    //     TimeCounters = timeCounters;
    // }

    public abstract void EnterTheBattlefield(List<CardInfoScr> cards, Transform place);
    public abstract void OnAttack(List<CardInfoScr> cards);
    
    public void GetDamage(int damage)
    {
        HP -= damage;
    }

    public void Heal(int heal)
    {
        if(MaxHP < HP + heal)
        {
            HP = MaxHP;
        }
        else
        {
            HP += heal;
        }
    }

}

public class Druid : Card
{
    public Druid(string name, string logoPath, int attack, int hp, int timeCounters)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        HP = hp;
        MaxHP = hp;
        TimeCounters = timeCounters;
    }

    public override void EnterTheBattlefield(List<CardInfoScr> cards, Transform place)
    {
        
        if(cards.Count != 1)
        {
            cards[cards.Count - 2].SelfCard.Heal(2);
        }
    }
    public override void OnAttack(List<CardInfoScr> cards){}
}

public class SwormSummoner : Card
{
    public GameObject CardPref = GameObject.Instantiate(Resources.Load("Prefabs/BigImageCard")) as GameObject;

    public SwormSummoner(string name, string logoPath, int attack, int hp, int timeCounters)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        HP = hp;
        TimeCounters = timeCounters;
    }

    public override void EnterTheBattlefield(List<CardInfoScr> cards, Transform place)
    {
        GameObject cardGO1 = GameObject.Instantiate(CardPref, place, false) as GameObject;
        cardGO1.GetComponent<CardInfoScr>().ShowCardInfo(new SomeCard("bug1", "Sprites/Cards/Bug", 1, 1, 0));
        cards.Insert(cards.Count-1, cardGO1.GetComponent<CardInfoScr>());

        cards[cards.Count-1].transform.SetParent(place);
        // cards[cards.Count-2].transform.SetAsLastSibling();
        cards[cards.Count-1].transform.SetAsLastSibling();

        GameObject cardGO2 = GameObject.Instantiate(CardPref, place, false) as GameObject;
        cardGO2.GetComponent<CardInfoScr>().ShowCardInfo(new SomeCard("bug2", "Sprites/Cards/Bug", 1, 1, 0));
        cards.Add(cardGO2.GetComponent<CardInfoScr>());
    }
    public override void OnAttack(List<CardInfoScr> cards){}
}
// public class Bug : Card
// {
//     public Bug(string name, string logoPath, int attack, int hp, int timeCounters)
//     {
//         Name = name;
//         Logo = Resources.Load<Sprite>(logoPath);
//         Attack = attack;
//         HP = hp;
//         TimeCounters = timeCounters;
//     }

//     public override void EnterTheBattlefield(List<CardInfoScr> cards, Transform place){}
// }
public class SomeCard : Card
{
    public SomeCard(string name, string logoPath, int attack, int hp, int timeCounters)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        MaxHP = hp;
        HP = hp;
        TimeCounters = timeCounters;
    }

    public override void EnterTheBattlefield(List<CardInfoScr> cards, Transform place){}
    public override void OnAttack(List<CardInfoScr> cards){}
}

public class Archer : Card
{
    public Archer(string name, string logoPath, int attack, int hp, int timeCounters)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        MaxHP = hp;
        HP = hp;
        TimeCounters = timeCounters;
    }

    public override void EnterTheBattlefield(List<CardInfoScr> cards, Transform place){}

    public override void OnAttack(List<CardInfoScr> cards)
    {
        cards[Random.Range(0, cards.Count)].SelfCard.GetDamage(1);

    }
}

// public class CardManager
// {
//     public static List<Card> AllCards = new List<Card>();
// }

public class CardManagerScr : MonoBehaviour
{
    public GameObject MyPref;
    void Awake()
    {
        // CardManager.AllCards.Add(new Card("spirit", "Sprites/Cards/spirit", 1, 2, 0));
        // CardManager.AllCards.Add(new Card("archer", "Sprites/Cards/archer", 2, 3, 1));
        // CardManager.AllCards.Add(new Card("berserker", "Sprites/Cards/berserker", 3, 4, 2));
        // CardManager.AllCards.Add(new Druid("spirit", "Sprites/Cards/spirit_1", 1, 2, 0));
    }
}
