using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EFishType
{
    C,
    B,
    A
}

public class Fish : MonoBehaviour
{
    public static List<Dictionary<string, float>> fishTypeInfo = new List<Dictionary<string, float>>()
    {
        new Dictionary<string, float>()
        {
            {"speed", 0.4f },
            {"waveLengthRate", 0.4f },
        },
        new Dictionary<string, float>()
        {
            {"speed", 0.8f },
            {"waveLengthRate", 0.8f },
        },
        new Dictionary<string, float>()
        {
            {"speed", 1.2f },
            {"waveLengthRate", 1.2f },
        }
    };

    [Header("FishStat")]
    [SerializeField]
    float speed = 0.5f;
    //물고기 움직임의 파장과 관련된 변수, 파장과 아래 변수는 반비례 관계
    [SerializeField]
    float waveLengthRate = 0.5f;

    float time = 0f;
    Vector3 velocity = Vector3.left;
    float angleGapFromLeftAxis = 0f;
    EFishType eFishType = EFishType.C;
    public EFishType PFishType{
        get
        {
            return eFishType;
        }
    }
    float waitTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        //raycast
        int raySize = 5;
        float anglePerRay = 15;
        List<RaycastHit2D> raycastHit2Ds = new List<RaycastHit2D>();
        for (int i = -raySize / 2; i <= raySize / 2; i++)
        {
            float rayAngle = 180 + angleGapFromLeftAxis + (i * anglePerRay);
            Vector3 rayDirVector = new Vector3(Mathf.Cos(rayAngle * Mathf.PI / 180), Mathf.Sin(rayAngle * Mathf.PI / 180), 0);
            int layerMask = (1 << LayerMask.NameToLayer("Fish"));
            Debug.DrawRay(transform.position, rayDirVector, Color.red);
            raycastHit2Ds.Add(Physics2D.Raycast(transform.position, rayDirVector, 1.0f, layerMask));
        }

        //rayhit
        for(int i = 0; i < raycastHit2Ds.Count; i++)
        {
            if (raycastHit2Ds[i].collider != null)
            {
                GameObject targetObj = raycastHit2Ds[i].collider.gameObject;

                if (eFishType != targetObj.GetComponent<Fish>().eFishType)
                {
                    float fishSlowRate = 0.8f;
                    speed *= fishSlowRate;
                    StopAllCoroutines();
                    StartCoroutine("GetBackSpeed");
                }
            }
        }

        //change transform
        if (time >= 10 * Mathf.PI) time = 0;
        time += Time.deltaTime * waveLengthRate;
        velocity = new Vector3(-10, 10 * Mathf.Cos(time), 0).normalized;

        transform.position += velocity * speed * Time.deltaTime;
        int sign = (velocity.y > 0) ? -1 : 1;
        angleGapFromLeftAxis = sign * Vector3.Angle(Vector3.left, velocity);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleGapFromLeftAxis));
    }

    public void SetFishStat(EFishType _eFishType)
    {
        speed = fishTypeInfo[(int)_eFishType]["speed"];
        waveLengthRate = fishTypeInfo[(int)_eFishType]["waveLengthRate"];
        eFishType = _eFishType;
    }

    IEnumerator GetBackSpeed()
    {
        yield return new WaitForSeconds(waitTime);

        SetFishStat(eFishType);
    }
    /*
    //PIE모드에서 씬뷰의 카메라에도 렌더링 되면 함수가 호출됨. 씬뷰에서 생성된 오브젝트
    //가 보이면 안됨
    private void OnBecameInvisible()
    {
        FishSpawner.ReturnFish(gameObject);
    }*/
}
