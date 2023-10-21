using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine;

[System.Serializable]
public class BaitSaveData
{
    public List<int> ownBaitIdxs = new List<int>();
}

public class BaitWindow : MonoBehaviour
{
    public const int BAIT_BOX_SIZE = 50;
    public const int BAIT_IMAGE_SIZE = 5;
    [SerializeField]
    GameObject boxPrefab;

    [SerializeField]
    public PlayerController playerController;

    GameObject content;
    public GameObject[] boxes = new GameObject[BAIT_BOX_SIZE];
    public int ownBaitSize = 0;

    [SerializeField]
    public Sprite defaultBoxImage;

    [SerializeField]
    Sprite[] baitImages = new Sprite[BAIT_IMAGE_SIZE];
    bool[] isOwned = new bool[BAIT_IMAGE_SIZE];

    public Sprite selectedBaitImage;
    Button baitUpBtn;

    string path;

    private void Start()
    {
#if UNITY_EDITOR
        path = Path.Combine(Application.dataPath, "ownBaitData.json");
#elif UNITY_ANDROID
        path = Application.persistentDataPath + "/ownBaitData.json";
#endif
        content = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        baitUpBtn = transform.Find("BaitUpBtn").GetComponent<Button>();
        baitUpBtn.onClick.AddListener(ChangeBaitImage);
        InitBoxes();
        InitOwnBaitInforFromJson();
        gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        SaveOwnBaitInfoToJson();
    }
    void InitBoxes()
    {
        for (int i = 0; i < BAIT_BOX_SIZE; i++)
        {
            GameObject newBox = Instantiate(boxPrefab, content.transform);
            boxes[i] = newBox;
        }
    }

    public void AddBaitImageInBox(int idx)
    {
        Image boxInnerImg;
        //같은 이미지가 이미 있다면 리턴
        for (int i = 0; i < ownBaitSize; i++)
        {
            boxInnerImg = boxes[i].transform.GetChild(0).GetComponent<Image>();
            if (boxInnerImg.sprite == baitImages[idx]) return;
        }

        boxInnerImg = boxes[ownBaitSize].transform.GetChild(0).GetComponent<Image>();
        boxInnerImg.sprite = baitImages[idx];

        ownBaitSize++;
        isOwned[idx] = true;
    }

    public void AddRandomBait()
    {
        if (ownBaitSize == BAIT_IMAGE_SIZE) return;

        int preBaitSize = ownBaitSize;
        while (preBaitSize == ownBaitSize)
        {
            int idx = Random.Range(0, BAIT_IMAGE_SIZE);

            AddBaitImageInBox(idx);
        }
    }

    void ChangeBaitImage()
    {
        if (selectedBaitImage == defaultBoxImage) return;

        playerController.SetBaitImage(selectedBaitImage);
    }

    public void SaveOwnBaitInfoToJson()
    {
        BaitSaveData baitSaveData = new BaitSaveData();

        for(int i = 0; i < BAIT_IMAGE_SIZE; i++)
        {
            if(isOwned[i])
            {
                baitSaveData.ownBaitIdxs.Add(i);
            }
        }

        string jsonload = JsonUtility.ToJson(baitSaveData, true);
        File.WriteAllText(path, jsonload);
    }

    public void InitOwnBaitInforFromJson()
    {
        if (!File.Exists(path))
        {
            Debug.Log("파일경로가 존재하지 않습니다");
        }
        else
        {
            BaitSaveData baitSaveData = new BaitSaveData();
            string loadJson = File.ReadAllText(path);
            baitSaveData = JsonUtility.FromJson<BaitSaveData>(loadJson);

            if (baitSaveData != null)
            {
                for (int i = 0; i < baitSaveData.ownBaitIdxs.Count; i++)
                {
                    AddBaitImageInBox(baitSaveData.ownBaitIdxs[i]);
                }
            }
        }
    }
}
