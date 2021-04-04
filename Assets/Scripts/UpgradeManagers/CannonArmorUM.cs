using System;
using System.Collections.Generic;

public class CannonArmorUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade()
    {
        base.SetThisUpgrade();

        fixed (int* i = &SaveManager.Instance.SavedValues.cannonArmorUpgradeLevel)
            SetThisUpgrade(i,
                GameConfigContainer.gameConfig.armor,
                CannonManager.Cannon.CannonSettings.armorMultiplyer * BackgroundManager.Background.backgroundSettings.armorMultiplyer,
                Progression.GetStatAmountProgression(SaveManager.Instance.SavedValues.CannonArmorUpgradeLevel),
                Progression.GetStatUpgradeCostProgression(SaveManager.Instance.SavedValues.CannonArmorUpgradeLevel));
    }

}
