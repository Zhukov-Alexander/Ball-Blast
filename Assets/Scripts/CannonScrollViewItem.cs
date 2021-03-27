using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static GameConfigContainer;

public class CannonScrollViewItem : MonoBehaviour
{
    public Cannon Cannon { get; set; }
    private void OnDestroy()
    {
        Destroy(Cannon.gameObject);
    }
    private void FixedUpdate()
    {
        Cannon.transform.position = transform.position;
    }
    public void SetScrollViewFromPrefab(int index)
    {
        GameObject cannonGO = Instantiate(gameConfig.cannonPrefabs[index]);
        cannonGO.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(a => a.sortingLayerName = "UI Foreground");
        Canvas canvas = cannonGO.GetComponentInChildren<Canvas>();
        canvas.sortingLayerName = "UI Foreground";

        Cannon = cannonGO.GetComponent<Cannon>();
        Cannon.PrefabNumber = index;
        SetCannonSimulation(false);
        UpdateCannonColor();
    }
    public void SetCannonSimulation(bool simulate)
    {
        Cannon.gameObject.GetComponentsInChildren<Rigidbody2D>().ToList().ForEach(a => a.simulated = simulate);
    }
    public void UpdateCannonColor()
    {
        if (SavedValues.Instance.OpendCannonsPrefabIndexes.Contains(Cannon.PrefabNumber))
        {
            Cannon.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(a => a.color = Color.white);
            Cannon.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);

        }
        else
        {
            Cannon.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(a => a.color = Color.black);
            Cannon.gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        }
    }

    internal void BuyAnimation()
    {
        Cannon.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
        Cannon.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(a => a.DOColor(Color.white, 0.5f).SetEase(Ease.InSine));
        Cannon.gameObject.GetComponentsInChildren<Image>().ToList().ForEach(a => a.DOFade(1, 0.5f).From(0).SetEase(Ease.InSine));
    }
}
