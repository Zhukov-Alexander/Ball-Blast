using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using static GameConfigContainer;

public class BonusShield : Bonus
{
    protected override void SetBonus(Cannon cannon)
    {
        cannon.HasShield = true;
        GameObject gameObject = Instantiate(animationGO, cannon.gameObject.transform.position, Quaternion.identity, cannon.gameObject.transform);
        Sequence sequence = DOTween.Sequence().AppendInterval(gameConfig.bonusShieldTime);
        sequence.AppendCallback(() => cannon.HasShield = false);
        BonusManager.Instance.activeBonuses[Type].Add(new KeyValuePair<Sequence, GameObject>(sequence, gameObject));
    }
}
