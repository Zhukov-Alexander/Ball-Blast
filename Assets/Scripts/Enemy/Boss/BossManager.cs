using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameConfigContainer;

public class BossManager : MonoBehaviour
{
    public static Action OnBossInstantiated { get; set; }
    public static Boss Boss { get; set; }
    
    private void Awake()
    {
        LevelMenu.OnStartBossfight += InstantiateBoss;
        LevelMenu.AddToEnd(DestroyBoss);
    }
    void InstantiateBoss()
    {
        Boss = Instantiate(gameConfig.bossPrefabs[(SaveManager.Instance.SavedValues.BossfightLevel - 1) % (gameConfig.bossPrefabs.Count)], gameConfig.bossPrefabPosition, Quaternion.identity, transform).GetComponent<Boss>();
        OnBossInstantiated();
    }

    private static void DestroyBoss()
    {
        foreach (var v in FindObjectsOfType<Boss>().ToList())
        {
            Destroy(v.gameObject);
        }
    }
}
