using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class UIAnimation : MonoBehaviour
{
    public abstract Sequence Open();
    public abstract Sequence Close();
    public static Sequence Open(GameObject gameObject)
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        List< UIAnimation> animations = gameObject.GetComponentsInChildren<UIAnimation>().ToList();
        animations.ForEach(a => sequence.Join(a.Open()));
        return sequence;
    }

    public static Sequence Close(GameObject gameObject)
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        List<UIAnimation> animations = gameObject.GetComponentsInChildren<UIAnimation>().ToList();
        animations.ForEach(a => sequence.Join(a.Close()));
        return sequence;
    }

}
