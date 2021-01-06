using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    public void RestartTheGame()
    {
        levelManager.StopPlayingLevel();
        levelManager.StartPlayingLevel();
    }
}
