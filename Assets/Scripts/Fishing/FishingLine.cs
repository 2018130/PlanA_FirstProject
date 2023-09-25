using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField]
    GameObject fishingFloats;
    LineRenderer lineRenderer;
    const float maxLineAcc = 15f;
    float lineAcc = 0f;
    float lineLenth = 15.0f;
    float maxDegree = 60;
    float degree = 30;
    int sign = 1;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        fishingFloats = transform.GetChild(0).gameObject;

        lineRenderer.positionCount = 2;
        Vector3 startPoint = new Vector3(100f, 12f, 0);
        lineRenderer.SetPosition(0, startPoint);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 endPoint = lineLenth * new Vector3(Mathf.Cos((degree + 270 - maxDegree / 2)* Mathf.PI / 180),
            Mathf.Sin((degree + 270 - maxDegree / 2) * Mathf.PI / 180), 0) + lineRenderer.GetPosition(0);

        lineRenderer.SetPosition(1, endPoint);
        fishingFloats.transform.position = endPoint;

        if (Mathf.Abs(degree) > maxDegree)
        {
            sign = -1;
        }else if(degree < 0)
        {
            sign = 1;
        }

        degree += sign * lineAcc * Time.deltaTime;

#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            sign *= -1;
            lineAcc = 10;
        }
#elif UNITY_ANDROID        
        if(Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            sign *= -1;
            lineAcc = 10;
        }
#endif
        if(lineAcc < maxLineAcc)
        {
            float lineAccWeight = 5.0f;
            lineAcc += Time.deltaTime * lineAccWeight;
        }
    }
}
