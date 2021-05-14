using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public void Open()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        UIAnimation.Open(pauseMenu);
    }
    public void Exit()
    {
        UIAnimation.Close(pauseMenu).OnComplete(() => {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            if (LevelModManager.CurrentLevelMod == LevelMod.Bossfight)
            {
                LevelMenu.OnEndBossfightLose();
            }
            else if (LevelModManager.CurrentLevelMod == LevelMod.Campain)
            {
                LevelMenu.OnEndCampainLose();
            }
        });
    }
    public void Resume()
    {
        UIAnimation.Close(pauseMenu).OnComplete(() => {
            pauseMenu.SetActive(false);
            DOTween.To(a => Time.timeScale = a, Time.timeScale, 1, 1).SetUpdate(true);
            });
    }
}
