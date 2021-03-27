using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static GameConfigContainer;

public class ResultsPanel : MonoBehaviour
{
    [SerializeField] ParticleSystem winPS;
    [SerializeField] ParticleSystem losePS;
    [SerializeField] Animator crownAnimator;
    [SerializeField] TextMeshProUGUI mod;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI state;
    [SerializeField] LevelSlider slider;
    [SerializeField] TextMeshProUGUI moneyTMP;
    [SerializeField] GameObject addButtonGO;
    float money;
    public static Action OnHide { get; set; }
    public void Hide()
    {
        OnHide();
        UIAnimation.Close(gameObject).AppendCallback(() => Destroy(gameObject)).Play();
    }
    public void SetCampainWin()
    {
        SetWin();
        SetCampain();
        UIAnimation.Open(gameObject).AppendCallback(AnimateWin).AppendCallback(() => slider.TweenSlider(LevelProgression.maxPoints, 0f, LevelProgression.currentPoints)).Play();
    }
    public void SetCampainLose()
    {
        SetLose();
        SetCampain();
        UIAnimation.Open(gameObject).AppendCallback(AnimateLose).AppendCallback(() => slider.TweenSlider(LevelProgression.maxPoints, 0f, LevelProgression.currentPoints)).Play();
    }
    public void SetBossfightWin()
    {
        SetWin();
        SetBossfight();
        UIAnimation.Open(gameObject).AppendCallback(AnimateWin).AppendCallback(() => slider.TweenSlider(LevelProgression.maxPoints, 0f, LevelProgression.maxPoints - LevelProgression.currentPoints)).Play();
        CalculateBossfightAdMoney();
        addButtonGO.SetActive(true);
    }
    public void SetBossfightLose()
    {
        SetLose();
        SetBossfight();
        addButtonGO.SetActive(false);
        UIAnimation.Open(gameObject).AppendCallback(AnimateLose).AppendCallback(() => slider.TweenSlider(LevelProgression.maxPoints, 0f, LevelProgression.maxPoints - LevelProgression.currentPoints)).Play();

    }
    void SetCampain()
    {
        slider.SetResultCampainNumbers();
        level.text = (SavedValues.Instance.CampainLevel).ToString();
        mod.text = "Level";
        slider.SetCampainIcon();
        slider.SetSlider(1,0);
        CalculateCampainAdMoney();
        if (money <= 0) addButtonGO.SetActive(false); 
        else addButtonGO.SetActive(true);
    }
    void SetBossfight()
    {
        slider.SetResultBossNumbers();
        level.text = (SavedValues.Instance.BossfightLevel).ToString();
        mod.text = "Bossfight";
        slider.SetBossfightIcon();
        slider.SetSlider(1, 0);
    }
    void SetWin()
    {
        state.text = "Complete";
    }
    void SetLose()
    {
        state.text = "Failed";
    }
    void CalculateCampainAdMoney()
    {
        money = gameConfig.adMoneyMultiplyer * LevelProgression.currentPoints;
        moneyTMP.text = money.NumberToTextInOneLineWithoutFraction();
    }
    void CalculateBossfightAdMoney()
    {
        money = gameConfig.adMoneyMultiplyer * LevelProgression.maxPoints * gameConfig.bossfightToCampainModMoneyMultiplyer;
        moneyTMP.text = money.NumberToTextInOneLineWithoutFraction();
    }
    public void ClickAdButton()
    {
        SavedValues.Instance.Coins += money;
        addButtonGO.SetActive(false);
        Hide();
    }
    void AnimateWin()
    {
        winPS.Emit(20);
        SoundManager.Instance.Win();
    }
    void AnimateLose()
    {
        crownAnimator.SetTrigger("Crack");
        losePS.Emit(20);
        SoundManager.Instance.Lose();
    }
}
