using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class CatchFish : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DeactiveToViewport();
        }
    }
    public void ActiveToViewport(AgentMovement catchedFish)
    {
        GetComponent<SkeletonAnimation>().AnimationName = "animation";

        transform.GetChild(0).Find("FishName").GetComponent<Text>().text = catchedFish.GetFishData().GetName();
        transform.GetChild(0).Find("FishInformation").GetComponent<Text>().text = catchedFish.GetFishData().GetInformation();

        GameObject fishImage = transform.Find("FishImage").gameObject;
        if (fishImage != null) {
            SkeletonAnimation skeletonAnimation = fishImage.GetComponent<SkeletonAnimation>();
            fishImage.GetComponent<MeshRenderer>().sortingOrder = 5;
            skeletonAnimation.skeletonDataAsset = Resources.Load<SkeletonDataAsset>(catchedFish.GetFishData().GetSpinePath());
            skeletonAnimation.AnimationName = "None";
        }

        gameObject.SetActive(true);
        //Time.timeScale = 0;
    }

    public void DeactiveToViewport()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
