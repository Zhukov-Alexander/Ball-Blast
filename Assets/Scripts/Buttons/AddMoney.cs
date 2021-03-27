using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMoney : MonoBehaviour
{
    public static Action<int> OnAddMoney { get; set; }
    public void AddMoreMoney()
    {
        OnAddMoney(10000);
    }
}
