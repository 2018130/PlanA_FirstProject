using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Habitat
{
    Sea,
    FreshWater
}

public enum FishSize
{
    Small,
    Normal,
    Middle,
    Large,
    UltraLarge,
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
    private float speed = 0;
    private int productCount = 0;
    private FishSize size = FishSize.Normal;
    private string information = "";
    private string spritePath;
    private Sprite sprite;
    private List<Habitat> habitats = new List<Habitat>();

    public void InitFishData(Dictionary<string, object> dictionaryFishData)
    {
        id = int.Parse(dictionaryFishData["Id"].ToString());
        fishName = dictionaryFishData["Name"].ToString();
        lv = dictionaryFishData["Lv"].ToString();
        speed = float.Parse(dictionaryFishData["Speed"].ToString());
        productCount = int.Parse(dictionaryFishData["ProductCount"].ToString());
        switch (dictionaryFishData["Size"].ToString())
        {
            case "small":
                {
                    size = FishSize.Small;
                    break;
                }
            case "normal":
                {
                    size = FishSize.Normal;
                    break;
                }
            case "middle":
                {
                    size = FishSize.Middle;
                    break;
                }
            case "large":
                {
                    size = FishSize.Large;
                    break;
                }
            case "ultraLarge":
                {
                    size = FishSize.UltraLarge;
                    break;
                }
            default:
                {
                    size = FishSize.Normal;
                    break;
                }
        }
        information = dictionaryFishData["Information"].ToString();
        spritePath = dictionaryFishData["SpritePath"].ToString();
        sprite = Resources.Load<Sprite>(spritePath);

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

    public void InitFishData(FishData baseFishData)
    {
        id = baseFishData.id;
        fishName = baseFishData.fishName;
        lv = baseFishData.lv;
        speed = baseFishData.speed;
        productCount = baseFishData.productCount;
        size = baseFishData.size;
        information = baseFishData.information;
        spritePath = baseFishData.spritePath;
        sprite = Resources.Load<Sprite>(spritePath);

        for (int i = 0; i < baseFishData.habitats.Count; i++)
        {
            habitats.Add(baseFishData.habitats[i]);
        }
    }

    //디버깅용
    public void PrintFishDataVar()
    {
        Debug.Log(id + " " + fishName + " " + lv + " " + speed + " " + productCount + " " + information + " " + " " + spritePath);
        for(int i = 0; i < habitats.Count; i++)
        {
            Debug.Log(habitats[i]);
        }
    }

    public string GetName()
    {
        return fishName;
    }
    public string GetInformation()
    {
        return information;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public int GetCount()
    {
        return productCount;
    }

    public int GetId()
    {
        return id;
    }

    public FishSize GetFishSize()
    {
        return size;
    }
}

public class FishDataBundle : MonoBehaviour
{
    //물고기의 id를 키값으로 하고 FishData형식의 밸류값를 갖는 Dictionary
    public static Dictionary<int, FishData> fishDatas = new Dictionary<int, FishData>();

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

    public static FishData CopyFishData(int id)
    {
        FishData fishData = new FishData();
        if (fishDatas.ContainsKey(id))
        {
            fishData.InitFishData(fishDatas[id]);

            return fishData;
        }

        return null;
    }

    //각 물고기의 id가 순차적이어야함
    //1회 랜덤값 추출 후 해당 id에 대응되는 fishData의 id가 없을경우 null 반환
    public static FishData GetRandomFishData()
    {
        //추후 수정해야함 아래 랜덤하게 수를 뽑는 과정에서 이미지의 개수와 매칭함
        int randId = Random.Range(1, 13);
        if (fishDatas.ContainsKey(randId))
        {
            return fishDatas[randId];
        }

        return null;
    }
}
