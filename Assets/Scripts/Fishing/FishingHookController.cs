using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FishingHookController : MonoBehaviour
{
    [SerializeField] private Transform fishingLine = null;
    [SerializeField] private Transform fishingHook = null;

    [SerializeField] private float speed = 35f;
    private LineRenderer lr = null;
    private new Camera camera = null;
    private Vector3 destPos = new Vector3(0f, -28f, 0f);

    [SerializeField] private PlayerController playerController;


    private void Start()
    {
        camera = Camera.main;
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, fishingHook.position);
        //lr.SetPosition(0, GetPos());
        lr.SetPosition(1, fishingLine.position);
    }

    private void Update()
    {
        float distance, topPosY = -15f, bottomPosY = -63f;
        float leftPosX = -13f, RightPosX = 13f;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
            xy.Raycast(ray, out distance);
            destPos = ray.GetPoint(distance) + new Vector3(0, 2, 0);

            if (bottomPosY > destPos.y || destPos.y > topPosY ||
                leftPosX > destPos.x || RightPosX < destPos.x)
            {
                destPos = transform.position;
            }
        }
//#else
        //화면 터치시 마지막 터치 위치로 이동
        if (Input.touchCount != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(Input.touchCount - 1).deltaPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
            xy.Raycast(ray, out distance);
            destPos = ray.GetPoint(distance);

            if (bottomPosY > destPos.y || destPos.y > topPosY ||
                leftPosX > destPos.x || RightPosX < destPos.x)
            {
                destPos = transform.position;
            }
        }
#endif
        if (Vector3.Distance(destPos, transform.position) >= 0.3f)
        {
            Vector3 dirVector = (destPos - transform.position).normalized;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + dirVector * Time.deltaTime * speed);
        }

        lr.SetPosition(0, fishingHook.position);
        //lr.SetPosition(0, GetPos());
        lr.SetPosition(1, fishingLine.position);
    }

    public void MoveDefault()
    {
        destPos = new Vector3(0f, -28f, 0f);
    }
    private Vector3 GetPos()
    {
        return transform.position;
    }

}
