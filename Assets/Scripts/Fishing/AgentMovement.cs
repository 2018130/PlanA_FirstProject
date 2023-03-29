using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Spine.Unity;

public class AgentMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private FishSpawner fishSp = null;
    private Vector3 beforePos = Vector3.zero;
    private Vector3 fishNext1Pos = Vector3.zero;
    private Vector3 fishNext1P = Vector3.zero;
    private Vector3 fishNext2P = Vector3.zero;
    private Vector3 fishNext2Pos = Vector3.zero;
    private Vector3 fishEndV = Vector3.zero;

    private Vector3 spawnPos = Vector3.zero;
    private Vector3 fishNext1Point;
    private Vector3 fishNext2Point;
    private Vector3 fishEndPoint;

    private float ranNext1Y = 0f;
    private float ranEnd1Y = 0f;
    private float ranStart2Y = 0f;
    private float ranNext2Y = 0f;
    private float ranEnd2Y = 0f;
    private float offsetAngle = 1f;

    private int boundaryX = 0;
    private int boundaryY = 0;

    FishData fishData = new FishData();
    SkeletonAnimation skeletonAnimation;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void Start()
    {
        FishData baseFishData = FishDataBundle.GetRandomFishData();
        if (baseFishData != null)
        {
            fishData.InitFishData(baseFishData);
        }

        skeletonAnimation.skeletonDataAsset = Resources.Load<SkeletonDataAsset>(fishData.GetImagePath());
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        skeletonAnimation.loop = true;
        skeletonAnimation.AnimationName = "animation";
        StartCoroutine("SetDest");
    }

    private void Update()
    {

        if (GetPos().y > beforePos.y) transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(0f, 0f, offsetAngle), 0.5f);
        else transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(0f, 0f, -offsetAngle), 0.5f);

    }

    private IEnumerator SetDest()
    {
        float boundrayYMin = -(boundaryY * 2f - 1);
        float boundrayYMax = (boundaryY * 2f + 1);
        while (true)
        {


            yield return new WaitForSeconds(0.4f);
            if (GetPos().x <= -7.4f && GetPos().y < -27)
            {
                fishSp.DestroyCurFish();
                Destroy(gameObject);
            }
            else if (GetPos().x <= -7.5f && GetPos().y > -27)
            {
                ranNext1Y = Random.Range(boundrayYMin, boundrayYMax);
                fishNext1Pos = fishNext1Point;
                fishNext1Pos.x = 0.5f;
                fishNext1Pos.y += (ranNext1Y * 0.1f);
                agent.SetDestination(fishNext1Pos);
            }
            else if (Vector3.Distance(GetPos(), fishNext1P) < 1f)
            {
                ranStart2Y = Random.Range(boundrayYMin, boundrayYMax);
                fishNext2P = fishNext2Point;
                fishNext2P.y += (ranStart2Y * 0.1f);
                GetComponent<SkeletonAnimation>().skeleton.FlipX = true;
                agent.SetDestination(fishNext2P);
            }
            else if (Vector3.Distance(GetPos(), fishNext1Pos) < 1f)
            {
                ranEnd1Y = Random.Range(boundrayYMin, boundrayYMax);
                fishNext1P = fishNext1Point;
                fishNext1P.y += (ranEnd1Y * 0.1f);
                agent.SetDestination(fishNext1P);

            }
            else if (Vector3.Distance(GetPos(), fishNext2Pos) < 1f)
            {
                ranEnd2Y = Random.Range(boundrayYMin, boundrayYMax);
                fishEndV = fishEndPoint;
                fishEndV.y += (ranEnd2Y * 0.1f);
                agent.SetDestination(fishEndV);
            }
            else if (Vector3.Distance(GetPos(), fishNext2P) < 1f)
            {
                ranNext2Y = Random.Range(boundrayYMin, boundrayYMax);
                fishNext2Pos = fishNext2Point;
                fishNext2Pos.x = -0.5f;
                fishNext2Pos.y += (ranNext2Y * 0.1f);
                agent.SetDestination(fishNext2Pos);
            }

            beforePos = GetPos();

        }

    }

    private Vector3 GetPos()
    {
        return transform.position;
    }

    public void SetBoundary(int _x, int _y, Vector3 _spawnPos, Vector3 _fishNext1Point, Vector3 _fishNext2Point, Vector3 _fishEndPoint)
    {
        boundaryX = _x;
        boundaryY = _y;
        spawnPos = _spawnPos;
        fishNext1Point = _fishNext1Point;
        fishNext2Point = _fishNext2Point;
        fishEndPoint = _fishEndPoint;
    }

    public void SetSpawner(FishSpawner _fishSpawner)
    {
        fishSp = _fishSpawner;
    }

}
