using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] slot, lockImage;
    public Image[] itemImage;
    public Sprite[] itemSprite;
    public GameObject explainPanel;
    public RectTransform CanvasRect;

    Card cards;

    LibraryManager lb;
    RandomSelect rs;

    private void Awake()
    {
        SlotClick(1);
    }

    public void Update()
    {
        //if (rs.deck[slotNum].isFirst == true) lockImage[slotNum].SetActive(false);
    }
    public void SlotClick(int slotNum)
    {
        lb = GameObject.Find("LibraryManager").GetComponent<LibraryManager>();
        rs = GameObject.Find("DrawingManager").GetComponent<RandomSelect>();
        cards = lb.cards[slotNum];
        
        for (int i = 0; i < slot.Length; i++)
        {
            //이름을 비교하여 이미지를 가져옴
            itemImage[i].sprite = itemSprite[lb.cards.FindIndex(x => x.cardName == rs.deck[i].cardName)];
        }
    }

    //설명창에 세부 내용을 만듦
    IEnumerator PointerCoroutine;
    //클릭을 하고 있음을 체크하는 함수
    public void PointerEnter(int slotNum)
    {
        Debug.Log(slotNum);
        PointerCoroutine = PointerEnterDelay(slotNum);
        StartCoroutine(PointerCoroutine);

        if (rs.deck[slotNum].isFirst == true)
        {
            //세부 정보를 가져옴(이름, 이미지, 수량, 등급)
            lockImage[slotNum].SetActive(false);
            explainPanel.GetComponentInChildren<Text>().text = rs.deck[slotNum].cardName;
            explainPanel.transform.GetChild(2).GetComponent<Image>().sprite = slot[slotNum].transform.GetChild(0).GetComponent<Image>().sprite;
            explainPanel.transform.GetChild(3).GetComponent<Text>().text = rs.deck[slotNum].cardAmount + "개";
            explainPanel.transform.GetChild(4).GetComponent<Text>().text = rs.deck[slotNum].cardGrade.ToString() + " 등급";
            explainPanel.transform.GetChild(5).GetComponent<Text>().text = rs.deck[slotNum].explain;
        }
        else
        {
            //세부 정보를 가져옴(이름, 이미지, 수량, 등급)
            explainPanel.GetComponentInChildren<Text>().text = "???";
            explainPanel.transform.GetChild(2).GetComponent<Image>().sprite = slot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
            explainPanel.transform.GetChild(3).GetComponent<Text>().text = "???" + " 개";
            explainPanel.transform.GetChild(4).GetComponent<Text>().text = "???";
            explainPanel.transform.GetChild(5).GetComponent<Text>().text = "???";
        }
    }

    //딜레이를 만듦
    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log(slotNum + "슬롯 들어옴");
        explainPanel.SetActive(true);
    }

    public void PointerExit(int slotNum)
    {
        StopCoroutine(PointerCoroutine);
        //Debug.Log(slotNum + "슬롯 나감");
        explainPanel.SetActive(false);
    }
}
