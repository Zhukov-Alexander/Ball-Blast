using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DG.Tweening;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public class SocialManager : Singleton<SocialManager>
{
    [SerializeField] GameObject startScreen;
    public bool isConnectedToGooglePlayServices;
    public Action OnAuthenticated;

    private void Awake()
    {
        PlayGamesClientConfiguration.Builder builder = new PlayGamesClientConfiguration.Builder();
        builder.EnableSavedGames();
        PlayGamesPlatform.InitializeInstance(builder.Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    private void Start()
    {
        SignIn(SignInInteractivity.CanPromptOnce, (status) => SaveManager.Instance.Load((result) => UIAnimation.Close(startScreen).AppendCallback(() => Destroy(startScreen))));
    }
    public void SignIn(SignInInteractivity signInInteractivity, Action<SignInStatus> callback = null)
    {
        PlayGamesPlatform.Instance.Authenticate(signInInteractivity, (result) =>
        {
            switch (result)
            {
                case SignInStatus.Success:
                    isConnectedToGooglePlayServices = true;
                    break;
                default:
                    isConnectedToGooglePlayServices = false;
                    break;
            }
            callback?.Invoke(result);
            OnAuthenticated?.Invoke();
        });   
    }
    public void SignOut()
    {
        if (Social.localUser.authenticated)
        {
            SaveManager.Instance.SaveCloud((savedGameRequestStatus) => PlayGamesPlatform.Instance.SignOut(),true);
            isConnectedToGooglePlayServices = false;
        }
    }
    public void Leaderboard()
    {
        Social.ShowLeaderboardUI();
    }
    public void Achievements()
    {
        Social.ShowAchievementsUI();
    }
}
