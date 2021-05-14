using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Localization;
using static GameConfigContainer;

public class ResultsPanel : MonoBehaviour
{
    [SerializeField] LocalizedString campainMode;
    [SerializeField] LocalizedString bossfightMode;
    [SerializeField] LocalizedString winLS;
    [SerializeField] LocalizedString loseLS;
    [SerializeField] ParticleSystem winPS;
    [SerializeField] ParticleSystem losePS;
    [SerializeField] Animator crownAnimator;
    [SerializeField] TextMeshProUGUI mod;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI state;
    [SerializeField] LevelSlider slider;
    [SerializeField] TextMeshProUGUI moneyTMP;
    [SerializeField] GameObject addButtonGO;
    double money;
    public static Action OnHide { get; set; }
    public void Hide()
    {
        if (AdManager.Instance.IsTimeReady() && AdManager.Instance.IsAdReady(AdManager.Instance.VideoId))
        {
            AdManager.Instance.ShowAd(AdManager.Instance.VideoId, (result) =>
            {
                OnHide();
                UIAnimation.Close(gameObject).AppendCallback(() => Destroy(gameObject)).Play();
                SaveManager.Instance.SaveCloud();
            });
        }
        else
        {
            OnHide();
            UIAnimation.Close(gameObject).AppendCallback(() => Destroy(gameObject)).Play();
            SaveManager.Instance.SaveCloud();
        }
    }
    public void SetCampainWin()
    {
        StartCoroutine(SetWin());
        StartCoroutine(SetCampain());
        UIAnimation.Open(gameObject).AppendCallback(AnimateWin).AppendCallback(() => slider.TweenSlider(LevelProgression.maxPoints, 0f, LevelProgression.currentPoints)).Play();
    }
    public void SetCampainLose()
    {
        addButtonGO.SetActive(false);
        StartCoroutine(SetLose());
        StartCoroutine(SetCampain());
        UIAnimation.Open(gameObject).AppendCallback(AnimateLose).AppendCallback(() => slider.TweenSlider(LevelProgression.maxPoints, 0f, LevelProgression.currentPoints)).Play();
    }
    public void SetBossfightWin()
    {
        StartCoroutine(SetWin());
        StartCoroutine(SetBossfight());
        UIAnimation.Open(gameObject).AppendCallback(AnimateWin).AppendCallback(() => slider.TweenSlider(LevelProgression.maxPoints, 0f, LevelProgression.maxPoints - LevelProgression.currentPoints)).Play();
    }
    public void SetBossfightLose()
    {
        addButtonGO.SetActive(false);
        StartCoroutine(SetLose());
        StartCoroutine(SetBossfight());
        UIAnimation.Open(gameObject).AppendCallback(AnimateLose).AppendCallback(() => slider.TweenSlider(LevelProgression.maxPoints, 0f, LevelProgression.maxPoints - LevelProgression.currentPoints)).Play();
    }
    IEnumerator SetCampain()
    {
        slider.SetResultCampainNumbers();
        level.text = (SaveManager.Instance.SavedValues.CampainLevel).ToString();
        slider.SetCampainIcon();
        slider.SetSlider(1,0);
        CalculateCampainAdMoney();
        var lsa = campainMode.GetLocalizedString();
        yield return lsa;
        mod.text = lsa.Result;

    }
    IEnumerator SetBossfight()
    {
        slider.SetResultBossNumbers();
        level.text = (SaveManager.Instance.SavedValues.BossfightLevel).ToString();
        slider.SetBossfightIcon();
        slider.SetSlider(1, 0);
        CalculateBossfightAdMoney();
        var lsa = bossfightMode.GetLocalizedString();
        yield return lsa;
        mod.text = lsa.Result;

    }
    IEnumerator SetWin()
    {
        var lsa = winLS.GetLocalizedString();
        yield return lsa;
        state.text = lsa.Result;
    }
    IEnumerator SetLose()
    {
        var lsa = loseLS.GetLocalizedString();
        yield return lsa;
        state.text = lsa.Result;
    }
    void CalculateCampainAdMoney()
    {
        money = gameConfig.adMoneyMultiplyer * LevelProgression.currentPoints;
        moneyTMP.text = "+" + money.NumberToTextInOneLine();
    }
    void CalculateBossfightAdMoney()
    {
        money = gameConfig.adMoneyMultiplyer * LevelProgression.maxPoints * gameConfig.bossfightToCampainModMoneyMultiplyer;
        moneyTMP.text = "+" + money.NumberToTextInOneLine();
    }
    public void ClickAdButton()
    {
        AdManager.Instance.ShowAd(AdManager.Instance.RewardedVideoId, (result) =>
        {
            if (result == ShowResult.Finished)
            {
                SaveManager.Instance.SavedValues.Coins += money;
            }
            addButtonGO.SetActive(false);
            Hide();
        });
    }
    void AnimateWin()
    {
        winPS.Emit(20);
        SoundManager.Instance.Win();
        if (money <= 0) addButtonGO.SetActive(false);
        else if (AdManager.Instance.IsAdReady(AdManager.Instance.RewardedVideoId)) addButtonGO.SetActive(true);
        else addButtonGO.SetActive(false);
    }
    void AnimateLose()
    {
        crownAnimator.SetTrigger("Crack");
        losePS.Emit(20);
        SoundManager.Instance.Lose();
    }
}
