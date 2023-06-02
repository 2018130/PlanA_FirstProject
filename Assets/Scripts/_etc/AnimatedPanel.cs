using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimatedPanel : MonoBehaviour
{
    SkeletonAnimation questCatSkeletonAnim;

    GameObject background;

    private void Awake()
    {
        Transform questCat = transform.Find("PanelCat");
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
        if (SceneManager.GetActiveScene().name.Contains("Share"))
        {
            GameObject.Find("SharehouseCat").GetComponent<SharehouseCat>().AnyPaenlOpened();
        }
        PlayClapAnimation();
    }

    public void DeactiveToViewport()
    {
        if (SceneManager.GetActiveScene().name.Contains("Share"))
        {
            GameObject.Find("SharehouseCat").GetComponent<SharehouseCat>().AnyPaenlClosed();
        }
        gameObject.SetActive(false);
    }
}
