﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static GameConfigContainer;

public abstract class UpgradeManager : MonoBehaviour
{
    [SerializeField] Button headerButton;
    [SerializeField] Image headerBackground;
    [SerializeField] Image upgradeForeground;
    [SerializeField] string upgradeName;
    [SerializeField] TextMeshProUGUI upgradeNameTMP;
    [SerializeField] protected TextMeshProUGUI upgradeStatsTMP;
    [SerializeField] protected TextMeshProUGUI upgradeStatsResultTMP;
    [SerializeField] Button upgradeButton;
    [SerializeField] protected TextMeshProUGUI upgradeCostTMP;
    private float upgradeCost;
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
    }

    unsafe protected void SetThisUpgrade(int* statLevel, float baseAmount, float multiplyer, float statProgression, float costProgression)
    {
        SetHeader();
        SetCurrentUpgradeText();
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
        void SetCurrentUpgradeText()
        {
            upgradeNameTMP.text = upgradeName;
        }
        void SetCurrentUpgradeStats()
        {
            string statText = NumberHandler.NumberToTextInOneLineWithFraction(baseAmount * statProgression);
            statText += " * " + HelperClass.MultiplyerToPercent(multiplyer);
            statText += " = ";
            upgradeStatsTMP.text = statText;
            upgradeStatsResultTMP.text = NumberHandler.NumberToTextInOneLineWithFraction(baseAmount * statProgression * multiplyer);
        }

        void SetUpgradeCost()
        {
            upgradeCost = (gameConfig.levelDuration * gameConfig.levelLifePerSec * costProgression);
            upgradeCostTMP.text = NumberHandler.NumberToTextInOneLineWithFraction(upgradeCost);
        }
        void SetUpgradeButton()
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => SoundManager.Instance.Bonus());
            upgradeButton.onClick.AddListener(Upgrade);

            if (SavedValues.Instance.Coins >= upgradeCost)
            {
                upgradeForeground.color = gameConfig.activeUpgradeColor;
            }
            else
            {
                upgradeForeground.color = gameConfig.passiveUpgradeColor;
            }

            void Upgrade()
            {
                if (SavedValues.Instance.Coins >= upgradeCost)
                {
                    SavedValues.Instance.Coins -= upgradeCost;
                    *statLevel += 1;
                    SavedValues.Save();
                    OnUpgrade();
                    SetThisUpgrade();
                }
            }
        }

    }

}