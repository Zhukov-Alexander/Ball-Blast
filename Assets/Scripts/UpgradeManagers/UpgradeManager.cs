using System;
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
    protected static UpgradeManager currentUpgradeManager;
    public static Action OnUpgrade;

    private void Awake()
    {
        if (UpgradeManagers == null) 
            UpgradeManagers = new List<UpgradeManager>();
        UpgradeManagers.Add(this);
        headerButton.onClick.AddListener(() => SetThisUpgrade());
        if (this is BulletsPerSecondUM)
        {
            MainMenu.OnEnter += () => SetThisUpgrade();
            Chest.OnRewardTaken += () => currentUpgradeManager.SetThisUpgrade(false);
        }
    }
    unsafe protected virtual void SetThisUpgrade(bool withEffects = true)
    {
        currentUpgradeManager = this;
        if (withEffects)
        {
            SoundManager.Instance.Button();
            Vibration.Vibrate(gameObject);
        }
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
                    SoundManager.Instance.Bonus();
                    SaveManager.Instance.SavedValues.Coins -= upgradeCost;
                    *statLevel += 1;
                    SaveManager.Instance.SaveLocal();
                    OnUpgrade();
                    TaskActiones.Instance.UpgradeStats(1);
                    if (this.GetType() == typeof(BonusDropUM))
                        TaskActiones.Instance.UpgradeBonusDrop(1);
                    else if (this.GetType() == typeof(BulletDamageUM))
                        TaskActiones.Instance.UpgradeBulletDamage(1);
                    else if (this.GetType() == typeof(BulletsPerSecondUM))
                        TaskActiones.Instance.UpgradeBulletsPerSecond(1);
                    else if (this.GetType() == typeof(CannonArmorUM))
                        TaskActiones.Instance.UpgradeCannonArmor(1);
                    else if (this.GetType() == typeof(CannonHealthUM))
                        TaskActiones.Instance.UpgradeCannonHealth(1);
                    else if (this.GetType() == typeof(CannonMoveForceUM))
                        TaskActiones.Instance.UpgradeCannonMoveForce(1);

                    SetThisUpgrade(false);
                    Vibration.Vibrate(upgradeButton.gameObject);
                }
            }
        }

    }

}
