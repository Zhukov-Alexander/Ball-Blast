using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameConfigContainer;

public class BallSpawnersManager : MonoBehaviour
{
    [SerializeField] List<BallSpawner> ballSpawners;
    public static double totalLevelLives;
    public static double mediumBallLives;
    List<BallSpawnSettings> ballSpawnSettings;

    public static Action OnLevelCalculated { get; set; }
    IEnumerator shooting;

    private void Awake()
    {
        LevelMenu.OnStartCampain += CalculateLevel;
        OnLevelCalculated += StartShooting;
        LevelMenu.OnEndCampainLose += (EndShooting);
        LevelMenu.OnEndCampainWin += (EndShooting);
        LevelMenu.AddToEnd(DestroyBalls);
        LastChanceMenu.OnLastChanceTaken += RestartShootingFromCurrentState;
    }

    private void CalculateLevel()
    {
        double levelLives = gameConfig.levelLifePerSec * gameConfig.levelDuration * Progression.GetCampainProgression();
        ballSpawnSettings = CalculateBallSpawnSettings(levelLives, gameConfig.levelDuration);
        OnLevelCalculated();
    }

    private void DestroyBalls()
    {
        foreach (var v in FindObjectsOfType<Ball>().ToList())
        {
            v.Destruction(false);
        }
    }
    void StartShooting()
    {
        shooting = ShootingBalls(ballSpawnSettings.Count);
        StartCoroutine(shooting);
    }
    void EndShooting()
    {
        StopCoroutine(shooting);
    }
    void RestartShootingFromCurrentState()
    {
        EndShooting();
        List<Ball> balls = FindObjectsOfType<Ball>().OrderBy(a => a.Number).ToList();
        balls.ForEach(a => a.ballSpawnSettings = new BallSpawnSettings(a.ballSpawnSettings.type, a.CurrentLives, a.ballSpawnSettings.spawner, a.ballSpawnSettings.waiting));
        List<BallSpawnSettings> settings = balls.Select(a => a.ballSpawnSettings).ToList();
        ballSpawnSettings.InsertRange(0, settings);
        DestroyBalls();
        StartShooting();
    }
    IEnumerator ShootingBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            BallSpawnSettings move = ballSpawnSettings[0];
            ballSpawnSettings.RemoveAt(0);
            ballSpawners[move.spawner].Shoot(move, i);
            yield return new WaitForSeconds(move.waiting);
        }
    }
    List<BallSpawnSettings> CalculateBallSpawnSettings(double levelLives, float levelDuration)
    {
        List<BallSpawnSettings> moves = new List<BallSpawnSettings>();

        int shots = (int)(gameConfig.levelShotsPerSec * levelDuration);
        List<float> ballsSizesWeights = HelperClass.GetTypeWeights(gameConfig.ballTypeProbabilities.Count, HelperClass.BasedTreeOfPow(2));
        List<int> sizes = HelperClass.GetListOfTypedIndexes(gameConfig.ballTypeProbabilities, ballsSizesWeights, shots);




        List<double> ballsLives = new List<double>();
        float insideLevelDifficultyProgression = gameConfig.insideCampainDifficultyProgressionPerSec * levelDuration;
        ballsLives = HelperClass.GetListWithLinearlyIncreasingValuesBasedOnTotalValue(levelLives, shots, insideLevelDifficultyProgression);

        List<double> ballsLivesMixed = HelperClass.MixValues(ballsLives, gameConfig.amountOfBallsToMixSimultaneosly, gameConfig.ballsMixCoef, gameConfig.ballMixCycles);

        List<double> ballsLivesResized = HelperClass.GetListWithAveragedValues(ballsLivesMixed, sizes, out double averageValue);
        mediumBallLives = averageValue;

        List<float> waitings = HelperClass.GetListWithEqualValues(levelDuration, sizes.Count, out float averageTimeBenweenShots);
        List<float> waitingsMixed = HelperClass.MixValues(waitings, gameConfig.amountOfWaitingsToMixSimultaneosly, gameConfig.waitingMixCoef, gameConfig.waitingMixCycles);

        List<int> spawners = HelperClass.GetListOfRandomInts(sizes.Count, 0, 1);

        for (int i = 0; i < sizes.Count; i++)
        {
            moves.Add(new BallSpawnSettings(sizes[i], ballsLivesResized[i], spawners[i], waitingsMixed[i]));
        }
        double f = 0;
        moves.ForEach(a => f += a.lives * HelperClass.TreeOfPow(2, a.type));
        totalLevelLives = f;

        /*string v = "";
        foreach (var item in moves)
        {
            v += "type: " + item.type + " lives: " + item.lives + " spawner: " + item.spawner + " waiting: " + item.waiting + Environment.NewLine;
        }
        Debug.Log(v);*/
        return moves;
    }
}
public class BallSpawnSettings
{
    public int type;
    public double lives;
    public int spawner;
    public float waiting;
    public BallSpawnSettings(int type, double life, int spawner, float waiting)
    {
        this.type = type;
        this.lives = life;
        this.spawner = spawner;
        this.waiting = waiting;
    }
}

