using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardGrade { SS, S, A, B, C }

[System.Serializable]
public class Card 
{
    public string cardName;
    public Sprite cardImage;
    public CardGrade cardGrade;
    public int weight;
    public bool isFirst;
    public string explain;
    //public GameObject decks;

    //추가 사항
    public int cardAmount;

    public Card(Card card)
    {
        this.cardGrade = card.cardGrade;
        this.cardName = card.cardName;
        this.cardImage = card.cardImage;
        this.weight = card.weight;
    }
}
