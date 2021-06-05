using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class UIAnimation : MonoBehaviour
{
    [SerializeField] public bool followParent = true;

    public abstract Sequence Open();
    public abstract Sequence Close();
    public static Sequence Open(GameObject gameObject, bool enable = true)
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        if(enable) gameObject.SetActive(true);
        List< UIAnimation> animations = gameObject.GetComponentsInChildren<UIAnimation>().ToList();
        animations.ForEach(a => {
            if (a.followParent)
            {
                sequence.Join(a.Open());
            }
            });
        return sequence;
    }

    public static Sequence Close(GameObject gameObject, bool disable = true)
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        List<UIAnimation> animations = gameObject.GetComponentsInChildren<UIAnimation>().ToList();
        animations.ForEach(a => {
            if(a.followParent)
            {
                sequence.Join(a.Close());
            }
        });
        if (disable) sequence.AppendCallback(() => gameObject.SetActive(false));
        return sequence;
    }

}
