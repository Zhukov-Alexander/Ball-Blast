using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIAnimationScale : UIAnimation
{
    [SerializeField] float timeToScale = 1;
    Vector3 scale;

    public Vector3 Scale
    {
        get
        {
            if(scale == null)
            {
                scale = gameObject.transform.localScale;
            }
            return scale;
        }
    }

    public override Sequence Open()
    {
        Sequence sequence = DOTween.Sequence();
        if (gameObject.TryGetComponent(out RectTransform rect))
        {
            sequence.Join(rect.DOScale(1, timeToScale).SetEase(Ease.OutSine));
        }
        return sequence;
    }

    public override Sequence Close()
    {
        Sequence sequence = DOTween.Sequence();
        if (gameObject.TryGetComponent(out RectTransform rect))
        {
            sequence.Join(rect.DOScale(0, timeToScale).SetEase(Ease.InSine));
        }
        return sequence;
    }
}
