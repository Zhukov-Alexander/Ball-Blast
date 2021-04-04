using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConfigContainer;

public class Coin : Currency
{
    public override void Collect()
    {
        SoundManager.Instance.Coin();
        SaveManager.Instance.SavedValues.Coins += Weight;
        Instantiate(floatingText, transform.position, Quaternion.identity).GetComponent<FloatingText>().SetText("+" + Weight.NumberToTextInOneLineWithoutFraction(), gameConfig.coinColor);
        Destroy();
    }
}
