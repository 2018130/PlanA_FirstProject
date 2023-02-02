using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject upperBar;

    Vector3 destinationPos = Vector3.zero;
    [SerializeField]
    float speed = 5.0f;

    float lastTouchedTime;
    float animationLatency = 5.0f;

    const int maxHealth = 10;
    int health;
    public int Health
    {
        get { return health; }
        set { 
            health = value;
            upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = health.ToString();
            if (health > maxHealth)
            {
                upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.red;
            }
            else if(upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color == Color.red)
            {
                upperBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.white;
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
    const int maxCoin = 9999999;
    int coin;

    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            if (coin > maxCoin)
            {
                upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = maxCoin.ToString();
                upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().color = Color.red;
            }
            else {
                if (upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().color == Color.red)
                {
                    upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().color = Color.white;
                }
                upperBar.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = coin.ToString();
            }
        }
    }

    private void Start()
    {
        Health = maxHealth;
        Bait = maxBait;
        Coin = 0;
        lastTouchedTime = 0f;
    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //마우스가 클릭된 위치로 플레이어의 위치를 이동시키는 코드 입니다.
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchedTime = Time.time;
            //원근투영, 정사영 둘다 가능
            destinationPos = ExchangeScreenPosToWorldPos(Input.mousePosition);
        }
#else
        //화면 터치시 마지막 터치 위치로 이동
        if(Input.touchCount != 0)
        {
            lastTouchedTime = Time.time;
            destinationPos = ExchangeScreenPosToWorldPos(Input.GetTouch(Input.touchCount - 1).position);
        }
#endif
        //플레이어 이동 코드
        if (Vector3.Distance(destinationPos, transform.position) >= 0.3f)
        {
            Vector3 dirVector = (destinationPos - transform.position).normalized;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + dirVector * Time.deltaTime * speed);
        }

        //입력 없을 때 랜덤 애니메이션 재생
        if(Time.time - lastTouchedTime >= animationLatency)
        {
            PlayRandomAnimation();
        }

        //체력, 미끼, 코인 수 키트키
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Health++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Bait++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Coin += 1000000;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            --Health;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            --Bait;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Coin -= 1000000;
        }

        //에니메이션 동작 키트키
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetAnimation("Action1");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetAnimation("Action2");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetAnimation("Action3");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetAnimation("Fishing");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetAnimation("Idle");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SetAnimation("Sleep");
        }
    }

    private void PlayRandomAnimation()
    {
        SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();
        Array animationNames = skeletonAnimation.skeleton.Data.Animations.ToArray();

        string newAnimationName = animationNames.GetValue(UnityEngine.Random.Range(0, animationNames.Length)).ToString();
        if(newAnimationName != "Fishing" && newAnimationName != "Sleep")
        {
            SetAnimation(newAnimationName);
            Debug.Log(newAnimationName);
            lastTouchedTime = Time.time;
        }//랜덤으로 재생하는 애니메이션이 채택되지 않은 경우
        else
        {
            lastTouchedTime = Time.time - animationLatency;
        }
    }

    public float SetAnimation(string action)
    {
        SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();

        if (skeletonAnimation.skeleton.Data.FindAnimation(action) != null)
        {
            float duration = skeletonAnimation.skeleton.Data.FindAnimation(action).Duration;
            skeletonAnimation.AnimationName = action;

            return duration;
        }

        return 0;
    }

    public Vector3 ExchangeScreenPosToWorldPos(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        xy.Raycast(ray, out distance);
        
        return ray.GetPoint(distance);
    }

    public void ResetPlayer()
    {
        gameObject.SetActive(true);
        transform.position = Vector3.zero;
        SetAnimation("Action1");
    }
}
