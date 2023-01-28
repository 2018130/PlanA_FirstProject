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
    
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && isTouchInTheBtn())
        {
            questionUseHealthPanel.GetComponent<QuestionUseHealth>().ActiveToViewport();
        }
#else
        if (Input.touchCount != 0 && isTouchInTheBtn())
        {
            questionUseHealthPanel.GetComponent<QuestionUseHealth>().ActiveToViewport();
        }
#endif
    }

    bool isTouchInTheBtn()
    {
<<<<<<< Updated upstream
        if (Input.touchCount != 0 && false)
        {
            // 낚시터 클릭시 화면 SetOn, 전환 그리고 플레이어 SetOff
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                toMainMenuBtn.SetActive(true);
                fishing.SetActive(true);
                player.SetActive(false);
                Camera.main.GetComponent<Animator>().SetTrigger("MoveToGameScreen");
=======
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(playerController != null)
        {
#if UNITY_EDITOR
            Vector3 touchedPos = playerController.ExchangeScreenPosToWorldPos(Input.mousePosition);
            if (touchedPos.x >= transform.position.x - transform.localScale.x / 2 &&
                touchedPos.x <= transform.position.x + transform.localScale.x / 2 &&
                touchedPos.y >= transform.position.y - transform.localScale.y / 2 &&
                touchedPos.y <= transform.position.y + transform.localScale.y / 2)
            {
                return true;
>>>>>>> Stashed changes
            }
#else
            Vector3 touchedPos = playerController.ExchangeScreenPosToWorldPos(Input.GetTouch(Input.touchCount - 1).position);
            Debug.Log(touchedPos);
            Debug.Log("leftPosX : " + (transform.position.x - transform.localScale.x / 2));
            if(touchedPos.x >= transform.position.x - transform.localScale.x / 2 &&
                touchedPos.x <= transform.position.x + transform.localScale.x / 2 &&
                touchedPos.y >= transform.position.y - transform.localScale.y / 2 &&
                touchedPos.y <= transform.position.y + transform.localScale.y / 2)
            {
                return true;
            }
#endif
        }

        return false;
    }
}
