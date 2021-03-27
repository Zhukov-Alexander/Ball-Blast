using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static GameConfigContainer;

public class SceneScrollViewItem : MonoBehaviour
{
    [SerializeField] float backgroundScaling;
    public Background Background { get; set; }
    private void OnDestroy()
    {
        Destroy(Background.gameObject);
    }
    private void FixedUpdate()
    {
        Background.transform.position = transform.position;
    }
    public void SetScrollViewFromPrefab(int index)
    {
        GameObject cannonGO = Instantiate(gameConfig.backgrounds[index]);
        cannonGO.transform.localScale = HelperClass.GetVector3(backgroundScaling).Multiply(cannonGO.transform.localScale);
        cannonGO.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(a => a.sortingLayerName = "UI Foreground");

        Background = cannonGO.GetComponent<Background>();
        Background.PrefabNumber = index;
        UpdateCannonColor();
    }
    public void UpdateCannonColor()
    {
        if (SavedValues.Instance.OpendBackgroundsPrefabIndexes.Contains(Background.PrefabNumber))
        {
            Background.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(a => a.color = Color.white);

        }
        else
        {
            Background.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(a => a.color = Color.black);

        }
    }

    internal void BuyAnimation()
    {
        SoundManager.Instance.Win();
        Background.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(a => a.DOColor(Color.white, 0.5f).SetEase(Ease.InSine));
    }
}
