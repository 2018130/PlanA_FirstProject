using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameSceneBtn : MonoBehaviour
{
    // 낙시터 클릭시 GameScene
    private void OnMouseDown()
    {
        //  인덱스 or 문자
        //SceneManager.LoadScene(1);
        SceneManager.LoadScene("GameScene");
    }

}
