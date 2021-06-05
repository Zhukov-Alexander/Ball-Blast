using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameConfigContainer;

public class TaskPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionTMP;
    [SerializeField] Image fillImg;
    [SerializeField] Slider slider;
    [SerializeField] Button button;
    [SerializeField] GameObject floatingText;
    [SerializeField] TextMeshProUGUI rewardTMP;
    Tasks.Task task;
    public static Action RewardTaken;
    public void Set(Tasks.Task task)
    {
        this.task = task;
        descriptionTMP.text = task.description;
        rewardTMP.text = task.reward.ToString();
        button.onClick.AddListener(GetReward);
        float value = (float)(task.refValue.value / task.targetValue);
        slider.maxValue = 1;
        slider.value = value;
        if (CheckForTasksCompletion())
        {
            fillImg.color = gameConfig.greenColor;
        }
        //Debug.LogWarning($"{task.description} => task.RefValue(): {task.RefValue()}, task.targetValue: {task.targetValue}");
    }
    bool CheckForTasksCompletion()
    {
        return task.refValue.value >= task.targetValue;
    }

    public void Destroy()
    {
        button.onClick.RemoveAllListeners();
        UIAnimation.Close(this.gameObject).OnComplete(() =>
        {
            DailyTasksMenu.Instance.taskPanels.Remove(this);
            Destroy(gameObject);
    });
    }
    public void GetReward()
    {
        if (CheckForTasksCompletion())
        {
            SaveManager.Instance.SavedValues.Diamonds += task.reward;
            SaveManager.Instance.SavedValues.DailyTasks.CurrentTasks.Remove(task);
            RewardTaken?.Invoke();
            Instantiate(floatingText, rewardTMP.transform.position, Quaternion.identity).GetComponent<FloatingText>().SetText(task.reward.ToString(), gameConfig.diamondColor);
            Destroy();
        }
    }
}
