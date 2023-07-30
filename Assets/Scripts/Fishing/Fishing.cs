using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;

public class Fishing : MonoBehaviour
{
    [SerializeField]
    GameObject fishingStartTextPanel;
    [SerializeField]
    GameObject timerProgressPanel;
    [SerializeField]
    GameObject moveToMainMenuBtn;
    [SerializeField]
    FishingBtn moveToGameSceneBtn;
    [SerializeField]
    GameObject questionUseHealthPanel;
    [SerializeField]
    GameObject catchFishPanel;
    [SerializeField]
    GameObject bottomBar;
    HookCaptureController hookCaptureController;

    [SerializeField]
    GameObject rod;

    float questProgressPercent = 0f;
    int maxFishingSucessCount = 3;
    int fishingSuccessCount = 0;

    private void Awake()
    {
        hookCaptureController = transform.Find("FishingController").Find("FishingHook").gameObject.GetComponent<HookCaptureController>();
    }
    private void Update()
    {
        if(fishingSuccessCount == maxFishingSucessCount && !questionUseHealthPanel.activeSelf)
        {
            EndOfOneRound();
            return;
        }
    }

    public void ActiveFishingStartTextPanel()
    {
        if (timerProgressPanel.activeSelf)
        {
            timerProgressPanel.SetActive(false);
            timerProgressPanel.SetActive(false);
        }
        fishingStartTextPanel.SetActive(true);
        StartCoroutine("DeactiveFishingStartTextPanel");
    }

    IEnumerator DeactiveFishingStartTextPanel()
    {
        float panelDisplayTime = 1f;
        yield return new WaitForSeconds(panelDisplayTime);

        fishingStartTextPanel.SetActive(false);
        float gamePlayTime = 60f;
        timerProgressPanel.GetComponent<Timer>().SetTimer(gamePlayTime);
        timerProgressPanel.GetComponent<Progress>().SetPercent(questProgressPercent);
    }

    public void MoveToMainMenuScreen()
    {
        timerProgressPanel.GetComponent<Timer>().DeactivePanel();
        timerProgressPanel.GetComponent<Progress>().DeactivePaenl();
        moveToMainMenuBtn.SetActive(false);
        moveToGameSceneBtn.ReduceBtnSizeAndSetOff();
        bottomBar.SetActive(true);
        Camera.main.GetComponent<MainCamera>().MoveToMainScreen();
    }

    public void DeactiveGameObject()
    {
        gameObject.SetActive(false);
    }
    public void EndOfOneRound()
    {
        Time.timeScale = 0;
        fishingSuccessCount = 0;
        questProgressPercent = 0f;
        questionUseHealthPanel.GetComponent<QuestionUseHealth>().ActiveToViewport();
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
}
