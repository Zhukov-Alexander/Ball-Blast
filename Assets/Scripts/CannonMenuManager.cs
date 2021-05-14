using System;
using System.ArrayExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ScrollSnaps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameConfigContainer;

public class CannonMenuManager : MonoBehaviour
{
    [SerializeField] SliderScript slider;
    [SerializeField] Scroll scroll;
    [SerializeField] Image buyButtonImg;
    [SerializeField] Button buyButtonBtn;
    [SerializeField] Button chooseButtonBtn;
    [SerializeField] Image exitButtonImg;
    [SerializeField] Button exitButton;
    [SerializeField] TextMeshProUGUI cannonCost;
    [SerializeField] GameObject container;
    [SerializeField] GameObject statsContainer;
    [SerializeField] GameObject statFieldPrefab;
    [SerializeField] Sprite bulletsPerSecondSprite;
    [SerializeField] Sprite bulletsDamageSprite;
    [SerializeField] Sprite cannonMoveForceSprite;
    [SerializeField] Sprite bonusDropSprite;
    [SerializeField] Sprite healthSprite;
    [SerializeField] Sprite armorSprite;

    //[SerializeField] TextMeshProUGUI cannonNameText;
    [SerializeField] TextMeshProUGUI unlockedCannonsText;
    [SerializeField] GameObject cannonScrollView;
    CannonScrollViewItem activeCannonScrollView;
    List<CannonScrollViewItem> activeCannonScrollViews = new List<CannonScrollViewItem>();
    StatField bulletsPerSecondSF;
    StatField bulletsDamageSF;
    StatField cannonMoveForceSF;
    StatField bonusDropSF;
    StatField healthSF;
    StatField armorSF;

    public static Action OnEnter;
    public static Action OnExit;
    int index = 0;

    private void Awake()
    {
        OnEnter();
        scroll.OnActiveRectTransformChanged += (a) => SetActiveScrollView(a.GetComponent<CannonScrollViewItem>());
        SetUnlockedCannonsText();
        AddCannonScrollViews();
        scroll.Set(index);
        slider.SetSlider(activeCannonScrollViews.Count, index);
        InstantiateStatFields();
        SetActiveScrollView(activeCannonScrollViews[index]);
    }
    public void Exit()
    {
        OnExit();
        Destroy(gameObject);
    }
    public void Choose()
    {
        ChooseCannon();
        Exit();
    }

    void ChooseCannon()
    {
        SaveManager.Instance.SavedValues.CannonPrefabNumber = activeCannonScrollView.Cannon.PrefabNumber;
    }
    void SetUnlockedCannonsText()
    {
        unlockedCannonsText.text = SaveManager.Instance.SavedValues.OpendCannonsPrefabIndexes.Count + "/" + gameConfig.cannonPrefabs.Count;
    }

    void AddCannonScrollViews()
    {
        for (int i = 0; i < gameConfig.cannonPrefabs.Count; i++)
        {
            GameObject cannonScrollView = Instantiate(this.cannonScrollView, container.transform);
            CannonScrollViewItem cannonScrollViewScript = cannonScrollView.GetComponent<CannonScrollViewItem>();
            activeCannonScrollViews.Add(cannonScrollViewScript);
            cannonScrollViewScript.SetScrollViewFromPrefab(i);
        }
    }

    void SetActiveScrollView(CannonScrollViewItem activeCannonScrollView)
    {
        SoundManager.Instance.Button(0.3f);
        this.activeCannonScrollView = activeCannonScrollView;
        index = activeCannonScrollViews.IndexOf(activeCannonScrollView);
        slider.SetSlider(activeCannonScrollViews.Count, index);
        //SetCannonName();
        SetCannonStats();
        if (SaveManager.Instance.SavedValues.OpendCannonsPrefabIndexes.Contains(activeCannonScrollView.Cannon.PrefabNumber))
        {
            chooseButtonBtn.gameObject.SetActive(true);
            buyButtonBtn.gameObject.SetActive(false);
        }
        else
        {
            chooseButtonBtn.gameObject.SetActive(false);
            buyButtonBtn.gameObject.SetActive(true);
            SetBuyButton();
        }

        void SetBuyButton()
        {
            int cost = gameConfig.baseCannonCost * SaveManager.Instance.SavedValues.OpendCannonsPrefabIndexes.Count;
            cannonCost.text = cost.NumberToTextInOneLine();
            if (SaveManager.Instance.SavedValues.Diamonds >= cost)
            {
                buyButtonBtn.onClick.RemoveAllListeners();
                buyButtonBtn.onClick.AddListener(Buy);
                buyButtonBtn.onClick.AddListener(() => SoundManager.Instance.Win());
                buyButtonBtn.onClick.AddListener(() => SetActiveScrollView(activeCannonScrollView));
                buyButtonImg.color = gameConfig.activeButtonColor;
            }
            else
            {
                buyButtonImg.color = gameConfig.passiveButtonColor;
                exitButtonImg.color = gameConfig.activeButtonColor;
            }

            void Buy()
            {
                SaveManager.Instance.SavedValues.Diamonds -= cost;
                SaveManager.Instance.SavedValues.OpendCannonsPrefabIndexes.Add(activeCannonScrollView.Cannon.PrefabNumber);
                SoundManager.Instance.Bonus();
                activeCannonScrollView.BuyAnimation();
        }
    }

}
/*void SetCannonName()
{
    cannonNameText.text = activeCannonScrollView.Cannon.CannonSettings.cannonName;
}*/
void InstantiateStatFields()
    {
        bulletsPerSecondSF = Instantiate(statFieldPrefab, statsContainer.transform, false).GetComponent<StatField>();
        bulletsDamageSF = Instantiate(statFieldPrefab, statsContainer.transform, false).GetComponent<StatField>();
        cannonMoveForceSF = Instantiate(statFieldPrefab, statsContainer.transform, false).GetComponent<StatField>();
        bonusDropSF = Instantiate(statFieldPrefab, statsContainer.transform, false).GetComponent<StatField>();
        healthSF = Instantiate(statFieldPrefab, statsContainer.transform, false).GetComponent<StatField>();
        armorSF = Instantiate(statFieldPrefab, statsContainer.transform, false).GetComponent<StatField>();
    }
void SetCannonStats()
        {
        SetStat(activeCannonScrollView.Cannon.CannonSettings.bulletsPerSecondMultiplyer, bulletsPerSecondSF, bulletsPerSecondSprite);
        SetStat(activeCannonScrollView.Cannon.CannonSettings.bulletsDamageMultiplyer, bulletsDamageSF, bulletsDamageSprite);
        SetStat(activeCannonScrollView.Cannon.CannonSettings.cannonMoveForceMultiplyer, cannonMoveForceSF, cannonMoveForceSprite);
        SetStat(activeCannonScrollView.Cannon.CannonSettings.bonusProbabilityMultiplyer, bonusDropSF, bonusDropSprite);
        SetStat(activeCannonScrollView.Cannon.CannonSettings.healthMultiplyer, healthSF, healthSprite);
        SetStat(activeCannonScrollView.Cannon.CannonSettings.armorMultiplyer, armorSF, armorSprite);

        void SetStat(float multiplyer, StatField statField, Sprite sprite)
        {
            statField.gameObject.SetActive(true);
            statField.Set(sprite, HelperClass.MultiplyerToPercent(multiplyer));
        }
    }
    
}
