using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fishbowl : MonoBehaviour
{
    public const int FISHBOWL_BOX_SIZE = 50;
    [SerializeField]
    GameObject boxPrefab;
    GameObject content;
    GameObject sellWindow;
    public GameObject[] boxes = new GameObject[FISHBOWL_BOX_SIZE];
    int itemSize = 0;
    public HashSet<Item> clickedItem = new HashSet<Item>();

    private void Start()
    {
        content = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        sellWindow = transform.GetChild(5).gameObject;
        InitBoxes();
        gameObject.SetActive(false);
    }

    void InitBoxes()
    {
        for(int i = 0; i < FISHBOWL_BOX_SIZE; i++)
        {
            GameObject newBox = Instantiate(boxPrefab, content.transform);
            boxes[i] = newBox;
        }
    }

    public void VisualizeBoxesWithItemInfo()
    {
        for(int i = 0; i < itemSize; i++)
        {
            GameObject box = boxes[i];
            Image boxImg = box.transform.GetChild(0).GetComponent<Image>();
            Item item = box.GetComponentInChildren<Item>();

            if(item.itemId != -1)
            {
                boxImg.sprite = item.itemImage;
            }
        }
    }

    public void OpenSellWindow()
    {
        int totalPrice = 0;
        foreach(Item selectedItem in clickedItem)
        {
            totalPrice += selectedItem.itemPrice;
        }

        sellWindow.transform.GetChild(3).GetComponent<Text>().text = totalPrice.ToString() + "G";
        sellWindow.SetActive(true);
    }

    public void CloseSellWindow()
    {
        clickedItem.Clear();
        for(int i = 0; i < itemSize; i++)
        {
            boxes[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
        sellWindow.SetActive(false);
    }

    public void SellSelectedItems()
    {
        int totalPrice = 0;
        foreach (Item selectedItem in clickedItem)
        {
            Debug.Log(selectedItem.itemId + "��° �ε��� ������ �ǸſϷ�");
        }

        CloseSellWindow();
    }

    public void SellAllItems_Confirm()
    {
        for(int i = 0; i < itemSize; i++)
        {
            clickedItem.Add(boxes[i].GetComponent<Item>());
        }

        sellWindow.SetActive(true);
    }

    public void AddItemInBox(Item newItem)
    {
        for(int i = 0; i < itemSize; i++)
        {
            Item item = boxes[i].GetComponent<Item>();
            
            //�������� �̹� �ִ� ���
            if(newItem.itemId == item.itemId)
            {
                item.itemCount += newItem.itemCount;

                return;
            }
        }

        //�� �������� ���
        InitItemInfoInBox(itemSize, newItem);
        itemSize++;
    }

    void InitItemInfoInBox(int boxIdx, Item item)
    {
        Item boxedItem = boxes[boxIdx].GetComponent<Item>();
        boxedItem.itemCount = item.itemCount;
        boxedItem.itemId = item.itemId;
        boxedItem.itemImage = item.itemImage;
        boxedItem.itemName = item.itemName;
        boxedItem.itemPrice = item.itemPrice;
        boxedItem.itemType = item.itemType;
    }
}