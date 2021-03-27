using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
[ExecuteInEditMode]
public class UIAnimationController : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("Open")]
    public void Open()
    {
        UIAnimation.Open(gameObject).Play();
    }
    [ContextMenu("Close")]
    public void Close()
    {
        UIAnimation.Close(gameObject).Play();
    }
#endif
}
