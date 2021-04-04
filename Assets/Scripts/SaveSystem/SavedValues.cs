using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SavedValues
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

    public bool Sound { get => sound; set { sound = value; SaveManager.Instance.SaveLocal();} }
    public bool Vibration { get => vibration; set { vibration = value; SaveManager.Instance.SaveLocal();} }
    public int BulletsPerSecondUpgradeLevel { get => bulletsPerSecondUpgradeLevel; set { bulletsPerSecondUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int BulletDamageUpgradeLevel { get => bulletDamageUpgradeLevel; set { bulletDamageUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int CannonMoveForceUpgradeLevel { get => cannonSpeedUpgradeLevel; set { cannonSpeedUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int BonusProbabilityUpgradeLevel { get => bonusDropProbabilityUpgradeLevel; set { bonusDropProbabilityUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int CannonHealthUpgradeLevel { get => cannonLivesUpgradeLevel; set { cannonLivesUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int CannonArmorUpgradeLevel { get => cannonArmorUpgradeLevel; set { cannonArmorUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int CannonPrefabNumber { get => cannonPrefabNumber; set { cannonPrefabNumber = value; SaveManager.Instance.SaveLocal(); } }
    public int ScenePrefabNumber { get => backgroundPrefabNumber; set { backgroundPrefabNumber = value; SaveManager.Instance.SaveLocal(); } }
    public float Coins { get => coins; set { coins = value; SaveManager.Instance.SaveLocal(); OnCoinsChanged?.Invoke(); } }

    public List<int> OpendCannonsPrefabIndexes { get => opendCannonsPrefabIndexes; set { opendCannonsPrefabIndexes = value; SaveManager.Instance.SaveLocal(); } }
    public List<int> OpendBackgroundsPrefabIndexes { get => opendBackgroundsPrefabIndexes; set { opendBackgroundsPrefabIndexes = value; SaveManager.Instance.SaveLocal(); } }

    public float Diamonds { get => diamonds; set { diamonds = value; SaveManager.Instance.SaveLocal(); OnDiamondsChanged?.Invoke(); } }
    public int CampainLevel { get => campainLevel; set { campainLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int BossfightLevel { get => bossfightLevel; set { bossfightLevel = value; SaveManager.Instance.SaveLocal(); } }
}
