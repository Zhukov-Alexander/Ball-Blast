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
    public void SetSlider(double maxValue, double value)
    {
        slider.maxValue = 1;
        slider.value = (float)(value / maxValue);
        SetFillColor();
    }
    void SetSlider(float maxValue, float value)
    {
        slider.maxValue = maxValue;
        slider.value = value;
        SetFillColor();
    }

    public Tween TweenSlider(double maxValue, double startValue, double endValue, float duration = 1)
    {
        Action<float> func = (a) => SetSlider(1, a);
        float start = (float)(startValue / maxValue);
        float end = (float)(endValue / maxValue);
        return DOTween.To(x => func(x), start, end, duration);

    }
    void SetFillColor()
    {
        fill.color = gradient.Evaluate(slider.value / slider.maxValue);
    }
}
