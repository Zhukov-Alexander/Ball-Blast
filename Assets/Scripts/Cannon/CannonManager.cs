﻿using System;
using System.Collections.Generic;
using UnityEngine;
using static GameConfigContainer;

public class CannonManager : MonoBehaviour
{

    public static Cannon Cannon { get; set; }
    public static Action OnCannonInstantiated { get; set; }

    private void Awake()
    {
        CannonMenuManager.OnExit += UpdateCannon;
        SceneMenuManager.OnExit += UpdateCannon;
        OptionesMenuManager.OnExit += UpdateCannon;
        MainMenu.OnFirstEnter += UpdateCannon;
    }
    void UpdateCannon()
    {
        if (Cannon != null)
        {
            Destroy(Cannon.gameObject);
        }
        InstantiateCannon();
    }
    void InstantiateCannon()
    {
        GameObject cannonGO = Instantiate(gameConfig.cannonPrefabs[SaveManager.Instance.SavedValues.CannonPrefabNumber], gameConfig.cannonInstantiatePosition, Quaternion.identity);
        Cannon = cannonGO.GetComponent<Cannon>();
        Cannon.PrefabNumber = SaveManager.Instance.SavedValues.CannonPrefabNumber;
        Cannon.UpdateProperties();
        //OnCannonInstantiated();
    }

}
