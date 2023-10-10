using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum EScreenState
{
    main,
    sharehouse,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Sprite selectedBaitImage;

    [SerializeField]
    GameObject upperBar;

    Vector3 destinationPos = Vector3.zero;
    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    SpriteRenderer fishingFloats;

    const int maxFishingLineLenth = 3000;
    int fishingLineLenth;
    public int FishingLineLenth
    {
        get { return fishingLineLenth; }
        set
        {
            fishingLineLenth = Mathf.Clamp(value, 1500, maxFishingLineLenth);
            if (upperBar != null)
            {
                upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = fishingLineLenth.ToString() + "m";
            }
        }
    }
    const int maxHealth = 1000;
    int health;
    public int Health
    {
        get { return health; }
        set { 
            health = Mathf.Clamp(value, 300, maxHealth);
            if (upperBar != null)
            {
                upperBar.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = health.ToString();
            }
        }
    }

    const int maxBait = 10;
    int bait;
    public int Bait
    {
        get { return bait; }
        set
        {
            bait = value;
            if (upperBar != null)
            {
                //upperBar.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = bait.ToString();
            }
        }
    }

    const int maxCoin = 9999999;
    int coin;
    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            if (upperBar != null)
            {
                if (coin > maxCoin)
                {
                    upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = maxCoin.ToString();
                    upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.red;
                }
                else
                {
                    if (upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color == Color.red)
                    {
                        upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.white;
                    }
                    upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = coin.ToString();
                }
            }
        }
    }

    private void Start()
    {
        InitializePlayerInfoFromPlayerPrefs();
        SetBaitImage(selectedBaitImage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 ExchangeScreenPosToWorldPos(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        xy.Raycast(ray, out distance);
        
        return ray.GetPoint(distance);
    }

    private void InitializePlayerInfoFromPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("Health"))
        {
            Health = 300;
            Bait = maxBait;
            Coin = 0;
            PlayerPrefs.SetInt("Health", Health);
            PlayerPrefs.SetInt("Bait", Bait);
            PlayerPrefs.SetInt("Coin", Coin);
            PlayerPrefs.SetInt("FishingLineLenth", Coin);
        }
        else
        {
            Health = PlayerPrefs.GetInt("Health");
            Bait = PlayerPrefs.GetInt("Bait");
            Coin = PlayerPrefs.GetInt("Coin");
            FishingLineLenth = PlayerPrefs.GetInt("FishingLineLenth");
        }
    }

    public void SavePlayerInfoToPlayerPrefs()
    {
        PlayerPrefs.SetInt("Health", Health);
        PlayerPrefs.SetInt("Bait", Bait);
        PlayerPrefs.SetInt("Coin", Coin);
        PlayerPrefs.SetInt("FishingLineLenth", FishingLineLenth);
    }

    public void IncreaseHBC()
    {
        Health++;
        FishingLineLenth++;
        Coin++;
    }

    private void OnApplicationQuit()
    {
        SavePlayerInfoToPlayerPrefs();
    }

    //yyyy-MM-dd HH:mm:ss
    private double GetElapsedTimeToSeconds(string date)
    {
        char[] separator = { '-', ':', ' ' };
        string[] dates = date.Split(separator);

        TimeSpan time = DateTime.Now - new DateTime(int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(dates[2]),
                                                        int.Parse(dates[3]), int.Parse(dates[4]), int.Parse(dates[5]));

        return time.TotalSeconds;
    }

    public void SetBaitImage(Sprite newBait)
    {
        selectedBaitImage = newBait;
        upperBar.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = selectedBaitImage;
        fishingFloats.sprite = newBait;
    }
}
