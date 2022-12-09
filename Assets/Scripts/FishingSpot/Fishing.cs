using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fishing : MonoBehaviour
{
    [SerializeField]
    GameObject rod;

    GameObject rodBody;
    GameObject fishingFloat;
    private void Start()
    {
        rodBody = rod.transform.Find("Body").gameObject;
        fishingFloat = rod.transform.Find("FishingFloat").gameObject;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            float maxUpperRate = (float)3 / 4;
            float minLowerRate = (float)1 / 4;

            float horizontalSpeed = 5.0f;
            float verticalSpeed = 5.0f;

            float maxBodyLength = 6.5f;
            float maxRotationableAngle = 30f;

            //상
            if(mousePos.y >= Screen.height * maxUpperRate)
            {
                float newScaleY = rodBody.transform.localScale.y - Time.deltaTime * verticalSpeed;
                if (newScaleY >= 1)
                {
                    rodBody.transform.localScale = new Vector3(rodBody.transform.localScale.x,
                        newScaleY,
                        rodBody.transform.localScale.z);
                }
            }//하
            else if(mousePos.y <= Screen.height * minLowerRate)
            {
                float newScaleY = rodBody.transform.localScale.y + Time.deltaTime * verticalSpeed;
                if (newScaleY <= maxBodyLength)
                {
                    rodBody.transform.localScale = new Vector3(rodBody.transform.localScale.x,
                        newScaleY,
                        rodBody.transform.localScale.z);
                }
            }
            else if(mousePos.x <= (float)Screen.width / 2)
            {
                float newRodAngle = rod.transform.localEulerAngles.z - Time.deltaTime * verticalSpeed;
                float angleOffset = 5f;
                //0 -> -0.001... 으로 변환 될 때 자동으로 360도가 됨
                if(newRodAngle >= 360 - maxRotationableAngle - angleOffset && newRodAngle <= 360)
                {
                    newRodAngle = Mathf.Clamp(newRodAngle, 360 - maxRotationableAngle, 360);
                }
                rod.transform.localEulerAngles = new Vector3(0, 0, newRodAngle);
            }
            else if(mousePos.x > (float)Screen.width / 2)
            {
                float newRodAngle = rod.transform.localEulerAngles.z + Time.deltaTime * verticalSpeed;
                float angleOffset = 5f;
                //360 -> 360.001... 으로 변환 될 때 자동으로 0도가 됨
                if (newRodAngle >= 0 && newRodAngle <= maxRotationableAngle + angleOffset)
                {
                    newRodAngle = Mathf.Clamp(newRodAngle, 0, maxRotationableAngle);
                    Debug.Log(newRodAngle);
                }
                rod.transform.localEulerAngles = new Vector3(0, 0, newRodAngle);
            }

            fishingFloat.transform.position = rodBody.transform.Find("EndPoint").position;
        }
#endif

        if (Input.touchCount != 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                
            }
        }
    }
}
