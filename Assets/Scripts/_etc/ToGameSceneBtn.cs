using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameSceneBtn : MonoBehaviour
{
    [SerializeField]
    GameObject fishing;

    [Space(10f)]
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject toMainMenuBtn;
    [SerializeField]
    GameObject questionUseHealthPanel;

    private void OnMouseDown()
    {
        questionUseHealthPanel.GetComponent<QuestionUseHealth>().ActiveToViewport();
    }

    private void Update()
    {
        if (Input.touchCount != 0 && false)
        {
            // 낚시터 클릭시 화면 SetOn, 전환 그리고 플레이어 SetOff
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                toMainMenuBtn.SetActive(true);
                fishing.SetActive(true);
                player.SetActive(false);
                Camera.main.GetComponent<Animator>().SetTrigger("MoveToGameScreen");
            }
        }
    }
}
