using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CannonSettings", menuName = "ScriptableObjects/CannonSettings")]

public class CannonSettings : ScriptableObject
{
    public string cannonName;
    public float bulletsDamageMultiplyer;
    public float cannonMoveForceMultiplyer;
    public float bonusProbabilityMultiplyer;
    public float bulletsPerSecondMultiplyer;
    public float healthMultiplyer;
    public float armorMultiplyer;
}
