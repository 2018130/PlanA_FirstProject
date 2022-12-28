using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    private FishSpawner fishSp = null;
    private Vector3 fishEndV = new Vector3(-5.1f, -1f, 0f);
    private Vector3 FishNext1P = new Vector3(5.3f, 0f, 0f);
    private Vector3 FishNext2P = new Vector3(5.3f, -2.5f, 0f);
    private Vector3 beforePos = new Vector3(0f, 0f, 0f);

    private float ranEnd1Y = 0f;
    private float ranStart2Y = 0f;
    private float ranNext2Y = 0f;
    private float ranEnd2Y = 0f;
    private float offsetAngle = 1f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        fishSp = GameObject.FindWithTag("FishSpawner").GetComponent<FishSpawner>();
    }

    private void Start()
    {
        StartCoroutine("SetDest");
    }

    private void Update()
    {

        if (GetPos().y > beforePos.y) transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(0f, 0f, offsetAngle), 0.5f);
        else transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(0f, 0f, -offsetAngle), 0.5f);

    }

    private IEnumerator SetDest()
    {
        while (true)
        {


            yield return new WaitForSeconds(0.4f);
            if (GetPos().x <= -4.0f && GetPos().y < -1)
            {
                fishSp.DestroyCurFish();
                Destroy(gameObject);
            }
            else if (GetPos().x <= -0.5f && GetPos().y > -1)
            {
                ranEnd1Y = Random.Range(1, 25);
                if (ranEnd1Y < 11) FishNext1P.y = (ranEnd1Y * 0.1f);
                else FishNext1P.y = ((ranEnd1Y - 11) * 0.1f);
                agent.SetDestination(FishNext1P);
            }
            else if (GetPos().x >= 3.8f && GetPos().y > -1)
            {
                ranStart2Y = Random.Range(1, 25);
                if (ranStart2Y < 11) FishNext2P.y = (ranStart2Y * 0.1f - 3f);
                else FishNext2P.y = ((ranStart2Y - 11) * 0.1f - 3f);
                GetComponent<SpriteRenderer>().flipX = true;
                agent.SetDestination(FishNext2P);
            }
            else if (GetPos().x >= 3.8f && GetPos().y < -1)
            {
                ranNext2Y = Random.Range(-5, 30);
                FishNext2P.x = 0f;
                if (ranNext2Y < 11) FishNext2P.y = (ranNext2Y * 0.1f - 3f);
                else FishNext2P.y = ((ranNext2Y - 11) * 0.1f - 3f);
                agent.SetDestination(FishNext2P);
            }
            else if (GetPos().x <= 0.5f && GetPos().y < -1)
            {
                ranEnd2Y = Random.Range(-5, 30);
                if (ranEnd2Y < 11) fishEndV.y = (ranEnd2Y * 0.1f - 3f);
                else fishEndV.y = ((ranEnd2Y - 11) * 0.1f - 3f);
                agent.SetDestination(fishEndV);
            }
            beforePos = GetPos();

        }

    }
    
    private Vector3 GetPos()
    {
        return transform.position;
    }


}
