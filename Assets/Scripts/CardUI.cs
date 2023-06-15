using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerDownHandler
{
    public Image chr;
    public Text cardName;
    public Text cardGrade;
    Animator animator;
    RandomSelect rs;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rs = GameObject.Find("DrawingManager").GetComponent<RandomSelect>();
    }
    // 카드의 정보를 초기화
    public void CardUISet(Card card)
    {
        chr.sprite = card.cardImage;
        cardName.text = card.cardName;
        cardGrade.text = card.cardGrade.ToString();
    }

    private void _ResultReset()
    {
        rs.ResultReset();
    }
    // 카드가 클릭되면 뒤집는 애니메이션 재생
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetTrigger("Flip");
        rs.count++;
        if (rs.count == rs.chance)
        {
            Invoke("_ResultReset", 1.5f);
        }
        Debug.Log(rs.count);
    }
}
