using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    Progress progress;

    bool isEndOfTimer = true;
    float maxTime = 0f;
    float currentTime = 0f;

    public void SetTimer(float maxTime)
    {
        gameObject.SetActive(true);
        this.maxTime = maxTime;
        this.currentTime = maxTime;
        isEndOfTimer = false;
    }

    private void Update()
    {
        if (!isEndOfTimer)
        {
            currentTime -= Time.deltaTime;
            //GetComponentInChildren<Text>().text = string.Format("{0:0.00#} ", currentTime);//.ToString().Substring(0, 5);
            progress.SetPercent(currentTime / maxTime);
            if(currentTime <= 0)
            {
                isEndOfTimer = true;
            }
        }
    }

    public void DeactivePanel()
    {
        gameObject.SetActive(false);
    }
}
