using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public enum EScreenState
{
    main,
    sharehouse,
}

[System.Serializable]
public class PlayerControllerSaveData
{
    public int Health;
    public int Coin;
    public int FishingLineLenth;
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController SPlayerController;

    [SerializeField]
    public ParticleSystem TouchParticle;

    [SerializeField]
    public Sprite selectedBaitImage;

    [SerializeField]
    GameObject upperBar;

    Vector3 destinationPos = Vector3.zero;

    [SerializeField]
    SpriteRenderer fishingFloats;

    GameObject confirmWindow;

    const int maxFishingLineLenth = 10000;
    public int MaxFishingLineLenth
    {
        get => maxFishingLineLenth;
    }

    int fishingLineLenth;
    public int FishingLineLenth
    {
        get { return fishingLineLenth; }
        set
        {
            fishingLineLenth = Mathf.Clamp(value, 1000, maxFishingLineLenth);
            if (upperBar != null)
            {
                upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = fishingLineLenth.ToString() + "m";
            }
        }
    }
    const int maxHealth = 500;
    public int MaxHealth
    {
        get => maxHealth;
    }
    int health;
    public int Health
    {
        get { return health; }
        set { 
            health = Mathf.Clamp(value, 300, maxHealth);
            if (upperBar != null)
            {
                upperBar.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = health.ToString();
            }
        }
    }

    const int maxCoin = 9999999;
    int coin;
    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            Debug.Log(coin + "  " + Coin);
            if (upperBar != null)
            {
                if (coin > maxCoin)
                {
                    upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = maxCoin.ToString();
                    upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.red;
                }
                else
                {
                    if (upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color == Color.red)
                    {
                        upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.white;
                    }
                    upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = coin.ToString();
                }
            }
        }
    }

    public string path = "";

    private void Start()
    {
#if UNITY_EDITOR
        path = Path.Combine(Application.dataPath, "playerData.json");
#elif UNITY_ANDROID
        path = Application.persistentDataPath + "/playerData.json";
#endif
        InitializePlayerInfoFromJson();
        SetBaitImage(selectedBaitImage);

        //
        Text[] textList = FindObjectsOfType<Text>();
        for (int i = 0; i < textList.Length; i++)
        {
            textList[i].font = Resources.Load<Font>("Fonts/jejuDolDam");
        }//
        

        if (SPlayerController != null)
        {
            Destroy(gameObject);
        }else
        {
            SPlayerController = this;
            DontDestroyOnLoad(this);
        }
        confirmWindow = transform.GetChild(3).gameObject;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.anyKeyDown)
        {
            Camera.main.GetComponent<Sound>().PlayUserTouchClip();
            ParticleSystem tp = Instantiate(TouchParticle, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            StartCoroutine(DespawnTouchParticle(tp));
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Camera.main.GetComponent<Sound>().PlayUserTouchClip();
            ParticleSystem tp = Instantiate(TouchParticle, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Quaternion.identity);
            StartCoroutine(DespawnTouchParticle(tp));
        }
#endif
    }
    public Vector3 ExchangeScreenPosToWorldPos(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        xy.Raycast(ray, out distance);
        
        return ray.GetPoint(distance);
    }

    private void InitializePlayerInfoFromJson()
    {
        if (!File.Exists(path))
        {
            Health = 300;
            Coin = 1;
            fishingLineLenth = 4000;
        }
        else
        {
            PlayerControllerSaveData playerControllerSaveData = new PlayerControllerSaveData();
            string loadJson = File.ReadAllText(path);
            playerControllerSaveData = JsonUtility.FromJson<PlayerControllerSaveData>(loadJson);

            if(playerControllerSaveData != null)
            {
                Health = playerControllerSaveData.Health;
                Coin = playerControllerSaveData.Coin;
                FishingLineLenth = playerControllerSaveData.FishingLineLenth;
                Debug.Log("from json coin : " + Coin);
            }
        }
    }

    public void SavePlayerInfoToJson()
    {
        PlayerControllerSaveData playercontrollerSaveData = new PlayerControllerSaveData();
        playercontrollerSaveData.Health = Health;
        playercontrollerSaveData.Coin = Coin;
        Debug.Log("save to json coin : " + playercontrollerSaveData.Coin);
        playercontrollerSaveData.FishingLineLenth = FishingLineLenth;

        string json = JsonUtility.ToJson(playercontrollerSaveData);
        File.WriteAllText(path, json);
    }

    public void IncreaseHBC()
    {
        Health++;
        FishingLineLenth++;
        Coin++;
    }

    private void OnApplicationQuit()
    {
        if (path.Length == 0) return;
    }

    //yyyy-MM-dd HH:mm:ss
    private double GetElapsedTimeToSeconds(string date)
    {
        char[] separator = { '-', ':', ' ' };
        string[] dates = date.Split(separator);

        TimeSpan time = DateTime.Now - new DateTime(int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(dates[2]),
                                                        int.Parse(dates[3]), int.Parse(dates[4]), int.Parse(dates[5]));

        return time.TotalSeconds;
    }

    public void SetBaitImage(Sprite newBait)
    {
        selectedBaitImage = newBait;
        upperBar.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = selectedBaitImage;
    }

    public void SetMainSceneUI()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);

        if(confirmWindow.activeSelf)
        {
            confirmWindow.SetActive(false);
        }
    }

    public void SetGameSceneUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public Item ChangeFishDataToItem(FishData fishData)
    {
        Item newItem = new Item();
        newItem.itemCount = fishData.GetCount();
        newItem.itemId = fishData.GetId();
        newItem.itemImage = fishData.GetSprite();
        newItem.itemName = fishData.GetName();
        //나중에 수정해야함
        newItem.itemPrice = fishData.GetId();

        newItem.itemType = EItemType.FISH;

        return newItem;
    }
    
    public static void OpenConfirmPanel()
    {
        Time.timeScale = 0;
        Text exitWindowText = SPlayerController.confirmWindow.transform.GetChild(2).GetComponent<Text>();

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            exitWindowText.text = "정말 종료하시겠습니까?";
            //yes btn
            SPlayerController.confirmWindow.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(SPlayerController.confirmWindow.GetComponent<ConfirmWindow>().ExitGame);
            //no btn
            SPlayerController.confirmWindow.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(SPlayerController.confirmWindow.GetComponent<ConfirmWindow>().CloseExitPanel);
        }
        else if(SceneManager.GetActiveScene().name == "GameScene")
        {
            exitWindowText.text = "메인화면으로 가기";
            //yes btn
            SPlayerController.confirmWindow.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Fishing.SFishing.MoveToMainMenuScreen);
            //no btn
            SPlayerController.confirmWindow.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(SPlayerController.confirmWindow.GetComponent<ConfirmWindow>().CloseExitPanel);
        }

        SPlayerController.confirmWindow.SetActive(true);
    }

    IEnumerator DespawnTouchParticle(ParticleSystem ps)
    {
        yield return new WaitForSeconds(1);

        Destroy(ps.gameObject);
    }
}
