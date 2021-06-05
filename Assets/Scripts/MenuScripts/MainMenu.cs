using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    [SerializeField] GameObject touchPanel;

    private void Awake()
    {
        ResultsPanel.OnHide += () => OnEnter();
    }
    void OpenMenu(IMenu menu)
    {
        menu.Open();
        OnExit();
    }
    public void OpenCannonMenu()
    {
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
    public void OpenTasksMenu()
    {
        SoundManager.Instance.Button();
        UIAnimation.Open(Instantiate(dailyTasksMenu, GetComponentInParent<Canvas>().transform, false));
        //OnExit();
    }
    public void OpenStoreMenu()
    {
        SoundManager.Instance.Button();
        UIAnimation.Open(Instantiate(storeMenu, GetComponentInParent<Canvas>().transform, false));
        OnExit();
    }
    private void FixedUpdate()
    {
        if ((Input.touchCount > 0 && HelperClass.IsTouchOverObject(touchPanel)) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        UIAnimation.Open(levelMenu);
        OnExit();
    }
}
