using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static GameConfigContainer;

public class PlaytimeReward : MonoBehaviour
{
    [SerializeField] int time;
    [SerializeField] float coinProbability;
    [SerializeField] int diamondsToAdd;
    [SerializeField] int levelCoinsToAdd;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image image;
    [SerializeField] Sprite closedChest;
    [SerializeField] Sprite openedChest;
    [SerializeField] LocalizedString readyLS;
    [SerializeField] GameObject floatingText;
    bool canGetReward = false;
    private void Awake()
    {
        SaveManager.Instance.OnLoaded += () => {
            StartCoroutine(CalculateRemainingTime());
        };
    }
    void ResetTimer()
    {
        SaveManager.Instance.SavedValues.PlaytimeRewardTimeSpan = TimeSpan.FromMinutes(time);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 160);
        image.sprite = closedChest;
        canGetReward = false;
        StartCoroutine(CalculateRemainingTime());
    }
    IEnumerator CalculateRemainingTime()
    {
        while (true)
        {
            SaveManager.Instance.SavedValues.PlaytimeRewardTimeSpan -= new TimeSpan(0, 0, 1);
            if (SaveManager.Instance.SavedValues.PlaytimeRewardTimeSpan.TotalSeconds <= 0)
            {
                SetReady();
                yield break;
            }
            else
            {
                textMesh.text = SaveManager.Instance.SavedValues.PlaytimeRewardTimeSpan.ToString(@"mm\:ss");
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }
    void SetReady()
    {
        StartCoroutine(SetCurrentUpgradeText());
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 190);
        image.sprite = openedChest;
        canGetReward = true;
    }
    IEnumerator SetCurrentUpgradeText()
    {
        var ls = readyLS.GetLocalizedString();
        yield return ls;
        textMesh.text = ls.Result;
    }

    public void GetReward()
    {
        if(canGetReward)
        {
            if (Random.value < coinProbability)
            {
                double rewardAmount = gameConfig.levelLifePerSec * gameConfig.levelDuration * Progression.GetLevelProgression(SaveManager.Instance.SavedValues.CampainLevel, levelCoinsToAdd, false);
                SaveManager.Instance.SavedValues.Coins += rewardAmount;
                Instantiate(floatingText, transform.position, Quaternion.identity).GetComponent<FloatingText>().SetText(rewardAmount.NumberToTextInOneLine(), gameConfig.coinColor);
            }
            else
            {
                SaveManager.Instance.SavedValues.Diamonds += diamondsToAdd;
                Instantiate(floatingText, transform.position, Quaternion.identity).GetComponent<FloatingText>().SetText(diamondsToAdd.NumberToTextInOneLine(), gameConfig.diamondColor);
            }
            Vibration.Vibrate(gameObject);
            ResetTimer();
        }
    }
}
