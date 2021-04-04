using System;
using System.Collections.Generic;
using UnityEngine;
using static GameConfigContainer;
public static class Progression
{
    public static float GetBossfightProgression()
    {
        return GetLevelProgression(SaveManager.Instance.SavedValues.BossfightLevel, 6);
    }
    public static float GetCampainProgression()
    {
        return GetLevelProgression(SaveManager.Instance.SavedValues.CampainLevel, 1);
    }
    public static float GetLevelProgression(int level, float coef)
    {
        return Mathf.Pow(1.2f, level * coef) * GetSin(0.5f * level, 0.6f, 1);
    }
    public static float GetStatUpgradeCostProgression(int statLevel)
    {
        return Mathf.Pow(1.25f, statLevel + 7) / 6;
    }
    public static float GetStatAmountProgression(int statLevel)
    {
        return GetLevelProgression(statLevel, 1);
    }
    public static float GetSin(float x, float o, float i)
    {
        return Mathf.Pow(Mathf.Sin(x), 2) * o + i;
    }
    static float InverseFuncPoint(float level, float max, float min)
    {
        return (level / (level + 1 / 0.01f)) * (max - min) + min;
    }
    public static float GetBonusProbabilityProgression()
    {
        return InverseFuncPoint(SaveManager.Instance.SavedValues.BonusProbabilityUpgradeLevel, 5, 1);
    }
    public static float GetBulletsPerSecondProgression()
    {
        return InverseFuncPoint(SaveManager.Instance.SavedValues.BulletsPerSecondUpgradeLevel, 4, 1);
    }
    public static float GetBulletsSpeedProgression()
    {
        return InverseFuncPoint(SaveManager.Instance.SavedValues.BulletsPerSecondUpgradeLevel, 2, 1);
    }
    public static float GetCannonMoveForceProgression()
    {
        return InverseFuncPoint(SaveManager.Instance.SavedValues.CannonMoveForceUpgradeLevel, 3, 1);
    }
}
