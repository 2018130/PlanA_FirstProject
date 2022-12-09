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

    // ������ Ŭ���� GameScene
    private void OnMouseDown()
    {
        //  �ε��� or ����
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
            // ������ Ŭ���� ȭ�� SetOn, ��ȯ �׸��� �÷��̾� SetOff
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
