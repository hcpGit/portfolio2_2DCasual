using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientFSMFactory : FSMStateFactory<E_ClientState> {
    static ClientFSMFactory instance;
    public static ClientFSMFactory GetInstance()
    {
        if (instance == null)
        {
            instance = new ClientFSMFactory();
            instance.Init();
        }
        return instance;
    }

    public override void Init()
    {
        for (int i = 0; i < (int)E_ClientState.MAX; i++)
        {
            stateDic.Add(
                (E_ClientState)i,
                createState((E_ClientState)i)
                );
        }
        for (int i = 0; i < (int)E_ClientState.MAX; i++)
        {
            MakeTransitions(GetState((E_ClientState)i), (E_ClientState)i);
        }
    }
    protected override FSMState createState(E_ClientState e)
    {
        FSMState state = new MyState(() => { });
        switch (e)
        {
            case E_ClientState.IDLE:
                break;

            case E_ClientState.COMMISSION:
                state = new MyState(
                   () => {
                       string charaText = TextManager.GetInstance().GetClientText(E_ClientState.COMMISSION,CharactorFrame.GetInstance().clientIdea);
                       string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.COMMISSION, CharactorFrame.GetInstance().clientIdea);
                       InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                   }
                   );
                break;
            case E_ClientState.COMMISSION_SAY_1:
                state = new MyState(
                    () => {
                        string charaText = TextManager.GetInstance().GetNormalClientQuestSay(
                                            E_ClientState.COMMISSION_SAY_1,
                                            CharactorFrame.GetInstance().clientIdea.OriginOrderedQuest.QuestList);
                        string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.COMMISSION_SAY_1, CharactorFrame.GetInstance().clientIdea);
                        InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                    }
                    );
                break;
                    /*
                    
                    TalkState(
                    TextManager.GetInstance().GetNormalClientQuestSay(
                    1,
                    CharactorFrame.GetInstance().clientIdea.OriginOrderedQuest.QuestList
                    ),"네,네.","뭐라구요?"
                    );*/
              
            case E_ClientState.COMMISSION_SAY_2:
                state = new MyState(
                  () => {
                      string charaText = TextManager.GetInstance().GetNormalClientQuestSay(
                                          E_ClientState.COMMISSION_SAY_2,
                                          CharactorFrame.GetInstance().clientIdea.OriginOrderedQuest.QuestList);
                      string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.COMMISSION_SAY_2, CharactorFrame.GetInstance().clientIdea);
                      InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                  }
                  );
                break;
            /*
            state = new TalkState(TextManager.GetInstance().GetNormalClientQuestSay(
                2,
                CharactorFrame.GetInstance().clientIdea.OriginOrderedQuest.QuestList
                ),"네,네.", "뭐라구요?");
            break;
            */
            case E_ClientState.COMMISSION_SAY_3:
                state = new MyState(
                  () => {
                      string charaText = TextManager.GetInstance().GetNormalClientQuestSay(
                                          E_ClientState.COMMISSION_SAY_3,
                                          CharactorFrame.GetInstance().clientIdea.OriginOrderedQuest.QuestList);
                      string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.COMMISSION_SAY_3, CharactorFrame.GetInstance().clientIdea);
                      InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);


                      //InteractiveManager.GetInstance().ShowTalk(charaText, "네,네.", "뭐라구요...?");
                  }
                  );
                break;

            /*
            state = new TalkState(TextManager.GetInstance().GetNormalClientQuestSay(
                3,
                CharactorFrame.GetInstance().clientIdea.OriginOrderedQuest.QuestList
                ), "네,네.", "뭐라구요?");
            break;
            */
            case E_ClientState.COMMISSION_MAKE_START:
                state = new MyState(
                () =>
                {
                    InteractiveManager.GetInstance().MakingQuest();
                }
                );
                break;
            case E_ClientState.COMMISION_MAKE_DONE:
                state = new MyState(
                  () => {
                      string charaText = TextManager.GetInstance().GetClientText(E_ClientState.COMMISION_MAKE_DONE, CharactorFrame.GetInstance().clientIdea);
                      string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.COMMISION_MAKE_DONE, CharactorFrame.GetInstance().clientIdea);
                      InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                  }
                  );

               // state = new TalkState("COMMISION_MAKE_DONE 만기 전에 끝내주십쇼.", "네 물론입죠.");
                break;
            case E_ClientState.COMMISSION_CANCEL:
                state = new MyState(
                 () => {
                     string charaText = TextManager.GetInstance().GetClientText(E_ClientState.COMMISSION_CANCEL, CharactorFrame.GetInstance().clientIdea);
                     string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.COMMISSION_CANCEL, CharactorFrame.GetInstance().clientIdea);
                     InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                 }
                 );
             //   state = new TalkState("COMMISSION_CANCEL 젠장, 드럽게 못 알아먹네. 의뢰 안 하겠소.", "미안합니다.");
                break;

            case E_ClientState.CHECK_NO_EXPIRE:
                state = new MyState(
                () => {
                    string charaText = TextManager.GetInstance().GetClientText(E_ClientState.CHECK_NO_EXPIRE, CharactorFrame.GetInstance().clientIdea);
                    string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.CHECK_NO_EXPIRE, CharactorFrame.GetInstance().clientIdea);
                    InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                }
                );

               // state = new TalkState("CHECK_NO_EXPIRE 의뢰한 물건들 잘 모아왔습니까?", "잠시만요^^");
                break;
            case E_ClientState.CHECK_EXPIRED:
                state = new MyState(
               () => {
                   string charaText = TextManager.GetInstance().GetClientText(E_ClientState.CHECK_EXPIRED, CharactorFrame.GetInstance().clientIdea);
                   string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.CHECK_EXPIRED, CharactorFrame.GetInstance().clientIdea);
                   InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
               }
               );
                /*
                state = new MyState(
                    () =>
                    {
                        InteractiveManager.GetInstance().ShowTalk("CHECK_EXPIRED 저번에 왔는데 응대를 못 받았더군.\n내 잘못은 아니니" +
                            "부득불 가격을 좀 깎아야겠네.", "네...");
                    }
                    );*/
                break;

            case E_ClientState.CHECK_START:
                state = new MyState(
                    () => {
                        InteractiveManager.GetInstance().CheckCommissionUIShow();
                    }

                    );
                break;

            case E_ClientState.CHECK_IMPERFACT_00:
                state = new MyState(
              () => {


                  string charaText = TextManager.GetInstance().GetClientText(E_ClientState.CHECK_IMPERFACT_00, CharactorFrame.GetInstance().clientIdea);
                  string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.CHECK_IMPERFACT_00, CharactorFrame.GetInstance().clientIdea);
                  InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);

              }
              );
