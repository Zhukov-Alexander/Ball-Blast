using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static GameConfigContainer;

public class BonusHealth : Bonus
{
    protected override void SetBonus(Cannon cannon)
    {
        TaskActiones.Instance.UseHealthBonus(1);
        cannon.CurrentHealth += cannon.MaximumHealth * gameConfig.bonusHealthCoef;
        GameObject gameObject = Instantiate(animationGO, cannon.gameObject.transform.position, Quaternion.identity, cannon.gameObject.transform);
        BonusManager.Instance.activeBonuses[Type].Add(new KeyValuePair<Sequence, GameObject> (null, gameObject));
    }
}
