using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    SkeletonAnimation questCatSkeletonAnim;

    GameObject background;

    private void Awake()
    {
        Transform questCat = transform.Find("QuestCat");
        if (questCat != null)
        {
            questCatSkeletonAnim = questCat.gameObject.GetComponent<SkeletonAnimation>();
        }

        background = transform.Find("Background").gameObject;
    }

    public void PlayClapAnimation()
    {
        if(questCatSkeletonAnim == null)
        {
            return;
        }

        if(questCatSkeletonAnim.AnimationName == "Action")
        {
            questCatSkeletonAnim.AnimationName = "Idle";
        }

        questCatSkeletonAnim.AnimationName = "Action";
    }

    public void ActiveToViewport()
    {
        gameObject.SetActive(true);
        PlayClapAnimation();
    }

    public void DeactiveToViewport()
    {
        gameObject.SetActive(false);
    }
}
