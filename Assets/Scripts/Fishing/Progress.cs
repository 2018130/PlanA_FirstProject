using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField]
    GameObject backGround;
    [SerializeField]
    GameObject progressBar;
    [SerializeField]
    GameObject baitIcon;

    float percent = 0f;

    public void SetPercent(float newPercent)
    {
        newPercent = Mathf.Clamp(newPercent, 0f, 1f);
        
        RectTransform backGroundRT = backGround.GetComponent<RectTransform>();
        RectTransform progressBarRT = progressBar.GetComponent<RectTransform>();
        progressBarRT.sizeDelta = new Vector2(backGroundRT.sizeDelta.x * newPercent, backGroundRT.sizeDelta.y);

        if (baitIcon)
        {
            RectTransform baitIconRT = baitIcon.GetComponent<RectTransform>();
            baitIconRT.anchoredPosition = new Vector2(progressBarRT.sizeDelta.x - 10, baitIconRT.anchoredPosition.y);
        }
    }

    public void DeactivePaenl()
    {
        gameObject.SetActive(false);
    }
}
