using System;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigContainer : MonoBehaviour
{
    [SerializeField] GameConfiguration gameConfiguration;
    public static GameConfiguration gameConfig;
    private void Awake()
    {
        gameConfig = gameConfiguration;
    }
    [ContextMenu("DeleteSave")]
    public void DeleteSave()
    {
        SavedValues.DeleteSave();
    }
    [ContextMenu("GetMoney")]
    public void GetMoney()
    {
        SavedValues.Instance.Coins += Mathf.Pow(10,14);
        SavedValues.Instance.Diamonds += 1000;
    }
}
