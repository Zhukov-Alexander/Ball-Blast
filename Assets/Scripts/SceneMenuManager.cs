using System;
using System.ArrayExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ScrollSnaps;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using static GameConfigContainer;

public class SceneMenuManager : MonoBehaviour
{
    [SerializeField] LocalizedString defeatTheLS;
    [SerializeField] TableReference tableReference;
    [SerializeField] SliderScript slider;
    [SerializeField] Scroll scroll;
    [SerializeField] Image openButtonImg;
    [SerializeField] Button openButtonBtn;
    [SerializeField] Button chooseButtonBtn;
    [SerializeField] Image exitButtonImg;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject container;
    [SerializeField] GameObject statsContainer;
    [SerializeField] GameObject statFieldPrefab;
    [SerializeField] Sprite bulletsPerSecondSprite;
    [SerializeField] Sprite bulletsDamageSprite;
    [SerializeField] Sprite cannonMoveForceSprite;
    [SerializeField] Sprite bonusDropSprite;
    [SerializeField] Sprite healthSprite;
    [SerializeField] Sprite armorSprite;
    [SerializeField] TextMeshProUGUI progressCondition;

    //[SerializeField] TextMeshProUGUI cannonNameText;
    [SerializeField] TextMeshProUGUI unlockedCannonsText;
    [SerializeField] GameObject cannonScrollViewItemPrefab;
    SceneScrollViewItem activeCannonScrollView;
    List<SceneScrollViewItem> activeCannonScrollViews = new List<SceneScrollViewItem>();
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
        scroll.OnActiveRectTransformChanged += (a) => SetActiveScrollView(a.GetComponent<SceneScrollViewItem>());
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
        SaveManager.Instance.SavedValues.ScenePrefabNumber = activeCannonScrollView.Background.PrefabNumber;
    }
    void SetUnlockedCannonsText()
    {
        unlockedCannonsText.text = SaveManager.Instance.SavedValues.OpendBackgroundsPrefabIndexes.Count + "/" + gameConfig.backgrounds.Count;
    }

    void AddCannonScrollViews()
    {
        for (int i = 0; i < gameConfig.cannonPrefabs.Count; i++)
        {
            GameObject cannonScrollView = Instantiate(this.cannonScrollViewItemPrefab, container.transform);
            SceneScrollViewItem cannonScrollViewScript = cannonScrollView.GetComponent<SceneScrollViewItem>();
            activeCannonScrollViews.Add(cannonScrollViewScript);
            cannonScrollViewScript.SetScrollViewFromPrefab(i);
        }
    }

    void SetActiveScrollView(SceneScrollViewItem activeCannonScrollView)
    {
        this.activeCannonScrollView = activeCannonScrollView;
        index = activeCannonScrollViews.IndexOf(activeCannonScrollView);
        slider.SetSlider(activeCannonScrollViews.Count, index);
        //SetCannonName();
        SetCannonStats();
        if (SaveManager.Instance.SavedValues.OpendBackgroundsPrefabIndexes.Contains(activeCannonScrollView.Background.PrefabNumber))
        {
            chooseButtonBtn.gameObject.SetActive(true);
            openButtonBtn.gameObject.SetActive(false);
            progressCondition.gameObject.SetActive(false);
        }
        else
        {
            chooseButtonBtn.gameObject.SetActive(false);
            openButtonBtn.gameObject.SetActive(true);
            progressCondition.gameObject.SetActive(true);
            StartCoroutine(SetCondition());
            SetOpenButton();
        }

        IEnumerator SetCondition()
        {
            TableEntryReference tableEntryReference = gameConfig.bossPrefabs[activeCannonScrollView.Background.PrefabNumber - 1].GetComponent<Boss>().BossSettings.bossName;
            var ls = new LocalizedString();
            ls.SetReference(tableReference, tableEntryReference);
            var lsa = ls.GetLocalizedString();
            var defeatTheLSa = defeatTheLS.GetLocalizedString();
            yield return lsa;
            if (!defeatTheLSa.IsDone) yield return defeatTheLSa;
            progressCondition.text = defeatTheLSa.Result + " " + lsa.Result.ToUpper();
        }

        void SetOpenButton()
        {
            if (SaveManager.Instance.SavedValues.BossfightLevel > activeCannonScrollView.Background.PrefabNumber)
            {
                openButtonBtn.onClick.RemoveAllListeners();
                openButtonBtn.onClick.AddListener(Open);
                openButtonBtn.onClick.AddListener(() => SetActiveScrollView(activeCannonScrollView));
                openButtonImg.color = gameConfig.activeButtonColor;
            }
            else
            {
                openButtonImg.color = gameConfig.passiveButtonColor;
                exitButtonImg.color = gameConfig.activeButtonColor;
            }

            void Open()
            {
                SaveManager.Instance.SavedValues.OpendBackgroundsPrefabIndexes.Add(activeCannonScrollView.Background.PrefabNumber);
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
        SetStat(activeCannonScrollView.Background.backgroundSettings.bulletsPerSecondMultiplyer, bulletsPerSecondSF, bulletsPerSecondSprite);
        SetStat(activeCannonScrollView.Background.backgroundSettings.bulletsDamageMultiplyer, bulletsDamageSF, bulletsDamageSprite);
        SetStat(activeCannonScrollView.Background.backgroundSettings.cannonMoveForceMultiplyer, cannonMoveForceSF, cannonMoveForceSprite);
        SetStat(activeCannonScrollView.Background.backgroundSettings.bonusProbabilityMultiplyer, bonusDropSF, bonusDropSprite);
        SetStat(activeCannonScrollView.Background.backgroundSettings.healthMultiplyer, healthSF, healthSprite);
        SetStat(activeCannonScrollView.Background.backgroundSettings.armorMultiplyer, armorSF, armorSprite);

        void SetStat(float multiplyer, StatField statField, Sprite sprite)
        {
            if (multiplyer != 1)
            {
                statField.gameObject.SetActive(true);
                statField.Set(sprite, HelperClass.MultiplyerToPercent(multiplyer));
            }
            else
                statField.gameObject.SetActive(false);
        }
    }

}
