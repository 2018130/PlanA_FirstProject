using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAndMoon : MonoBehaviour
{
    GameObject sun;
    GameObject moon;

    float dayTime = 55f;
    float nightTime = 81f;

    private void Start()
    {
        sun = gameObject;
        moon = transform.parent.Find("Moon").gameObject;

        StartCoroutine(C_SunRising());
    }

    IEnumerator C_SunRising()
    {
        sun.SetActive(true);
        moon.SetActive(false);

        yield return new WaitForSeconds(dayTime);

        StartCoroutine(C_MoonRising());
    }

    IEnumerator C_MoonRising()
    {
        sun.SetActive(false);
        moon.SetActive(true);

        yield return new WaitForSeconds(nightTime);

        StartCoroutine(C_SunRising());
    }
}
