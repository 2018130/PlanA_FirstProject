using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUseHealth : MonoBehaviour
{
    [SerializeField]
    GameObject fishing;

    [Space(10f)]
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    GameObject toMainMenuBtn;
    [SerializeField]
    FishingBtn fishingBtn;

    public void PlayToFishing()
    {
        if (playerController.Health <= 0) return;

        playerController.GetComponent<PlayerController>().Health -= 1;

        gameObject.SetActive(false);
        toMainMenuBtn.SetActive(true);
        fishing.SetActive(true);

        Camera.main.GetComponent<MainCamera>().MoveToGameScreen();
    }

    public void ActiveToViewport()
    {
        GameObject holdingHealth = transform.GetChild(0).Find("HoldingHealth").gameObject;
        if(holdingHealth != null)
        {
            holdingHealth.GetComponent<Text>().text = playerController.Health.ToString();
        }

        gameObject.SetActive(true);
    }

    public void DeactiveToViewport()
    {

        //게임화면에 있는 경우
        if (Camera.main.transform.position.y <= Camera.main.GetComponent<MainCamera>().GameViewPostionY)
        {
            Time.timeScale = 1.0f;
            fishing.GetComponent<Fishing>().MoveToMainMenuScreen();
        }

        gameObject.SetActive(false);
    }
}
