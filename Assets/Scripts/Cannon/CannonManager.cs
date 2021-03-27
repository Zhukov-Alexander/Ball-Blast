using System;
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
        StartMenu.OnFirstEnter += UpdateCannon;
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
        GameObject cannonGO = Instantiate(gameConfig.cannonPrefabs[SavedValues.Instance.CannonPrefabNumber], gameConfig.cannonInstantiatePosition, Quaternion.identity);
        Cannon = cannonGO.GetComponent<Cannon>();
        Cannon.PrefabNumber = SavedValues.Instance.CannonPrefabNumber;
        Cannon.UpdateProperties();
        //OnCannonInstantiated();
    }

}
