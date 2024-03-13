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

    string path = "";

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
        //fishDatas초기화 코드
        if (FishDataBundle.fishDatas.Count != fishDataSize)
        {
            foreach (KeyValuePair<int, FishData> data in FishDataBundle.fishDatas)
            {
                fishDatas[fishDataSize] = data.Value;

                fishDataSize++;
            }

            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                gameObject.SetActive(false);
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                gameObject.SetActive(false);
            }
        }
#endif
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
        fishImg.sprite = fishDatas[index].GetSprite();

        if (!isCollected[index])
        {
            fishImg.color = new Color(0, 0, 0);
        }
        else
        {
            fishImg.color = new Color(1, 1, 1);
        }
    }

    public void AddCollectedFish(FishData fishData)
    {
        for(int i = 0; i < fishDataSize; i++)
        {
            if(fishDatas[i].GetId() == fishData.GetId())
            {
                isCollected[i] = true;
                Debug.Log(fishData.GetName() + " was collected");

                break;
            }
        }
    }

    public void ExitCollection()
    {
        SaveCollectionInfoToJson();
        gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        if (path.Length == 0) return;

        SaveCollectionInfoToJson();
    }

    public void SaveCollectionInfoToJson()
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
