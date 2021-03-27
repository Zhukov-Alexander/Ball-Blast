using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade()
    {
        base.SetThisUpgrade();

        fixed (int* i = &SavedValues.Instance.bulletDamageUpgradeLevel)
        SetThisUpgrade(i,
                GameConfigContainer.gameConfig.bulletDamage,
                CannonManager.Cannon.CannonSettings.bulletsDamageMultiplyer * BackgroundManager.Background.backgroundSettings.bulletsDamageMultiplyer,
                Progression.GetStatAmountProgression(SavedValues.Instance.BulletDamageUpgradeLevel),
                Progression.GetStatUpgradeCostProgression(SavedValues.Instance.BulletDamageUpgradeLevel));
    }
}
