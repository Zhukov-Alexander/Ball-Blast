using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LastChanceMenu : MonoBehaviour
{
    [SerializeField] int chanceCost;
    [SerializeField] GameObject container;
    [SerializeField] GameObject takeChanceStone;
    public static Action OnLastChanceTaken { get; set; }
    public static bool isTaken = false;
    private void Awake()
    {
        ShowLastChanceMenu();
    }
    public void TakeChanceAd()
    {
        //watch ads
        isTaken = true;
        OnLastChanceTaken();
        HideLastChanceMenu();
    }
    public void TakeChanceStone()
    {
        SavedValues.Instance.Diamonds -= chanceCost;
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
        if (isTaken)
        {
            isTaken = false;
            if (LevelModManager.CurrentLevelMod == LevelMod.Bossfight)
            {
                LevelMenu.OnEndBossfightLose();
            }
            else
                LevelMenu.OnEndCampainLose();
            Destroy(gameObject);
        }
        else
        {
            SoundManager.Instance.Heartbeat();
            SoundManager.Instance.slow.TransitionTo(0.5f);
            UIAnimation.Open(gameObject).Play();
            if (SavedValues.Instance.Diamonds >= chanceCost)
            {
                takeChanceStone.SetActive(true);
            }
            else
            {
                takeChanceStone.SetActive(false);
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
