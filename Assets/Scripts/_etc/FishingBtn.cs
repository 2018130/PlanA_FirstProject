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
    Sprite fishingBtnOnImage;
    [SerializeField]
    Sprite fishingBtnOffImage;
    [SerializeField]
    Fishing fishing;

    public void ClickToFisingBtn()
    {
        mainCat.GetComponent<Animator>().SetTrigger("StartFishing");
        mainCat.transform.GetChild(0).GetComponent<Animator>().SetTrigger("StartFishing");
        float animationPlayTime = mainCat.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;

        StartCoroutine("C_MoveToGameScreen", animationPlayTime);
    }

    public void ReduceBtnSizeAndSetOff()
    {
        GetComponent<Image>().sprite = fishingBtnOffImage;
        transform.localScale = transform.localScale * 5.0f / 6;
    }

    IEnumerator C_MoveToGameScreen(float animationPlayTime)
    {
        yield return new WaitForSeconds(animationPlayTime);

        PlayerController.SPlayerController.SetGameSceneUI();
        SceneManager.LoadSceneAsync("GameScene");
    }
}
