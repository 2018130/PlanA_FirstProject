using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine.Events;
using UnityEngine;

public class NewFishMove : MonoBehaviour
{
    FishData fishData = new FishData();

    Rigidbody2D rightBody2D;

    SkeletonAnimation skeletonAnimation;

    float fishSpeed = 1f;
    const float maxFishAngle = 0f;
    float fishAngle = 0f;
    float fishAngleWeight = 15.0f;
    int fishAngleSign = 1;
    int fishDirectionSign = 1;
    public int fishSize = 1;

    bool isCorutineRunning = false;
    bool isDetectedObject = false;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void Start()
    {
        rightBody2D = GetComponent<Rigidbody2D>();
        UnityAction unityAction = new UnityAction(RemoveFish);
        Fishing.SFishing.onFishingEnd.AddListener(unityAction);

        StartCoroutine("C_DetectObjectTimer");
    }

    // Update is called once per frame
    void Update()
    {
        float newPosX = transform.position.x;
        float newPosY = transform.position.y;
        //배경화면 이동에 따른 물고기 이동
        newPosY += FishingBackground.SFishingBackground.BackgroundSpeed * Time.deltaTime;

        //앞으로 나아감
        //물고기 공통 속도 * 개체별 속도
        newPosX += Mathf.Cos(fishAngle * Mathf.PI / 180) * fishSpeed * Time.deltaTime * fishData.GetSpeed() * fishDirectionSign;
        newPosY += Mathf.Sin(fishAngle * Mathf.PI / 180) * fishSpeed * Time.deltaTime * fishData.GetSpeed();
        
        transform.position = new Vector3(newPosX, newPosY, 0);

        //방향 전환
        transform.rotation = Quaternion.Euler(new Vector3(0,0, fishAngle));

        if(Mathf.Abs(fishAngle) > maxFishAngle)
        {
            fishAngleSign *= -1;
        }
        //fishAngle += fishAngleSign * Time.deltaTime * fishAngleWeight;

        if (!isCorutineRunning)
        {
            StartCoroutine("C_DetectObjectTimer");
        }

        Turn();

        float upperRemovePosY = 13f;
        if (upperRemovePosY < transform.position.y)
        {
            RemoveFish();
        }
    }

    IEnumerator C_DetectObjectTimer()
    {
        isCorutineRunning = true;
        yield return new WaitForSeconds(0.5f);

        if (DetectFish())
        {
            SlowDown();
        }
        else
        {
            RecoverFishSpeed();
        }


        isCorutineRunning = false;
    }

    bool DetectFish()
    {
        Vector2 startPoint = transform.position;
        Vector2 rayDir = new Vector2(Mathf.Cos(fishAngle * Mathf.PI / 180), Mathf.Sin(fishAngle * Mathf.PI / 180)) * 5f;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, rayDir, 5f, 1 << 6);

        if(hit)
        {
            return true;
        }

        return false;
    }

    void SlowDown()
    {
        fishSpeed = 0.6f;
    }

    void RecoverFishSpeed()
    {
        fishSpeed = 1f;
    }

    void InitFishData()
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.skeletonDataAsset = Resources.Load<SkeletonDataAsset>(fishData.GetSpinePath());
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            skeletonAnimation.loop = true;
            Spine.Animation[] animations = skeletonAnimation.skeletonDataAsset.GetSkeletonData(false).Animations.Items;
            int animationIndex = Random.Range(0, animations.Length);
            skeletonAnimation.AnimationName = animations[animationIndex].ToString();
        }
    }

    public void SpawnFish(Vector2 spawnPos)
    {
        gameObject.SetActive(true);
        fishData = FishDataBundle.GetRandomFishData();
        InitFishData();
        transform.position = spawnPos;
    }

    public void RemoveFish()
    {
        NewFishSpawner.S_NewFishSpawner.InsertReadyQueue(gameObject);
        fishSpeed = 1f;
        fishAngleSign = 1;
        fishDirectionSign = 1;
        isCorutineRunning = false;
        isDetectedObject = false;
        gameObject.SetActive(false);
    }

    void Turn()
    {
        float rightTurnPointX = 115f;
        float leftTurnPointX = 84f;
        bool isTurn = false;

        if (transform.position.x > rightTurnPointX)
        {
            fishDirectionSign = -1;
            isTurn = true;
        }
        
        if(transform.position.x < leftTurnPointX)
        {
            fishDirectionSign = 1;
            isTurn = true;
        }

        if (isTurn)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public FishData GetFishData()
    {
        return fishData;
    }
}
