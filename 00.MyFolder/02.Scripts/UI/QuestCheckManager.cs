using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestCheckManager : MonoBehaviour {

    static QuestCheckManager instance ;

    class QCItem
    {
        public QCItem(GameObject g, Text t, QuestPerMob q)
        {
            go = g; numText = t;qpm = q;
        }
        public GameObject go;
        public Text numText;
        public QuestPerMob qpm;
    }
    public GameObject itemBtn;
    public GameObject nowQuestContents;
    public GameObject nowInvenContents;
    public GameObject nowSelectedContents;

    List<QCItem> nowInven = new List<QCItem>();
    List<QCItem> selectedItem = new List<QCItem>();
    
    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "cqc";
    public Text writtenQuestListTxt;
    public Text nowInvenListTxt;
    public Text selectedListTxt;
    public Text submitBtnTxt;

    void Awake()
    {
        instance = this;
        writtenQuestListTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "writtenquestlist");
        nowInvenListTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "nowinven");
        selectedListTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "selectedlist");
        submitBtnTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "submitbtn");

    }

    public static QuestCheckManager GetInstance()
    {
        return instance;
    }

    public void SetNowMobEvidenceInven()
    {
        if (nowInven == null) nowInven = new List<QCItem>();
        List<QuestPerMob> list = Inventory.GetInstance().GetNowMobEvidencesInven();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = Instantiate(itemBtn);
            Text numText;
            SetItemBtnCtx(temp, list[i],out numText);

            QCItem qc = new QCItem(temp, numText, list[i]);
            nowInven.Add(qc);
            Button btn = temp.GetComponent<Button>();
            btn.onClick.AddListener(
               ()=>
               {

                   AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
                   OnClickQCInven(qc);
               }
                );

            temp.transform.SetParent(nowInvenContents.transform);
        }
    }

    void OnClickQCInven(QCItem qc) {
        if (qc.qpm.number <= 0) Debug.LogError("QC OnClickQCInven");

        SettleSelectItem(qc.qpm.mob, qc.qpm.evidence);

        if (qc.qpm.number == 1)
        {
            nowInven.Remove(qc);
            Destroy(qc.go);
            return;
        }

        qc.qpm.number--;
        qc.numText.text = qc.qpm.number.ToString();
    }

    void OnClickQCSelected(QCItem qc)
    {
        if (qc.qpm.number <= 0) Debug.LogError("QC OnClickQCSelected");

        SettleNowInvenItem(qc.qpm.mob, qc.qpm.evidence);

        if (qc.qpm.number == 1)
        {
            selectedItem.Remove(qc);
            Destroy(qc.go);
            return;
        }
        qc.qpm.number--;
        qc.numText.text = qc.qpm.number.ToString();
    }


    public void SettleSelectItem(E_Monster mob ,E_Evidence evidence)  //셀렉티드 아이템이 만들어지는 곳.
    {
        QuestPerMob qpm = new QuestPerMob(mob, evidence, 1);

        if (selectedItem == null)
        {
            selectedItem = new List<QCItem>();
        }
        for (int i = 0; i < selectedItem.Count; i++)
        {
            if (selectedItem[i].qpm.IsIt(mob, evidence))
            {
                selectedItem[i].qpm.number++;
                selectedItem[i].numText.text = selectedItem[i].qpm.number.ToString();
                return;
            }
        }

        //새로 만들 차례
        GameObject temp = Instantiate(itemBtn);
        Text numText;
        SetItemBtnCtx(temp, qpm,out numText);
        QCItem qc = new QCItem(temp, numText, qpm);
        
        selectedItem.Add(qc);

        Button btn = temp.GetComponent<Button>();
        btn.onClick.AddListener(
           () =>
           {
               AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
               OnClickQCSelected(qc);
           }
            );


        temp.transform.SetParent(nowSelectedContents.transform);
    }

    void SettleNowInvenItem(E_Monster mob, E_Evidence evidence)
    {
        for (int i = 0; i < nowInven.Count; i++)
        {
            if (nowInven[i].qpm.IsIt(mob, evidence))
            {
                nowInven[i].qpm.number++;
                nowInven[i].numText.text = nowInven[i].qpm.number.ToString();
                return;
            }
        }
        QuestPerMob qpm = new QuestPerMob(mob, evidence, 1);

        GameObject temp = Instantiate(itemBtn);
        Text numText;
        SetItemBtnCtx(temp, qpm, out numText);

        QCItem qc = new QCItem(temp, numText, qpm);
        nowInven.Add(qc);
        Button btn = temp.GetComponent<Button>();
        btn.onClick.AddListener(
           () =>
           {
               AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
               OnClickQCInven(qc);
           }
            );

        temp.transform.SetParent(nowInvenContents.transform);

    }
    
 
    void SetItemBtnCtx(GameObject btn , QuestPerMob qpm,out Text numText)
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetColoredEvidenceSprite(qpm.mob,qpm.evidence); //qpm 에 맞춰서 넣어주기.
        btn.transform.GetChild(1).GetComponent<Text>().text =  MobEviInfoManager.GetInstance().GetMobName(qpm.mob);
        btn.transform.GetChild(2).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetEvidenceName(qpm.evidence);
        btn.transform.GetChild(3).GetComponent<Text>().text = qpm.number.ToString();
        numText = btn.transform.GetChild(3).GetComponent<Text>();
    }
    void SetItemBtnCtx(GameObject btn, QuestPerMob qpm)
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetColoredEvidenceSprite(qpm.mob, qpm.evidence); //qpm 에 맞춰서 넣어주기.
        btn.transform.GetChild(1).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetMobName(qpm.mob);
        btn.transform.GetChild(2).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetEvidenceName(qpm.evidence);
        btn.transform.GetChild(3).GetComponent<Text>().text = qpm.number.ToString();
    }
    public void SetNowQuestList(Quest quest)
    {
        List<QuestPerMob> questList = quest.QuestList;

        foreach (QuestPerMob qpm in questList)
        {
            GameObject temp = Instantiate(itemBtn);
            
            Destroy(temp.GetComponent<Button>());

            SetItemBtnCtx(temp, qpm);
            temp.transform.SetParent(nowQuestContents.transform);
        }
    }

    void RenewInventory()
    {
        Dictionary<string, int> newInventory = new Dictionary<string, int>();
        for (int i = 0; i < nowInven.Count; i++)
        {
            newInventory.Add(Inventory.GetMobEviKey(nowInven[i].qpm), nowInven[i].qpm.number);
        }
        Inventory.GetInstance().RenewMobEviInven(newInventory);
    }

    public void InitQCCanvas(Quest quest)
    {
        nowInven.Clear();
        selectedItem.Clear();
        SetNowQuestList(quest);
        SetNowMobEvidenceInven();
    }

    public void ShowQCUI()
    {
        this.gameObject.SetActive(true);
    }

    public void ExitQCCanvas()  //제출 버튼
    {
        ClearAllBtns();
        
        RenewInventory();
        nowInven.Clear();
        
        List<QuestPerMob> checkedList = new List<QuestPerMob>();
        foreach (QCItem qc in selectedItem)
        {
            checkedList.Add(qc.qpm);
        }
        
        selectedItem.Clear();

        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.UNDERTAKE);
        InteractiveManager.GetInstance().QuestCheckIsDone(checkedList);
        HideQCUI();
    }

    public void HideQCUI()
    {
        this.gameObject.SetActive(false);
    }

    void ClearAllBtns()
    {
        for (int i = 0; i < nowQuestContents.transform.childCount; i++)
        {
            Destroy(nowQuestContents.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < nowInvenContents.transform.childCount; i++)
        {
            Destroy(nowInvenContents.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < nowSelectedContents.transform.childCount; i++)
        {
            Destroy(nowSelectedContents.transform.GetChild(i).gameObject);
        }
    }
}
