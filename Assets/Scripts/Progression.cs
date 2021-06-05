using System;
using System.Collections.Generic;
using UnityEngine;
using static GameConfigContainer;
public static class Progression
{
    public static float GetBossfightProgression()
    {
        return GetLevelProgression(SaveManager.Instance.SavedValues.BossfightLevel, 7, false);
    }
    public static float GetCampainProgression(int levelsToCalculate = 1)
    {
        float value = new float();
        for (int i = 0; i < levelsToCalculate; i++)
        {
            value += GetLevelProgression(SaveManager.Instance.SavedValues.CampainLevel + i, 1, true);
        }
        return value;
    }
    public static float GetLevelProgression(int level, float coef, bool useSin)
    {
        float a = Mathf.Pow(1.2f, level * coef);
        if(useSin) a *= GetSin(0.5f * level, 0.6f, 1);
        return a;
    }
    public static float GetStatUpgradeCostProgression(int statLevel)
    {
        return Mathf.Pow(1.25f-0.048f*Mathf.Pow(statLevel,1.4f)/(400+ Mathf.Pow(statLevel, 1.4f)), statLevel + 10) / 6;
    }
    public static float GetStatAmountProgression(int statLevel)
    {
        return GetLevelProgression(statLevel, 1, false);
    }
    public static float GetSin(float x, float o, float i)
    {
        return Mathf.Pow(Mathf.Sin(x), 2) * o + i;
    }
    static float InverseFuncPoint(float level, float max, float min)
    {
        return (level / (level + 1 / 0.02f)) * (max - min) + min;
    }
    public static float GetBonusProbabilityProgression()
    {
        return InverseFuncPoint(SaveManager.Instance.SavedValues.BonusProbabilityUpgradeLevel, 4, 1);
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
    public static float GetCannonArmorProgression()
    {
        return InverseFuncPoint(SaveManager.Instance.SavedValues.CannonArmorUpgradeLevel, 4, 1);
    }
}
