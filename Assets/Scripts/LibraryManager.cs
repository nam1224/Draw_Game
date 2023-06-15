using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    public List<Card> cards = new List<Card>(); //deck 복사
    public List<Card> cardsResult = new List<Card>(); //deck의 result의 값을 가져옴
    RandomSelect randomSelect;


    //리스트로 된 카드의 정보를 가져옴
    public void GetCardsInfo()
    {
        randomSelect = GameObject.Find("DrawingManager").GetComponent<RandomSelect>();

        for (int i = 0; i < randomSelect.deck.Count; i++)
        {
            cards.Add(randomSelect.deck[i]);
        }
        
    }

    //리스트로 된 뽑기 내역을 가져옴
    public void GetCardsResult()
    {
        for (int i = 0; i < randomSelect.result.Count; i++)
        {
            cardsResult.Add(randomSelect.result[i]);
        }
    }
}
