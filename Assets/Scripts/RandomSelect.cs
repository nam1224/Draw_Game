using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSelect : MonoBehaviour
{
    public List<Card> deck = new List<Card>();  // 카드 덱
    LibraryManager libManager;
    GameManager gameManager;
    NestedScrollManager nested;
    public Scrollbar scrollbar;
    public int total = 0;  // 카드들의 가중치 총합
    public int chance = 10; //카드 뽑는 장수
    public int count = 0;

    void Start()
    {
        libManager = GameObject.Find("LibraryManager").GetComponent<LibraryManager>();
        libManager.GetCardsInfo();
        for (int i = 0; i < deck.Count; i++)
        {
            // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
            total += deck[i].weight;
        }
        // 실행
        //ResultSelect();
        panel.SetActive(false);
    }

    public GameObject draw; //뽑기 위한 버튼

    public List<Card> result = new List<Card>();  // 랜덤하게 선택된 카드를 담을 리스트

    public Transform parent;
    public GameObject cardprefab;

    public GameObject panel; //뽑기 할 때 나타나는 패널


    //버튼으로 작동
    public void ResultSelect()
    {
        //nested = GameObject.Find("Scroll View_parent").GetComponent<NestedScrollManager>();
        //nested.isEvent = true; //event가 발생했다고 알려서 화면 이동을 제한함

        for (int i = 0; i < chance; i++)
        {
            // 가중치 랜덤을 돌리면서 결과 리스트에 넣어줍니다.
            result.Add(RandomCard());
            // 비어 있는 카드를 생성하고
            CardUI cardUI = Instantiate(cardprefab, parent).GetComponent<CardUI>();
            // 생성 된 카드에 결과 리스트의 정보를 넣어줍니다.
            cardUI.tag = "clone";
            cardUI.CardUISet(result[i]);
        }

        libManager.GetCardsResult();
        draw.SetActive(false);
        panel.SetActive(true);
    }

    IEnumerator TimeDelay(float time)
    {
        yield return new WaitForSeconds(time);
    }

    //카드들을 삭제하고 result 리스트를 초기화
    public void ResultReset()
    {
        //복제된 clone 객체들을 삭제
        for (int i = 0; i < chance; i++)
        {
            GameObject[] removeObj = GameObject.FindGameObjectsWithTag("clone");
            //print(removeObj[i]);
            Destroy(removeObj[i]);
        }
        
        //정보를 도감에 저장
        for (int i = 0; i < result.Count; i++)
        {
            for (int j = 0; j < deck.Count; j++)
            {
                if(result[i].cardName == deck[j].cardName)
                {
                    deck[j].isFirst = true;
                    deck[j].cardAmount++;
                }
            }
        }

        //result 리스트의 정보를 초기화
        for (int i = 0; i < result.Count; i++)
        {
            result.RemoveAt(i);
        }
        
        //뽑기 panel off, 뽑기 버튼 on, count 초기화
        panel.SetActive(false);
        draw.SetActive(true);
        count = 0;
    }


    // 가중치 랜덤의 설명은 영상을 참고.
    public Card RandomCard()
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f)); //RoundToInt : 소수점 반올림해줌

        for (int i = 0; i < deck.Count; i++)
        {
            weight += deck[i].weight;
            if (selectNum <= weight)
            {
                Card temp = new Card(deck[i]);
                return temp;
            }
        }
        return null;
    }
}
