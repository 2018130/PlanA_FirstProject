using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        Invoke("DestroyShootingStar", skeletonAnimation.skeleton.Data.FindAnimation("animation").Duration);
    }


    void DestroyShootingStar()
    {
        Destroy(gameObject);
    }
}
