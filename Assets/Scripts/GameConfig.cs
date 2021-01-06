using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("BallSpawner")]
    public GameObject ballPrefab;
    public int minBallSpawnerShootAngle;
    public int maxBallSpawnerShootAngle;
    public float ballSpawnerForceToShoot;

    [Space]
    [Header("Cannon")]
    public Vector2 forceToMoveCannon;
    public int bulletDamage;
    public float moveBuffer;

    [Space]
    [Header("Ball")]
    public List<float> ballColorThresholds;
    public List<Color> ballColors;
    public List<float> ballScales;
    public int minBallShotAngle;
    public int maxBallShotAngle;
    public float ballForceToShoot;

    [Space]
    [Header("LevelManager")]
    public int baseLevelLife;
    public float baseLevelDuration;
    public int baseLevelShots;
    public float insideLevelDifficultyProgression;
    public int amountOfBallsToMixSimultaneosly;
    public float ballsMixCoef;
    public int ballMixCycles;
    public int amountOfWaitingsToMixSimultaneosly;
    public float waitingMixCoef;
    public int waitingMixCycles;
    public List<float> ballSizeProbabilities;

    [Space]
    [Header("Trajectory")]
    public int simulatedIterationesOfTrajectory;
    public GameObject mockBallPrefab;
}
