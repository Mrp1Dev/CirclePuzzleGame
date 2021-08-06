using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : Singleton<AdManager>
{
    private enum AdType
    {
        None,
        Revival,
        Coins
    }

    [SerializeField] private GameObject adLoadingPanel;
    [SerializeField] private GameObject puzzleLostPanel;
    [SerializeField] private float extraTimeOnRevival;
    [field: SerializeField] public int CoinIncreaseOnWatch { get; private set; }

    private const string adUnitID = "ca-app-pub-3940256099942544/5224354917";
    private bool adFailedToLoad;
    private RewardedAd rewardedAd;
    private AdType currentRunningAd;

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
        RequestAd();
    }

    private IEnumerator TryShowRewardedAd(AdType adType)
    {
        adLoadingPanel.SetActive(true);
        while (!rewardedAd.IsLoaded())
        {
            if (adFailedToLoad)
            {
                adLoadingPanel.SetActive(false);
                yield break;
            }

            yield return null;
        }

        rewardedAd.Show();
        adLoadingPanel.SetActive(false);
        currentRunningAd = adType;
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleRewardedAdFailedToLoad event received with message: " + args.LoadAdError);
        adFailedToLoad = true;
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
            + args.AdError);
        currentRunningAd = AdType.None;
        adLoadingPanel.SetActive(false);
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardedAdClosed event received");
        adLoadingPanel.SetActive(false);
        RequestAd();
    }

    private void RequestAd()
    {
        var request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        switch (currentRunningAd)
        {
            case AdType.Revival:
                puzzleLostPanel.SetActive(false);
                Player.Instance.ResetValues(extraTimeOnRevival);
                break;
            case AdType.Coins when Player.Instance == null:
                PlayerPrefs.SetInt(PlayerPrefsKeys.CoinsKey,
                    PlayerPrefs.GetInt(PlayerPrefsKeys.CoinsKey, 0) + CoinIncreaseOnWatch);
                break;
            case AdType.Coins:
                Player.Instance.Coins += CoinIncreaseOnWatch;
                break;
            default:
                return;
        }
    }

    public void OnReviveWanted()
    {
        TryReloadAd();
        StartCoroutine(TryShowRewardedAd(AdType.Revival));
    }

    private void TryReloadAd()
    {
        if (!adFailedToLoad) return;
        adFailedToLoad = false;
        RequestAd();
    }

    public void OnCoinsWanted()
    {
        TryReloadAd();
        StartCoroutine(TryShowRewardedAd(AdType.Coins));
    }
}
