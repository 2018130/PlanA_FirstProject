using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameSceneBtn : MonoBehaviour
{
    // ������ Ŭ���� GameScene
    private void OnMouseDown()
    {
        //  �ε��� or ����
        //SceneManager.LoadScene(1);
        SceneManager.LoadScene("GameScene");
    }

}
