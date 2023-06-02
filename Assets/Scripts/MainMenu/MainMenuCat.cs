using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

public class MainMenuCat : MonoBehaviour
{
    float timer = 0f;
    float animationLatency = 5.0f;

    // Update is called once per frame
    void Update()
    {
        //랜덤 애니메이션 재생
        if (timer >= animationLatency)
        {
            PlayRandomAnimation();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    private void PlayRandomAnimation()
    {
        SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();

        if (skeletonAnimation != null)
        {
            Array animationNames = skeletonAnimation.skeleton.Data.Animations.ToArray();
            string newAnimationName = animationNames.GetValue(UnityEngine.Random.Range(0, animationNames.Length)).ToString();
            if (newAnimationName != "Fishing" && newAnimationName != "Sleep")
            {
                SetAnimation(newAnimationName);
            }
        }
    }

    public float SetAnimation(string action)
    {
        SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();

        if (skeletonAnimation.skeleton.Data.FindAnimation(action) != null)
        {
            float duration = skeletonAnimation.skeleton.Data.FindAnimation(action).Duration;
            skeletonAnimation.AnimationName = action;

            return duration;
        }

        return 0;
    }
}
