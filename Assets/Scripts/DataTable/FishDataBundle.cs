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

        //�������� 2�� �ִ°�� ""�� , �� ���� ���� �Ǿ� �־� �۾� �߰�
        habitats.Clear();
        string[] strHavitats = dictionaryFishData["Habitat"].ToString().Split(",");
        for(int i = 0; i < strHavitats.Length; i++)
        {
            if(strHavitats[i] == "�ٴ�")
            {
                habitats.Add(Habitat.Sea);
            }else if(strHavitats[i] == "�ι�")
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

    //������
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
    //������� id�� Ű������ �ϰ� FishData������ ������� ���� Dictionary
    public static Dictionary<int, FishData> fishDatas = new Dictionary<int, FishData>();

    private void Start()
    {
        InitFishDatas();
    }

    //csv������ ��� �о� fishDatas������ ����ȭ �մϴ�.
    //�̶� habitat�� enum�� ����ϰ� �ΰ� �̻��� �� �����Ƿ� List�� �ֽ��ϴ�.
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

    //csv������ �о� �Ķ���ͷ� ���޹��� id�� �ϳ��� FishData�� �������� ����ȭ ��
    //���ſ� ������ ��� true, �׷��� ������ false����
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

    //�� ������� id�� �������̾����
    //1ȸ ������ ���� �� �ش� id�� �����Ǵ� fishData�� id�� ������� null ��ȯ
    public static FishData GetRandomFishData()
    {
        //���� �����ؾ��� �Ʒ� �����ϰ� ���� �̴� �������� �̹����� ������ ��Ī��
        int randId = Random.Range(1, 13);
        if (fishDatas.ContainsKey(randId))
        {
            return fishDatas[randId];
        }

        return null;
    }
}
