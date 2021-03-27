using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SavedValues : Savable<SavedValues>
{
    private bool sound = true;
    private bool vibration = true;
    public int bulletsPerSecondUpgradeLevel;
    public int bulletDamageUpgradeLevel;
    public int cannonSpeedUpgradeLevel;
    public int bonusDropProbabilityUpgradeLevel;
    public int bulletSpeedUpgradeLevel;
    public int cannonLivesUpgradeLevel;
    public int cannonArmorUpgradeLevel;
    private int cannonPrefabNumber;
    private int backgroundPrefabNumber;
    private float coins;
    private float diamonds;
    private int campainLevel = 1;
    private int bossfightLevel = 1;
    private List<int> opendCannonsPrefabIndexes = new List<int>() { 0 };
    private List<int> opendBackgroundsPrefabIndexes = new List<int>() { 0 };

    [NonSerialized] public Action OnCoinsChanged;
    [NonSerialized] public Action OnDiamondsChanged;

    public bool Sound { get => sound; set { sound = value; Save();} }
    public bool Vibration { get => vibration; set { vibration = value; Save();} }
    public int BulletsPerSecondUpgradeLevel { get => bulletsPerSecondUpgradeLevel; set { bulletsPerSecondUpgradeLevel = value; Save(); } }
    public int BulletDamageUpgradeLevel { get => bulletDamageUpgradeLevel; set { bulletDamageUpgradeLevel = value; Save(); } }
    public int CannonMoveForceUpgradeLevel { get => cannonSpeedUpgradeLevel; set { cannonSpeedUpgradeLevel = value; Save(); } }
    public int BonusProbabilityUpgradeLevel { get => bonusDropProbabilityUpgradeLevel; set { bonusDropProbabilityUpgradeLevel = value; Save(); } }
    public int CannonHealthUpgradeLevel { get => cannonLivesUpgradeLevel; set { cannonLivesUpgradeLevel = value; Save(); } }
    public int CannonArmorUpgradeLevel { get => cannonArmorUpgradeLevel; set { cannonArmorUpgradeLevel = value; Save(); } }
    public int CannonPrefabNumber { get => cannonPrefabNumber; set { cannonPrefabNumber = value; Save(); } }
    public int ScenePrefabNumber { get => backgroundPrefabNumber; set { backgroundPrefabNumber = value; Save(); } }
    public float Coins { get => coins; set { coins = value; Save(); OnCoinsChanged?.Invoke(); } }

    public List<int> OpendCannonsPrefabIndexes { get => opendCannonsPrefabIndexes; set { opendCannonsPrefabIndexes = value; Save(); } }
    public List<int> OpendBackgroundsPrefabIndexes { get => opendBackgroundsPrefabIndexes; set { opendBackgroundsPrefabIndexes = value; Save(); } }

    public float Diamonds { get => diamonds; set { diamonds = value; Save(); OnDiamondsChanged?.Invoke(); } }
    public int CampainLevel { get => campainLevel; set { campainLevel = value; Save(); } }
    public int BossfightLevel { get => bossfightLevel; set { bossfightLevel = value; Save(); } }
}
