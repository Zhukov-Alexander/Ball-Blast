using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossSettings", menuName = "ScriptableObjects/BossSettings")]
public class BossSettings : ScriptableObject
{
    public string bossName = "Boss";
    public float livesCoef = 1;
    public float damageCoef = 1;
    public float armorCoef = 1;
    public float speedCoef = 1;
}
