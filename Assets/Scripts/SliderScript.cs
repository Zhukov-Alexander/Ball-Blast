using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] Slider slider;
    public void SetSlider(float maxValue, float value)
    {
        slider.maxValue = maxValue;
        slider.value = value;
        SetFillColor();
    }
    public Tween TweenSlider(float maxValue, float startValue, float endValue, float duration = 1)
    {
        Action<float> func = (a) => SetSlider(maxValue, a);
        return DOTween.To(x => func(x), startValue, endValue, duration);

    }
    void SetFillColor()
    {
        fill.color = gradient.Evaluate(slider.value / slider.maxValue);
    }
}
