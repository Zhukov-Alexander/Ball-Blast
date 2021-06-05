using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] LevelProgression levelProgression;
    [SerializeField] GameObject lastChanceMenu;
    [SerializeField] GameObject resultsMenu;
    public static Action OnStartCampain { get; set; }
    public static Action OnStartBossfight { get; set; }
    public static Action OnEndCampainWin { get; set; }
    public static Action OnEndCampainLose { get; set; }
    public static Action OnEndBossfightWin { get; set; }
    public static Action OnEndBossfightLose { get; set; }
    private void Awake()
    {
        OnEndCampainWin += () => TaskActiones.Instance.WinLevels(1);
        OnEndCampainWin += () => Instantiate(resultsMenu, GetComponentInParent<Canvas>().transform, false).GetComponent<ResultsPanel>().SetCampainWin();
        OnEndCampainLose += () => Instantiate(resultsMenu, GetComponentInParent<Canvas>().transform, false).GetComponent<ResultsPanel>().SetCampainLose();
        OnEndBossfightWin += () => Instantiate(resultsMenu, GetComponentInParent<Canvas>().transform, false).GetComponent<ResultsPanel>().SetBossfightWin();
        OnEndBossfightLose += () => Instantiate(resultsMenu, GetComponentInParent<Canvas>().transform, false).GetComponent<ResultsPanel>().SetBossfightLose();

        OnEndCampainWin += () => SaveManager.Instance.SavedValues.CampainLevel++;
        OnEndCampainWin += () => Social.ReportScore(SaveManager.Instance.SavedValues.CampainLevel, "CgkI-u2t7t4eEAIQAQ", (a) => { });
        OnEndCampainLose += () => PlayGamesPlatform.Instance.IncrementAchievement("CgkI-u2t7t4eEAIQAA", 1, (bool success) => { });
        OnEndBossfightLose += () => PlayGamesPlatform.Instance.IncrementAchievement("CgkI-u2t7t4eEAIQAA", 1, (bool success) => { });
        OnEndBossfightWin += () => SaveManager.Instance.SavedValues.BossfightLevel++;

        AddToEnd(() => SecondChanceMenu.isTaken = false);
        AddToEnd(() => UIAnimation.Close(gameObject));
        BallSpawnersManager.OnLevelCalculated += levelProgression.SetCampain;
        BossManager.OnBossInstantiated += levelProgression.SetBossfight;
        Ball.OnTakeDamage += levelProgression.ChangePointsBall;
        Boss.OnTakeDamage += levelProgression.ChangePointsBoss;
        Cannon.OnLostAllLives += () => Instantiate(lastChanceMenu, GetComponentInParent<Canvas>().transform, false);
    }
    private void OnEnable()
    {
        if (LevelModManager.CurrentLevelMod == LevelMod.Campain)
        {
            LevelMenu.OnStartCampain();
        }
        else if (LevelModManager.CurrentLevelMod == LevelMod.Bossfight)
        {
            LevelMenu.OnStartBossfight();
        }
    }
    public static void AddToStart(Action action)
    {
        OnStartCampain += action;
        OnStartBossfight += action;
    }
    public static void AddToEnd(Action action)
    {
        OnEndCampainWin += action;
        OnEndCampainLose += action;
        OnEndBossfightWin += action;
        OnEndBossfightLose += action;
    }
    internal static void RemoveFromStart(Action action)
    {
        OnStartCampain -= action;
        OnStartBossfight -= action;
    }

    internal static void RemoveFromEnd(Action action)
    {
        OnEndCampainWin -= action;
        OnEndCampainLose -= action;
        OnEndBossfightWin -= action;
        OnEndBossfightLose -= action;
    }

}