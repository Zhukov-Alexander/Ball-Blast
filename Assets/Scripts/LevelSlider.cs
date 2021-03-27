using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlider : SliderScript
{
    [SerializeField] Sprite bossfightIcon;
    [SerializeField] Sprite campainIcon;
    [SerializeField] Color bossfightColor;
    [SerializeField] Color campainColor;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI leftNumber;
    [SerializeField] TextMeshProUGUI rightNumber;
    public void SetCampainIcon()
    {
        icon.sprite = campainIcon;
        icon.color = campainColor;
    }
    public void SetBossfightIcon()
    {
        icon.sprite = bossfightIcon;
        icon.color = bossfightColor;
    }
    public void SetLevelBossNumbers()
    {
        rightNumber.text = SavedValues.Instance.BossfightLevel.ToString();
    }
    public void SetLevelCampainNumbers()
    {
        rightNumber.text = SavedValues.Instance.CampainLevel.ToString();
    }
    public void SetResultBossNumbers()
    {
        leftNumber.text = (SavedValues.Instance.BossfightLevel).ToString();
        rightNumber.text = (SavedValues.Instance.BossfightLevel + 1).ToString();
    }
    public void SetResultCampainNumbers()
    {
        leftNumber.text = (SavedValues.Instance.CampainLevel).ToString();
        rightNumber.text = (SavedValues.Instance.CampainLevel + 1).ToString();
    }
}
