using System;
using System.Collections.Generic;

public class BonusDropUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade()
    {
        base.SetThisUpgrade();

        fixed (int* i = &SavedValues.Instance.bonusDropProbabilityUpgradeLevel)
            SetThisUpgrade(i,
                GameConfigContainer.gameConfig.bonusProbability,
                CannonManager.Cannon.CannonSettings.bonusProbabilityMultiplyer * BackgroundManager.Background.backgroundSettings.bonusProbabilityMultiplyer,
                Progression.GetBonusProbabilityProgression(),
                Progression.GetStatUpgradeCostProgression(SavedValues.Instance.BonusProbabilityUpgradeLevel));
    }

}
