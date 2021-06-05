using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class SecondChanceMenu : MonoBehaviour
{
    [SerializeField] int chanceCost;
    [SerializeField] GameObject container;
    [SerializeField] GameObject takeChanceStone;
    [SerializeField] GameObject takeChanceAd;
    public static Action OnLastChanceTaken { get; set; }
    public static bool isTaken = false;
    private void Awake()
    {
        ShowLastChanceMenu();
        OnLastChanceTaken += () => TaskActiones.Instance.UseSecondChance(1);
    }
    public void TakeChanceAd()
    {
        AdManager.Instance.ShowAd(AdManager.Instance.RewardedVideoId, (result) =>
        {
            if (result == ShowResult.Finished)
            {
                isTaken = true;
                OnLastChanceTaken();
            }
            HideLastChanceMenu();
        });
    }
    public void TakeChanceStone()
    {
        SaveManager.Instance.SavedValues.Diamonds -= chanceCost;
        isTaken = true;
        OnLastChanceTaken();
        HideLastChanceMenu();
    }
    public void SkipChance()
    {
        isTaken = false;
        if (LevelModManager.CurrentLevelMod == LevelMod.Bossfight)
        {
            LevelMenu.OnEndBossfightLose();
        }
        else
            LevelMenu.OnEndCampainLose();

        HideLastChanceMenu();
    }

    private void ShowLastChanceMenu()
    {
        if (isTaken || (SaveManager.Instance.SavedValues.Diamonds < chanceCost && !AdManager.Instance.IsAdReady(AdManager.Instance.RewardedVideoId)))
        {
            if (LevelModManager.CurrentLevelMod == LevelMod.Bossfight)
            {
                LevelMenu.OnEndBossfightLose();
            }
            else
                LevelMenu.OnEndCampainLose();
            isTaken = false;
            Destroy(gameObject);
        }
        else
        {
            SoundManager.Instance.Heartbeat();
            SoundManager.Instance.slow.TransitionTo(0.5f);
            UIAnimation.Open(gameObject).Play();
            if (SaveManager.Instance.SavedValues.Diamonds >= chanceCost)
            {
                takeChanceStone.SetActive(true);
            }
            else
            {
                takeChanceStone.SetActive(false);
            }
            if (AdManager.Instance.IsAdReady(AdManager.Instance.RewardedVideoId))
            {
                takeChanceAd.SetActive(true);
            }
            else
            {
                takeChanceAd.SetActive(false);
            }
            container.SetActive(true);
            Time.timeScale = 0;
        }
    }
    private void HideLastChanceMenu()
    {
        UIAnimation.Close(gameObject).AppendCallback(() => Destroy(gameObject)).Play();
        SoundManager.Instance.Heartbeat(1,false);
        SoundManager.Instance.standard.TransitionTo(0.5f);
        Time.timeScale = 1;
    }
}