/*
                state = new MyState(
                        InteractiveManager.GetInstance().ShowTalk("CHECK_IMPERFACT_00 0퍼센트, 장난하나.", "죄송합니다.");
                    }

                    );*/
                break;
            case E_ClientState.CHECK_IMPERFACT_50:
                state = new MyState(
             () => {
                 string charaText = TextManager.GetInstance().GetClientText(E_ClientState.CHECK_IMPERFACT_50, CharactorFrame.GetInstance().clientIdea);
                 string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.CHECK_IMPERFACT_50, CharactorFrame.GetInstance().clientIdea);
                 InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
             }
             );/*
                state = new MyState(
                    () => {
#if DEBUG_TEST
                        fsmTestHunter.GetInstance().SetNowStateIndicator(e.ToString());
#endif
                        InteractiveManager.GetInstance().ShowTalk("CHECK_IMPERFACT_50 50퍼센트, 반절도 못채웠다....", "죄송합니다.");
                    }

                    );*/
                break;
            case E_ClientState.CHECK_IMPERFACT_80:
                state = new MyState(
             () => {
                 string charaText = TextManager.GetInstance().GetClientText(E_ClientState.CHECK_IMPERFACT_80, CharactorFrame.GetInstance().clientIdea);
                 string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.CHECK_IMPERFACT_80, CharactorFrame.GetInstance().clientIdea);
                 InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
             }
             );/*
                state = new MyState(
                    () => {
#if DEBUG_TEST
                        fsmTestHunter.GetInstance().SetNowStateIndicator(e.ToString());
#endif
                        InteractiveManager.GetInstance().ShowTalk("CHECK_IMPERFACT_80 80퍼센트, 아쉽군.", "네.");
                    }

                    );*/
                break;
            case E_ClientState.CHECK_IMPERFACT_95:
                state = new MyState(
             () => {
                 string charaText = TextManager.GetInstance().GetClientText(E_ClientState.CHECK_IMPERFACT_95, CharactorFrame.GetInstance().clientIdea);
                 string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.CHECK_IMPERFACT_95, CharactorFrame.GetInstance().clientIdea);
                 InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
             }
             );/*
                state = new MyState(
                    () => {
#if DEBUG_TEST
                        fsmTestHunter.GetInstance().SetNowStateIndicator(e.ToString());
#endif
                        InteractiveManager.GetInstance().ShowTalk("CHECK_IMPERFACT_95 95퍼센트, 간발의 차로군.", "아이고.");
                    }

                    );*/
                break;
            case E_ClientState.CHECK_PERFACT:
                state = new MyState(
             () => {
                 string charaText = TextManager.GetInstance().GetClientText(E_ClientState.CHECK_PERFACT, CharactorFrame.GetInstance().clientIdea);
                 string[] playerTexts = TextManager.GetInstance().GetClient_PlayerText(E_ClientState.CHECK_PERFACT, CharactorFrame.GetInstance().clientIdea);
                 InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
             }
             );
                /*
                state = new MyState(
                    () => {
#if DEBUG_TEST
                        fsmTestHunter.GetInstance().SetNowStateIndicator(e.ToString());
#endif
                        InteractiveManager.GetInstance().ShowTalk("CHECK_PERFACT 100퍼센트, 완벽하군.", "감사합니다.");
                    }

                    );*/
                break;

            case E_ClientState.LEAVE:
                
                state = new MyState(() => {
                    InteractiveManager.GetInstance().CharactorLeave(E_Charactor.CLIENT);
                });
                break;

        }
        return state;
    }
    protected override void MakeTransitions(FSMState state, E_ClientState e)
    {
        state.ClearTransitions();
        switch (e)
        {
            case E_ClientState.IDLE:
                state.AddTransitions(
                    new MyTransition(
                    () =>
                    {
                        bool hasCommision = true;//캐릭터 틀에서 수임 여부
#if DEBUG_TEST
                        hasCommision = fsmTestHunter.GetInstance().hasCommision;
#endif
                        hasCommision = CharactorFrame.GetInstance().clientIdea.hasCommission;
                        if (hasCommision == false)//캐릭터 틀에서 수임 했는지 여부를 받아오기.
                            return true;
                        else return false;
                    },
                    GetState(E_ClientState.COMMISSION), null
                    )
                    ,
                    new MyTransition(
                        () =>
                        {
                            bool hasCommision = true;//캐릭터 틀에서 수임 여부
#if DEBUG_TEST
                            hasCommision = fsmTestHunter.GetInstance().hasCommision;
#endif
                            if (hasCommision)//캐릭터 틀에서 수임 했는지 여부를 받아오기.
                            {
                                bool expired;
#if DEBUG_TEST
                                expired = fsmTestHunter.GetInstance().expired;
#endif
                                expired = CharactorFrame.GetInstance().clientIdea.haveComeBeforeExpire;
                                if (expired)
                                {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                            }
                            else return false;//의미없음
                        },
                        GetState(E_ClientState.CHECK_EXPIRED),
                GetState(E_ClientState.CHECK_NO_EXPIRE)
                        )
                    );
                break;

            case E_ClientState.COMMISSION:
                state.AddTransitions(
                    new TriggerTransition(GetState(E_ClientState.COMMISSION_SAY_1))
                    );
                break;
            case E_ClientState.COMMISSION_SAY_1:
                state.AddTransitions(
                    new PlayerChoiceTransition(GetState(E_ClientState.COMMISSION_MAKE_START), GetState(E_ClientState.COMMISSION_SAY_2))
                    );
                break;
            case E_ClientState.COMMISSION_SAY_2:
                state.AddTransitions(
                    new PlayerChoiceTransition(GetState(E_ClientState.COMMISSION_MAKE_START), GetState(E_ClientState.COMMISSION_SAY_3))
                    );
                break;
            case E_ClientState.COMMISSION_SAY_3:
                state.AddTransitions(
                     new PlayerChoiceTransition(GetState(E_ClientState.COMMISSION_MAKE_START), GetState(E_ClientState.COMMISSION_CANCEL))
                    );
                break;
            case E_ClientState.COMMISSION_MAKE_START:
                state.AddTransitions(new TriggerTransition(GetState(E_ClientState.COMMISION_MAKE_DONE)));
                break;
            case E_ClientState.COMMISION_MAKE_DONE:
            case E_ClientState.COMMISSION_CANCEL:
                state.AddTransitions(new TriggerTransition(GetState(E_ClientState.LEAVE)));
                break;

            case E_ClientState.CHECK_NO_EXPIRE:
            case E_ClientState.CHECK_EXPIRED:
                    state.AddTransitions(new TriggerTransition(GetState(E_ClientState.CHECK_START)));
                break;

            case E_ClientState.CHECK_START:
                state.AddTransitions(
                    new MyTransition(
                        () => {
                            float completeness; //qc 끝난 거랑 얘가 갖고 있는 원래 퀘스트랑 비겨ㅛ해서 나온 완성도
                            completeness = EventParameterStorage.GetInstance().QuestCompareCompleteness;
                            if (completeness <= 0)
                                return true;
                            else return false;
                        }
                        , GetState(E_ClientState.CHECK_IMPERFACT_00),null
                        ),
                    new MyTransition(
                        () => {
                            float completeness; //qc 끝난 거랑 얘가 갖고 있는 원래 퀘스트랑 비겨ㅛ해서 나온 완성도
                            completeness = EventParameterStorage.GetInstance().QuestCompareCompleteness;
                            if (completeness > 0 && completeness <= 50)
                                return true;
                            else return false;
                        }
                        , GetState(E_ClientState.CHECK_IMPERFACT_50), null
                        ),
                    new MyTransition(
                        () => {
                            float completeness; //qc 끝난 거랑 얘가 갖고 있는 원래 퀘스트랑 비겨ㅛ해서 나온 완성도
                            completeness = EventParameterStorage.GetInstance().QuestCompareCompleteness;
                            if (completeness  > 50 && completeness <= 80)
                                return true;
                            else return false;
                        }
                        , GetState(E_ClientState.CHECK_IMPERFACT_80), null
                        ),
                    new MyTransition(
                        () => {
                            float completeness; //qc 끝난 거랑 얘가 갖고 있는 원래 퀘스트랑 비겨ㅛ해서 나온 완성도
                            completeness = EventParameterStorage.GetInstance().QuestCompareCompleteness;
                            if (completeness > 80 && completeness <= 95)
                                return true;
                            else return false;
                        }
                        , GetState(E_ClientState.CHECK_IMPERFACT_95), null
                        ),
                    new MyTransition(
                        () => {
                            float completeness; //qc 끝난 거랑 얘가 갖고 있는 원래 퀘스트랑 비겨ㅛ해서 나온 완성도
                            completeness = EventParameterStorage.GetInstance().QuestCompareCompleteness;
                            if (completeness > 95)
                                return true;
                            else return false;
                        }
                        , GetState(E_ClientState.CHECK_PERFACT), null
                        )
                    );
                break;

            case E_ClientState.CHECK_IMPERFACT_00:
            case E_ClientState.CHECK_IMPERFACT_50:
            case E_ClientState.CHECK_IMPERFACT_80:
            case E_ClientState.CHECK_IMPERFACT_95:
            case E_ClientState.CHECK_PERFACT:
                state.AddTransitions(new TriggerTransition(GetState(E_ClientState.LEAVE)));
                break;

            case E_ClientState.LEAVE:
                break;


        }
    }
}
