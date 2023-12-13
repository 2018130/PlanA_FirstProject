using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine.Events;
using UnityEngine;

public class NewFishMove : MonoBehaviour
{
    FishData fishData = new FishData();

    Rigidbody2D rightBody2D;

    SpriteRenderer spriteRenderer;

    float fishSpeed = 1f;
    const float maxFishAngle = 0f;
    float fishAngle = 0f;
    float fishAngleWeight = 15.0f;
    int fishAngleSign = 1;
    int fishDirectionSign = 1;
    public int fishSize = 1;

    bool isCorutineRunning = false;
    bool isDetectedObject = false;

    bool makeBig = false;

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
        
        if(makeBig)
        {
            MakeBig();
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

    public void SpawnFish(Vector2 spawnPos)
    {
        gameObject.SetActive(true);
        fishData = FishDataBundle.GetRandomFishData();
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = fishData.GetSprite();
        transform.position = spawnPos;
        switch(fishData.GetFishSize())
        {
            case FishSize.Small:
                {
                    transform.localScale = new Vector2(-0.1f, 0.1f);
                    break;
                }
            case FishSize.Normal:
                {
                    transform.localScale = new Vector2(-0.25f, 0.25f);
                    break;
                }
            case FishSize.Middle:
                {
                    transform.localScale = new Vector2(-0.5f, 0.5f);
                    break;
                }
            case FishSize.Large:
                {
                    transform.localScale = new Vector2(-1f, 1f);
                    break;
                }
            case FishSize.UltraLarge:
                {
                    transform.localScale = new Vector2(-1.5f, 1.5f);
                    break;
                }
        }
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
        float rightTurnPointX = 17f;
        float leftTurnPointX = -17f;
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

    public void MoveTo(Vector3 targetPosition)
    {
        float moveSpeed = 1f;

        Vector3 dir = (targetPosition - transform.position).normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public void MakeSmall()
    {
        float speed = 1f;
        float minScale = 0.25f;

        if (Mathf.Abs(transform.localScale.x) >= minScale)
        {
            int sign = (int)(transform.localScale.x / Mathf.Abs(transform.localScale.x));
            float nextScaleX = transform.localScale.x - sign * speed * Time.deltaTime;
            float nextScaleY = transform.localScale.y - speed * Time.deltaTime;

            transform.localScale = new Vector3(nextScaleX, nextScaleY);
        }
    }

    public void MakeBig()
    {
        makeBig = true;
        float speed = 1f;
        float maxScale = 0f;
        switch (fishData.GetFishSize())
        {
            case FishSize.Small:
                {
                    maxScale = 0.1f;
                    break;
                }
            case FishSize.Normal:
                {
                    maxScale = 0.25f;
                    break;
                }
            case FishSize.Middle:
                {
                    maxScale = 0.5f;
                    break;
                }
            case FishSize.Large:
                {
                    maxScale = 1f;
                    break;
                }
            case FishSize.UltraLarge:
                {
                    maxScale = 1.5f;
                    break;
                }
        }

        if (Mathf.Abs(transform.localScale.x) <= maxScale)
        {
            int sign = (int)(transform.localScale.x / Mathf.Abs(transform.localScale.x));
            float nextScaleX = transform.localScale.x + sign * speed * Time.deltaTime;
            float nextScaleY = transform.localScale.y + speed * Time.deltaTime;

            transform.localScale = new Vector3(nextScaleX, nextScaleY);
        }
        else
        {
            makeBig = false;
        }
    }

    public void SetFishSpeed(float value)
    {
        fishSpeed = value;
    }
}
