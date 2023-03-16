using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    GameObject questionUseHealthPanel;

    [SerializeField]
    GameObject rod;

    float questProgressPercent = 0f;
    int maxFishingSucessCount = 3;
    int fishingSucessCount = 0;

    private void Update()
    {
        if(fishingSucessCount == maxFishingSucessCount)
        {
            EndOfOneRound();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CatchFish(null);
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

    
    public void CatchFish(FishData fishData)
    {
        //임시 코드입니다 추후 fishData에서 데이터를 읽어와야 합니다.
        fishingSucessCount++;

        questProgressPercent += 0.1f;
        questProgressPanel.GetComponent<QuestProgress>().SetPercent(questProgressPercent);
    }
}
