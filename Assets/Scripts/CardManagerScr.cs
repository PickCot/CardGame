using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, HP;
    public int TimeCounters;

    public bool IsAlive
    {
        get{return HP > 0;}
    }

    public Card(string name, string logoPath, int attack, int hp, int timeCounters)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        HP = hp;
        TimeCounters = timeCounters;
    }

    // public void ChangeAttackState(bool can)
    // {
    //     CanAttack = can;
    // }
    public void GetDamage(int damage)
    {
        HP -= damage;
    }

}

public class CardManager
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManagerScr : MonoBehaviour
{
    void Awake()
    {
        CardManager.AllCards.Add(new Card("spirit", "Sprites/Cards/spirit_1", 1, 2, 0));
        CardManager.AllCards.Add(new Card("archer", "Sprites/Cards/archer_1", 2, 3, 1));
        CardManager.AllCards.Add(new Card("berserker", "Sprites/Cards/berserker_1", 3, 4, 2));
    }
}
