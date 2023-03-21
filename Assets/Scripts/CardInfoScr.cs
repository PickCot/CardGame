using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScr : MonoBehaviour
{
    public Card SelfCard;
    //public CardController CC;
    public Image Logo;
    public TextMeshProUGUI Name, Attack, HP, TimeCounters;
    public GameObject HideObj;

    public CardInfoScr(Card card)
    {
        SelfCard = card;
    }

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        HideObj.SetActive(true);
        TimeCounters.text = "";
    }

    public void ShowCardInfo(Card card)
    {
        HideObj.SetActive(false);
        SelfCard = card;
        
        Logo.sprite = card.Logo;
        Logo.preserveAspect = true;
        Name.text = card.Name;
        RefreshData();
    }
    
    public void RefreshData()
    {
        Attack.text = SelfCard.Attack.ToString();
        HP.text = SelfCard.HP.ToString();
        TimeCounters.text = SelfCard.TimeCounters.ToString();
    }
}
