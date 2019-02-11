using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ds))]
public class fdgs : Editor
{
#if UNITY_EDITOR
    bool[] showArr = new bool[311];

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        OnGUITest();

    }
    private void OnGUITest()
    {
        
       // ShowNowCFrame();
       // Debug.Log("qweqwe");
        CharactorManager cm = CharactorManager.GetInstance();
        List<CharactorIdea> charaList = cm.CharaList;
        List<CharactorIdea> visitedToday = cm.VisitedToday;
        Queue<CharactorManager.VisitChara> expireQue = cm.ExpireCharaQue;

        EditorGUILayout.Separator();

        showArr[0] = EditorGUILayout.Foldout(showArr[0], "캐릭터리스트");
        if (showArr[0])
        {
            foreach (CharactorIdea ci in charaList)
            {
                    // Debug.Log(charaList.Count + "만큼 래릭터 리스트");
                    ShowCharaIdea(ci);
                EditorGUILayout.Separator();
            }
        }
        EditorGUILayout.Separator();

        showArr[1] = EditorGUILayout.Foldout(showArr[1], "방문자 리스트");
        if (showArr[1])
        {
            foreach (CharactorIdea ci in visitedToday)
            {
                    //  Debug.Log(visitedToday.Count + "만큼 방문자 리스트");
                    ShowCharaIdea(ci);
                EditorGUILayout.Separator();
            }
        }
        EditorGUILayout.Separator();
        showArr[2] = EditorGUILayout.Foldout(showArr[2], "만기큐 ");
        if (showArr[2])
        {
            foreach (CharactorManager.VisitChara ci in expireQue)
            {
                //  Debug.Log(visitedToday.Count + "만큼 방문자 리스트");
                ShowCharaIdea(ci.one);
                EditorGUILayout.Separator();
            }
        }

        EditorGUILayout.Separator();
        //EditorGUILayout.LabelField("퀘스트 매니저");

        showArr[5] = EditorGUILayout.Foldout(showArr[5], "퀘스트 매니저");
        if (showArr[5])
        {

            QuestManager qm = QuestManager.GetInstance();
            foreach (Quest q in qm.GetNowWrittenQuestList())
            {
                ShowQuest(q);
                EditorGUILayout.Separator();
            }
        }

        EditorGUILayout.Separator();
        showArr[3] = EditorGUILayout.Foldout(showArr[3], "인벤토리 - 증거품 ");
        if (showArr[3])
        {

            foreach (QuestPerMob qpm in Inventory.GetInstance().GetNowMobEvidencesInven())
            {
                EditorGUILayout.TextField(qpm.ToString());
            }
        }

        showArr[4] = EditorGUILayout.Foldout(showArr[4], "인벤토리 - 무기 ");
        if (showArr[4])
        {
            foreach (Inventory.weaponInven wi in Inventory.GetInstance().WeaponInventory)
            {
                EditorGUILayout.TextField ("갯수 = " + wi.number, wi.weapon.ToString());
            }
        }

        EditorGUILayout.Separator();
        showArr[7] = EditorGUILayout.Foldout(showArr[7], "위협도에 애드된 헌터 리스트");
        if (showArr[7])
        {
            foreach (HunterIdea ci in WholeMonsterRiskManager.GetInstance().HuntersWhohaveQuest)
            {
                //  Debug.Log(visitedToday.Count + "만큼 방문자 리스트");
                ShowCharaIdea(ci);
                EditorGUILayout.Separator();
            }
        }


    }
    void ShowCharaIdea(CharactorIdea ci)
    {
       // Debug.Log(ci.CharaName);
        HunterIdea hunter = ci as HunterIdea;
        ClientIdea client = ci as ClientIdea;

        if (hunter != null)
        {
            EditorGUILayout.TextField("☆★헌터", ci.CharaName);
            EditorGUILayout.ObjectField(ci.CharaSprite, typeof(Sprite), true);
            EditorGUILayout.TextField("연관키", ci.associatedQuestKey);

            if (hunter.GetExpire() != null)
            {
                EditorGUILayout.TextField("헌터 겟 익스파이어.", hunter.GetExpire().ToString());
            }
            else
            {
                EditorGUILayout.TextField("헌터 겟 익스파이어.", "아직 존재 안함.");
            }
           


            EditorGUILayout.Toggle("커미션 가진 여부", ci.hasCommission);

            EditorGUILayout.TextField("밑은 헌팅 리스트 ,  만기여부=", hunter.haveComeBeforeExpire.ToString());
            int idx = 0;
            if (hunter.HuntedList != null)
            {
                foreach (QuestPerMob qpm in hunter.HuntedList)
                {
                    EditorGUILayout.TextField("QPM [" + idx + "]", qpm.ToString());
                    idx++;
                }
            }
            else {
                EditorGUILayout.TextField("헌팅 리스트 존재하지 않아.","");
            }

        }
        else
        {
            EditorGUILayout.TextField("☆★클라이언트", ci.CharaName);
            EditorGUILayout.ObjectField(ci.CharaSprite, typeof(Sprite), true);
            EditorGUILayout.TextField("연관키", ci.associatedQuestKey);
            EditorGUILayout.Toggle("커미션 가진 여부", ci.hasCommission);
            EditorGUILayout.TextField("설정된 만기 기간은 = ", client.DaysExpire.ToString());

            EditorGUILayout.TextField("밑은 의뢰인 오리진 퀘스트, 만기 텀 여부는",client.haveComeBeforeExpire.ToString());
            ShowQuest(client.OriginOrderedQuest);

        }
    }
    void ShowQuest(Quest q)
    {
        EditorGUILayout.TextField( "의뢰인이름="+q.ClientName , "키="+q.Key );

        if (q.OrderDate == null&&q.ExpireDate!=null)
            EditorGUILayout.TextField("입기= null", "만기=" + q.ExpireDate.ToString());

        else if (q.OrderDate != null && q.ExpireDate == null)
            EditorGUILayout.TextField("입기=" + q.OrderDate.ToString(), "만기=null");

        else if (q.OrderDate == null && q.ExpireDate == null)
            EditorGUILayout.TextField("입기= null", "만기=null");
        else
        EditorGUILayout.TextField("입기=" + q.OrderDate.ToString(), "만기=" + q.ExpireDate.ToString());
        
        for (int i = 0; i < q.QuestList.Count; i++)
        {
            EditorGUILayout.TextField("QPM [" + i + "]", q.QuestList[i].ToString());
        }
    }

    void ShowNowCFrame()
    {
        HunterIdea ch = CharactorFrame.GetInstance().hunterIdea;
        ClientIdea cc = CharactorFrame.GetInstance().clientIdea;
        if (ch == null && cc == null)
        {
            EditorGUILayout.LabelField("현재 캐릭터 프레임 = 널");
            return;
        }
        if (ch != null && cc != null)
        {
            Debug.LogError("캐릭터 프레임에 현재 헌터 클라이언트 모두 있음" + ch.CharaName + cc.CharaName);
            return;
        }
        if (ch != null)
        {
            EditorGUILayout.LabelField("현재 캐릭터 프레임 = 헌터"+ch.CharaName);
            ShowCharaIdea(ch);
            return;
        }
        if (cc != null)
        {
            EditorGUILayout.LabelField("현재 캐릭터 프레임 = 클라이언트" + cc.CharaName);
            ShowCharaIdea(cc);
            return;
        }

    }

#endif
}

public class ForDebugShowQR : MonoBehaviour {
}
