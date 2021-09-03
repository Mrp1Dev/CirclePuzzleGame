using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : Singleton<AdManager>
{

    private const string testAdUnitID = "ca-app-pub-3940256099942544/5224354917";
    private const string rewardedAdUnitID = "ca-app-pub-3940256099942544/5224354917";
    private const string interstitialAdUnitID = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] private GameObject adLoadingPanel;
    [SerializeField] private GameObject puzzleLostPanel;
    [SerializeField] private GameObject endlessPuzzleLostPanel;
    [SerializeField] private float extraTimeOnRevival;
    private bool adFailedToLoad;
    private AdType currentRunningAd;
    private InterstitialAd interstitial;
    private bool puzzleRunningAfterInterstitial;

    private RewardedAd rewardedAd;
    private bool rewardedAdFailedToLoad;

    private bool adsInitialized = false;
    [field: SerializeField] public int CoinIncreaseOnWatch { get; private set; }

    private void Start()
    {
        List<string> deviceIds = new List<string>() { AdRequest.TestDeviceSimulator };

        deviceIds.Add("21EC2B006E2B12A4D98062ADEDA39B00");

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        MobileAds.Initialize(status => InitAds());
    }

    private void InitAds()
    {
        rewardedAd = new RewardedAd(rewardedAdUnitID);

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

        interstitial = new InterstitialAd(interstitialAdUnitID);

        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;

        RequestRewardedAd();
        RequestAd();
        adsInitialized = true;
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        Player.Instance.PuzzleRunning = puzzleRunningAfterInterstitial;
        RequestAd();
    }

    private void HandleOnAdOpened(object sender, EventArgs e)
    {
        Player.Instance.PuzzleRunning = false;
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        print("interstitial ad failed to load! " + e.LoadAdError);
        adFailedToLoad = true;
    }

    private IEnumerator TryShowRewardedAd(AdType adType)
    {
        adLoadingPanel.SetActive(true);
        while (!rewardedAd.IsLoaded())
        {
            if (rewardedAdFailedToLoad)
            {
                adLoadingPanel.SetActive(false);
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }

        rewardedAd.Show();
        adLoadingPanel.SetActive(false);
        currentRunningAd = adType;
    }

    private IEnumerator TryShowInterstitialAd()
    {
        while (!interstitial.IsLoaded())
        {
            if (adFailedToLoad) yield break;

            yield return null;
        }

        interstitial.Show();
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleRewardedAdFailedToLoad event received with message: " + args.LoadAdError);
        rewardedAdFailedToLoad = true;
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
        RequestRewardedAd();
    }

    private void RequestRewardedAd()
    {
        var request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    private void RequestAd()
    {
        var request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        print($"{nameof(HandleUserEarnedReward)} was called.");
        switch (currentRunningAd)
        {
            case AdType.Revival:
                if (PuzzleCycler.Instance.EndlessMode) endlessPuzzleLostPanel.SetActive(false);
                else puzzleLostPanel.SetActive(false);
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
    private void TryReloadRewardedAd()
    {
        if (!rewardedAdFailedToLoad) return;
        rewardedAdFailedToLoad = false;
        RequestRewardedAd();
    }

    private void TryReloadAd()
    {
        if (!adFailedToLoad) return;
        adFailedToLoad = false;
        RequestAd();
    }

    public void OnReviveWanted()
    {
        print($"{nameof(OnReviveWanted)} was called, were ads initialized yet?: {adsInitialized}");
        TryReloadRewardedAd();
        StartCoroutine(TryShowRewardedAd(AdType.Revival));
    }

    public void OnCoinsWanted()
    {
        print($"{nameof(OnCoinsWanted)} was called, were ads initialized yet?: {adsInitialized}");
        TryReloadRewardedAd();
        StartCoroutine(TryShowRewardedAd(AdType.Coins));
    }
    
    public void OnTestRewardedAdWanted()
    {
        print($"{nameof(OnTestRewardedAdWanted)} was called, were ads initialized yet?: {adsInitialized}");
        TryReloadRewardedAd();
        StartCoroutine(TryShowRewardedAd(AdType.None));

    }

    public void OnInterstitialWanted(bool puzzleRunningAfterAd = false)
    {
        print($"{nameof(OnInterstitialWanted)} was called, were ads initialized yet?: {adsInitialized}");
        puzzleRunningAfterInterstitial = puzzleRunningAfterAd;
        TryReloadAd();
        StartCoroutine(TryShowInterstitialAd());
    }

    private enum AdType
    {
        None,
        Revival,
        Coins
    }
}
