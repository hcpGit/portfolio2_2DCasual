using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartSceneUIManager : MonoBehaviour {
    public GameObject newStartBtn;
    public GameObject continueStartBtn;
    public Text continueBtnText;
    public Text continueBtnDateText;
    public Text SettingText;
    public Text newStartWarnTxt;
    public GameObject newStartWarnPanel;
    public Button korBtn;
    public Button engBtn;

    InGameTime loadedGameDate;
    bool hasSavedGame = false;

    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "start";

    private void OnDestroy()
    {
        LanguageManager.GetInstance().RemoveListnerLanguageChange(langChanged);
    }

    public void InitStartUI()
    {
        newStartWarnPanel.SetActive(false);
        if (SaveManager.GetInstance().IsThereSavedGame())
        {
            hasSavedGame = true;
            loadedGameDate = SaveManager.GetInstance().GetSavedDate();
            SetStartBtn(loadedGameDate);
        }
        else SetStartBtn(null);
        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                korBtn.interactable = false;
                engBtn.interactable = true;
                break;
            case E_Language.ENGLISH:
                engBtn.interactable = false;
                korBtn.interactable = true;
                break;
        }

        StartTextSetting();
    }

    void SetStartBtn(InGameTime date)
    {
        if (date == null)  //처음 시작임
        {
            continueStartBtn.SetActive(false);
            newStartBtn.SetActive(true);
            return;
        }
        SetContinueDateTxt(date);
        continueStartBtn.SetActive(true);
        newStartBtn.SetActive(false);
    }


    void SetContinueDateTxt(InGameTime date)
    {
        continueBtnDateText.text = LanguageManager.GetInstance().GetDateString(date);
    }
    public void OnSettingBtnClick()
    {
        SettingUIManager.GetInstance().Show();
    }

    public void OnClick_NewStartFromLoad()  //세이브 데이터 있는데 뉴스타트를 눌렀을때 경고창 뜨게.
    {
        newStartWarnPanel.SetActive(true);
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
    }
    public void OnClick_NewStart()
    {
        GameManager.GetInstance().NewStartGame();
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
    }
    public void OnClick_ContinueStart()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        GameManager.GetInstance().LoadGame();
    }
    public void OnClick_NewStartWarnConfirm()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        OnClick_NewStart();
    }
    public void OnClick_NewStartWarnCancel()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        newStartWarnPanel.SetActive(false);
    }
    public void OnClick_LangBtn(string lang)
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
        switch (lang)
        {
            case "kor":
                LanguageManager.GetInstance().SetLanguage(E_Language.KOREAN);
                break;
            case "eng":
                LanguageManager.GetInstance().SetLanguage(E_Language.ENGLISH);
                break;
        }
    }
    
    public void langChanged()
    {
        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                korBtn.interactable = false;
                engBtn.interactable = true;
                break;
            case E_Language.ENGLISH:
                engBtn.interactable = false;
                korBtn.interactable = true;
                break;
        }

        StartTextSetting();

        if (!hasSavedGame) return;
        SetContinueDateTxt(loadedGameDate); //세이브가 있었으면 세이브 데이트도 바까주기.
    }

    void StartTextSetting()
    {
        SettingText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "config");
        continueBtnText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "load");
        newStartWarnTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "warnNewStart");
    }
    public void ShowLeaderboard()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
#endif
    }
    public void ShowAchivement()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
#endif
    }
}
