using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Spine.Unity;

public class Fishing : MonoBehaviour
{
    public UnityEvent onFishingEnd = new UnityEvent();

    public static Fishing SFishing;

    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    Progress progressPanel;
    [SerializeField]
    Text smallFishCountText;
    [SerializeField]
    Text mediumFishCountText;
    [SerializeField]
    Text largeFishCountText;
    [SerializeField]
    Text fishingLineText;
    [SerializeField]
    GameObject failPanel;
    [SerializeField]
    GameObject caeraPanel;
    [SerializeField]
    GameObject currentHealthUI;
    [SerializeField]
    GameObject fishbowl;
    [SerializeField]
    GameObject collection;

    float questProgressPercent = 0f;
    int maxFishingSucessCount = 3;
    int fishingSuccessCount = 0;

    int catchedSmallFishCount = 0;
    public int CatchedSmallFishCount
    {
        get
        {
            return catchedSmallFishCount;
        }

        set
        {
            catchedSmallFishCount = value;
            smallFishCountText.text = "x" + catchedSmallFishCount;
        }
    }
    int catchedMediumFishCount = 0;
    public int CatchedMediumFishCount
    {
        get
        {
            return catchedMediumFishCount;
        }

        set
        {
            catchedMediumFishCount = value;
            mediumFishCountText.text = "x" + catchedMediumFishCount;
        }
    }
    int catchedLargeFishCount = 0;
    public int CatchedLargeFishCount
    {
        get
        {
            return catchedLargeFishCount;
        }

        set
        {
            catchedLargeFishCount = value;
            largeFishCountText.text = "x" + catchedLargeFishCount;
        }
    }
    float fishingLineSpeed = 100f;
    float fishingLineLenth = 0;
    public float FishingLineLenth
    {
        get
        {
            return fishingLineLenth;
        }
        set
        {
            fishingLineLenth = Mathf.Clamp(value, 0, playerController.FishingLineLenth);
            fishingLineText.text = (int)fishingLineLenth + "m/" + playerController.FishingLineLenth + "m";
        }
    }

    float progressBar1PosX = 237f;
    int currentHealth = 10;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;
            currentHealthUI.GetComponentInChildren<Text>().text = currentHealth.ToString() + "/" + playerController.Health;

