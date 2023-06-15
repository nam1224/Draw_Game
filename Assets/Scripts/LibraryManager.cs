using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    public List<Card> cards = new List<Card>(); //deck ����
    public List<Card> cardsResult = new List<Card>(); //deck�� result�� ���� ������
    RandomSelect randomSelect;


    //����Ʈ�� �� ī���� ������ ������
    public void GetCardsInfo()
    {
        randomSelect = GameObject.Find("DrawingManager").GetComponent<RandomSelect>();

        for (int i = 0; i < randomSelect.deck.Count; i++)
        {
            cards.Add(randomSelect.deck[i]);
        }
        
    }

    //����Ʈ�� �� �̱� ������ ������
    public void GetCardsResult()
    {
        for (int i = 0; i < randomSelect.result.Count; i++)
        {
            cardsResult.Add(randomSelect.result[i]);
        }
    }
}
