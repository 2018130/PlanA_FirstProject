using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishLvData : MonoBehaviour
{
    //ex LV : S, percentage : 35%
    Dictionary<string, float> fishLvData = new Dictionary<string, float>();

    private void Start()
    {
        InitFishLvData();
    }

    //csv������ ��� �о� fishLvData������ ����ȭ �մϴ�.
    public void InitFishLvData()
    {
        fishLvData.Clear();

        TextAsset fishDataCSV = Resources.Load<TextAsset>("DataTable_FishLv");
        if (fishDataCSV != null)
        {
            List<Dictionary<string, object>> fishLvDataList = CSVAssetReader.Read(fishDataCSV);
            for (int i = 0; i < fishLvDataList.Count; i++)
            {
                fishLvData.Add(fishLvDataList[i]["Lv"].ToString(),
                    float.Parse(fishLvDataList[i]["Percentage"].ToString()));
            }
        }
    }
}
