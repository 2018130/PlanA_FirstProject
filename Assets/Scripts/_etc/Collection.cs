using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;
using UnityEngine;

public class Collection : MonoBehaviour
{
    const int MAX_FISH_SIZE = 100;

    FishData[] fishDatas = new FishData[MAX_FISH_SIZE];
    bool[] isCollected = new bool[MAX_FISH_SIZE];
    int curIndex = 0;
    int fishDataSize = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitializeCollectionInfoFromPlayerPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        //fishDatas초기화 코드
        if(FishDataBundle.fishDatas.Count != fishDataSize)
        {
            foreach (KeyValuePair<int, FishData> data in FishDataBundle.fishDatas)
            {
                fishDatas[fishDataSize] = data.Value;

                fishDataSize++;
            }

            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isCollected[0] = true;
            SetCollection(0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isCollected[0] = false;
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

        if(!gameObject.activeSelf)
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
        gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        SaveCollectionInfoToPlayerPrefs();
    }

    void SaveCollectionInfoToPlayerPrefs()
    {
        string str = "";

        for(int i = 0; i < fishDatas.Length; i++)
        {
            if (isCollected[i])
            {
                str += i.ToString() + " ";
            }
        }

        PlayerPrefs.SetString("CollectedFishIndex", str);
    }

    void InitializeCollectionInfoFromPlayerPrefs()
    {
        if (PlayerPrefs.GetString("CollectedFishIndex").Length == 0) return;


        char[] separator = { ' ' };
        string[] indexs = PlayerPrefs.GetString("CollectedFishIndex").Split(separator);

        for(int i = 0; i < indexs.Length; i++)
        {
            indexs[i] = indexs[i].Trim();
        }
        for (int i = 0; i < fishDataSize; i++)
        {
            isCollected[i] = false;
        }
        for(int i = 0; i < indexs.Length - 1; i++)
        {
            isCollected[int.Parse(indexs[i])] = true;
        }
    }
}
