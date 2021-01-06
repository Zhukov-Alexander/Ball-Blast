using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LibraryOfUsefulAlgorithms;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameConfig gameConfig;
    [SerializeField] BallSpawner ballSpawner1;
    [SerializeField] BallSpawner ballSpawner2;
    public List<Move> moves;
    public List<Ball> balls = new List<Ball>();
    public float MediumBallLife { get; set; }
    public void StartPlayingLevel()
    {
        StartCoroutine(PlayLevel());
    }
    public void StopPlayingLevel()
    {
        StopCoroutine(PlayLevel());
        DestroyBalls();
    }

    private void DestroyBalls()
    {
        foreach (var v in balls.ToList())
        {
            balls.Remove(v);
            v.Destroy();
        }
    }

    IEnumerator PlayLevel()
    {
        CalculateLevel();
        foreach (Move move in moves)
        {
            if(move.spawner == 1)
            {
                ballSpawner1.Shoot(move.life, move.size);
            }
            else
            {
                ballSpawner2.Shoot(move.life, move.size);
            }
            yield return new WaitForSeconds(move.waiting);
        }
    }
    void CalculateLevel()
    {
        moves = new List<Move>();

        List<int> sizes = FirstPage.GetListOfTypedIndexes(gameConfig.ballSizeProbabilities, gameConfig.baseLevelShots, out int amountOfShots);

        List<float> ballsLives = FirstPage.GetListWithLinearlyIncreasingWeightedValues(gameConfig.baseLevelLife, gameConfig.baseLevelShots, gameConfig.insideLevelDifficultyProgression);
        List<float> ballsLivesMixed = FirstPage.MixValues(ballsLives, gameConfig.amountOfBallsToMixSimultaneosly, gameConfig.ballsMixCoef, gameConfig.ballMixCycles);

        List<float> ballsLivesResized = FirstPage.GetListWithAveragedValues(ballsLivesMixed, sizes, out float averageValue);
        MediumBallLife = averageValue;

        List<float> waitings = FirstPage.GetListWithEqualValues(gameConfig.baseLevelDuration, amountOfShots, out float averageTimeBenweenShots);
        List<float> waitingsMixed = FirstPage.MixValues(waitings, gameConfig.amountOfWaitingsToMixSimultaneosly, gameConfig.waitingMixCoef, gameConfig.waitingMixCycles);

        List<int> spawners = FirstPage.GetListOfRandomInts(amountOfShots, 0, 1);



        for (int i = 0; i < amountOfShots; i++)
        {
            moves.Add(new Move(sizes[i], ballsLivesResized[i], spawners[i], waitingsMixed[i]));
        }
        string str = "";
        for (int i = 0; i < moves.Count; i++)
        {
            Move v = moves[i];
            str += i + ". size: " + v.size + ". life: " + v.life + ". spawner: " + v.spawner + ". waiting: " + v.waiting + Environment.NewLine;
        }
        Debug.Log(str);
    }
}
public class Move
{
    public int size;
    public float life;
    public int spawner;
    public float waiting;
    public Move(int size, float life, int spawner, float waiting)
    {
        this.size = size;
        this.life = life;
        this.spawner = spawner;
        this.waiting = waiting;
    }
}