            RectTransform progressBar1 = currentHealthUI.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<RectTransform>();
            progressBar1.anchoredPosition = new Vector2(progressBar1PosX * currentHealth / playerController.Health,
                progressBar1.anchoredPosition.y);
        }
    }

    int warningTwinkle = 2, warningTwinkleCount = 0;
    bool startTwinkle = false;
    bool isTwinkleCoroutineActived = false;

    bool isInvincible = false;

    List<FishData> catchedFishs = new List<FishData>();

    private void Start()
    {
        Text[] textList = FindObjectsOfType<Text>();
        for (int i = 0; i < textList.Length; i++)
        {
            textList[i].font = Resources.Load<Font>("Fonts/jejuDolDam");
        }//

        if (SFishing == null)
        {
            SFishing = this;
        }
        else
        {
            Destroy(this);
        }

        SpriteRenderer fishingFloatsBaitImg = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
        if (fishingFloatsBaitImg)
        {
            fishingFloatsBaitImg.sprite = PlayerController.SPlayerController.selectedBaitImage;
        }

        InitFishing();
    }

    private void Update()
    {
        FishingLineLenth += Time.deltaTime * fishingLineSpeed;
        progressPanel.SetPercent(FishingLineLenth / playerController.FishingLineLenth);

        if (FishingLineLenth == playerController.FishingLineLenth)
        {
            if (!caeraPanel.activeSelf)
            {
                OpenCaeraPanel();
            }
        }

        if(startTwinkle && !isTwinkleCoroutineActived && warningTwinkleCount < warningTwinkle)
        {
            isInvincible = true;
            StartCoroutine(SetBackgroundColorToRed());

            //마지막 루프일 때
            if(warningTwinkle == warningTwinkleCount + 1)
            {
                startTwinkle = false;
                warningTwinkleCount = 0;
                isTwinkleCoroutineActived = false;
                isInvincible = false;
                return;
            }

            warningTwinkleCount++;
        }
    }

    void InitFishing()
    {
        playerController = PlayerController.SPlayerController;
        if (playerController)
        {
            Transform gameUI = playerController.transform.GetChild(1);
            progressPanel = gameUI.GetChild(0).GetComponent<Progress>();
            smallFishCountText = gameUI.GetChild(4).GetChild(1).GetComponent<Text>();
            mediumFishCountText = gameUI.GetChild(5).GetChild(1).GetComponent<Text>();
            largeFishCountText = gameUI.GetChild(6).GetChild(1).GetComponent<Text>();
            fishingLineText = gameUI.GetChild(1).GetComponent<Text>();
            failPanel = gameUI.GetChild(7).gameObject;
            caeraPanel = gameUI.GetChild(8).gameObject;
            currentHealthUI = gameUI.GetChild(3).gameObject;
            fishbowl = playerController.transform.GetChild(0).GetChild(5).GetChild(4).gameObject;
            collection = playerController.transform.GetChild(0).GetChild(7).gameObject;
            //pause Btn
            gameUI.GetChild(2).GetComponent<Button>().onClick.AddListener(playerController.OpenConfirmPanel);
            //FailPanel Confirm Btn
            gameUI.GetChild(7).GetChild(6).GetComponent<Button>().onClick.AddListener(MoveToMainMenuScreen);
            gameUI.GetChild(7).GetChild(7).GetComponent<Button>().onClick.AddListener(GetADReward);
            //CaeraPanel Confirm Btn
            gameUI.GetChild(8).GetChild(5).GetComponent<Button>().onClick.AddListener(MoveToMainMenuScreen);
            gameUI.GetChild(8).GetChild(6).GetComponent<Button>().onClick.AddListener(GetADReward);

            CurrentHealth = playerController.Health;
            CatchedSmallFishCount = 0;
            CatchedMediumFishCount = 0;
            CatchedLargeFishCount = 0;
            fishingLineLenth = 0;

            failPanel.SetActive(false);
            caeraPanel.SetActive(false);

            catchedFishs.Clear();
        }
    }

    public void MoveToMainMenuScreen()
    {
        onFishingEnd.Invoke();
        Time.timeScale = 1;

        playerController.SetMainSceneUI();
        fishbowl.GetComponent<Fishbowl>().SaveItemInfoToJson();
        collection.GetComponent<Collection>().SaveCollectionInfoToJson();
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void DeactiveGameObject()
    {
        gameObject.SetActive(false);
    }

    public void CatchFish(NewFishMove catchedFish)
    {
        switch (catchedFish.fishSize)
        {
            case 1:
                {
                    CatchedSmallFishCount++;
                    break;
                }
            case 2:
                {
                    CatchedMediumFishCount++;
                    break;
                }
            case 3:
                {
                    CatchedLargeFishCount++;
                    break;
                }
        }

        if (catchedFish.GetFishData().GetDamage() != 0 && !isInvincible)
        {
            CurrentHealth -= catchedFish.GetFishData().GetDamage();
            startTwinkle = true;
        }

        catchedFishs.Add(catchedFish.GetFishData());

        collection.GetComponent<Collection>().AddCollectedFish(catchedFish.GetFishData());
        fishbowl.GetComponent<Fishbowl>().AddItemInBox(playerController.ChangeFishDataToItem(catchedFish.GetFishData()));
    }

    void OpenCaeraPanel()
    {
        Text[] texts = caeraPanel.GetComponentsInChildren<Text>();

        texts[0].text = "x" + catchedSmallFishCount.ToString();
        texts[1].text = "x" + catchedMediumFishCount.ToString();
        texts[2].text = "x" + catchedLargeFishCount.ToString();
        Time.timeScale = 0f;

        caeraPanel.SetActive(true);
    }

    public void OpenFailPanel()
    {
        Text[] texts = failPanel.GetComponentsInChildren<Text>();

        texts[0].text = "x" + catchedSmallFishCount.ToString();
        texts[1].text = "x" + catchedMediumFishCount.ToString();
        texts[2].text = "x" + catchedMediumFishCount.ToString();
        Time.timeScale = 0f;

        failPanel.SetActive(true);
    }
    public IEnumerator SetBackgroundColorToWhite()
    {
        float whiteBrightTime = 0.2f;

        yield return new WaitForSeconds(whiteBrightTime);

        SpriteRenderer fishingBackground = transform.GetChild(0).GetComponent<SpriteRenderer>();
        fishingBackground.color = new Color(1, 1, 1);

        isTwinkleCoroutineActived = false;
    }

    public IEnumerator SetBackgroundColorToRed()
    {
        isTwinkleCoroutineActived = true;
        float redBrightTime = 0.2f;

        yield return new WaitForSeconds(redBrightTime);

        SpriteRenderer fishingBackground = transform.GetChild(0).GetComponent<SpriteRenderer>();
        fishingBackground.color = new Color(1, 0.5f, 0.5f);

        StartCoroutine(SetBackgroundColorToWhite());
    }

    public void GetADReward()
    {
        for(int i = 0; i < catchedFishs.Count; i++)
        {
            fishbowl.GetComponent<Fishbowl>().AddItemInBox(playerController.ChangeFishDataToItem(catchedFishs[i]));
        }
        Debug.Log("보상 획득 완료");
    }
}
