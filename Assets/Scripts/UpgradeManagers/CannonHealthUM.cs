using System;
using System.Collections.Generic;
public class CannonHealthUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade(bool withEffects = true)
    {
        base.SetThisUpgrade(withEffects);

        fixed (int* i = &SaveManager.Instance.SavedValues.cannonLivesUpgradeLevel)
            SetThisUpgrade(
                i, 
                GameConfigContainer.gameConfig.health, 
                CannonManager.Cannon.CannonSettings.healthMultiplyer * BackgroundManager.Background.backgroundSettings.healthMultiplyer,
                Progression.GetStatAmountProgression(SaveManager.Instance.SavedValues.CannonHealthUpgradeLevel),
                Progression.GetStatUpgradeCostProgression(SaveManager.Instance.SavedValues.CannonHealthUpgradeLevel));
    }

}
