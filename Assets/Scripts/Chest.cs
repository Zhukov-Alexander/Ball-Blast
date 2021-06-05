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

public class Chest : MonoBehaviour
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
    public static Action OnRewardTaken;
    bool canGetReward = false;
    private void Awake()
    {
        SaveManager.Instance.OnLoaded += () => {
            StartCoroutine(UpdateRemainingTime());
        };
        OnRewardTaken += () => TaskActiones.Instance.OpenChest(1);
    }
    void ResetTimer()
    {
        SaveManager.Instance.SavedValues.PlaytimeRewardTimeSpan = DateTime.Now + TimeSpan.FromMinutes(time);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 160);
        image.sprite = closedChest;
        canGetReward = false;
        StartCoroutine(UpdateRemainingTime());
    }
    IEnumerator UpdateRemainingTime()
    {
        while (true)
        {
            TimeSpan remainingTime = SaveManager.Instance.SavedValues.PlaytimeRewardTimeSpan - DateTime.Now;
            if (remainingTime.TotalSeconds <= 0)
            {
                SetReady();
                yield break;
            }
            else
            {
                textMesh.text = remainingTime.ToString(@"mm\:ss");
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
            double rewardAmount = gameConfig.levelLifePerSec * gameConfig.levelDuration * Progression.GetLevelProgression(SaveManager.Instance.SavedValues.CampainLevel, 1, false) * levelCoinsToAdd;
            SaveManager.Instance.SavedValues.Coins += rewardAmount;
            Instantiate(floatingText, transform.position, Quaternion.identity).GetComponent<FloatingText>().SetText(rewardAmount.NumberToTextInOneLine(), gameConfig.coinColor);
            Vibration.Vibrate(gameObject);
            ResetTimer();
            OnRewardTaken?.Invoke();
        }
    }
}
