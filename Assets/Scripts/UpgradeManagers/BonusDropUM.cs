using System;
using System.Collections.Generic;

public class BonusDropUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade(bool withEffects = true)
    {
        base.SetThisUpgrade(withEffects);

        fixed (int* i = &SaveManager.Instance.SavedValues.bonusDropProbabilityUpgradeLevel)
            SetThisUpgrade(i,
                GameConfigContainer.gameConfig.bonusProbability,
                CannonManager.Cannon.CannonSettings.bonusProbabilityMultiplyer * BackgroundManager.Background.backgroundSettings.bonusProbabilityMultiplyer,
                Progression.GetBonusProbabilityProgression(),
                Progression.GetStatUpgradeCostProgression(SaveManager.Instance.SavedValues.BonusProbabilityUpgradeLevel));
    }

}
