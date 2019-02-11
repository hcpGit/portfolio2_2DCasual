using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    static HandManager instance;

    //파치먼트 움직임 관할
    
    public Transform parchmentOriginPos;
    
    public Parchment parchment;
    [SerializeField]
    GameObject parchmentGameObject;

    List<QuestPerMob> nowHandleQPMList;   //현재 단위 전체 퀘스트 목록
    List<QuestPerMob> nowParchmentHandleQPMs;   //현재 파치먼트에 담긴 퀘스트들 (그, 최대 4개인 그거.)
    
    void Awake()
    {
        instance = this;
    }
    public static HandManager GetInstance()
    {
        return instance;
    }

    public void HandlingMonsterHuntingCommission()
    {
        nowHandleQPMList = new List<QuestPerMob>();
        nowParchmentHandleQPMs = new List<QuestPerMob>();
        
        CleanParchment();

        StartCoroutine(UnfoldMotion());
    }

    public void StampParchment(E_Monster mob , E_Evidence evidence , int number)
    {
       // Debug.LogFormat("몹 {0} 증거품 {1} 몇 개 {2}", mob, evidence, number);
        if (nowHandleQPMList == null|| nowParchmentHandleQPMs==null) Debug.LogError("도장 찍을 퀘스트가 없음");

        nowParchmentHandleQPMs.Add(new QuestPerMob( mob, evidence, number));

        parchment.Stamp(mob, evidence, number);
    }

    public void CompleteParchment()
    {
        MergeParchmentToQuest();
       
        nowHandleQPMList.OrderByMobEvi();
        
        InteractiveManager.GetInstance().QuestMakingDone(nowHandleQPMList);
        
        StartCoroutine(FoldMotion(false));
    }
    public void OneMoreParchment()
    {
        MergeParchmentToQuest();
        StartCoroutine(FoldMotion(true));
    }
    public void DumpParchment()
    {
        nowParchmentHandleQPMs.Clear();
        StartCoroutine(DumpMotion());
    }
   
    void MergeParchmentToQuest()
    {
        nowHandleQPMList.AddQPMList(nowParchmentHandleQPMs);
        nowParchmentHandleQPMs.Clear();
    }

    public void CleanParchment()
    {
        parchment.CleanThisParchment();
    }
    public bool IsThereEmptySpaceInParchment()
    {
        return parchment.CanStampMore();
    }


    IEnumerator DumpMotion()
    {
        QMakeUIManager.GetInstance().HideQMUI();
        float time = 0f;
        while (time < 1f)
        {
            parchmentGameObject.transform.Translate((Vector3.back)* (Time.deltaTime * 4f) , Space.World);
            time += Time.deltaTime;
            yield return  null;
        }
        CleanParchment();
        StartCoroutine(UnfoldMotion());
    }

    IEnumerator UnfoldMotion()
    {

        QMakeUIManager.GetInstance().HideQMUI();
        CleanParchment();

        ParchmentPosToOrigin();
        parchmentGameObject.transform.Translate(Vector3.right * 2f);
        float time = 0f;
        while (time < 1f)
        {
            parchmentGameObject.transform.Translate((Vector3.left) * (Time.deltaTime * 2f), Space.World);
           
            time += Time.deltaTime;
            yield return null;
        }
        ParchmentPosToOrigin();

        QMakeUIManager.GetInstance().ShowQMUI();
    }
    IEnumerator FoldMotion(bool onemore)
    {

        QMakeUIManager.GetInstance().HideQMUI();
        ParchmentPosToOrigin();
        float time = 0f;
        while (time < 1f)
        {
            parchmentGameObject.transform.Translate((Vector3.right) * (Time.deltaTime * 3f), Space.World);

            time += Time.deltaTime;
            yield return null;
        }

        CleanParchment();

        if (onemore)
        {
            StartCoroutine(UnfoldMotion());
        }
    }

    void ParchmentPosToOrigin()
    {
        parchmentGameObject.transform.SetPositionAndRotation(parchmentOriginPos.position, parchmentOriginPos.rotation);
    }

}
