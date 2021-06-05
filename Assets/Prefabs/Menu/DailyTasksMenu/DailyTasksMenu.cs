using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static GameConfigContainer;
using System.Linq;
using UnityEngineInternal;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Globalization;

public class DailyTasksMenu : Singleton<DailyTasksMenu>
{
    [SerializeField] GameObject taskPanelPrefab;
    [SerializeField] Transform taskPanelContainer;
    [SerializeField] TextMeshProUGUI timerTMP;
    [SerializeField] GameObject TasksCompletedRewardGO;
    [SerializeField] TextMeshProUGUI tasksCompletedRewardTMP;
    [SerializeField] Button tasksCompletedRewardBtn;
    [SerializeField] Slider tasksCompletedRewardSlider;
    [SerializeField] Image tasksCompletedRewardImage;
    [SerializeField] GameObject floatingText;
    [SerializeField] GameObject exitButtonGO;

    [NonSerialized] public List<TaskPanel> taskPanels = new List<TaskPanel>();
    private void Awake()
    {
        tasksCompletedRewardBtn.onClick.AddListener(TakeTasksCompletedReward);
        TaskPanel.RewardTaken += () =>
        {
            SetTasksCompletedReward();
        };
    }
    private void OnEnable()
    {
        StartCoroutine(UpdateTimer());
        CreateTaskPanels();
        SetTasksCompletedReward();
    }
    IEnumerator UpdateTimer()
    {
        DateTime updateTime = DateTime.ParseExact(SaveManager.Instance.SavedValues.DailyTasks.UpdateDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        while (true)
        {
            TimeSpan diference = updateTime - DateTime.Now;
            if (diference.TotalSeconds > 0)
            {
                timerTMP.text = diference.ToString(@"hh\:mm\:ss");
            }
            else
            {
                UIAnimation.Close(gameObject).OnComplete(() => {
                    Destroy(gameObject);
                });
            }
            yield return new WaitForSecondsRealtime(1f);
        }

    }
    void CreateTaskPanels()
    {
        foreach (var item in SaveManager.Instance.SavedValues.DailyTasks.CurrentTasks)
        {
            TaskPanel taskPanel = Instantiate(taskPanelPrefab, taskPanelContainer, false).GetComponent<TaskPanel>();
            taskPanel.Set(item);
            taskPanels.Add(taskPanel);
        }
    }
    void DestroyTaskPanels()
    {
        taskPanels.ForEach(a=> Destroy(a.gameObject));
        taskPanels.Clear();
    }
    public void SetTasksCompletedReward()
    {
        tasksCompletedRewardTMP.text = gameConfig.allTasksCompletedReward.ToString();
        tasksCompletedRewardSlider.maxValue = SaveManager.Instance.SavedValues.DailyTasks.AllTasksInfo.Count;
        tasksCompletedRewardSlider.value = tasksCompletedRewardSlider.maxValue / SaveManager.Instance.SavedValues.DailyTasks.CurrentTasks.Count;
        if (SaveManager.Instance.SavedValues.DailyTasks.CurrentTasks.Count == 0)
        {
            tasksCompletedRewardImage.color = gameConfig.greenColor;
        }
    }
    void TakeTasksCompletedReward()
    {
        if (SaveManager.Instance.SavedValues.DailyTasks.CurrentTasks.Count == 0)
        {
            SaveManager.Instance.SavedValues.DailyTasks.allTasksRewardTaken = true;
            SaveManager.Instance.SavedValues.Diamonds += gameConfig.allTasksCompletedReward;
            Instantiate(floatingText, transform.position, Quaternion.identity).GetComponent<FloatingText>().SetText(gameConfig.allTasksCompletedReward.ToString(), gameConfig.coinColor);
            UIAnimation.Close(TasksCompletedRewardGO);
        }
    }
    public void Close()
    {
        Vibration.Vibrate(exitButtonGO);
        UIAnimation.Close(gameObject).OnComplete(() => Destroy(gameObject));
    }
}
