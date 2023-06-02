using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUseBait : MonoBehaviour
{
    [SerializeField]
    GameObject fishing;

    [Space(10f)]
    [SerializeField]
    PlayerController playerController;

    public void ActiveToViewport()
    {
        Time.timeScale = 0;
        GameObject holdingBait = transform.GetChild(0).Find("HoldingBait").gameObject;
        if (holdingBait != null)
        {
            holdingBait.GetComponent<Text>().text = playerController.Bait.ToString();
        }
        gameObject.SetActive(true);
    }

    public void UseBait()
    {
        DeactiveToViewport();
        if (playerController.Bait <= 0)
        {
            Debug.Log("플레이어의 미끼 수가 0보다 작습니다.");
            return;
        }

        playerController.Bait -= 1;
    }

    public void DeactiveToViewport()
    {
        Time.timeScale = 1;
        fishing.GetComponent<Fishing>().ActiveFishingStartTextPanel();
        gameObject.SetActive(false);
    }

}
