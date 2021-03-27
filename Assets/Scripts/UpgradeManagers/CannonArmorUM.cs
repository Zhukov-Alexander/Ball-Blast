using System;
using System.Collections.Generic;

public class CannonArmorUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade()
    {
        base.SetThisUpgrade();

        fixed (int* i = &SavedValues.Instance.cannonArmorUpgradeLevel)
            SetThisUpgrade(i,
                GameConfigContainer.gameConfig.armor,
                CannonManager.Cannon.CannonSettings.armorMultiplyer * BackgroundManager.Background.backgroundSettings.armorMultiplyer,
                Progression.GetStatAmountProgression(SavedValues.Instance.CannonArmorUpgradeLevel),
                Progression.GetStatUpgradeCostProgression(SavedValues.Instance.CannonArmorUpgradeLevel));
    }

}
