using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using static GameConfigContainer;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] Image ru, en;
    private void OnEnable()
    {
        SetSelectedButton();
    }
    void SetSelectedButton()
    {
        string code = LocalizationSettings.Instance.GetSelectedLocale().Identifier.Code;
        en.color = gameConfig.passiveButtonColor;
        ru.color = gameConfig.passiveButtonColor;

        if (code == "en")
        {
            en.color = gameConfig.activeButtonColor;
        }
        else if(code == "ru")
        {
            ru.color = gameConfig.activeButtonColor;
        }
    }
    void SetLanguage(string s)
    {
        PlayerPrefs.SetString("Language", s);
        LocalizationSettings.Instance.SetSelectedLocale(LocalizationSettings.AvailableLocales.Locales.Find(a=>a.Identifier.Code == s));
        SoundManager.Instance.Button();
        SetSelectedButton();
    }
    public void SetEnglish()
    {
        SetLanguage("en");
    }
    public void SetRussian()
    {
        SetLanguage("ru");
    }
}
