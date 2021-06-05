using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPerSecondUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade(bool withEffects = true)
    {
        base.SetThisUpgrade(withEffects);

        fixed (int* i = &SaveManager.Instance.SavedValues.bulletsPerSecondUpgradeLevel)
        SetThisUpgrade(i,
                GameConfigContainer.gameConfig.bulletsPerSecond,
                CannonManager.Cannon.CannonSettings.bulletsPerSecondMultiplyer * BackgroundManager.Background.backgroundSettings.bulletsPerSecondMultiplyer,
                Progression.GetBulletsPerSecondProgression(),
                Progression.GetStatUpgradeCostProgression(SaveManager.Instance.SavedValues.BulletsPerSecondUpgradeLevel));
    }
}
