using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobAdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    string adUnitId;

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            //ÃÊ±âÈ­ ¿Ï·á
        });

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        LoadRewardedAd();
    }

    public void LoadRewardedAd() //±¤°í ·Îµå ÇÏ±â
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });
    }

    public void ShowAd() //±¤°í º¸±â
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                //º¸»ó È¹µæÇÏ±â
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                Time.timeScale = 0;
            });
        }
        else
        {
            LoadRewardedAd();
        }
    }

    private void RegisterReloadHandler(RewardedAd ad) //±¤°í Àç·Îµå
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += (null);
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }
}
