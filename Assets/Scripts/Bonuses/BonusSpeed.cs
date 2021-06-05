using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using static GameConfigContainer;

public class BonusSpeed : Bonus
{
    protected override void SetBonus(Cannon cannon)
    {
        TaskActiones.Instance.UseSpeedBonus(1);
        float bulletRate = cannon.BulletsPerSecond;
        cannon.BulletsPerSecond *= gameConfig.bonusSpeedCoef;
        cannon.StartShooting();
        GameObject gameObject = Instantiate(animationGO, cannon.gameObject.transform.position, Quaternion.identity, cannon.gameObject.transform);
        Sequence sequence = DOTween.Sequence().AppendInterval(gameConfig.bonusSpeedTime);
        sequence.AppendCallback(() => cannon.BulletsPerSecond = bulletRate);
        sequence.AppendCallback(() => cannon.StartShooting());
        BonusManager.Instance.activeBonuses[Type].Add(new KeyValuePair<Sequence, GameObject>(sequence, gameObject));
    }
}
