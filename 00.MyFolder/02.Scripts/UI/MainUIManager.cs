using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainUIManager : MonoBehaviour {
    static MainUIManager instance;

    public InventoryShow invenShow;
    
    public Text YearMonDayText;
    public Text HourMinText;
    public Text goldText;
    
    public Text CharaText;
    public Text CharaNameText;
    public GameObject charactorPanel;
    public GameObject PlayerTextPanel;

    public GameObject playerText2Btn;
    public GameObject playerText1Btn;

    public Text Player1BtnText;
    public Text player2BtnText_1;
    public Text player2BtnText_2;

    public GameObject InventoryBtn;
    public GameObject questBtn;
    public GameObject hunterBtn;

    public Text settingText;

    public static MainUIManager GetInstance()
    {
        return instance;
    }
    void Awake()
    {
        instance = this;

        settingText.text = UIGeneralTextsManager.GetUIGeneralText("start", "config");


        YearMonDayText = GameObject.Find("YearMonDayText").GetComponent<Text>();
        HourMinText = GameObject.Find("HourMinText").GetComponent<Text>();
        goldText = GameObject.Find("goldText").GetComponent<Text>();

        

        CharaText = GameObject.Find("CharaText").GetComponent<Text>();
        CharaNameText = GameObject.Find("CharaNameText").GetComponent<Text>();

        charactorPanel = GameObject.Find("CharaTextPanel");
        PlayerTextPanel =  GameObject.Find("PlayerTextPanel");

        Player1BtnText = GameObject.Find("Player1BtnText").GetComponent<Text>();
        player2BtnText_1 = GameObject.Find("player2BtnText_1").GetComponent<Text>();
        player2BtnText_2 = GameObject.Find("player2BtnText_2").GetComponent<Text>();

        playerText2Btn = GameObject.Find("playerText2Btn");
        playerText1Btn = GameObject.Find("playerText1Btn");
    }
    private void Start()
    {
        QuestRoomSceneUIManager.GetInstance().OnlyShowMainUIOrigin();
        goldText.text = GoldManager.GetInstance().Gold.ToString();
        GoldManager.GetInstance().AddGoldChangeListner(GoldCountShow);
    }
    private void OnDestroy()
    {
        GoldManager.GetInstance().RemoveGoldChangeListner(GoldCountShow);
    }

    void GoldCountShow()
    {
        if (gameObject.activeSelf == true)
            {
                StopCoroutine(GoldTxtAdjust());
                StartCoroutine(GoldTxtAdjust());
            }
    }

    IEnumerator GoldTxtAdjust()
    {
        int beforeGold = int.Parse( goldText.text);
        int newGold = GoldManager.GetInstance().Gold;
      
        int goldGap = newGold - beforeGold;
        int anim = 0;
        if (goldGap == 0) yield break;

        

        if (goldGap > 0)
            anim = 1;
        else anim = -1;

        if (Mathf.Abs(goldGap) > 100)
        {
            anim = goldGap / 100;
            goldGap = 100;
        }



        for (int i = 0; i < goldGap; i++)
        {
            beforeGold += anim;
            goldText.text = beforeGold.ToString();
            yield return null;
        }
        goldText.text = newGold.ToString();
    }


    public void ShowOrigin()
    {
        ShowMainCanvas();
        HideCharaTextPanel();
        HidePlayerTextPanel();
        ShowPanelBtns();
    }
    public void ShowOnlyTimePanel()
    {
        ShowMainCanvas();
        HideCharaTextPanel();
        HidePlayerTextPanel();
        HidePanelBtns();
    }

    public void ShowPanelBtns()
    {
        InventoryBtn.SetActive(true);
            questBtn.SetActive(true);
        hunterBtn.SetActive(true);
    }
    public void HidePanelBtns()
    {
        InventoryBtn.SetActive(false);
        questBtn.SetActive(false);
        hunterBtn.SetActive(false);
    }


    public void HideCharaTextPanel()
    {
        charactorPanel.SetActive(false);
    }
    public void ShowCharaTextPanel()
    {
        charactorPanel.SetActive(true);
    }

    public void SetCharaText(string str)
    {
        CharaText.text = str;
    }
    public void SetCharaNameText(string str)
    {
        CharaNameText.text = str;
    }


    public void HidePlayerTextPanel()
    {
        PlayerTextPanel.SetActive(false);
    }
    public void ShowPlayerTextPanel()
    {
        PlayerTextPanel.SetActive(true);
    }

    public void ShowPlayerText1Btn()
    {
        playerText1Btn.SetActive(true);
        playerText2Btn.SetActive(false);
    }
    public void ShowPlayerText1Btn(string str)
    {
        SetPlayer1BtnText(str);
        playerText1Btn.SetActive(true);
        playerText2Btn.SetActive(false);
    }
    public void ShowPlayerText2Btn()
    {
        playerText2Btn.SetActive(true);
        playerText1Btn.SetActive(false);
    }
    public void ShowPlayerText2Btn(string str1 , string str2)
    {
        SetPlayer2Btn_1Text(str1);
        SetPlayer2Btn_2Text(str2);
        playerText2Btn.SetActive(true);
        playerText1Btn.SetActive(false);
    }


    public void SetPlayer1BtnText(string str)
    {
        Player1BtnText.text = str;
    }
    public void SetPlayer2Btn_1Text(string str)
    {
        player2BtnText_1.text = str;
    }
    public void SetPlayer2Btn_2Text(string str)
    {
        player2BtnText_2.text = str;
    }





    public void HideMainCanvas()
    {
        this.gameObject.SetActive(false);
    }
    public void ShowMainCanvas()
    {
        this.gameObject.SetActive(true);
    }




    public void AdjustTime(uint hour, uint minute)
    {
        string min = minute.ToString();
        if (minute == 0)
            min = "00";

        HourMinText.text = hour.ToString() + " : " + min;
    }
    public void AdjustYMD(uint year, uint month,uint day)
    {
        YearMonDayText.text = year.ToString() + "년 " + month.ToString() +"월 "+day.ToString()+"일";
    }
    public void AdjustYMD(string ymdTxt)
    {
        YearMonDayText.text = ymdTxt;
    }
    public void AdjustGold(int gold)
    {
        goldText.text = gold.ToString();
    }

    public void ShowThreats()
    {
        ThreatUIManager.GetInstance().Show();
    }
    public void ShowInven()
    {
        invenShow.Show();
    }
    public void ShowSetting()
    {
        SettingUIManager.GetInstance().Show();
    }
}
