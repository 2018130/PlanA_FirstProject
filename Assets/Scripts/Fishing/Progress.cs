using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField]
    GameObject backGround;
    [SerializeField]
    GameObject progressBar;

    float percent = 0f;

    public void SetPercent(float newPercent)
    {
        newPercent = Mathf.Clamp(newPercent, 0f, 1f);

        if (!gameObject.active)
        {
            gameObject.SetActive(true);
            if (backGround == null || progressBar == null)
            {
                backGround = transform.GetChild(0).gameObject;
                progressBar = transform.GetChild(1).gameObject;
            }
        }

        RectTransform backGroundRT = backGround.GetComponent<RectTransform>();
        RectTransform progressBarRT = progressBar.GetComponent<RectTransform>();
        progressBarRT.sizeDelta = new Vector2(backGroundRT.sizeDelta.x * newPercent, backGroundRT.sizeDelta.y);
    }

    public void DeactivePaenl()
    {
        gameObject.SetActive(false);
    }
}
