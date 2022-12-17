using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    Fishing fishing;

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
            GetComponentInChildren<Text>().text = currentTime.ToString().Substring(0, 5);
            if(currentTime <= 0)
            {
                isEndOfTimer = true;
                EndOfTime();
            }
        }
    }

    void EndOfTime()
    {
        fishing.EndOfOneRound();
    }

    public void DeactivePanel()
    {
        gameObject.SetActive(false);
    }
}
