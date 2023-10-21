using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine;

public class CollectionSaveData
{
    public List<int> collectedFishIdx = new List<int>();
}

public class Collection : MonoBehaviour
{
    const int MAX_FISH_SIZE = 100;

    FishData[] fishDatas = new FishData[MAX_FISH_SIZE];
    bool[] isCollected = new bool[MAX_FISH_SIZE];
    int curIndex = 0;
    int fishDataSize = 0;

    string path;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        path = Path.Combine(Application.dataPath, "collectionData.json");
#elif UNITY_ANDROID
        path = Application.persistentDataPath + "/collectionData.json";
#endif
        InitializeCollectionInfoFromJson();
    }

    // Update is called once per frame
    void Update()
    {
        //fishDatas�ʱ�ȭ �ڵ�
        if (FishDataBundle.fishDatas.Count != fishDataSize)
        {
            foreach (KeyValuePair<int, FishData> data in FishDataBundle.fishDatas)
            {
                fishDatas[fishDataSize] = data.Value;

                fishDataSize++;
            }

            gameObject.SetActive(false);
        }

        //������ �ڵ�
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (!isCollected[0])
            {
                isCollected[0] = true;
                SetCollection(0);
            }
            else
            {
                isCollected[0] = false;
                SetCollection(0);
            }
        }
    }

    public void PreCollectionFishInfo()
    {
        curIndex = curIndex - 1 < 0 ? fishDataSize - 1 : curIndex - 1;
        SetCollection(curIndex);
    }

    public void NextCollectionFishInfo()
    {
        curIndex = curIndex + 1 >= fishDataSize ? 0 : curIndex + 1;
        SetCollection(curIndex);
    }

    public void SetCollection(int index)
    {
        if (index < 0 || index >= fishDataSize) return;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        Text page = transform.GetChild(1).GetComponent<Text>();
        Text name = transform.GetChild(2).GetComponent<Text>();
        Text description = transform.GetChild(4).GetComponent<Text>();
        Image fishImg = transform.GetChild(3).GetComponent<Image>();

        page.text = (curIndex + 1).ToString() + "/" + fishDataSize.ToString();
        name.text = fishDatas[curIndex].GetName();
        description.text = fishDatas[curIndex].GetInformation();

        if (!isCollected[index])
        {
            fishImg.color = new Color(0, 0, 0);
        }
        else
        {
            fishImg.color = new Color(1, 1, 1);
        }
    }

    public void ExitCollection()
    {
        SaveCollectionInfoToJson();
        gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        SaveCollectionInfoToJson();
    }

    void SaveCollectionInfoToJson()
    {
        CollectionSaveData collectionDatabase = new CollectionSaveData();

        for (int i = 0; i < fishDatas.Length; i++)
        {
            if (isCollected[i])
            {
                collectionDatabase.collectedFishIdx.Add(i);
            }
        }
        string json = JsonUtility.ToJson(collectionDatabase, true);
        File.WriteAllText(path, json);

    }

    void InitializeCollectionInfoFromJson()
    {
        if (!File.Exists(path)) return;

        string jsonLoad = File.ReadAllText(path);
        CollectionSaveData collectionDatabase = new CollectionSaveData();
        collectionDatabase = JsonUtility.FromJson<CollectionSaveData>(jsonLoad);

        if (collectionDatabase != null)
        {
            for (int i = 0; i < fishDataSize; i++)
            {
                isCollected[i] = false;
            }
            for (int i = 0; i < collectionDatabase.collectedFishIdx.Count; i++)
            {
                isCollected[collectionDatabase.collectedFishIdx[i]] = true;
            }
        }
    }
}
