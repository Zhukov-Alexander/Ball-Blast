using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameConfigContainer;

public class LevelModManager : MonoBehaviour
{
    [SerializeField] GameObject campainModGO;
    [SerializeField] GameObject bossfightModGO;

    public static LevelMod CurrentLevelMod { get; set; }

    private void Awake()
    {
        UpdateLevelMode();
    }

    public void ChangeLevelMode()
    {
        SoundManager.Instance.Button();
        if (CurrentLevelMod == LevelMod.Campain)
            SetBossfightMode();
        else
            SetCampainMode();
    }
    private void UpdateLevelMode()
    {
        if (CurrentLevelMod == LevelMod.Campain)
            SetCampainMode();
        else
            SetBossfightMode();
    }
    private void SetCampainMode()
    {
        bossfightModGO.SetActive(false);
        campainModGO.SetActive(true);
        CurrentLevelMod = LevelMod.Campain;
    }

    private void SetBossfightMode()
    {
        campainModGO.SetActive(false);
        bossfightModGO.SetActive(true);
        CurrentLevelMod = LevelMod.Bossfight;
    }
}
public enum LevelMod
{
    Campain,
    Bossfight
}
