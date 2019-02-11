using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ThreatUIManager : SingletonMono<ThreatUIManager> {
    [SerializeField]
    Image[] bars;
    [SerializeField]
    Text[] texts;
    [SerializeField]
    Image skullImg;

    float animSec=1f;
    float animSecDiv;
    int mobTypeCount;

    public Button exitBtn;
    public GameObject note;
    public Text noteText;

    public enum E_ThreatUIState
    {
        IDLE,
        FIRST_SHOW_ANIM,
        ADJUST_SHOW_ANIM,
    }

    E_ThreatUIState state = E_ThreatUIState.IDLE;
    public E_ThreatUIState State
    {
        get { return state; }
    }

    WaitForSeconds ws = new WaitForSeconds(0.5f);
    
    public void Show()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.GROWL);
        this.gameObject.SetActive(true);
        
    }
    public void Hide()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
        this.gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        mobTypeCount = (int)E_Monster.MAX;
        animSecDiv = 1 / animSec;
        for (int i = 0; i < mobTypeCount; i++)
        {
            texts[i].text = MobEviInfoManager.GetInstance().GetMobName((E_Monster)i);
        }
        Hide();
    }

    private void OnEnable()
    {
        List<WholeMonsterRiskManager.MonsterRisk> nowMonsterRiskList = WholeMonsterRiskManager.GetInstance().MonsterRiskList;
        if (nowMonsterRiskList.Count != mobTypeCount) return;

        Debug.Log("위협도 ui 킴 표시할 위협도는 " + WholeMonsterRiskManager.GetInstance().DebugString());
        StartCoroutine(FirstShowAnimation(nowMonsterRiskList));

        note.SetActive(false);
    }
    private void OnDisable()
    {
        note.SetActive(false);
        StopAllCoroutines();
        state = E_ThreatUIState.IDLE;
        for (int i = 0; i < mobTypeCount; i++)
        {
            bars[i].fillAmount = 0;
        }
    }

    public void IADThreat(bool increase, List<WholeMonsterRiskManager.MonsterRisk> list)
    {
        StartCoroutine(IADAnimation(increase, list));
    }


    IEnumerator IADAnimation(bool increase, List<WholeMonsterRiskManager.MonsterRisk> list)
    {
        if (list.Count != mobTypeCount) Debug.LogError("오류");

        while (state != E_ThreatUIState.IDLE) yield return ws;

        string result = "증감리스트 ";
        foreach (WholeMonsterRiskManager.MonsterRisk m in list)
        {
            result += "[" + m.mob.ToString() + "-" + m.nowNum + "]";
        }

        Debug.Log(increase.ToString() + "애니메이션"+result); 


        float[] nowBarsLength = new float[mobTypeCount];
        for (int i = 0; i < mobTypeCount; i++)
        {
            nowBarsLength[i] = bars[i].fillAmount;
        }

        state = E_ThreatUIState.ADJUST_SHOW_ANIM;
        float[] iadArr = new float[mobTypeCount];   //증감되는 수치.
        
        for (int i = 0; i < mobTypeCount; i++)
        {
            int maxThreatNum = WholeMonsterRiskManager.GetInstance().GetMaxMobThreat((E_Monster)i);
            iadArr[i] =  (float)list[i].nowNum /(float) maxThreatNum;

         //   Debug.LogFormat("몬스터 {0}의 최대치는 {1} 현재 필마운트치는 {2} 증감 필마운트는 {3}", ((E_Monster)i).ToString(), maxThreatNum, nowBarsLength[i], iadArr[i]);
            
        }

        if (increase)
        {
            float maxThreatRedDegree = 0f;
            for (int i = 0; i < mobTypeCount; i++)
            {
                if (nowBarsLength[i] + iadArr[i] > 1f)
                {
                    iadArr[i] = 1f - nowBarsLength[i];
                }

                if (nowBarsLength[i] + iadArr[i] > maxThreatRedDegree)
                    maxThreatRedDegree = nowBarsLength[i] + iadArr[i];
      //  Debug.LogFormat("몬스터 {0}의 증감치{1} , 현재+증감치 = {2}" , ((E_Monster)i).ToString() , iadArr[i],  (nowBarsLength[i] + iadArr[i])        );
            }
     //   Debug.Log("증감최대치=" + maxThreatRedDegree);



            noteText.text = UIGeneralTextsManager.GetUIGeneralText("threat", "increase");
            /*  Vector2 noteStart = new Vector2(-960, 0);
              note.transform.localPosition = noteStart;
              Vector2 noteArrive = new Vector2(2450, 0);
              */
            note.SetActive(true);

            float nowTime = 0f;
            float redDegreeIA = maxThreatRedDegree - (1f-skullImg.color.g);  //요만큼 이동.
            if (redDegreeIA < 0) redDegreeIA = 0;

            Color skullColor = new Color(1,1,1,1);
            float originG = skullImg.color.g;
            float originB = skullImg.color.b;
            
            while (nowTime <= animSec)
            {
                nowTime += Time.deltaTime;
                float toFill = (nowTime * animSecDiv);

                for (int i = 0; i < mobTypeCount; i++)
                {
                    bars[i].fillAmount = nowBarsLength[i]+ (toFill * iadArr[i]);
                }

                float redIA = redDegreeIA * toFill;

                skullColor.g = originG -redIA;
                skullColor.b = originB - redIA;

                skullImg.color = skullColor;

               // note.transform.localPosition = Vector2.Lerp(noteStart, noteArrive, toFill);

                yield return null;
            }
            for (int i = 0; i < mobTypeCount; i++)
            {
                bars[i].fillAmount = nowBarsLength[i] + iadArr[i];
            }
            skullImg.color = new Color(1,originG - redDegreeIA , originG - redDegreeIA, 1);
            //   note.transform.localPosition = noteStart;
            note.SetActive(false);
        }
        else {
            //감소 애니메이션.

            float maxThreatRedDegree = 0f;
            for (int i = 0; i < mobTypeCount; i++)
            {
                if (nowBarsLength[i] - iadArr[i] < 0f)
                {
                    iadArr[i] = nowBarsLength[i];
                }

                if (nowBarsLength[i] - iadArr[i] > maxThreatRedDegree)
                    maxThreatRedDegree = nowBarsLength[i] - iadArr[i];
            }

            float nowTime = 0f;
            float redDegreeID = (1f-skullImg.color.g) - maxThreatRedDegree;  //요만큼 이동.

            Color skullColor = new Color(1, 1, 1, 1);
            float originG = skullImg.color.g;
            float originB = skullImg.color.b;



            noteText.text = UIGeneralTextsManager.GetUIGeneralText("threat", "hunters");
            /*
            Vector2 noteStart = new Vector2(-960, 0);
            note.transform.localPosition = noteStart;
            Vector2 noteArrive = new Vector2(2450, 0);
            */
            note.SetActive(true);

            while (nowTime <= animSec)
            {
                nowTime += Time.deltaTime;
                float toFill = (nowTime * animSecDiv);

                for (int i = 0; i < mobTypeCount; i++)
                {
                    bars[i].fillAmount = nowBarsLength[i] - (toFill * iadArr[i]);
                }
                float redIA = redDegreeID * toFill;

                skullColor.g = originG+ redIA;
                skullColor.b= originB+ redIA;

                skullImg.color = skullColor;


                // note.transform.localPosition = Vector2.Lerp(noteStart, noteArrive, toFill);
               

                yield return null;
            }
            for (int i = 0; i < mobTypeCount; i++)
            {
                bars[i].fillAmount = nowBarsLength[i] - iadArr[i];
            }
            skullImg.color = new Color(1, originG + redDegreeID, originG + redDegreeID, 1);
           // note.transform.localPosition = noteStart;
            note.SetActive(false);
        }

        state = E_ThreatUIState.IDLE;
    }



    IEnumerator FirstShowAnimation(List<WholeMonsterRiskManager.MonsterRisk> list)  //밑에서 부터 올라감.
    {
        state = E_ThreatUIState.FIRST_SHOW_ANIM;
        float maxNowThreat=0;
        float[] barLength = new float[mobTypeCount];

        for (int i = 0; i < mobTypeCount; i++)
        {
            int maxThreatNum = WholeMonsterRiskManager.GetInstance().GetMaxMobThreat((E_Monster)i);
            barLength[i] = (float) list[i].nowNum /(float)maxThreatNum;
            if (barLength[i] > 1f)
            {
                barLength[i] = 1f;
            }
            if (maxNowThreat < barLength[i])
            {
                maxNowThreat = barLength[i];
            }
       //     Debug.LogFormat("몬스터 {0}의 최대치는 {1} 현재 치는 {2} 필마운트는 {3}", ((E_Monster)i).ToString(), maxThreatNum, list[i].nowNum, barLength[i]);
        }
    //    Debug.Log(maxNowThreat);

        skullImg.color = Color.white;
        Color skullColor=new Color(1,1,1,1);
        float skullRedDegree;

        float nowTime = 0f;
        while (nowTime <= animSec)
        {
            nowTime += Time.deltaTime;
            float toFill = (nowTime *animSecDiv);

            for (int i = 0; i < mobTypeCount; i++)
            {
                bars[i].fillAmount = toFill * barLength[i];
            }

            skullRedDegree = maxNowThreat * toFill;

            skullColor.g = 1f - skullRedDegree;
            skullColor.b = 1f- skullRedDegree;

            skullImg.color = skullColor;

            yield return null;
        }
        for (int i = 0; i < mobTypeCount; i++)
        {
            bars[i].fillAmount = barLength[i];
        }

        state = E_ThreatUIState.IDLE;
    }
    

    //모킹
    /*
    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            List<WholeMonsterRiskManager.MonsterRisk> list = new List<WholeMonsterRiskManager.MonsterRisk>();
            for (int i = 0; i < 8; i++) {
                WholeMonsterRiskManager.MonsterRisk temp = new WholeMonsterRiskManager.MonsterRisk((E_Monster)i);
                temp.nowNum = 2;
                list.Add(temp);
            }


            IADThreat(true, list);
            IADThreat(false, list);
        }
        if (Input.GetKeyDown("d"))
        {
            List<WholeMonsterRiskManager.MonsterRisk> list = new List<WholeMonsterRiskManager.MonsterRisk>();
            for (int i = 0; i < 8; i++)
            {
                WholeMonsterRiskManager.MonsterRisk temp = new WholeMonsterRiskManager.MonsterRisk((E_Monster)i);
                temp.nowNum = 4;
                list.Add(temp);
            }


            IADThreat(false, list);
        }}
        */
    
    
}
