using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
public class GameConfiguration : ScriptableObject
{
    public double epsilon;

    [Header("BallSpawner")]
    public List<GameObject> ballPrefabs;
    public Vector2Int ballSpawnerShootAngles;
    public Vector2 ballSpawnerForceToShoot;
    public Vector2 torqueAmountMinMax;

    [Space]
    [Header("Ball")]
    public Gradient ballColorGradient;
    public List<float> ballTypeProbabilities;
    public Vector2Int ballShotAngles;
    public Vector2 ballForceToShoot;
    public float timeToDownScaleEnemy;
    public AnimationCurve ScaleEnemyAnimationCurve;

    [Space]
    [Header("Cannon")]
    public float bulletDamage;
    public float cannonMoveForce;
    public float bonusProbability;
    public float bulletsPerSecond;
    public float bulletsSpeed;
    public float health;
    public float armor;
    public Vector3 cannonInstantiatePosition;
    public List<GameObject> cannonPrefabs;
    public float moveBuffer;
    public float cannonSliderDurationCoef;

    [Space]
    [Header("LevelManager")]
    public float levelLifePerSec;
    public float levelDuration;
    public float levelShotsPerSec;
    public float insideCampainDifficultyProgressionPerSec;
    public int amountOfBallsToMixSimultaneosly;
    public float ballsMixCoef;
    public int ballMixCycles;
    public int amountOfWaitingsToMixSimultaneosly;
    public float waitingMixCoef;
    public int waitingMixCycles;
    public float maxBallPositionY;
    public float minBallPositionY;
    public float ballChangePosForceY;
    public float maxBallAngularVelocity;
    public float baseBallAngularDrag;
    public float ballAngularDragCoef;

    [Space]
    [Header("Trajectory")]
    public int simulatedIterationesOfTrajectory;
    public GameObject mockBallPrefab;

    [Space]
    [Header("CurrencyManager")]
    public List<float> coinProbabilities;
    public List<int> coinWeights;
    public Vector2 randomCoinsMultiplyer;


    [Space]
    [Header("Diamond")]
    public int amountOfSpawnedDiamonds;
    public GameObject diamondPrefab;
    public Color diamondColor;

    [Space]
    [Header("Coin")]
    public List<GameObject> coinPrefabs;
    public Color coinColor;


    [Space]
    [Header("UpgradePanal")]
    public Color activeHeaderColor;
    public Color passiveHeaderColor;
    public Color activeUpgradeColor;
    public Color passiveUpgradeColor;
    public string firstUpgradeTabTag;


    [Space]
    [Header("Boss")]
    public List<GameObject> bossPrefabs;
    public Vector3 bossPrefabPosition;
    public float bossLives;
    public float bossDamage;
    public float bossArmor;
    public float bossSpeed;
    public float bossfightToCampainModMoneyMultiplyer;

    [Space]
    [Header("CannonMenu")]
    public int baseCannonCost;
    public float snapMoveCoef;
    public float minDistance;
    public float minScrollVelocity;
    public float cannonMoveToScrollItemTime;
    public float constSnapMove;

    [Space]
    [Header("Background")]
    public List<GameObject> backgrounds;
    public Vector3 backgroundPrefabPosition;
    public Color activeButtonColor;
    public Color passiveButtonColor;

    [Space]
    [Header("ResultsMenu")]
    public float adMoneyMultiplyer;

    [Space]
    [Header("Bonuses")]
    public float bonusHealthCoef;
    public float bonusShieldTime;
    public float bonusSpeedCoef;
    public float bonusSpeedTime;
}
