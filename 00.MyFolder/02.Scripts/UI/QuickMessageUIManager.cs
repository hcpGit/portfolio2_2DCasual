using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickMessageUIManager : MonoBehaviour {
    static QuickMessageUIManager instance;
    [System.Serializable]
    enum E_QMState
    {
        IDLE,
        SHOWING,
    }
    WaitForSeconds ws = new WaitForSeconds(1f);
    Queue<string> QMQue = new Queue<string>();
    public Text qmText;
    [SerializeField]
    E_QMState nowQMState = E_QMState.IDLE; 

    void Awake()
    {
        instance = this;
    }
    public static QuickMessageUIManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        StartCoroutine(ShowQMCheck());
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    //완벽해!!!!!!!!!!!!!!!!!!!!!!!!
    public void ShowQuickMessage(string ctx)
    {
       // Debug.Log("퀵메세지 큐인 " + ctx);
        //좌상귀에서 스윽 들어왔다가 사라짐.
        QMQue.Enqueue(ctx);
    }
    
    void ShowQM()
    {
       // Debug.Log("쇼큐엡");
        if (nowQMState == E_QMState.SHOWING) return;
        if (QMQue.Count > 0)
        {
            qmText.text = QMQue.Dequeue();
            nowQMState = E_QMState.SHOWING;
            Debug.Log("쇼큐엡 - 간다! +"+ qmText.text);

            StartCoroutine(QMAnim());
        }
    }

    IEnumerator ShowQMCheck()
    {
        while (true) //게임 매니져는 어디서든 하루 일과가 있는 동안에는. 변수를 받아오기. 아니면 게임매니져에서 스타트랑 스탑을 관리해주던가.
        {
            if (GameManager.GetInstance().IsDayOnGoing())
            {
                if (QMQue.Count > 0)
                {
                    ShowQM();
                }
            }
            yield return ws;
        }
    }

    IEnumerator QMAnim()
    {
        this.transform.localPosition = new Vector2(-1213, 326);
        float time = 0f;
        while (time < 0.4f)
        {
            time += Time.deltaTime;
            float ToFill = time *(1/ 0.4f);
            transform.localPosition = Vector2.Lerp(new Vector2(-1213,326), new Vector2(-704, 326), ToFill);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
         time = 0f;
        while (time < 0.4f)
        {
            time += Time.deltaTime;
            float ToFill = time * (1 / 0.4f);
            transform.localPosition = Vector2.Lerp(new Vector2(-704, 326), new Vector2(-1213, 326), ToFill);
            yield return null;
        }

        nowQMState = E_QMState.IDLE;
    }

}
