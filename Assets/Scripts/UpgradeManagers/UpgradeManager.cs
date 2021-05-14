﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static GameConfigContainer;
using UnityEngine.Localization;

public abstract class UpgradeManager : MonoBehaviour
{
    [SerializeField] LocalizedString localizedString;
    [SerializeField] Button headerButton;
    [SerializeField] Image headerBackground;
    [SerializeField] Image upgradeForeground;
    [SerializeField] TextMeshProUGUI upgradeNameTMP;
    [SerializeField] protected TextMeshProUGUI upgradeStatsTMP;
    [SerializeField] protected TextMeshProUGUI upgradeStatsResultTMP;
    [SerializeField] Button upgradeButton;
    [SerializeField] protected TextMeshProUGUI upgradeCostTMP;
    private double upgradeCost;
    private static List<UpgradeManager> UpgradeManagers;
    public static Action OnUpgrade;

    private void Awake()
    {
        if (UpgradeManagers == null) 
            UpgradeManagers = new List<UpgradeManager>();
        UpgradeManagers.Add(this);
        headerButton.onClick.AddListener(SetThisUpgrade);
        if (this is BulletsPerSecondUM)
        {
            StartMenu.OnEnter += SetThisUpgrade;
        }
    }
    unsafe protected virtual void SetThisUpgrade()
    {
        SoundManager.Instance.Button();
        Vibration.Vibrate(gameObject);
    }

    unsafe protected void SetThisUpgrade(int* statLevel, float baseAmount, float multiplyer, float statProgression, float costProgression)
    {
        SetHeader();
        StartCoroutine(SetCurrentUpgradeText());
        SetCurrentUpgradeStats();
        SetUpgradeCost();
        SetUpgradeButton();

        void SetHeader()
        {
            foreach (var item in UpgradeManagers)
            {
                item.headerBackground.color = gameConfig.passiveHeaderColor;
            }
            headerBackground.color = gameConfig.activeHeaderColor;
        }
        IEnumerator SetCurrentUpgradeText()
        {
            var ls = localizedString.GetLocalizedString();
            yield return ls;
            upgradeNameTMP.text = ls.Result;
        }
        void SetCurrentUpgradeStats()
        {
            string statText = NumberHandler.NumberToTextInOneLine(baseAmount * statProgression, true);
            statText += " * " + HelperClass.MultiplyerToPercent(multiplyer);
            statText += " = ";
            upgradeStatsTMP.text = statText;
            upgradeStatsResultTMP.text = NumberHandler.NumberToTextInOneLine(baseAmount * statProgression * multiplyer, true);
        }

        void SetUpgradeCost()
        {
            upgradeCost = (gameConfig.levelDuration * gameConfig.levelLifePerSec * costProgression);
            upgradeCostTMP.text = NumberHandler.NumberToTextInOneLine(upgradeCost, true);
        }
        void SetUpgradeButton()
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => SoundManager.Instance.Bonus());
            upgradeButton.onClick.AddListener(Upgrade);

            if (SaveManager.Instance.SavedValues.Coins >= upgradeCost)
            {
                upgradeForeground.color = gameConfig.activeUpgradeColor;
            }
            else
            {
                upgradeForeground.color = gameConfig.passiveUpgradeColor;
            }

            void Upgrade()
            {
                if (SaveManager.Instance.SavedValues.Coins >= upgradeCost)
                {
                    SaveManager.Instance.SavedValues.Coins -= upgradeCost;
                    *statLevel += 1;
                    SaveManager.Instance.SaveLocal();
                    OnUpgrade();
                    SetThisUpgrade();
                    Vibration.Vibrate(gameObject);
                    Vibration.Vibrate(upgradeButton.gameObject);
                }
            }
        }

    }

}
