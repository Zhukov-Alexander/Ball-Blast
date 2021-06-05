using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    [SerializeField] protected GameObject animationGO;
    public int Type { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 10)
        {
            SoundManager.Instance.Bonus();
            BonusManager.Instance.Complete(Type);
            TaskActiones.Instance.UseBonuses(1);
            SetBonus(collision.gameObject.GetComponentInParent<Cannon>());
            Destroy(gameObject);
        }
    }
    protected abstract void SetBonus(Cannon cannon);
}
