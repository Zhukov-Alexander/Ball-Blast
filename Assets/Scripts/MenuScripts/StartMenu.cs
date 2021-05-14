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
    [SerializeField] GameObject levelMenu;
    public static Action OnExit { get; set; }
    public static Action OnEnter { get; set; }
    public static Action OnFirstEnter { get; set; }
    private bool canStartLevel = true;


    private void Awake()
    {
        OnEnter += () => UIAnimation.Open(gameObject, false);
        OnExit += () => UIAnimation.Close(gameObject, false);
        LevelMenu.AddToStart(() => OnExit());
        ResultsPanel.OnHide += () => OnEnter();
        CannonMenuManager.OnEnter += () => OnExit();
        CannonMenuManager.OnExit += () => OnEnter();
        SceneMenuManager.OnEnter += () => OnExit();
        SceneMenuManager.OnExit += () => OnEnter();
        OptionesMenuManager.OnExit += () => OnEnter();
        OnFirstEnter += OptionesMenuManager.UpdateState;
        OnFirstEnter += () => OnEnter();
        OnExit += () => canStartLevel = false;
        OnEnter += () => canStartLevel = true;

        Action onFirstEnter = () => OnFirstEnter();
        SaveManager.Instance.OnLoaded += onFirstEnter;
        OnFirstEnter += () => SaveManager.Instance.OnLoaded -= onFirstEnter;

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
        UIAnimation.Open(levelMenu);
    }

}
