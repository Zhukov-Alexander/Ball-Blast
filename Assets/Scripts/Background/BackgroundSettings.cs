using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BackgroundSettings", menuName = "ScriptableObjects/BackgroundSettings")]
public class BackgroundSettings : ScriptableObject
{
    public float bulletsDamageMultiplyer = 1;
    public float cannonMoveForceMultiplyer = 1;
    public float bonusProbabilityMultiplyer = 1;
    public float bulletsPerSecondMultiplyer = 1;
    public float healthMultiplyer = 1;
    public float armorMultiplyer = 1;
}
