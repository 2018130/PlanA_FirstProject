using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Health,
    Bait,
    Sharehouse,
}

public class ItemData
{
    private int number = 0;
    private string itemName = "";
    private int index = 0;
    public int Index
    {
        get { return index; }
    }
    private ItemType itemType = ItemType.None;
    private string information = "";

    public void InitItemData(Dictionary<string, object> dictionaryItemData)
    {
        number = int.Parse(dictionaryItemData["Number"].ToString());
        itemName = dictionaryItemData["Item_Name"].ToString();
        index = int.Parse(dictionaryItemData["Index"].ToString());
        switch (dictionaryItemData["Index"].ToString())
        {
            case "체력":
                itemType = ItemType.Health;
                break;
            case "미끼":
                itemType = ItemType.Bait;
                break;
            case "쉐어하우스용":
                itemType = ItemType.Sharehouse;
                break;
            default:
                itemType = ItemType.None;
                break;
        }
        information = dictionaryItemData["Information"].ToString();
    }

    //디버깅용
    public void PrintItemData()
    {
        Debug.Log(number + " " + itemName + " " + index + " " + itemType + " " + information);
    }
}
public class ItemDataBundle : MonoBehaviour
{
    Dictionary<int, ItemData> itemDatas = new Dictionary<int, ItemData>();

    private void Start()
    {
        InitItemDatas();
        foreach(var itemData in itemDatas){
            itemData.Value.PrintItemData();
        }
    }
    //csv파일을 모두 읽어 itemDatas변수에 할당 합니다.
    public void InitItemDatas()
    {
        itemDatas.Clear();

        TextAsset itemDataCSV = Resources.Load<TextAsset>("DataTable_Item");
        if (itemDataCSV != null)
        {
            List<Dictionary<string, object>> itemDataList = CSVAssetReader.Read(itemDataCSV);
            for (int i = 0; i < itemDataList.Count; i++)
            {
                ItemData newItemData = new ItemData();
                newItemData.InitItemData(itemDataList[i]);
                itemDatas.Add(newItemData.Index, newItemData);
            }
        }
    }
}
