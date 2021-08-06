using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private const string adUnitID = "ca-app-pub-3940256099942544/5224354917";
    private RewardedAd rewardedAd;

    private void Start()
    {
        MobileAds.Initialize(status => { });
        rewardedAd = new RewardedAd(adUnitID);
        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        var request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        StartCoroutine(TryShowRewardedAd());
    }

    private IEnumerator TryShowRewardedAd()
    {
        while (!rewardedAd.IsLoaded()) yield return null;
        rewardedAd.Show();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToLoad event received with message: "
            + args.LoadAdError);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
            + args.AdError);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        var type = args.Type;
        var amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
            + amount + " " + type);
    }
}
