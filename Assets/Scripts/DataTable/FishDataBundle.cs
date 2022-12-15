using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Habitat
{
    Sea,
    FreshWater
}

public class FishData
{
    private int id = 0;
    public int Id
    {
        get { return id; }
    }
    private string fishName = "";
    private string lv = "";
    private int speed = 0;
    private int productCount = 0;
    private string information = "";
    private List<Habitat> habitats = new List<Habitat>();

    public void InitFishData(Dictionary<string, object> dictionaryFishData)
    {
        id = int.Parse(dictionaryFishData["Id"].ToString());
        fishName = dictionaryFishData["Name"].ToString();
        lv = dictionaryFishData["Lv"].ToString();
        speed = int.Parse(dictionaryFishData["Speed"].ToString());
        productCount = int.Parse(dictionaryFishData["ProductCount"].ToString());
        information = dictionaryFishData["Information"].ToString();

        //서식지가 2개 있는경우 ""내 , 로 따로 구분 되어 있어 작업 추가
        habitats.Clear();
        string[] strHavitats = dictionaryFishData["Habitat"].ToString().Split(",");
        for(int i = 0; i < strHavitats.Length; i++)
        {
            if(strHavitats[i] == "바다")
            {
                habitats.Add(Habitat.Sea);
            }else if(strHavitats[i] == "민물")
            {
                habitats.Add(Habitat.FreshWater);
            }
        }
    }

    //디버깅용
    public void PrintFishDataVar()
    {
        Debug.Log(id + " " + fishName + " " + lv + " " + speed + " " + productCount + " " + information);
        for(int i = 0; i < habitats.Count; i++)
        {
            Debug.Log(habitats[i]);
        }
    }
}

public class FishDataBundle : MonoBehaviour
{
    //물고기의 id를 키값으로 하고 FishData형식의 밸류값를 갖는 Dictionary
    Dictionary<int, FishData> fishDatas = new Dictionary<int, FishData>();

    private void Start()
    {
        InitFishDatas();
    }

    //csv파일을 모두 읽어 fishDatas변수에 동기화 합니다.
    //이때 habitat은 enum을 사용하고 두개 이상일 수 있으므로 List에 넣습니다.
    public void InitFishDatas()
    {
        fishDatas.Clear();

        TextAsset fishDataCSV = Resources.Load<TextAsset>("DataTable_Fish");
        if (fishDataCSV != null)
        {
            List<Dictionary<string, object>> fishDataList = CSVAssetReader.Read(fishDataCSV);
            for (int i = 0; i < fishDataList.Count; i++)
            {
                FishData newFishData = new FishData();
                newFishData.InitFishData(fishDataList[i]);
                fishDatas.Add(newFishData.Id, newFishData);
            }
        }
    }

    //csv파일을 읽어 파라미터로 전달받은 id의 하나의 FishData의 변수들을 동기화 함
    //갱신에 성공한 경우 true, 그렇지 않으면 false리턴
    public bool InitFishData(int id)
    {
        TextAsset fishDataCSV = Resources.Load<TextAsset>("DataTable_Fish");
        if (fishDataCSV != null)
        {
            List<Dictionary<string, object>> fishDataList = CSVAssetReader.Read(fishDataCSV);
            for (int i = 0; i < fishDataList.Count; i++)
            {
                if (id == int.Parse(fishDataList[i]["ID"].ToString()))
                {
                    fishDatas[id].InitFishData(fishDataList[i]);

                    return true;
                }
            }
        }

        return false;
    }
}
