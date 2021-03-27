using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIAnimationColor : UIAnimation
{
    [SerializeField] float timeToFade;
    List<float> initialAlphas;
    public override Sequence Open()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        List<Graphic> graphics = GetComponentsInChildren<Graphic>().ToList();
        initialAlphas = new List<float>();
        for (int i = 0; i < graphics.Count; i++)
        {
            initialAlphas.Add(graphics[i].color.a);
            sequence.Join(graphics[i].DOFade(initialAlphas[i], timeToFade).From(0).SetEase(Ease.OutSine));
        }
        return sequence;
    }

    public override Sequence Close()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        List<Graphic> graphics = GetComponentsInChildren<Graphic>().ToList();
        for (int i = 0; i < graphics.Count; i++)
        {
            sequence.Join(graphics[i].DOFade(0, timeToFade).SetEase(Ease.InSine));
        }
        return sequence;
    }

}
