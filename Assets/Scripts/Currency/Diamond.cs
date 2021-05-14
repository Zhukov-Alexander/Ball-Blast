using System;
using System.Collections.Generic;
using UnityEngine;
using static GameConfigContainer;

public class Diamond : Currency
{

    public override void Collect()
    {
        SoundManager.Instance.Coin();
        SaveManager.Instance.SavedValues.Diamonds += (int)Weight;
        Instantiate(floatingText, transform.position, Quaternion.identity).GetComponent<FloatingText>().SetText(Weight.NumberToTextInOneLine(), gameConfig.diamondColor);
        Destroy();
    }
}
