using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject optionesMenu;
    [SerializeField] GameObject sceneMenuManager;
    [SerializeField] GameObject cannonMenuManager;
    [SerializeField] Animator animator;
    public static Action OnExit { get; set; }
    public static Action OnEnter { get; set; }
    public static Action OnFirstEnter { get; set; }

    private void Awake()
    {
        OnEnter += () => animator.SetTrigger("Open");
        OnExit += () => animator.SetTrigger("Close");
        LevelMenu.AddToStart(() => OnExit());
        ResultsPanel.OnHide += () => OnEnter();
        CannonMenuManager.OnEnter += () => OnExit();
        CannonMenuManager.OnExit += () => OnEnter();
        SceneMenuManager.OnEnter += () => OnExit();
        SceneMenuManager.OnExit += () => OnEnter();
        OptionesMenuManager.OnExit += () => OnEnter();
        OnFirstEnter += OptionesMenuManager.UpdateState;
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
}
