using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Spine.Unity;

public class Fishing : MonoBehaviour
{
    public UnityEvent onFishingEnd = new UnityEvent();

    public static Fishing SFishing;

    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    GameObject fishingStartTextPanel;
    [SerializeField]
    Progress progressPanel;
    [SerializeField]
    GameObject moveToMainMenuBtn;
    [SerializeField]
    FishingBtn moveToGameSceneBtn;
    [SerializeField]
    GameObject catchFishPanel;
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
    HookCaptureController hookCaptureController;

    [SerializeField]
    GameObject rod;

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

    private void Awake()
    {
        hookCaptureController = transform.Find("FishingController").Find("FishingHook").gameObject.GetComponent<HookCaptureController>();
    }

    private void Start()
    {
        if(SFishing == null)
        {
            SFishing = this;
        }

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CurrentHealth = playerController.Health;
        CatchedSmallFishCount = 0;
        CatchedMediumFishCount = 0;
        CatchedLargeFishCount = 0;
        fishingLineLenth = 0;
    }

    private void Update()
    {
        FishingLineLenth += Time.deltaTime * fishingLineSpeed;
        progressPanel.SetPercent(FishingLineLenth / playerController.FishingLineLenth);

        if(FishingLineLenth == playerController.FishingLineLenth)
        {
            if(!caeraPanel.activeSelf)
            {
                OpenCaeraPanel();
            }
        }
    }

    public void MoveToMainMenuScreen()
    {
        if (caeraPanel.activeSelf)
        {
            caeraPanel.SetActive(false);
        }

        if (failPanel.activeSelf)
        {
            failPanel.SetActive(false);
        }

        onFishingEnd.Invoke();
        Camera.main.GetComponent<MainCamera>().MoveToMainScreen();
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void DeactiveGameObject()
    {
        gameObject.SetActive(false);
    }

    public void CatchFish(AgentMovement catchedFish)
    {
        fishingSuccessCount++;

        CatchFish catchFish = catchFishPanel.GetComponent<CatchFish>();
        if(catchedFish != null)
        {
            hookCaptureController.SetTriggerBlocked(true);
            catchFish.ActiveToViewport(catchedFish);
        }
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

        CatchFish catchFish = catchFishPanel.GetComponent<CatchFish>();
        if (catchedFish != null)
        {
            hookCaptureController.SetTriggerBlocked(true);
        }
    }

    void OpenCaeraPanel()
    {
        Text[] texts = caeraPanel.GetComponentsInChildren<Text>();

        texts[0].text = "x" + catchedSmallFishCount.ToString();
        texts[1].text = "x" + catchedMediumFishCount.ToString();
        texts[2].text = "x" + catchedLargeFishCount.ToString();
        Time.timeScale = 0;

        caeraPanel.SetActive(true);
    }

    public void OpenFailPanel()
    {
        Text[] texts = failPanel.GetComponentsInChildren<Text>();

        texts[0].text = "x" + catchedSmallFishCount.ToString();
        texts[1].text = "x" + catchedMediumFishCount.ToString();
        texts[2].text = "x" + catchedLargeFishCount.ToString();
        Time.timeScale = 0;

        failPanel.SetActive(true);
    }
}
