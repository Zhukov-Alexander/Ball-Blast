using System;
using System.ArrayExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : Singleton<AdManager>, IUnityAdsListener
{
    string gameId = "4070212";
    bool testMode = true;
    public string RewardedVideoId { get; set; } = "rewardedVideo";
    public string VideoId { get; set; } = "video";

    public Action<ShowResult> OnAdFinished;

    DateTime lastTimeWatchedAd;
    TimeSpan timeBetweenAds = new TimeSpan(0, 3, 0);
    void Awake()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
        lastTimeWatchedAd = DateTime.Now;
    }
    public bool IsAdReady(string surfacingId)
    {
        return Advertisement.IsReady(surfacingId);
    }
    public bool IsTimeReady()
    {
        if ((DateTime.Now - lastTimeWatchedAd) >= timeBetweenAds)
        {
            return true;
        }
        else
            return false;
    }
    public void ShowAd(string surfacingId, Action<ShowResult> subscriber)
    {
        OnAdFinished += subscriber;
        if (Advertisement.IsReady(surfacingId))
        {
            lastTimeWatchedAd = DateTime.Now;
            Advertisement.Show(surfacingId);
        }
        else
        {
            OnUnityAdsDidFinish(surfacingId, ShowResult.Failed);
        }
    }

    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            OnAdFinished(ShowResult.Finished);
        }
        else
        {
            OnAdFinished(ShowResult.Failed);
        }
        Array.ForEach(OnAdFinished.GetInvocationList(), (a) => OnAdFinished -= (Action<ShowResult>)a);
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == RewardedVideoId)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
