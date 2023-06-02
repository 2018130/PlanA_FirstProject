using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SharehouseBtn : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;

    [SerializeField]
    Sprite sharehouseOnBtn;
    [SerializeField]
    Sprite sharehouseOffBtn;

    GameObject sharehouseBtn;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Main"))
        {
            ReduceBtnSizeAndSetOff();
        }
        else if (SceneManager.GetActiveScene().name.Contains("Share"))
        {
            IncreaseBtnSizeAndSetOn();
        }

    }
    public void ClickSharehouseBtn()
    {
        PlayerController playerController = canvas.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (SceneManager.GetActiveScene().name.Contains("Main"))
            {
                SceneManager.LoadScene(1);
            }
            else if (SceneManager.GetActiveScene().name.Contains("Share"))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void IncreaseBtnSizeAndSetOn()
    {
        GetComponent<Image>().sprite = sharehouseOnBtn;
        transform.localScale = transform.localScale * 1.2f;
    }

    public void ReduceBtnSizeAndSetOff()
    {
        GetComponent<Image>().sprite = sharehouseOffBtn;
        if(transform.localScale.x > 1.0f)
        {
            transform.localScale = transform.localScale * 5.0f / 6;
        }
    }
}
