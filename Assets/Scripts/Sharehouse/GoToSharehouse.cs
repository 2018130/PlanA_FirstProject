using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSharehouse : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;

    GameObject sharehouseBtn;

    private void Awake()
    {
        sharehouseBtn = canvas.transform.Find("BottomBar").Find("SharehouseBtn").gameObject;
    }
    public void OpenSharehouse()
    {
        SceneManager.LoadScene(1);
    }
}
