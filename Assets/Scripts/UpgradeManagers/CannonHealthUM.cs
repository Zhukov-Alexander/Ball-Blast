using System;
using System.Collections.Generic;
public class CannonHealthUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade()
    {
        base.SetThisUpgrade();

        fixed (int* i = &SavedValues.Instance.cannonLivesUpgradeLevel)
            SetThisUpgrade(
                i, 
                GameConfigContainer.gameConfig.health, 
                CannonManager.Cannon.CannonSettings.healthMultiplyer * BackgroundManager.Background.backgroundSettings.healthMultiplyer,
                Progression.GetStatAmountProgression(SavedValues.Instance.CannonHealthUpgradeLevel),
                Progression.GetStatUpgradeCostProgression(SavedValues.Instance.CannonHealthUpgradeLevel));
    }

}
