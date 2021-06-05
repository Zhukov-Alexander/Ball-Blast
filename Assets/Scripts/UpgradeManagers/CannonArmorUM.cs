using System;
using System.Collections.Generic;

public class CannonArmorUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade(bool withEffects = true)
    {
        base.SetThisUpgrade(withEffects);

        fixed (int* i = &SaveManager.Instance.SavedValues.cannonArmorUpgradeLevel)
            SetThisUpgrade(i,
                GameConfigContainer.gameConfig.armor,
                CannonManager.Cannon.CannonSettings.armorMultiplyer * BackgroundManager.Background.backgroundSettings.armorMultiplyer,
                Progression.GetCannonArmorProgression(),
                Progression.GetStatUpgradeCostProgression(SaveManager.Instance.SavedValues.CannonArmorUpgradeLevel));
    }

}
