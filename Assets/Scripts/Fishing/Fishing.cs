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
    GameObject timerPanel;
    [SerializeField]
    GameObject questProgressPanel;
    [SerializeField]
    GameObject moveToMainMenuBtn;
    [SerializeField]
    FishingBtn moveToGameSceneBtn;
    [SerializeField]
    GameObject questionUseHealthPanel;
    [SerializeField]
    GameObject catchFishPanel;

    [SerializeField]
    GameObject rod;

    float questProgressPercent = 0f;
    int maxFishingSucessCount = 3;
    int fishingSucessCount = 0;

    private void Update()
    {
        if(fishingSucessCount == maxFishingSucessCount && !questionUseHealthPanel.activeSelf)
        {
            EndOfOneRound();
            return;
        }
    }

    public void ActiveFishingStartTextPanel()
    {
        if (timerPanel.activeSelf || questProgressPanel.activeSelf)
        {
            timerPanel.SetActive(false);
            questProgressPanel.SetActive(false);
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
        timerPanel.GetComponent<Timer>().SetTimer(gamePlayTime);
        questProgressPanel.GetComponent<QuestProgress>().SetPercent(questProgressPercent);
    }

    public void MoveToMainMenuScreen()
    {
        timerPanel.GetComponent<Timer>().DeactivePanel();
        questProgressPanel.GetComponent<QuestProgress>().DeactivePaenl();
        moveToMainMenuBtn.SetActive(false);
        moveToGameSceneBtn.ReduceBtnSizeAndSetOff();
        Camera.main.GetComponent<MainCamera>().MoveToMainScreen();
    }

    public void DeactiveGameObject()
    {
        gameObject.SetActive(false);
    }
    public void EndOfOneRound()
    {
        Time.timeScale = 0;
        fishingSucessCount = 0;
        questProgressPercent = 0f;
        questionUseHealthPanel.GetComponent<QuestionUseHealth>().ActiveToViewport();
    }

    
    public void CatchFish(AgentMovement catchedFish)
    {
        fishingSucessCount++;

        CatchFish catchFish = catchFishPanel.GetComponent<CatchFish>();
        if(catchedFish != null)
        {
            catchFish.ActiveToViewport(catchedFish);
        }
    }
}
