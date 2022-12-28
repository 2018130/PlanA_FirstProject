using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingHookController : MonoBehaviour
{
    [SerializeField] private Transform fishingLine = null;
    [SerializeField] private Transform fishingHook = null;

    [SerializeField] private float speed = 3.0f;
    private LineRenderer lr = null;
    private new Camera camera = null;
    private Vector3 destPos = new Vector3(0f, 5f, 0f);



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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            destPos = camera.ScreenToWorldPoint(mousePos);
            destPos.z = 0f;
        }

        if (Vector3.Distance(destPos, GetPos()) >= 0.01f)
        {
            Vector3 dirVector = (destPos - GetPos());
            dirVector.z = 0f;
            transform.Translate(dirVector * Time.deltaTime * speed);

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
