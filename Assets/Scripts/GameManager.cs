﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    private void Start()
    {
        levelManager.StartPlayingLevel();
    }
}
