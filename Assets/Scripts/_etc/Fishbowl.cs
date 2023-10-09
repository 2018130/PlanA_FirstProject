using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fishbowl : MonoBehaviour
{
    public const int FISHBOWL_BOX_SIZE = 50;
    [SerializeField]
    GameObject boxPrefab;
    [SerializeField]
    PlayerController playerController;
    GameObject content;
    GameObject sellWindow;
    public GameObject[] boxes = new GameObject[FISHBOWL_BOX_SIZE];
    int itemSize = 0;
    public HashSet<Item> clickedItem = new HashSet<Item>();
    [SerializeField]
    Sprite defaultBoxImage;

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
            else
            {
                boxImg.sprite = defaultBoxImage;
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
            totalPrice += selectedItem.itemPrice * selectedItem.itemCount;
            selectedItem.itemCount = 0;
            selectedItem.itemId = -1;
        }

        playerController.Coin += totalPrice;
        int removeSize = RemoveEmptyItemInBox();
        CloseSellWindow();
        itemSize -= removeSize;
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
            
            //아이템이 이미 있는 경우
            if(newItem.itemId == item.itemId)
            {
                item.itemCount += newItem.itemCount;

                return;
            }
        }

        //새 아이템인 경우
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

    int RemoveEmptyItemInBox()
    {
        for(int i = 0; i < itemSize; i++)
        {
            Item item = boxes[i].GetComponent<Item>();

            if(item.itemId == -1 || item.itemCount <= 0)
            {
                for (int j = i + 1; j < itemSize; j++)
                {
                    Item nextItem = boxes[j].GetComponent<Item>();

                    if(nextItem.itemId != -1 && nextItem.itemCount > 0)
                    {
                        item.InitItem(nextItem);
                        nextItem.itemId = -1;
                        i = j - 1;
                        break;
                    }
                }
            }
        }
        VisualizeBoxesWithItemInfo();

        int removeSize = 0;

        for(int i = itemSize - 1; i >= 0; i--)
        {
            if (boxes[i].GetComponent<Item>().itemId == -1) removeSize++;
        }

        return removeSize;
    }
}
