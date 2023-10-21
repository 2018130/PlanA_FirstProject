using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine;

[System.Serializable]
public class FishbowlSaveData
{
    public List<int> ownItemId = new List<int>();
    public List<int> ownItemCount = new List<int>();
}

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

    string path;

    private void Start()
    {
#if UNITY_EDITOR
        path = Path.Combine(Application.dataPath, "fishbowlData.json");
#elif UNITY_ANDROID
        path = Application.persistentDataPath + "/fishbowlData.json";
#endif
        content = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        sellBtn = transform.Find("SellBtn").gameObject;
        allSellBtn = transform.Find("AllSellBtn").gameObject;
        openTreasureBtn = transform.Find("OpenTreasureBtn").gameObject;
        sellWindow = transform.Find("SellWindow").gameObject;
        InitBoxes();
        InitItemInfoFromJson();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(boxes[9].GetComponent<Item>().itemType != EItemType.TREASURE)
        {
            Item testBox = boxes[9].GetComponent<Item>();
            testBox.itemId = 1000;
            testBox.itemType = EItemType.TREASURE;
        }
    }

    private void OnApplicationQuit()
    {
        SaveItemInfoToJson();
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
        playerController.SavePlayerInfoToJson();
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
    public void SaveItemInfoToJson()
    {
        FishbowlSaveData fishbowlSaveData = new FishbowlSaveData();

        for (int i = 0; i < itemSize; i++)
        {
            Item item = boxes[i].GetComponent<Item>();
            fishbowlSaveData.ownItemCount.Add(item.itemCount);
            fishbowlSaveData.ownItemId.Add(item.itemId);
        }

        string json = JsonUtility.ToJson(fishbowlSaveData, true);

        File.WriteAllText(path, json);
    }

    void InitItemInfoFromJson()
    {
        FishbowlSaveData fishbowlSaveData = new FishbowlSaveData();

        if (!File.Exists(path))
        {
            Debug.Log("파일경로가 존재하지 않습니다");
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            fishbowlSaveData = JsonUtility.FromJson<FishbowlSaveData>(loadJson);

            if (fishbowlSaveData != null)
            {
                for (int i = 0; i < fishbowlSaveData.ownItemId.Count; i++)
                {
                    if (FishDataBundle.fishDatas.ContainsKey(fishbowlSaveData.ownItemId[i]))
                    {

                        FishData fishData = FishDataBundle.fishDatas[fishbowlSaveData.ownItemId[i]];

                        if (fishData != null)
                        {
                            Item item = fishing.ChangeFishDataToItem(fishData);
                            item.itemCount = fishbowlSaveData.ownItemCount[i];
                            AddItemInBox(item);
                        }
                    }
                }
            }
        }
    }
}
