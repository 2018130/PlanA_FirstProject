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

    public void DisplayQuestionUseHealthPanel()
    {
        questionUseHealthPanel.GetComponent<QuestionUseHealth>().ActiveToViewport();
    }

    public void ClickToFisingBtn()
    {
        Image btnImage = GetComponent<Image>();
        if (btnImage.sprite.name.Contains("off"))
        {
            mainCat.GetComponent<MainMenuCat>().SetAnimation("Fishing");
            GetComponent<Image>().sprite = fishingBtnOnImage;
            transform.localScale = transform.localScale * 1.2f;
            DisplayQuestionUseHealthPanel();
        }
        else
        {
            fishing.MoveToMainMenuScreen();
        }
    }

    public void ReduceBtnSizeAndSetOff()
    {
        GetComponent<Image>().sprite = fishingBtnOffImage;
        transform.localScale = transform.localScale * 5.0f/6;
    }
}
