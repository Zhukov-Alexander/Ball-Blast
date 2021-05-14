using System;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class OptionesMenuManager : MonoBehaviour
{
    [SerializeField] LocalizedString textLS;
    [SerializeField] LocalizedString titleLS;
    [SerializeField] LocalizedString localizedString;
    [SerializeField] TextMeshProUGUI accountTMP;
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
        if (SaveManager.Instance.SavedValues.Sound)
        {
            SoundManager.Instance.SetMaster(0);
        }
        else
        {
            SoundManager.Instance.SetMaster(-80);
        }
        if (SaveManager.Instance.SavedValues.Vibration)
        {
        }
        else
        {
        }
    }
    public void UpdateButtons()
    {
        if (SaveManager.Instance.SavedValues.Sound)
        {
            soundProhibitIcon.SetActive(false);
        }
        else
        {
            soundProhibitIcon.SetActive(true);
        }
        if (SaveManager.Instance.SavedValues.Vibration)
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
        SaveManager.Instance.SavedValues.Sound = SaveManager.Instance.SavedValues.Sound == true ? SaveManager.Instance.SavedValues.Sound = false :  SaveManager.Instance.SavedValues.Sound = true;
        UpdateState();
        UpdateButtons();
    }
    public void Vibration()
    {
        SaveManager.Instance.SavedValues.Vibration = SaveManager.Instance.SavedValues.Vibration == true ? SaveManager.Instance.SavedValues.Vibration = false : SaveManager.Instance.SavedValues.Vibration = true;
        UpdateState();
        UpdateButtons();
    }
    public void Language()
    {

    }
    public void Account()
    {
        IAccount();
    }
    void IAccount()
    {
        if (SocialManager.Instance.isConnectedToGooglePlayServices)
        {
            SocialManager.Instance.SignOut();
            StartCoroutine(SetLocalizedString(accountTMP));
        }
        else
        {
            SocialManager.Instance.SignIn(SignInInteractivity.CanPromptAlways, (result) =>
            { 
                if(result == SignInStatus.Success)
                {
                    accountTMP.text = Social.localUser.userName;
                }
                else
                {
                    StartCoroutine(SetLocalizedString(accountTMP));
                }
                SaveManager.Instance.Load();
            });
        }
    }
    IEnumerator SetLocalizedString(TextMeshProUGUI textMeshProUGUI)
    {
        var lsa = localizedString.GetLocalizedString();
        yield return lsa;
        textMeshProUGUI.text = lsa.Result;
    }
    public void RateUs()
    {
        StartCoroutine(HelperClass.CheckInternetConnection((a) => { if (a) Application.OpenURL("market://details?id=" + Application.identifier); }));
    }
    public void Share()
    {
        StartCoroutine(HelperClass.CheckInternetConnection((a) => {
            if (a) StartCoroutine(CShare());
        }));
        IEnumerator CShare()
        {
            var lsaS = titleLS.GetLocalizedString();
            var lsaT = textLS.GetLocalizedString();
            yield return lsaS;
            if(!lsaT.IsDone) yield return lsaT;
            new NativeShare().SetSubject(lsaS.Result).SetText(lsaT.Result).SetUrl("market://details?id=" + Application.identifier);
        }
    }
}
