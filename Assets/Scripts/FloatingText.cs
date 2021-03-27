using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] float moveAmount;
    [SerializeField] float duration;
    private void Awake()
    {
        transform.DOMoveY(transform.position.y + moveAmount, duration).SetEase(Ease.OutQuad);
        GetComponentInChildren<TextMeshProUGUI>().DOFade(0, duration).SetEase(Ease.OutSine).OnComplete(()=>Destroy(gameObject));
    }
    public void SetText(string text, Color color)
    {
        TextMeshProUGUI textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = color;
    }
}
