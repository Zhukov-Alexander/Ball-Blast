using System;
using System.Collections.Generic;
using System.Linq;
using static GameConfigContainer;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Reflection;

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
    private double coins;
    private int diamonds;
    private int campainLevel = 1;
    private int bossfightLevel = 1;
    private List<int> opendCannonsPrefabIndexes = new List<int>() { 0 };
    private List<int> opendBackgroundsPrefabIndexes = new List<int>() { 0 };
    private string playtimeRewardTimeSpan;
    private Tasks dailyTasks;
    private Tasks weeklyTasks;

    [NonSerialized] public Action OnCoinsChanged;
    [NonSerialized] public Action OnDiamondsChanged;

    public bool Sound { get => sound; set { sound = value; SaveManager.Instance.SaveLocal(); } }
    public bool Vibration { get => vibration; set { vibration = value; SaveManager.Instance.SaveLocal(); } }
    public int BulletsPerSecondUpgradeLevel { get => bulletsPerSecondUpgradeLevel; set { bulletsPerSecondUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int BulletDamageUpgradeLevel { get => bulletDamageUpgradeLevel; set { bulletDamageUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int CannonMoveForceUpgradeLevel { get => cannonSpeedUpgradeLevel; set { cannonSpeedUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int BonusProbabilityUpgradeLevel { get => bonusDropProbabilityUpgradeLevel; set { bonusDropProbabilityUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int CannonHealthUpgradeLevel { get => cannonLivesUpgradeLevel; set { cannonLivesUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int CannonArmorUpgradeLevel { get => cannonArmorUpgradeLevel; set { cannonArmorUpgradeLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int CannonPrefabNumber { get => cannonPrefabNumber; set { cannonPrefabNumber = value; SaveManager.Instance.SaveLocal(); } }
    public int ScenePrefabNumber { get => backgroundPrefabNumber; set { backgroundPrefabNumber = value; SaveManager.Instance.SaveLocal(); } }
    public double Coins { get => coins; set {
            double d = value - coins;
            if (d > 0)
            {
                Debug.Log("value " + value + " add " + d);
                TaskActiones.Instance.EarnCoins(d);
            }
            coins = value; 
            SaveManager.Instance.SaveLocal(); 
            OnCoinsChanged?.Invoke(); } }

    public List<int> OpendCannonsPrefabIndexes { get => opendCannonsPrefabIndexes; set { opendCannonsPrefabIndexes = value; SaveManager.Instance.SaveLocal(); } }
    public List<int> OpendBackgroundsPrefabIndexes { get => opendBackgroundsPrefabIndexes; set { opendBackgroundsPrefabIndexes = value; SaveManager.Instance.SaveLocal(); } }

    public int Diamonds { get => diamonds; set {
            int d = value - diamonds;
            if (d > 0)
            {
                TaskActiones.Instance.EarnDiamonds(d);
            }
            diamonds = value; 
            SaveManager.Instance.SaveLocal(); 
            OnDiamondsChanged?.Invoke(); } }
    public int CampainLevel { get => campainLevel; set { campainLevel = value; SaveManager.Instance.SaveLocal(); } }
    public int BossfightLevel { get => bossfightLevel; set { bossfightLevel = value; SaveManager.Instance.SaveLocal(); } }

    public DateTime PlaytimeRewardTimeSpan
    {
        get
        {
            if (playtimeRewardTimeSpan != null && playtimeRewardTimeSpan.Length > 0)
            {
                return DateTime.Parse(playtimeRewardTimeSpan);
            }
            else
            {
                return new DateTime();
            }
        }
        set
        {
            playtimeRewardTimeSpan = value.ToString();
        }
    }

    public Tasks DailyTasks
    {
        get
        {
            if(dailyTasks == null || dailyTasks.UpdateDate.Length == 0 || DateTime.Now > DateTime.ParseExact(dailyTasks.UpdateDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture))
            {
                dailyTasks = new Tasks(new TimeSpan(1,6,0,0), TaskType.daily);
            }
            return dailyTasks;
        }
    }
}

[Serializable]
public class Tasks
{
    private List<Task> currentTasks;

    public EnemyTaskGroup enemyTaskGroup = new EnemyTaskGroup();
    public LevelTaskGroup levelTaskGroup = new LevelTaskGroup();
    public CannonTaskGroup cannonTaskGroup = new CannonTaskGroup();
    public UpgradeTaskGroup upgradeTaskGroup = new UpgradeTaskGroup();
    public DifferentTaskGroup differentTaskGroup = new DifferentTaskGroup();
    public bool allTasksRewardTaken;
    private string updateDate;
    TaskType taskType;

    [NonSerialized] List<List<TaskConfig>> allTasksInfo;
    public List<List<TaskConfig>> AllTasksInfo
    {
        get
        {
            if (allTasksInfo == null || allTasksInfo.Count == 0)
                SetAllTasksInfo();
            return allTasksInfo;
        }

        set
        {
            allTasksInfo = value;
        }
    }

    unsafe void SetAllTasksInfo()
    {
            List<List<TaskConfig>>  allTasks = new List<List<TaskConfig>>();
            #region EnemyTaskGroup
            List<TaskConfig> enemyTaskBuilderGroup = new List<TaskConfig>();
            allTasks.Add(enemyTaskBuilderGroup);
                enemyTaskBuilderGroup.Add(new TaskConfig(taskType,
                    EnemyTaskGroupEnum.dealDamage,
                    enemyTaskGroup.dealDamage,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => gameConfig.levelLifePerSec * gameConfig.levelDuration * Progression.GetCampainProgression((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Deal " + c.NumberToTextInOneLine() + " damage"));
                enemyTaskBuilderGroup.Add(new TaskConfig(taskType,
                    EnemyTaskGroupEnum.defeatBosses,
                    enemyTaskGroup.defeatBosses,
                    (a) => gameConfig.taskReward.y,
                    (b) => 1,
                    (c) => "Defeat a boss"));
                enemyTaskBuilderGroup.Add(new TaskConfig(taskType,
                    EnemyTaskGroupEnum.defeatEnemies,
                    enemyTaskGroup.defeatEnemies,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Defeat " + c.NumberToTextInOneLine() + " enemies"));
                enemyTaskBuilderGroup.Add(new TaskConfig(taskType,
                    EnemyTaskGroupEnum.defeatEnemiesByBody,
                    enemyTaskGroup.defeatEnemiesByBody,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) * 0.3f),
                    (c) => "Defeat " + c.NumberToTextInOneLine() + " enemies by body"));
                enemyTaskBuilderGroup.Add(new TaskConfig(taskType,
                    EnemyTaskGroupEnum.defeatEnemiesByBullets,
                    enemyTaskGroup.defeatEnemiesByBullets,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) * 0.7f),
                    (c) => "Defeat " + c.NumberToTextInOneLine() + " enemies by bullets"));
                enemyTaskBuilderGroup.Add(new TaskConfig(taskType,
                    EnemyTaskGroupEnum.defeatBigEnemies,
                    enemyTaskGroup.defeatBigEnemies,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.ballTypeProbabilities[2] / gameConfig.ballTypeProbabilities.Sum() * gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Defeat " + c.NumberToTextInOneLine() + " big enemies"));
                enemyTaskBuilderGroup.Add(new TaskConfig(taskType,
                    EnemyTaskGroupEnum.defeatMediumEnemies,
                    enemyTaskGroup.defeatMediumEnemies,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)((gameConfig.ballTypeProbabilities[1] / gameConfig.ballTypeProbabilities.Sum()) * gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Defeat " + c.NumberToTextInOneLine() + " medium enemies"));
                enemyTaskBuilderGroup.Add(new TaskConfig(taskType,
                    EnemyTaskGroupEnum.defeatSmallEnemies,
                    enemyTaskGroup.defeatSmallEnemies,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)((gameConfig.ballTypeProbabilities[0] / gameConfig.ballTypeProbabilities.Sum()) * gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Defeat " + c.NumberToTextInOneLine() + " small enemies"));
            #endregion

            #region LevelTaskGroup
            List<TaskConfig> levelTaskBuilderGroup = new List<TaskConfig>();
            allTasks.Add(levelTaskBuilderGroup);
                levelTaskBuilderGroup.Add(new TaskConfig(taskType,
                    LevelTaskGroupEnum.winLevels,
                    levelTaskGroup.winLevels,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y),
                    (c) => "Win " + c.NumberToTextInOneLine() + " levels"));
                levelTaskBuilderGroup.Add(new TaskConfig(taskType,
                    LevelTaskGroupEnum.winLevelsStandingStill,
                    levelTaskGroup.winLevelsStandingStill,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 3),
                    (c) => "Win " + c.NumberToTextInOneLine() + " levels not moving"));
                levelTaskBuilderGroup.Add(new TaskConfig(taskType,
                    LevelTaskGroupEnum.winLevelsWithAlmostNoLives,
                    levelTaskGroup.winLevelsWithAlmostNoLives,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 4),
                    (c) => "Win " + c.NumberToTextInOneLine() + " levels with less than 10% lives"));
                levelTaskBuilderGroup.Add(new TaskConfig(taskType,
                    LevelTaskGroupEnum.winLevelsWithNoBonusesUsed,
                    levelTaskGroup.winLevelsWithNoBonusesUsed,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 4),
                    (c) => "Win " + c.NumberToTextInOneLine() + " levels with no bonuses used"));
                levelTaskBuilderGroup.Add(new TaskConfig(taskType,
                    LevelTaskGroupEnum.winLevelsWithNoDamageTaken,
                    levelTaskGroup.winLevelsWithNoDamageTaken,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 5),
                    (c) => "Win " + c.NumberToTextInOneLine() + " levels with no damage taken"));
                levelTaskBuilderGroup.Add(new TaskConfig(taskType,
                    LevelTaskGroupEnum.winLevelsWithoutSecondChance,
                    levelTaskGroup.winLevelsWithoutSecondChance,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 3),
                    (c) => "Win " + c.NumberToTextInOneLine() + " levels with no second chance taken"));
                levelTaskBuilderGroup.Add(new TaskConfig(taskType,
                    LevelTaskGroupEnum.winLevelsWithSecondChance,
                    levelTaskGroup.winLevelsWithSecondChance,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 3),
                    (c) => "Win " + c.NumberToTextInOneLine() + " levels after taking second chance"));
            #endregion

            #region CannonTaskGroup
            List<TaskConfig> cannonTaskBuilderGroup = new List<TaskConfig>();
            allTasks.Add(cannonTaskBuilderGroup);
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.reflectDamage,
                    cannonTaskGroup.reflectDamage,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => gameConfig.levelLifePerSec * gameConfig.levelDuration * Progression.GetCampainProgression((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)) * 0.2f * gameConfig.armor,
                    (c) => "Reflect " + c.NumberToTextInOneLine() + " damage"));
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.shootBullets,
                    cannonTaskGroup.shootBullets,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.bulletsPerSecond * gameConfig.levelDuration * 2 * Progression.GetCampainProgression((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y))),
                    (c) => "Shoot " + c.NumberToTextInOneLine() + " bullets"));
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.takeBonusCoins,
                    cannonTaskGroup.takeBonusCoins,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 2),
                    (c) => "Take bonus coins " + c.NumberToTextInOneLine() + " times"));
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.takeDamage,
                    cannonTaskGroup.takeDamage,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => gameConfig.levelLifePerSec * gameConfig.levelDuration * Progression.GetCampainProgression((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)) * 0.2f * (1 - gameConfig.armor),
                    (c) => "Take " + c.NumberToTextInOneLine() + " damage"));
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.useSecondChance,
                    cannonTaskGroup.useSecondChance,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 2),
                    (c) => "Take second chance " + c.NumberToTextInOneLine() + " times"));
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.useBonuses,
                    cannonTaskGroup.useBonuses,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) * gameConfig.bonusProbability * 3),
                    (c) => "Use " + c.NumberToTextInOneLine() + " bonuses"));
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.useHealthBonus,
                    cannonTaskGroup.useHealthBonus,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) * gameConfig.bonusProbability / BonusManager.Instance.bonusPrefabs.Count * 3),
                    (c) => "Use " + c.NumberToTextInOneLine() + " health bonuses"));
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.useShieldBonus,
                    cannonTaskGroup.useShieldBonus,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) * gameConfig.bonusProbability / BonusManager.Instance.bonusPrefabs.Count * 3),
                    (c) => "Use " + c.NumberToTextInOneLine() + " shield bonuses"));
                cannonTaskBuilderGroup.Add(new TaskConfig(taskType,
                    CannonTaskGroupEnum.useSpeedBonus,
                    cannonTaskGroup.useSpeedBonus,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(gameConfig.levelDuration * gameConfig.levelShotsPerSec * (int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) * gameConfig.bonusProbability / BonusManager.Instance.bonusPrefabs.Count * 3),
                    (c) => "Use " + c.NumberToTextInOneLine() + " speed bonuses"));
            #endregion

            #region UpgradeTaskGroup
            List<TaskConfig> upgradeTaskBuilderGroup = new List<TaskConfig>();
            allTasks.Add(upgradeTaskBuilderGroup);
                upgradeTaskBuilderGroup.Add(new TaskConfig(taskType,
                    UpgradeTaskGroupEnum.upgradeBonusDrop,
                    upgradeTaskGroup.upgradeBonusDrop,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Upgrade bonus drop rate " + c.NumberToTextInOneLine() + " times"));
                upgradeTaskBuilderGroup.Add(new TaskConfig(taskType,
                    UpgradeTaskGroupEnum.upgradeBulletDamage,
                    upgradeTaskGroup.upgradeBulletDamage,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Upgrade bullet damage " + c.NumberToTextInOneLine() + " times"));
                upgradeTaskBuilderGroup.Add(new TaskConfig(taskType,
                    UpgradeTaskGroupEnum.upgradeBulletsPerSecond,
                    upgradeTaskGroup.upgradeBulletsPerSecond,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Upgrade fire rate " + c.NumberToTextInOneLine() + " times"));
                upgradeTaskBuilderGroup.Add(new TaskConfig(taskType,
                    UpgradeTaskGroupEnum.upgradeCannonArmor,
                    upgradeTaskGroup.upgradeCannonArmor,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Upgrade cannon armor " + c.NumberToTextInOneLine() + " times"));
                upgradeTaskBuilderGroup.Add(new TaskConfig(taskType,
                    UpgradeTaskGroupEnum.upgradeCannonHealth,
                    upgradeTaskGroup.upgradeCannonHealth,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Upgrade cannon health " + c.NumberToTextInOneLine() + " times"));
                upgradeTaskBuilderGroup.Add(new TaskConfig(taskType,
                    UpgradeTaskGroupEnum.upgradeCannonMoveForce,
                    upgradeTaskGroup.upgradeCannonMoveForce,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Upgrade cannon speed " + c.NumberToTextInOneLine() + " times"));
                upgradeTaskBuilderGroup.Add(new TaskConfig(taskType,
                    UpgradeTaskGroupEnum.upgradeStats,
                    upgradeTaskGroup.upgradeStats,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => (int)(HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) * 5f),
                    (c) => "Upgrade stats " + c.NumberToTextInOneLine() + " times"));
            #endregion

            #region DifferentTaskGroup
            List<TaskConfig> differentTaskBuilderGroup = new List<TaskConfig>();
            allTasks.Add(differentTaskBuilderGroup);
                differentTaskBuilderGroup.Add(new TaskConfig(taskType,
                    DifferentTaskGroupEnum.earnCoins,
                    differentTaskGroup.earnCoins,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => gameConfig.levelLifePerSec * gameConfig.levelDuration * Progression.GetCampainProgression((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y)),
                    (c) => "Earn " + c.NumberToTextInOneLine() + " coins"));
                differentTaskBuilderGroup.Add(new TaskConfig(taskType,
                    DifferentTaskGroupEnum.earnDiamonds,
                    differentTaskGroup.earnDiamonds,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt((int)HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) * 2),
                    (c) => "Earn " + c.NumberToTextInOneLine() + " diamonds"));
                differentTaskBuilderGroup.Add(new TaskConfig(taskType,
                    DifferentTaskGroupEnum.openChest,
                    differentTaskGroup.openChest,
                    (a) => (int)HelperClass.CustomNormalization(a, gameConfig.taskReward.x, gameConfig.taskReward.y),
                    (b) => Mathf.CeilToInt(HelperClass.CustomNormalization(b, gameConfig.taskTargetMultiplier.x, gameConfig.taskTargetMultiplier.y) / 3),
                    (c) => "Open " + c.NumberToTextInOneLine() + " chests"));
            #endregion
            this.allTasksInfo = allTasks;
    }

    public Tasks(TimeSpan updateSpan, TaskType taskType)
    {
        DateTime now = DateTime.Now;
        updateDate = new DateTime(now.Year, now.Month, now.Day + updateSpan.Days, updateSpan.Hours, 0, 0).ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        this.taskType = taskType;
        SetCurrentTasks();
        SaveManager.Instance.SaveLocal();
    }
    public List<Task> CurrentTasks { get => currentTasks;}
    public string UpdateDate { get => updateDate; }

    void SetCurrentTasks()
    {
        List<Task> chosenTasks = new List<Task>();
        foreach (var typedTasks in AllTasksInfo)
        {
            foreach (var v in typedTasks)
            {
                TaskConfig taskConfig = v; //HelperClass.GetRandomObjectsFromList(typedTasks).First();
                chosenTasks.Add(taskConfig.MakeTask());
            }
        }
        currentTasks = chosenTasks;
    }
    public unsafe class TaskConfig
    {
        public TaskType taskType;
        public Enum refEnum;
        public Struct refValue;
        public Func<float, int> rewardFunc;
        public Func<float, double> targetFunc;
        public Func<double, string> descriptionFunc;

        public TaskConfig(TaskType taskType, Enum refEnum, Struct refValue, Func<float, int> rewardFunc, Func<float, double> targetFunc, Func<double, string> descriptionFunc)
        {
            this.taskType = taskType;
            this.refEnum = refEnum;
            this.refValue = refValue;
            this.rewardFunc = rewardFunc;
            this.targetFunc = targetFunc;
            this.descriptionFunc = descriptionFunc;
        }
        public Task MakeTask()
        {
            return new Task(this);
        }
    }
    [Serializable]
    public unsafe class Task
    {
        TaskType taskType;
        Enum refEnum;
        public Struct refValue;
        public int reward;
        public double targetValue;
        public string description;

        public Task(TaskConfig taskBuilder)
        {
            float rand = Random.value;
            taskType = taskBuilder.taskType;
            refEnum = taskBuilder.refEnum;
            refValue = taskBuilder.refValue;
            reward = taskBuilder.rewardFunc(rand);
            targetValue = taskBuilder.targetFunc(rand);
            description = taskBuilder.descriptionFunc(targetValue);
        }
    }
}
public enum TaskType
{
    daily,
    weekly,
    monthly
}
public class TaskActiones
{
    private static TaskActiones instance;
    public static TaskActiones Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new TaskActiones();
            }
            return instance;
        }
    }

    public Action<double> DealDamage { get; set; }
    public Action<int> DefeatEnemies { get; set; }
    public Action<int> DefeatEnemiesByBullets { get; set; }
    public Action<int> DefeatEnemiesByBody { get; set; }
    public Action<int> DefeatSmallEnemies { get; set; }
    public Action<int> DefeatMediumEnemies { get; set; }
    public Action<int> DefeatBigEnemies { get; set; }
    public Action<int> DefeatBosses { get; set; }

    public Action<int> WinLevels { get; set; }
    public Action<int> WinLevelsWithNoDamageTaken { get; set; }
    public bool damageTaken;
    public Action<int> WinLevelsWithAlmostNoLives { get; set; }
    public Action<int> WinLevelsStandingStill { get; set; }
    public bool standStill = true;
    public Action<int> WinLevelsWithNoBonusesUsed { get; set; }
    public bool bonusUsed;
    public Action<int> WinLevelsWithoutSecondChance { get; set; }
    public Action<int> WinLevelsWithSecondChance { get; set; }

    public Action<double> TakeDamage { get; set; }
    public Action<double> ReflectDamage { get; set; }
    public Action<int> ShootBullets { get; set; }
    public Action<int> UseBonuses { get; set; }
    public Action<int> UseShieldBonus { get; set; }
    public Action<int> UseSpeedBonus { get; set; }
    public Action<int> UseHealthBonus { get; set; }
    public Action<int> UseSecondChance { get; set; }
    public Action<int> TakeBonusCoins { get; set; }

    public Action<int> UpgradeStats { get; set; }
    public Action<int> UpgradeBonusDrop { get; set; }
    public Action<int> UpgradeBulletDamage { get; set; }
    public Action<int> UpgradeBulletsPerSecond { get; set; }
    public Action<int> UpgradeCannonArmor { get; set; }
    public Action<int> UpgradeCannonHealth { get; set; }
    public Action<int> UpgradeCannonMoveForce { get; set; }

    public Action<int> OpenChest { get; set; }
    public Action<double> EarnCoins { get; set; }
    public Action<int> EarnDiamonds { get; set; }
    public Action<double> PlayTimeTotalSeconds { get; set; }

    TaskActiones()
    {
        List<Tasks> _tasks = new List<Tasks>() 
        {
            SaveManager.Instance.SavedValues.DailyTasks
        };
        _tasks.ForEach(a =>
        {
            DealDamage += (b) => SaveManager.Instance.SavedValues.DailyTasks.enemyTaskGroup.dealDamage.value += b;
            DefeatEnemies += (b) => SaveManager.Instance.SavedValues.DailyTasks.enemyTaskGroup.defeatEnemies.value += b;
            DefeatEnemiesByBullets += (b) => SaveManager.Instance.SavedValues.DailyTasks.enemyTaskGroup.defeatEnemiesByBullets.value += b;
            DefeatEnemiesByBody += (b) => SaveManager.Instance.SavedValues.DailyTasks.enemyTaskGroup.defeatEnemiesByBody.value += b;
            DefeatSmallEnemies += (b) => SaveManager.Instance.SavedValues.DailyTasks.enemyTaskGroup.defeatSmallEnemies.value += b;
            DefeatMediumEnemies += (b) => SaveManager.Instance.SavedValues.DailyTasks.enemyTaskGroup.defeatMediumEnemies.value += b;
            DefeatBigEnemies += (b) => SaveManager.Instance.SavedValues.DailyTasks.enemyTaskGroup.defeatBigEnemies.value += b;
            DefeatBosses += (b) => SaveManager.Instance.SavedValues.DailyTasks.enemyTaskGroup.defeatBosses.value += b;

            WinLevels += (b) => SaveManager.Instance.SavedValues.DailyTasks.levelTaskGroup.winLevels.value += b;
            WinLevelsWithNoDamageTaken += (b) =>
            {
                if (damageTaken == false)
                {
                    SaveManager.Instance.SavedValues.DailyTasks.levelTaskGroup.winLevelsWithNoDamageTaken.value += b;
                }
                damageTaken = false;
            };
            TakeDamage += (b) => damageTaken = true;
            WinLevels += WinLevelsWithNoDamageTaken;
            WinLevelsWithAlmostNoLives += (b) =>
            {
                if (CannonManager.Cannon.CurrentHealth / CannonManager.Cannon.MaximumHealth <= 0.1f)
                    SaveManager.Instance.SavedValues.DailyTasks.levelTaskGroup.winLevelsWithAlmostNoLives.value += b;
            };
            WinLevels += WinLevelsWithAlmostNoLives;
            WinLevelsStandingStill += (b) =>
            {
                if (standStill)
                {
                    SaveManager.Instance.SavedValues.DailyTasks.levelTaskGroup.winLevelsStandingStill.value += b;
                }
                standStill = true;
            };
            WinLevels += WinLevelsStandingStill;
            WinLevelsWithNoBonusesUsed += (b) =>
            {
                if (!bonusUsed)
                {
                    SaveManager.Instance.SavedValues.DailyTasks.levelTaskGroup.winLevelsWithNoBonusesUsed.value += b;
                }
            };
            WinLevels += WinLevelsWithNoBonusesUsed;
            WinLevelsWithoutSecondChance += (b) =>
            {
                if (!SecondChanceMenu.isTaken)
                {
                    SaveManager.Instance.SavedValues.DailyTasks.levelTaskGroup.winLevelsWithoutSecondChance.value += b;
                }
            };
            WinLevels += WinLevelsWithoutSecondChance;
            WinLevelsWithSecondChance += (b) =>
            {
                if (SecondChanceMenu.isTaken)
                {
                    SaveManager.Instance.SavedValues.DailyTasks.levelTaskGroup.winLevelsWithSecondChance.value += b;
                }
            };
            WinLevels += WinLevelsWithSecondChance;

            TakeDamage += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.takeDamage.value += b;
            ReflectDamage += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.reflectDamage.value += b;
            ShootBullets += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.shootBullets.value += b;
            UseBonuses += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.useBonuses.value += b;
            UseShieldBonus += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.useShieldBonus.value += b;
            UseSpeedBonus += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.useSpeedBonus.value += b;
            UseHealthBonus += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.useHealthBonus.value += b;
            UseSecondChance += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.useSecondChance.value += b;
            TakeBonusCoins += (b) => SaveManager.Instance.SavedValues.DailyTasks.cannonTaskGroup.takeBonusCoins.value += b;

            UpgradeStats += (b) => SaveManager.Instance.SavedValues.DailyTasks.upgradeTaskGroup.upgradeStats.value += b;
            UpgradeBonusDrop += (b) => SaveManager.Instance.SavedValues.DailyTasks.upgradeTaskGroup.upgradeBonusDrop.value += b;
            UpgradeBulletDamage += (b) => SaveManager.Instance.SavedValues.DailyTasks.upgradeTaskGroup.upgradeBulletDamage.value += b;
            UpgradeBulletsPerSecond += (b) => SaveManager.Instance.SavedValues.DailyTasks.upgradeTaskGroup.upgradeBulletsPerSecond.value += b;
            UpgradeCannonArmor += (b) => SaveManager.Instance.SavedValues.DailyTasks.upgradeTaskGroup.upgradeCannonArmor.value += b;
            UpgradeCannonHealth += (b) => SaveManager.Instance.SavedValues.DailyTasks.upgradeTaskGroup.upgradeCannonHealth.value += b;
            UpgradeCannonMoveForce += (b) => SaveManager.Instance.SavedValues.DailyTasks.upgradeTaskGroup.upgradeCannonMoveForce.value += b;

            OpenChest += (b) => SaveManager.Instance.SavedValues.DailyTasks.differentTaskGroup.openChest.value += b;
            EarnCoins += (b) => SaveManager.Instance.SavedValues.DailyTasks.differentTaskGroup.earnCoins.value += b;
            EarnDiamonds += (b) => SaveManager.Instance.SavedValues.DailyTasks.differentTaskGroup.earnDiamonds.value += b;

        });
    }

}
[Serializable]
public class EnemyTaskGroup
{
    public Double dealDamage = new Double();
    public Int defeatEnemies = new Int();
    public Int defeatEnemiesByBullets = new Int();
    public Int defeatEnemiesByBody = new Int();
    public Int defeatSmallEnemies = new Int();
    public Int defeatMediumEnemies = new Int();
    public Int defeatBigEnemies = new Int();
    public Int defeatBosses = new Int();
}

[Serializable]
public class LevelTaskGroup
{
    public Int winLevels = new Int();
    public Int winLevelsWithNoDamageTaken = new Int();
    public Int winLevelsWithAlmostNoLives = new Int();
    public Int winLevelsStandingStill = new Int();
    public Int winLevelsWithNoBonusesUsed = new Int();
    public Int winLevelsWithoutSecondChance = new Int();
    public Int winLevelsWithSecondChance = new Int();
}

[Serializable]
public class CannonTaskGroup
{
    public Double takeDamage = new Double();
    public Double reflectDamage = new Double();
    public Int shootBullets = new Int();
    public Int useBonuses = new Int();
    public Int useShieldBonus = new Int();
    public Int useSpeedBonus = new Int();
    public Int useHealthBonus = new Int();
    public Int useSecondChance = new Int();
    public Int takeBonusCoins = new Int();
}

[Serializable]
public class UpgradeTaskGroup
{
    public Int upgradeStats = new Int();
    public Int upgradeBonusDrop = new Int();
    public Int upgradeBulletDamage = new Int();
    public Int upgradeBulletsPerSecond = new Int();
    public Int upgradeCannonArmor = new Int();
    public Int upgradeCannonHealth = new Int();
    public Int upgradeCannonMoveForce = new Int();
}

[Serializable]
public class DifferentTaskGroup
{
    public Int openChest = new Int();
    public Double earnCoins = new Double();
    public Int earnDiamonds = new Int();
}
[Serializable]
public abstract class Struct
{
    public double value
    {
        get
        {
            if (this is Int)
            {
                return (this as Int).value;
            }
            else if (this is Double)
            {
                return (this as Double).value;
            }
            else
                return new double();
        }
    }
}
[Serializable]
public class Double : Struct
{
    public new double value;
    
}
[Serializable]
public class Int : Struct
{
    public new int value;
}
public enum EnemyTaskGroupEnum
{
    dealDamage,
    defeatEnemies,
    defeatEnemiesByBullets,
    defeatEnemiesByBody,
    defeatSmallEnemies,
    defeatMediumEnemies,
    defeatBigEnemies,
    defeatBosses
}
public enum LevelTaskGroupEnum
{
    winLevels,
    winLevelsWithNoDamageTaken,
    winLevelsWithAlmostNoLives,
    winLevelsStandingStill,
    winLevelsWithNoBonusesUsed,
    winLevelsWithoutSecondChance,
    winLevelsWithSecondChance
}
public enum CannonTaskGroupEnum
{
    takeDamage,
    reflectDamage,
    shootBullets,
    useBonuses,
    useShieldBonus,
    useSpeedBonus,
    useHealthBonus,
    useSecondChance,
    takeBonusCoins,
}
public enum UpgradeTaskGroupEnum
{
    upgradeStats,
    upgradeBonusDrop,
    upgradeBulletDamage,
    upgradeBulletsPerSecond,
    upgradeCannonArmor,
    upgradeCannonHealth,
    upgradeCannonMoveForce,
}
public enum DifferentTaskGroupEnum
{
    openChest,
    earnCoins,
    earnDiamonds,
}
