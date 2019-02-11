using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parchment : MonoBehaviour{
    class showSet
    {
        Vector2 size3 = new Vector2(3f, 3f);
        SpriteRenderer mobSprite;
        SpriteRenderer numSprite;
        SpriteRenderer evidenceSprite;
        SpriteRenderer numTenthSprite_10;
        SpriteRenderer numTenthSprite_1;

        public showSet(Transform pos)
        {
            SettingRs(pos);
        }

        public void SettingRs(Transform pos)
        {
            mobSprite = pos.GetChild(0).GetComponent<SpriteRenderer>();
            numSprite = pos.GetChild(1).GetComponent<SpriteRenderer>();
            evidenceSprite = pos.GetChild(2).GetComponent<SpriteRenderer>();
            numTenthSprite_10 = pos.GetChild(3).GetComponent<SpriteRenderer>();
            numTenthSprite_1 = pos.GetChild(4).GetComponent<SpriteRenderer>();
        }

        public void StampMob(E_Monster mob)
        {
             mobSprite.sprite = SpriteManager.GetInstance().GetMobStampedSprite(mob); //어딘가에서 몹 얼굴들 스프라이트 저장해둔거 받아오기
        }
        public void StampNum(int number)
        {
            if (number < 10)
            {
                numTenthSprite_10.sprite = null;
                numTenthSprite_1.sprite = null;

                numSprite.sprite = SpriteManager.GetInstance().GetNumSprite(number); //어딘가에서 몹 얼굴들 스프라이트 저장해둔거 받아오기
                numSprite.size = size3;
                return;
            }

            if (number <100)
            {
                numSprite.sprite = null;

                int tenth = number / 10;
                int oneth = number % 10;
                numTenthSprite_10.sprite = SpriteManager.GetInstance().GetNumSprite(tenth); 
                numTenthSprite_1.sprite = SpriteManager.GetInstance().GetNumSprite(oneth);
                numTenthSprite_10.size = size3;
                numTenthSprite_1.size = size3;
            }
        }
        public void StampEvidence(E_Evidence evidence)
        {
            evidenceSprite.sprite = SpriteManager.GetInstance().GetEvidenceStampedSprite(evidence); //어딘가에서 몹 얼굴들 스프라이트 저장해둔거 받아오기
            evidenceSprite.size = size3;
        }
        public void Clean()
        {
            mobSprite.sprite = null;
            numSprite.sprite = null;
            evidenceSprite.sprite = null;
            numTenthSprite_10.sprite = null;
            numTenthSprite_1.sprite = null;
        }
    }

    showSet[] qPoses;
    int stamped=0;

    //파치먼트 두개를 풀링 갖고 있는 채로 버리고 새거 꺼내오고
    //새거 또 쓰고 하는 거 충분?
    //퀘스트들 관리하는 리스트.

    //파치먼트 몇개를 이어서 한개의 퀘스트 만드는 기능.

    private void Awake()
    {
        qPoses = new showSet[4];
        for (int i = 0; i < 4; i++)
        {
           // Debug.Log(i + "번째 파치먼트 자식=" + transform.GetChild(i).name);
            qPoses[i] = new showSet(transform.GetChild(i));
        }
    }

    public bool CanStampMore()
    {
        if (stamped < 0 || stamped >= 4) return false;
        return true;
    }

    public void Stamp(E_Monster mob , E_Evidence evidence , int number)
    {
        if (stamped >= 4) return;
        StampMob(stamped, mob);
        StampNum(stamped, number);
        StampEvidence(stamped, evidence);
        stamped++;
    }


    public void StampMob(int whichLine, E_Monster mob)
    {
        qPoses[whichLine].StampMob(mob);
    }
    public void StampNum(int whichLine, int number)
    {
        qPoses[whichLine].StampNum(number);
    }
    public void StampEvidence(int whichLine, E_Evidence evidence)
    {
        qPoses[whichLine].StampEvidence(evidence);
    }

    public void CleanThisParchment()
    {
        for (int i = 0; i < qPoses.Length; i++)
        {
            qPoses[i].Clean();
        }
        stamped = 0;
    }
}
