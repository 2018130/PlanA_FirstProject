using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchFish : MonoBehaviour
{
    public void ActiveToViewport()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void DeactiveToViewport()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetRewardText(string text)
    {
        transform.GetChild(0).Find("Reward").GetComponent<Text>().text = text;
    }
}
