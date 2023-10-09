using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBright: MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField]
    float brightSpeed = 0f;
    [SerializeField]
    float maxBright = 1f;
    [SerializeField]
    float minBright = 0f;
    [SerializeField]
    List<float> brightReverseTime;
    int nextBrightReverseTimeIndex = 0;
    bool isCoroutinePlaying = false;
    int brightDirection = 1;
    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color newColor = new Color(spriteRenderer.color.r + Time.deltaTime * brightDirection * brightSpeed,
            spriteRenderer.color.g + Time.deltaTime * brightDirection * brightSpeed,
            spriteRenderer.color.b + Time.deltaTime * brightDirection * brightSpeed);

        if(newColor.r <= maxBright && newColor.r >= minBright)
        {
            spriteRenderer.color = newColor;
        }

        if(time >= brightReverseTime[nextBrightReverseTimeIndex])
        {
            ReverseBright();
        }

        time += Time.deltaTime;
    }

    void ReverseBright()
    {
        brightDirection *= -1;
        if (nextBrightReverseTimeIndex < brightReverseTime.Count - 1)
        {
            nextBrightReverseTimeIndex++;
        }
        else
        {
            nextBrightReverseTimeIndex = 0;
        }
        time = 0;
    }
}
