using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TwinklePanel : MonoBehaviour
{
    float brightSpeed = 0.5f;
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(text != null)
        {
            if(text.color.a >= 1)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }
            else
            {
                float newAValue = text.color.a + Time.deltaTime * brightSpeed;
                text.color = new Color(text.color.r, text.color.g, text.color.b, newAValue);
            }
        }
    }

}
