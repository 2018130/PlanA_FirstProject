using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum EItemType
{
    NONE,
    FISH,
    TREASURE
}
public class Item : MonoBehaviour
{
    Fishbowl fishbowl;

    public int itemId = -1;
    public int itemPrice = 0;
    public int itemCount = 0;
    public string itemName = "";
    public EItemType itemType = EItemType.NONE;
    public Sprite itemImage;

    private void Awake()
    {
        itemId = -1;
    }

    private void Start()
    {
        fishbowl = transform.parent.parent.parent.parent.GetComponent<Fishbowl>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Item item = new Item();
            item.itemId = 1000;
            item.itemPrice = 1000;
            item.itemCount = 1;
            item.itemName = "treasure";
            item.itemType = EItemType.TREASURE;
            fishbowl.AddItemInBox(item);
        }
    }
    /*
     * 아이템을 클릭하게 되면 해당 아이템의 id를 Fishbowl의 set에 전달
     */
    public void ClickBox()
    {
        if (itemId == -1) return;

        if (fishbowl)
        {
            HashSet<Item> clickedBoxId = fishbowl.clickedItem;

            if (clickedBoxId.Contains(this))
            {
                clickedBoxId.Remove(this);
                for (int i = 0; i < fishbowl.boxes.Length; i++)
                {
                    Item item = fishbowl.boxes[i].GetComponent<Item>();
                    if (item.itemId == itemId)
                    {
                        fishbowl.boxes[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);

                        break;
                    }
                }
            }
            else
            {
                clickedBoxId.Add(this);
                for(int i = 0; i < fishbowl.boxes.Length; i++)
                {
                    Item item = fishbowl.boxes[i].GetComponent<Item>();
                    if (item.itemId == itemId)
                    {
                        fishbowl.boxes[i].GetComponent<Image>().color = new Color(0.58f, 0.58f, 0.58f);

                        break;
                    }
                }
            }

            switch(itemType)
            {
                case EItemType.TREASURE:
                    {
                        fishbowl.sellBtn.SetActive(false);
                        fishbowl.allSellBtn.SetActive(false);
                        fishbowl.openTreasureBtn.SetActive(true);

                        break;
                    }
                case EItemType.FISH:
                    {
                        fishbowl.sellBtn.SetActive(true);
                        fishbowl.allSellBtn.SetActive(true);
                        fishbowl.openTreasureBtn.SetActive(false);

                        break;
                    }
            }
        }
    }

    public void InitItem(Item newItem)
    {
        itemId = newItem.itemId;
        itemPrice = newItem.itemPrice;
        itemCount = newItem.itemCount;
        itemName = newItem.itemName;
        itemType = newItem.itemType;
        itemImage = newItem.itemImage;  
    }

    public void UseItem()
    {
        switch (itemType)
        {
            case EItemType.TREASURE:
                {
                    Debug.Log("보물 아이템 사용 완료");
                    itemId = -1;
                    itemCount = 0;

                    break;
                }
        }
    }
}
