using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Action OnGameStarted { get; set; }

    private void Awake()
    {
        
    }
}
