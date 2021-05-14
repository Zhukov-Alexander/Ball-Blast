using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class UIAnimationMovement : UIAnimation
{
    [SerializeField] float timeToMove = 1;
    [SerializeField] Vector3 openedPosition;
    [SerializeField] Vector3 closedPosition;
    public override Sequence Open()
    {
        Sequence sequence = DOTween.Sequence();
        if (gameObject.TryGetComponent(out RectTransform rect))
        {
            sequence.Join(rect.DOAnchorPos(openedPosition, timeToMove).From(closedPosition).SetEase(Ease.OutSine));
        }
        return sequence;
    }

    public override Sequence Close()
    {
        Sequence sequence = DOTween.Sequence();
        if (gameObject.TryGetComponent(out RectTransform rect))
        {
            sequence.Join(rect.DOAnchorPos(closedPosition, timeToMove).SetEase(Ease.InSine));
        }
        return sequence;
    }
}
