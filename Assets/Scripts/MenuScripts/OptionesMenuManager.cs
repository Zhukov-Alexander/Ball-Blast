using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionesMenuManager : MonoBehaviour
{
    public static Action OnExit;
    public static Action OnEnter;
    public GameObject soundProhibitIcon;
    public GameObject vibrationProhibitIcon;
    private void Awake()
    {
        UpdateButtons();
    }
    public void Enter()
    {
        OnEnter?.Invoke();
    }
    public void Exit()
    {
        OnExit?.Invoke();
        Destroy(gameObject);
    }
    public static void UpdateState()
    {
        if (SavedValues.Instance.Sound)
        {
            SoundManager.Instance.SetMaster(0);
        }
        else
        {
            SoundManager.Instance.SetMaster(-80);
        }
        if (SavedValues.Instance.Vibration)
        {
        }
        else
        {
        }
    }
    public void UpdateButtons()
    {
        if (SavedValues.Instance.Sound)
        {
            soundProhibitIcon.SetActive(false);
        }
        else
        {
            soundProhibitIcon.SetActive(true);
        }
        if (SavedValues.Instance.Vibration)
        {
            vibrationProhibitIcon.SetActive(false);
        }
        else
        {
            vibrationProhibitIcon.SetActive(true);
        }
    }
    public void Sound()
    {
        SavedValues.Instance.Sound = SavedValues.Instance.Sound == true ? SavedValues.Instance.Sound = false :  SavedValues.Instance.Sound = true;
        UpdateState();
        UpdateButtons();
    }
    public void Vibration()
    {
        SavedValues.Instance.Vibration = SavedValues.Instance.Vibration == true ? SavedValues.Instance.Vibration = false : SavedValues.Instance.Vibration = true;
        UpdateState();
        UpdateButtons();
    }
    public void Language()
    {

    }
    public void Account()
    {

    }
    public void RateUs()
    {

    }
    public void Share()
    {

    }
}
