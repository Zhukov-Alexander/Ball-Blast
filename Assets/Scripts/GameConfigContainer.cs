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
        SaveManager.Instance.DeleteLocalSave();
    }
    [ContextMenu("GetMoney")]
    public void GetMoney()
    {
        SaveManager.Instance.SavedValues.Coins += Mathf.Pow(10,14);
        SaveManager.Instance.SavedValues.Diamonds += 1000;
    }
}
