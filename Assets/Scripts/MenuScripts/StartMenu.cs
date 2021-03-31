using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject touchPanel;
    [SerializeField] GameObject optionesMenu;
    [SerializeField] GameObject sceneMenuManager;
    [SerializeField] GameObject cannonMenuManager;
    public static Action OnExit { get; set; }
    public static Action OnEnter { get; set; }
    public static Action OnFirstEnter { get; set; }
    private bool canStartLevel = true;


    private void Awake()
    {
        OnEnter += () => UIAnimation.Open(gameObject);
        OnExit += () => UIAnimation.Close(gameObject);
        LevelMenu.AddToStart(() => OnExit());
        ResultsPanel.OnHide += () => OnEnter();
        CannonMenuManager.OnEnter += () => OnExit();
        CannonMenuManager.OnExit += () => OnEnter();
        SceneMenuManager.OnEnter += () => OnExit();
        SceneMenuManager.OnExit += () => OnEnter();
        OptionesMenuManager.OnExit += () => OnEnter();
        OnFirstEnter += OptionesMenuManager.UpdateState;
        OnExit += () => canStartLevel = false;
        OnEnter += () => canStartLevel = true;
    }
    private void Start()
    {
        OnFirstEnter();
        OnEnter();
    }
    public void OpenCannonMenu()
    {
        SoundManager.Instance.Button();
        UIAnimation.Open(Instantiate(cannonMenuManager, GetComponentInParent<Canvas>().transform, false));
        OnExit();
    }
    public void OpenSceneMenu()
    {
        SoundManager.Instance.Button();
        UIAnimation.Open(Instantiate(sceneMenuManager, GetComponentInParent<Canvas>().transform, false));
        OnExit();
    }
    public void OpenOptionesMenu()
    {
        SoundManager.Instance.Button();
        UIAnimation.Open(Instantiate(optionesMenu, GetComponentInParent<Canvas>().transform, false));
        OnExit();
    }
    private void FixedUpdate()
    {
        if (canStartLevel == true)
        {
            if ((Input.touchCount > 0 && HelperClass.IsTouchOverObject(touchPanel)) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartGame();
            }
        }
    }
    public void StartGame()
    {
        if (LevelModManager.CurrentLevelMod == LevelMod.Campain)
        {
            LevelMenu.OnStartCampain();
        }
        else if (LevelModManager.CurrentLevelMod == LevelMod.Bossfight)
        {
            LevelMenu.OnStartBossfight();
        }
    }

}
