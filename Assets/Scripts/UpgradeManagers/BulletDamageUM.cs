using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade(bool withEffects = true)
    {
        base.SetThisUpgrade(withEffects);

        fixed (int* i = &SaveManager.Instance.SavedValues.bulletDamageUpgradeLevel)
        SetThisUpgrade(i,
                GameConfigContainer.gameConfig.bulletDamage,
                CannonManager.Cannon.CannonSettings.bulletsDamageMultiplyer * BackgroundManager.Background.backgroundSettings.bulletsDamageMultiplyer,
                Progression.GetStatAmountProgression(SaveManager.Instance.SavedValues.BulletDamageUpgradeLevel),
                Progression.GetStatUpgradeCostProgression(SaveManager.Instance.SavedValues.BulletDamageUpgradeLevel));
    }
}
