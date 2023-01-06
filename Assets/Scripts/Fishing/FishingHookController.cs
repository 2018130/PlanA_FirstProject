using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingHookController : MonoBehaviour
{
    [SerializeField] private Transform fishingLine = null;
    [SerializeField] private Transform fishingHook = null;

    [SerializeField] private float speed = 15f;
    private LineRenderer lr = null;
    private new Camera camera = null;
    private Vector3 destPos = new Vector3(0f, -15f, 0f);



    private void Start()
    {
        camera = Camera.main;
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, fishingHook.position);
        lr.SetPosition(1, GetPos());
        lr.SetPosition(2, fishingLine.position);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
            float distance;
            xy.Raycast(ray, out distance);
            destPos = ray.GetPoint(distance);
            if (destPos.y > -19f)
                destPos = transform.position;
        }

        if (Vector3.Distance(destPos, transform.position) >= 0.1f)
        {
            Vector3 dirVector = (destPos - transform.position).normalized;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + dirVector * Time.deltaTime * speed);
        }

        lr.SetPosition(0, fishingHook.position);
        lr.SetPosition(1, GetPos());
        lr.SetPosition(2, fishingLine.position);



    }

    private Vector3 GetPos()
    {
        return transform.position;
    }


}
