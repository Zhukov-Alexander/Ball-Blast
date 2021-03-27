using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatField : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI TMP;
    public void Set(Sprite sprite, string text)
    {
        image.sprite = sprite;
        TMP.text = text;
    }
}
