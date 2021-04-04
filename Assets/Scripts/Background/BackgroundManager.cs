using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConfigContainer;

public class BackgroundManager : MonoBehaviour
{
    public static Background Background { get; set; }
    public static Action OnBackgroundInstantiated { get; set; }

    private void Awake()
    {
        CannonMenuManager.OnExit += UpdateBackground;
        SceneMenuManager.OnExit += UpdateBackground;
        StartMenu.OnFirstEnter += UpdateBackground;
    }
    void UpdateBackground()
    {
        if (Background != null)
        {
            Destroy(Background.gameObject);
        }
        InstantiateBackground();
    }
    void InstantiateBackground()
    {
        GameObject backgroundGO = Instantiate(gameConfig.backgrounds[SaveManager.Instance.SavedValues.ScenePrefabNumber], gameConfig.backgroundPrefabPosition, Quaternion.identity);
        Background = backgroundGO.GetComponent<Background>();
        Background.UpdateProperties();
        //OnBackgroundInstantiated();
    }
}
