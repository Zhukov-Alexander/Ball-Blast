using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static GameConfigContainer;

public class LevelProgression : MonoBehaviour
{
    [SerializeField] LevelSlider levelProgressionSlider;
    public static float currentPoints;
    public static float maxPoints;
    Sequence sequence;
    bool locked;

    public void SetCampain()
    {
        locked = false;
        sequence = DOTween.Sequence();
        currentPoints = 0;
        maxPoints = BallSpawnersManager.totalLevelLives;
        levelProgressionSlider.SetLevelCampainNumbers();
        levelProgressionSlider.SetCampainIcon();
        levelProgressionSlider.SetSlider(maxPoints, currentPoints);
    }
    public void SetBossfight()
    {
        locked = false;
        sequence = DOTween.Sequence();
        currentPoints = BossManager.Boss.InitialLives;
        maxPoints = BossManager.Boss.InitialLives;
        levelProgressionSlider.SetLevelBossNumbers();
        levelProgressionSlider.SetBossfightIcon();
        levelProgressionSlider.SetSlider(maxPoints, currentPoints);
    }
    public void ChangePointsBall(float points)
    {
        sequence.Append(levelProgressionSlider.TweenSlider(maxPoints, currentPoints, currentPoints + points, points / maxPoints * 5f));
        currentPoints += points;
        if ((currentPoints > maxPoints || HelperClass.NearlyEqual(currentPoints, maxPoints, maxPoints * gameConfig.epsilon)) && !locked)
        {
            locked = true;
            LevelMenu.OnEndCampainWin();
        }
    }
    public void ChangePointsBoss(float points)
    {
        sequence.Append(levelProgressionSlider.TweenSlider(maxPoints, currentPoints, currentPoints - points, points / maxPoints * 5f));
        currentPoints -= points;
        if ((currentPoints < 0 || HelperClass.NearlyEqual(currentPoints, 0, maxPoints * gameConfig.epsilon)) && !locked)
        {
            locked = true;
            LevelMenu.OnEndBossfightWin();
        }
    }
}
