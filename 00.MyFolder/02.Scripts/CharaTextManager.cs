using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaTextManager : Singleton<CharaTextManager> {
    //엑셀등에서 텍스트 불러와서 캐릭터 상태 입력 받아서 텍스트 반환함.

    public string GetCharaText(E_ClientState state)
    {
        string str="";
        switch (state)
        {
            case E_ClientState.COMMISSION:
                str = "의뢰하러 왔습니다.";
                break;

            case E_ClientState.COMMISSION_SAY_1:
                str = "괸   (세이1).";
                break;

            case E_ClientState.COMMISSION_SAY_2:
                str = "고1ㄴ(세이2)";
                break;

            case E_ClientState.COMMISSION_SAY_3:
                str = "고블린 하나 눈 증거품! (세이3)";
                break;

            case E_ClientState.COMMISSION_CANCEL:
                str = "답답해서 여기서 의뢰 안합니다 (캔슬)";
                break;
           // case E_ClientState.COMMISION_DONE:
            //    str = "만기 전에 끝내주십쇼. (던)";
             //   break;

            case E_ClientState.LEAVE:
                str = "여기서 떠나는 모션 띄우면 될듯. 바이바이 (리브)";
                break;
        }
        Debug.LogFormat("상태 = {0} , 말 = {1}", state, str);
        return str;
    }
}
