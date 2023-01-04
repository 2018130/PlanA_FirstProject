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

    //������
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
    //������� id�� Ű������ �ϰ� FishData������ ������� ���� Dictionary
    Dictionary<int, FishData> fishDatas = new Dictionary<int, FishData>();

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
}
