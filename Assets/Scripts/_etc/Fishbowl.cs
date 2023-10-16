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
    public int itemSize = 0;

    public HashSet<Item> clickedItem = new HashSet<Item>();

    [SerializeField]
    Sprite defaultBoxImage;

    [SerializeField]
    Fishing fishing;

    public GameObject sellBtn;
    public GameObject allSellBtn;
    public GameObject openTreasureBtn;

    private void Start()
    {
        content = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        sellBtn = transform.Find("SellBtn").gameObject;
        allSellBtn = transform.Find("AllSellBtn").gameObject;
        openTreasureBtn = transform.Find("OpenTreasureBtn").gameObject;
        sellWindow = transform.Find("SellWindow").gameObject;
        InitBoxes();
        InitItemInfoFromPlayerPrefs();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        SaveItemInfoToPlayerPrefs();
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

            boxes[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
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

        OpenSellWindow();
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

    public int RemoveEmptyItemInBox()
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
                        nextItem.itemCount = 0;
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

    /*
     * 아이템 정보를 id/개수로 저장
     */
    public void SaveItemInfoToPlayerPrefs()
    {
        string str = "";

        for (int i = 0; i < itemSize; i++)
        {
            Item item = boxes[i].GetComponent<Item>();
            str += item.itemId + "/" + item.itemCount + " ";
        }
        Debug.Log("-" + str + "-");
        PlayerPrefs.SetString("OwnItemIDAndCount", str);
    }

    void InitItemInfoFromPlayerPrefs()
    {
        string[] datas = PlayerPrefs.GetString("OwnItemIDAndCount").Split(' ');

        if (datas.Length == 1) return;

        for (int i = 0; i < datas.Length; i++)
        {
            string[] itemInfoAndCount = datas[i].Split('/');
            if (itemInfoAndCount.Length == 1) return;

            if (FishDataBundle.fishDatas.ContainsKey(int.Parse(itemInfoAndCount[0])))
            {

                FishData fishData = FishDataBundle.fishDatas[int.Parse(itemInfoAndCount[0])];

                if (fishData != null)
                {
                    Item item = fishing.ChangeFishDataToItem(fishData);
                    item.itemCount = int.Parse(itemInfoAndCount[1]);
                    AddItemInBox(item);
                }
            }
        }
    }
}
