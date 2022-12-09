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

    // 낚시터 클릭시 GameScene
    private void OnMouseDown()
    {
        //  인덱스 or 문자
        //SceneManager.LoadScene(1);
        //SceneManager.LoadScene("GameScene");
#if UNITY_EDITOR
        toMainMenuBtn.SetActive(true);
        fishing.SetActive(true);
        player.SetActive(false);
        Camera.main.GetComponent<Animator>().SetTrigger("MoveToGameScreen");
#endif
    }

    private void Update()
    {
        if (Input.touchCount != 0)
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
