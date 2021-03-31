using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        OnEndCampainWin += () => Instantiate(resultsMenu, GetComponentInParent<Canvas>().transform, false).GetComponent<ResultsPanel>().SetCampainWin();
        OnEndCampainLose += () => Instantiate(resultsMenu, GetComponentInParent<Canvas>().transform, false).GetComponent<ResultsPanel>().SetCampainLose();
        OnEndBossfightWin += () => Instantiate(resultsMenu, GetComponentInParent<Canvas>().transform, false).GetComponent<ResultsPanel>().SetBossfightWin();
        OnEndBossfightLose += () => Instantiate(resultsMenu, GetComponentInParent<Canvas>().transform, false).GetComponent<ResultsPanel>().SetBossfightLose();

        OnEndCampainWin += () => SavedValues.Instance.CampainLevel++;
        OnEndBossfightWin += () => SavedValues.Instance.BossfightLevel++;

        AddToStart(() => GetComponentsInChildren<Animator>().ToList().ForEach(a=> a.SetTrigger("Open")));
        AddToEnd(() => GetComponentsInChildren<Animator>().ToList().ForEach(a=> a.SetTrigger("Close")));
        AddToEnd(() => LastChanceMenu.isTaken = false);
        BallSpawnersManager.OnLevelCalculated += levelProgression.SetCampain;
        BossManager.OnBossInstantiated += levelProgression.SetBossfight;
        Ball.OnTakeDamage += levelProgression.ChangePointsBall;
        Boss.OnTakeDamage += levelProgression.ChangePointsBoss;
        Cannon.OnLostAllLives += () => Instantiate(lastChanceMenu, GetComponentInParent<Canvas>().transform, false);
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