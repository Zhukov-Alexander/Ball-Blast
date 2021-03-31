using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using static GameConfigContainer;
using System.ArrayExtensions;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsTMP;
    [SerializeField] TextMeshProUGUI diamondsTMP;

    private void Awake()
    {
        ResultsPanel.OnHide += CollectCurrency;
        StartMenu.OnEnter += SetCoins;
        StartMenu.OnEnter+=SetDiamonds;
        Ball.OnDestroy += SpawnCoinsCampain;
        Boss.OnDestroy += SpawnCoinsBossfight;
        Boss.OnDestroy += (a,b) => SpawnDiamonds(b);
        SavedValues.Instance.OnCoinsChanged += SetCoins;
        SavedValues.Instance.OnDiamondsChanged += SetDiamonds;
    }

    private void CollectCurrency()
    {
        FindObjectsOfType<Currency>().ToList().ForEach(a=>a.Collect());
    }

    void SpawnCoinsCampain(float ballInitialLives, Vector2 ballPosition)
    {
        SpawnCoins(ballInitialLives, ballPosition, Progression.GetCampainProgression(), 1, Random.Range(gameConfig.randomCoinsMultiplyer.x, gameConfig.randomCoinsMultiplyer.y));
    }
    void SpawnCoinsBossfight(float ballInitialLives, Vector2 ballPosition)
    {
        SpawnCoins(ballInitialLives, ballPosition, Progression.GetBossfightProgression(), gameConfig.bossfightToCampainModMoneyMultiplyer);
    }
    void SpawnCoins(float ballInitialLives, Vector2 ballPosition, float progression, float modMultiplyer, float randomMultiplyer = 1)
    {
        float amount = ballInitialLives * modMultiplyer * randomMultiplyer;
        List<float> coinWeights = gameConfig.coinWeights.Select(a => (progression * modMultiplyer * a)).ToList();
        List<int> coinsTypes = HelperClass.GetListOfTypedIndexes(gameConfig.coinProbabilities, coinWeights, amount);
        StartCoroutine(InstantiateCoins(coinsTypes, ballPosition, coinWeights));
    }
    IEnumerator InstantiateCoins(List<int> coinsTypes, Vector2 ballPosition, List<float> coinWeight)
    {
        for (int i = 0; i < coinsTypes.Count; i++)
        {
            if (i % 10 == 0) yield return new WaitForEndOfFrame();

            GameObject coinGO = Instantiate(gameConfig.coinPrefabs[coinsTypes[i]], ballPosition, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
            coinGO.GetComponent<Rigidbody2D>().AddForce(HelperClass.RandomDirecton(), ForceMode2D.Impulse);
            Coin coin = coinGO.GetComponent<Coin>();
            coin.Weight = coinWeight[coinsTypes[i]];
        }
    }
    void SpawnDiamonds(Vector2 position)
    {
        for (int i = 0; i < gameConfig.amountOfSpawnedDiamonds; i++)
        {
            GameObject diamondGO = Instantiate(gameConfig.diamondPrefab, position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
            Diamond diamond = diamondGO.GetComponent<Diamond>();
            diamond.Weight = SavedValues.Instance.BossfightLevel;
        }
    }
    public void SetCoins()
    {
        coinsTMP.text = SavedValues.Instance.Coins.NumberToTextInOneLineWithoutFraction();
        
    }
    public void SetDiamonds()
    {
        diamondsTMP.text = SavedValues.Instance.Diamonds.NumberToTextInOneLineWithoutFraction();
    }


}
