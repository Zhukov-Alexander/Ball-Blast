﻿using System;
using System.Collections.Generic;

public class CannonMoveForceUM : UpgradeManager
{
    unsafe protected override void SetThisUpgrade()
    {
        base.SetThisUpgrade();
        fixed (int* i = &SavedValues.Instance.cannonSpeedUpgradeLevel)
            SetThisUpgrade(i,
                GameConfigContainer.gameConfig.cannonMoveForce,
                CannonManager.Cannon.CannonSettings.cannonMoveForceMultiplyer * BackgroundManager.Background.backgroundSettings.cannonMoveForceMultiplyer,
                Progression.GetCannonMoveForceProgression(),
                Progression.GetStatUpgradeCostProgression(SavedValues.Instance.CannonMoveForceUpgradeLevel));

    }

}