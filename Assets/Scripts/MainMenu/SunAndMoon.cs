using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAndMoon : MonoBehaviour
{
    GameObject sun;
    GameObject moon;

    float dayTime = 3f;
    float nightTime = 3f;

    private void Start()
    {
        sun = gameObject;
        moon = transform.parent.Find("Moon").gameObject;

        StartCoroutine(C_SunRising());
    }

    IEnumerator C_SunRising()
    {
        sun.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        moon.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(dayTime);

        StartCoroutine(C_MoonRising());
    }

    IEnumerator C_MoonRising()
    {
        sun.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        moon.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(nightTime);

        StartCoroutine(C_SunRising());
    }
}
