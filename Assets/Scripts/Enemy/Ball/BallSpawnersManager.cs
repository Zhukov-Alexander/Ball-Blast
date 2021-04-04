using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameConfigContainer;

public class BallSpawnersManager : MonoBehaviour
{
    [SerializeField] List<BallSpawner> ballSpawners;
    public static float totalLevelLives;
    public static float mediumBallLives;
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
    }

    private void CalculateLevel()
    {
        float levelLives = gameConfig.levelLifePerSec * gameConfig.levelDuration * Progression.GetCampainProgression();
        ballSpawnSettings = CalculateBallSpawnSettings(levelLives, gameConfig.levelDuration);
        OnLevelCalculated();
    }

    private void DestroyBalls()
    {
        foreach (var v in FindObjectsOfType<Ball>().ToList())
        {
            Destroy(v.gameObject);
        }
    }
    void StartShooting()
    {
        shooting = ShootingBalls();
        StartCoroutine(shooting);
    }
    void EndShooting()
    {
        StopCoroutine(shooting);
    }

    IEnumerator ShootingBalls()
    {
        for (int i = 0; i < ballSpawnSettings.Count; i++)
        {
            BallSpawnSettings move = ballSpawnSettings[i];
            ballSpawners[move.spawner].Shoot(move.lives, move.type, i);
            yield return new WaitForSeconds(move.waiting);
        }
    }
    List<BallSpawnSettings> CalculateBallSpawnSettings(float levelLives, float levelDuration)
    {
        List<BallSpawnSettings> moves = new List<BallSpawnSettings>();

        int shots = (int)(gameConfig.levelShotsPerSec * levelDuration);
        List<float> ballsSizesWeights = HelperClass.GetTypeWeights(gameConfig.ballTypeProbabilities.Count, HelperClass.BasedTreeOfPow(2));
        List<int> sizes = HelperClass.GetListOfTypedIndexes(gameConfig.ballTypeProbabilities, ballsSizesWeights, shots);




        float insideLevelDifficultyProgression; 
        List<float> ballsLives = new List<float>();
        insideLevelDifficultyProgression = gameConfig.insideCampainDifficultyProgressionPerSec * levelDuration;
        ballsLives = HelperClass.GetListWithLinearlyIncreasingValuesBasedOnTotalValue(levelLives, shots, insideLevelDifficultyProgression);

        List<float> ballsLivesMixed = HelperClass.MixValues(ballsLives, gameConfig.amountOfBallsToMixSimultaneosly, gameConfig.ballsMixCoef, gameConfig.ballMixCycles);

        //Debug.Log("ballsLivesMixed " + ballsLivesMixed.Count + "/ sizes " + sizes.Count);
        List<float> ballsLivesResized = HelperClass.GetListWithAveragedValues(ballsLivesMixed, sizes, out float averageValue);
        mediumBallLives = averageValue;

        List<float> waitings = HelperClass.GetListWithEqualValues(levelDuration, sizes.Count, out float averageTimeBenweenShots);
        List<float> waitingsMixed = HelperClass.MixValues(waitings, gameConfig.amountOfWaitingsToMixSimultaneosly, gameConfig.waitingMixCoef, gameConfig.waitingMixCycles);

        List<int> spawners = HelperClass.GetListOfRandomInts(sizes.Count, 0, 1);

        for (int i = 0; i < sizes.Count; i++)
        {
            moves.Add(new BallSpawnSettings(sizes[i], ballsLivesResized[i], spawners[i], waitingsMixed[i]));
        }
        float f = 0;
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
    public float lives;
    public int spawner;
    public float waiting;
    public BallSpawnSettings(int type, float life, int spawner, float waiting)
    {
        this.type = type;
        this.lives = life;
        this.spawner = spawner;
        this.waiting = waiting;
    }
}

