using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinkleStar : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        Invoke("DestroyTwinkleStar", skeletonAnimation.skeleton.Data.FindAnimation("animation").Duration);
    }


    void DestroyTwinkleStar()
    {
        Destroy(gameObject);
    }
}
