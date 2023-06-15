using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;


[System.Serializable]
public class Item
{
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing, string _Index)
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; Index = _Index; }

    public string Type, Name, Explain, Number, Index;
    public bool isUsing;
}


public class dkjdk : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList, CurItemList;
    public string curType = "Character";
    public GameObject[] Slot, UsingImage;
    public Image[] TabImage, ItemImage;
    public Sprite TabIdleSprite, TabSelectSprite;
    public Sprite[] ItemSprite;
    public GameObject ExplainPanel;
    public RectTransform[] SlotPos;
    public RectTransform CanvasRect;
    public InputField ItemNameInput, ItemNumberInput;
    IEnumerator PointerCoroutine;
    RectTransform ExplainRect;



    void Start()
    {
        // ��ü ������ ����Ʈ �ҷ�����
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));
        }

        Load();
        ExplainRect = ExplainPanel.GetComponent<RectTransform>();
    }

    void Update()
    {
        // ���콺 ��ġ�� ĵ���� ��ġ�� ��ȯ�� ����â�� ���
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        ExplainRect.anchoredPosition = anchoredPos + new Vector2(-180, -165);
    }



    public void GetItemClick()
    {
        Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);
        ItemNumberInput.text = ItemNumberInput.text == "" ? "1" : ItemNumberInput.text;
        if (curItem != null) curItem.Number = (int.Parse(curItem.Number) + int.Parse(ItemNumberInput.text)).ToString();
        else
        {
            // ��ü���� ���� �������� ã�� �� �����ۿ� �߰�
            Item curAllItem = AllItemList.Find(x => x.Name == ItemNameInput.text);
            if (curAllItem != null)
            {
                curAllItem.Number = ItemNumberInput.text;
                MyItemList.Add(curAllItem);
            }
        }

        MyItemList.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));
        Save();
    }

    public void RemoveItemClick()
    {
        Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);
        if (curItem != null)
        {

            int curNumber = int.Parse(curItem.Number) - int.Parse(ItemNumberInput.text == "" ? "1" : ItemNumberInput.text);

            if (curNumber <= 0) MyItemList.Remove(curItem);
            else curItem.Number = curNumber.ToString();
        }

        MyItemList.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));
        Save();
    }

    public void ResetItemClick()
    {
        Item BasicItem = AllItemList.Find(x => x.Name == "Pig");
        MyItemList = new List<Item>() { BasicItem };
        Save();
    }



    public void SlotClick(int slotNum)
    {
        Item CurItem = CurItemList[slotNum];
        Item UsingItem = CurItemList.Find(x => x.isUsing == true);

        if (curType == "Character")
        {
            if (UsingItem != null) UsingItem.isUsing = false;
            CurItem.isUsing = true;
        }
        else
        {
            CurItem.isUsing = !CurItem.isUsing;
            if (UsingItem != null) UsingItem.isUsing = false;
        }
        Save();
    }


    public void TabClick(string tabName)
    {
        // ���� ������ ����Ʈ�� Ŭ���� Ÿ�Ը� �߰�
        curType = tabName;
        CurItemList = MyItemList.FindAll(x => x.Type == tabName);


        for (int i = 0; i < Slot.Length; i++)
        {
            // ���԰� �ؽ�Ʈ ���̱�
            bool isExist = i < CurItemList.Count;
            Slot[i].SetActive(isExist);
            Slot[i].GetComponentInChildren<Text>().text = isExist ? CurItemList[i].Name : "";

            // ������ �̹����� ��������� ���̱�
            if (isExist)
            {
                ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
                UsingImage[i].SetActive(CurItemList[i].isUsing);
            }
        }

        // �� �̹���
        int tabNum = 0;
        switch (tabName)
        {
            case "Character": tabNum = 0; break;
            case "Balloon": tabNum = 1; break;
        }
        for (int i = 0; i < TabImage.Length; i++)
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
    }


    public void PointerEnter(int slotNum)
    {
        // ���Կ� ���콺�� �ø��� 0.5���Ŀ� ����â ���
        PointerCoroutine = PointerEnterDelay(slotNum);
        StartCoroutine(PointerCoroutine);

        // ����â�� �̸�, �̹���, ����, ���� ��Ÿ����
        ExplainPanel.GetComponentInChildren<Text>().text = CurItemList[slotNum].Name;
        ExplainPanel.transform.GetChild(2).GetComponent<Image>().sprite = Slot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
        ExplainPanel.transform.GetChild(3).GetComponent<Text>().text = CurItemList[slotNum].Number + "��";
        ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = CurItemList[slotNum].Explain;
    }

    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
    }

    public void PointerExit(int slotNum)
    {
        StopCoroutine(PointerCoroutine);
        ExplainPanel.SetActive(false);
    }


    void Save()
    {
        string jdata = JsonConvert.SerializeObject(MyItemList);
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);

        TabClick(curType);
    }

    void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
    }

}