using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FishingBtn : MonoBehaviour
{

    [Space(10f)]
    [SerializeField]
    GameObject mainCat;
    [SerializeField]
    GameObject questionUseHealthPanel;

    [SerializeField]
    Sprite fishingBtnOnImage;
    [SerializeField]
    Sprite fishingBtnOffImage;
    [SerializeField]
    Fishing fishing;

    public void ClickToFisingBtn()
    {
        Image btnImage = GetComponent<Image>();

        //mainCat.GetComponent<MainMenuCat>().SetAnimation("Fishing");
        MainCamera mainCamera = Camera.main.GetComponent<MainCamera>();
        if (mainCamera)
        {
            mainCamera.MoveToGameScreen();
        }
    }

    public void ReduceBtnSizeAndSetOff()
    {
        GetComponent<Image>().sprite = fishingBtnOffImage;
        transform.localScale = transform.localScale * 5.0f / 6;
    }
}
