using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreMenu : MonoBehaviour
{
    public static Action OnEnter;
    public static Action OnExit;
    public void Exit()
    {
        OnExit();
        Destroy(gameObject);
    }
}
