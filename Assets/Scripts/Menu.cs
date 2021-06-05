using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Menu<T> : Singleton<T>, IMenu where T : Singleton<T>
{
    public Action OnOpenStarted;
    public Action OnOpenCompleted;
    public Action OnCloseStarted;
    public Action OnCloseCompleted;
    public void Open()
    {
        OnOpenStarted?.Invoke();
        SoundManager.Instance.Button();
        UIAnimation.Open(gameObject).OnComplete(() => OnOpenCompleted?.Invoke());
    }
    public void Close()
    {
        OnCloseStarted?.Invoke();
        SoundManager.Instance.Button();
        UIAnimation.Close(gameObject).OnComplete(() => OnCloseCompleted?.Invoke());
    }
}
public interface IMenu
{
    void Open();
    void Close();
}
