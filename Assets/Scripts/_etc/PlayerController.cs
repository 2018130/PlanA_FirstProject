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
    GameObject upperBar;

    Vector3 destinationPos = Vector3.zero;
    [SerializeField]
    float speed = 5.0f;

    const int maxHealth = 10;
    int health;
    float lastHealthRecoverTime = 0f;
    float healthRecoverTimeForSecond = 600f;
    public int Health
    {
        get { return health; }
        set { 
            health = value;
            if (upperBar != null)
            {
                upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = health.ToString();
                if (health > maxHealth)
                {
                    upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.red;
                }
                else if (upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color == Color.red)
                {
                    upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.white;
                }
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
                upperBar.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = bait.ToString();
                if (bait > maxBait)
                {
                    upperBar.transform.GetChild(2).GetChild(1).GetComponent<Text>().color = Color.red;
                }
                else if (upperBar.transform.GetChild(2).GetChild(1).GetComponent<Text>().color == Color.red)
                {
                    upperBar.transform.GetChild(2).GetChild(1).GetComponent<Text>().color = Color.white;
                }
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
                    upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = maxCoin.ToString();
                    upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().color = Color.red;
                }
                else
                {
                    if (upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().color == Color.red)
                    {
                        upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().color = Color.white;
                    }
                    upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = coin.ToString();
                }
            }
        }
    }

    private void Awake()
    {
        upperBar = transform.Find("UpperBar").gameObject;
    }

    private void Start()
    {
        InitializePlayerInfoFromPlayerPrefs();
        int recoverHealthSize = (int)((int)(Time.time - lastHealthRecoverTime) / healthRecoverTimeForSecond);
        if(recoverHealthSize + Health >= maxHealth)
        {
            Health = maxHealth;
            lastHealthRecoverTime = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //특정 시간 이후 체력 자동 회복 
        if (Health < maxHealth && Time.time - lastHealthRecoverTime >= healthRecoverTimeForSecond)
        {
            ++Health;
            lastHealthRecoverTime = Time.time;
        }
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
            Health = maxHealth;
            Bait = maxBait;
            Coin = 0;
            lastHealthRecoverTime = 0f;
            PlayerPrefs.SetInt("Health", Health);
            PlayerPrefs.SetInt("Bait", Bait);
            PlayerPrefs.SetInt("Coin", Coin);
            PlayerPrefs.SetFloat("lastHealthRecoverTime", lastHealthRecoverTime);
        }
        else
        {
            Health = PlayerPrefs.GetInt("Health");
            Bait = PlayerPrefs.GetInt("Bait");
            Coin = PlayerPrefs.GetInt("Coin");
            lastHealthRecoverTime = PlayerPrefs.GetFloat("lastHealthRecoverTime");
        }
    }

    public void SavePlayerInfoToPlayerPrefs()
    {
        PlayerPrefs.SetInt("Health", Health);
        PlayerPrefs.SetInt("Bait", Bait);
        PlayerPrefs.SetInt("Coin", Coin);
        PlayerPrefs.SetFloat("lastHealthRecoverTime", lastHealthRecoverTime);
    }

    public void IncreaseHBC()
    {
        Health++;
        Bait++;
        Coin++;
    }

    private void OnApplicationQuit()
    {
        SavePlayerInfoToPlayerPrefs();
    }
}
