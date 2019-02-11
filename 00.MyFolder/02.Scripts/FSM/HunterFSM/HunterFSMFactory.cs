using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterFSMFactory : FSMStateFactory<E_HunterState> {
    static HunterFSMFactory instance;
    public static HunterFSMFactory GetInstance()
    {
        if (instance == null)
        {
            instance = new HunterFSMFactory();
            instance.Init();
        }
        return instance;
    }
    
    public override void Init()
    {
        for (int i = 0; i < (int)E_HunterState.MAX; i++)
        {
            stateDic.Add(
                (E_HunterState)i,
                createState((E_HunterState)i)
                );
        }
        for (int i = 0; i < (int)E_HunterState.MAX; i++)
        {
            MakeTransitions(GetState((E_HunterState)i), (E_HunterState)i);
        }
    }

    protected override FSMState createState(E_HunterState e)
    {
        FSMState state = new MyState(() => { });
        switch (e)
        {
            case E_HunterState.IDLE:
                break;
            case E_HunterState.INQUIRE_QUEST:
                state = new MyState(
                   () => {
                       string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.INQUIRE_QUEST, CharactorFrame.GetInstance().hunterIdea);
                       string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.INQUIRE_QUEST, CharactorFrame.GetInstance().hunterIdea);
                       InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                   }
                   );

              //  state = new TalkState("퀘스트수임 - 수임할 의뢰 있나요?", "네.", "아니요."); //나중에 엑셀이랑 연동해서 텍스트 받아오기.
                break;
            case E_HunterState.INQUIRE_QUEST_SELECT_START:
                state = new MyState(() => {
                    InteractiveManager.GetInstance().HunterInquireQuest();
                });
                break;
            case E_HunterState.INQUIRE_QUEST_ACCEPT:
                state = new MyState(
                 () => {
                     string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.INQUIRE_QUEST_ACCEPT, CharactorFrame.GetInstance().hunterIdea);
                     string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.INQUIRE_QUEST_ACCEPT, CharactorFrame.GetInstance().hunterIdea);
                     InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                 }
                 );


                //  state = new TalkState("퀘스트 어셉트 - 후회 없을 거에요.?", "네. 수고요.");
                break;

                /* 무기 렌탈을 퀘스트 인콰이어때 같이 하는 걸로 뺌.
            case E_HunterState.INQUIRE_QUEST_RENTAL_REQUIRE:
                state = new TalkState("퀘스트 - 렌잘징징 - 너무 어려운데요.? 무기 빌려주세요.", "네." , "그냥 가세요.");
                break;

            case E_HunterState.INQUIRE_QUEST_RENTAL_START:
                state = new MyState(() => {
#if DEBUG_TEST
                    fsmTestHunter.GetInstance().SetNowStateIndicator(e.ToString());
#endif

                    InteractiveManager.GetInstance().HunterRentalShow(); });
                break;
                */

            case E_HunterState.INQUIRE_QUEST_CANCEL:
                state = new MyState(
                 () => {
                     string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.INQUIRE_QUEST_CANCEL, CharactorFrame.GetInstance().hunterIdea);
                     string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.INQUIRE_QUEST_CANCEL, CharactorFrame.GetInstance().hunterIdea);
                     InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                 }
                 );
               // state = new TalkState("퀘스트 캔슬, 나를 놓치다니.", "가세요.");
                break;
                
            case E_HunterState.HUNT_REWARD:
                state = new MyState(
                 () => {
                     string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.HUNT_REWARD, CharactorFrame.GetInstance().hunterIdea);
                     string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.HUNT_REWARD, CharactorFrame.GetInstance().hunterIdea);
                     InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                 }
                 );
               // state = new TalkState(" 헌팅 리워드 첫인사 - 안녕하세요. 의뢰 맡았던 사람이에요..", "어서오세요.");
                break;

            case E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_NOT_BROKEN:
                state = new MyState(()=>
                {
                    InteractiveManager.GetInstance().HunterReturnRental();
                    string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_NOT_BROKEN, CharactorFrame.GetInstance().hunterIdea);
                    string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_NOT_BROKEN, CharactorFrame.GetInstance().hunterIdea);
                    InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);

                   // InteractiveManager.GetInstance().ShowTalk("무기 잘썼어요.", "그래요.");
                }
                );
                break;

            case E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_BROKEN:
                state = new MyState(() =>
                {
                    InteractiveManager.GetInstance().HunterReturnRental();

                    string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_NOT_BROKEN, CharactorFrame.GetInstance().hunterIdea);
                    string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_NOT_BROKEN, CharactorFrame.GetInstance().hunterIdea);
                    InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                  //  InteractiveManager.GetInstance().ShowTalk("무기 부서진 게 있네요.. 미안욧", "젠장!");
                }
              );
                break;

            case E_HunterState.HUNT_REWARD_EXPIRED:
                state = new MyState(
                () => {
                    string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.HUNT_REWARD_EXPIRED, CharactorFrame.GetInstance().hunterIdea);
                    string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.HUNT_REWARD_EXPIRED, CharactorFrame.GetInstance().hunterIdea);
                    InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                }
                );

             //   state = new TalkState("HUNT_REWARD_EXPIRED 저번에 왔는데 없더라구요. 만기 지났지만 내 잘못 아닙니다..", "아,녜.");
                break;

            case E_HunterState.HUNT_REWARD_START:
                state = new MyState(()=> {
                    InteractiveManager.GetInstance().HunterRewardUIShow(); });
                break;
                
            case E_HunterState.HUNT_REWARD_ALL_PAYMENT:
                state = new MyState(() => {
                    InteractiveManager.GetInstance().HunterRewardCalculate(E_RewardType.ALL_PAYMENT);
                    
                   string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.HUNT_REWARD_ALL_PAYMENT, CharactorFrame.GetInstance().hunterIdea);
                   string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.HUNT_REWARD_ALL_PAYMENT, CharactorFrame.GetInstance().hunterIdea);
                   InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
               
              
                  //  InteractiveManager.GetInstance().ShowTalk("완전지불. 감사합니다.", "잘가세요!");
                });
                break;

            case E_HunterState.HUNT_REWARD_PARTIAL_PAYMENT:
                state = new MyState(() => {
                    InteractiveManager.GetInstance().HunterRewardCalculate(E_RewardType.PARTIAL_PAYMENT);

                    string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.HUNT_REWARD_PARTIAL_PAYMENT, CharactorFrame.GetInstance().hunterIdea);
                    string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.HUNT_REWARD_PARTIAL_PAYMENT, CharactorFrame.GetInstance().hunterIdea);
                    InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);

                    //InteractiveManager.GetInstance().ShowTalk("부분지불. 그래도, 감사합니다.", "잘가세요!");
                });
                break;

            case E_HunterState.HUNT_REWARD_DENIED:
                state = new MyState(() => {
                    InteractiveManager.GetInstance().HunterRewardCalculate(E_RewardType.PAYMENT_DENY);

                    string charaText = TextManager.GetInstance().GetHunterText(E_HunterState.HUNT_REWARD_DENIED, CharactorFrame.GetInstance().hunterIdea);
                    string[] playerTexts = TextManager.GetInstance().GetHunter_PlayerText(E_HunterState.HUNT_REWARD_DENIED, CharactorFrame.GetInstance().hunterIdea);
                    InteractiveManager.GetInstance().ShowTalk(charaText, playerTexts);
                    //InteractiveManager.GetInstance().ShowTalk("젠장! 다신 오나 봐라.", "뉘예뉘예");
                });
                break;
                

            case E_HunterState.LEAVE:


                state = new MyState(() => {
                InteractiveManager.GetInstance().CharactorLeave(E_Charactor.HUNTER);
                });
                break;
        }

        return state;
    }

    protected override void MakeTransitions(FSMState state, E_HunterState e)
    {
        state.ClearTransitions();
        switch (e)
        {
            case E_HunterState.IDLE:
                state.AddTransitions(
                    new MyTransition(
                        () =>
                        {
                            bool hasCommision=true;//캐릭터 틀에서 수임 여부
                            hasCommision = CharactorFrame.GetInstance().hunterIdea.hasCommission;

                            if (hasCommision)
                                return true;
                            else return false;
                        },
                        GetState(E_HunterState.HUNT_REWARD),
                        GetState(E_HunterState.INQUIRE_QUEST)
                        )
                    );
                break;
            case E_HunterState.INQUIRE_QUEST:
                state.AddTransitions(new PlayerChoiceTransition(
                    GetState(E_HunterState.INQUIRE_QUEST_SELECT_START)
                    , GetState(E_HunterState.INQUIRE_QUEST_CANCEL)));
                break;
            case E_HunterState.INQUIRE_QUEST_SELECT_START:  //퀘스트 인콰이;어 ui에서 플레이어초이스에 퀘스트 선택 여부를 담음.
                state.AddTransitions(
                new PlayerChoiceTransition(
                    GetState(E_HunterState.INQUIRE_QUEST_ACCEPT), GetState(E_HunterState.INQUIRE_QUEST_CANCEL)));

                    /*  무기렌탈 로직을 퀘스트 인콰이어와 합쳤음.
                    ,
                new MyTransition(
                    ()=>
                    {
                        bool pc = EventParameterStorage.GetInstance().PlayerChoice;

                            if (pc == true)//퀘스트 선택창의 제출 버튼을 누른것.
                        {
                            //요구능력 치에 따라서 징징과 어셉트를 구분해야함.
#if DEBUG_TEST
                            if (fsmTestHunter.GetInstance().isHuntersPowerMoreThanQuest)
                            {
                                return true;
                            }
#else
                            if (
                            QuestManager.GetInstance().GetQuest(
                            EventParameterStorage.GetInstance().selectedQuestKey)
                            .GetWeight() 
                            <=
                            CharactorFrame.GetInstance().hunterIdea.HuntingCapabillity
                            
                            ) //만약 헌터의 능력치가 퀘스트의 요구 능력치를 상회한다면. )
                            {
                                return true;
                            }
#endif
                            else return false;//헌터의 능력치가 퀘스트의 요구 능력치에 못미침.
                        }
                        return false;   //의미 없음.
                    }
                    ,GetState(E_HunterState.INQUIRE_QUEST_ACCEPT),
                    GetState(E_HunterState.INQUIRE_QUEST_RENTAL_REQUIRE)
                    )

    */

                    
                break;
            case E_HunterState.INQUIRE_QUEST_ACCEPT:
                state.AddTransitions(new TriggerTransition(GetState(E_HunterState.LEAVE)));
                break;

                /*무기 렌탈 로직을 퀘스트 인콰이어와 합침.
            case E_HunterState.INQUIRE_QUEST_RENTAL_REQUIRE:
                state.AddTransitions(new PlayerChoiceTransition(GetState(E_HunterState.INQUIRE_QUEST_RENTAL_START),
                    GetState(E_HunterState.INQUIRE_QUEST_ACCEPT)//징징 했으나 무시해서 그냥 의뢰만 받아감.
                    ));
                break;

            case E_HunterState.INQUIRE_QUEST_RENTAL_START:
                state.AddTransitions(new TriggerTransition(GetState(E_HunterState.LEAVE)));
                //무기를 빌려주든 말든 그 로직처리는 ui 제출 버튼 쪽에서 할테니까.
                //그냥 리브 해주면 됨.
                break;
                */

            case E_HunterState.INQUIRE_QUEST_CANCEL:
                state.AddTransitions(new TriggerTransition(GetState(E_HunterState.LEAVE)));
                break;

            case E_HunterState.HUNT_REWARD:
                state.AddTransitions(
                    new MyTransition(
                        () =>
                        {
                            bool rentalSth = true;//캐릭터 틀이든 뭐든 대여한 무기가 있는지, 없는지 검사함.
                            bool isRentalBroken = true; //무기의 내구도 도 조건이야...
                            HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
                            rentalSth = hunter.DidRentalWeapon();

                            if (rentalSth)
                            {
                                isRentalBroken = hunter.IsBrokenRental();
                            }
                            
                            if (rentalSth && isRentalBroken) //빌린게 있음. 그리고 그것은 부서진 것이 존재한다.
                            {
                                return true;
                            }
                            else return false;
                        }
                        , GetState(E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_BROKEN), null
                        )
                        ,
                    new MyTransition(
                        () =>
                        {
                            bool rentalSth = true;//캐릭터 틀이든 뭐든 대여한 무기가 있는지, 없는지 검사함.
                            bool isRentalBroken = true; //무기의 내구도 도 조건이야...

                            HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
                            rentalSth = hunter.DidRentalWeapon();

                            if (rentalSth)
                            {
                                isRentalBroken = hunter.IsBrokenRental();
                            }
                            if (rentalSth && !isRentalBroken) //빌린게 있음.
                            {
                                return true;
                            }
                            else return false;
                        }
                        , GetState(E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_NOT_BROKEN), null
                        )
                        ,
                    new MyTransition(
                        () =>
                        {
                            bool rentalSth = true;//캐릭터 틀이든 뭐든 대여한 무기가 있는지, 없는지 검사함.
                            bool expired = true; //만기에 늦었는지.

                            HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
                            rentalSth = hunter.DidRentalWeapon();
                            

                            expired = hunter.haveComeBeforeExpire;

                            if (!rentalSth && expired) //빌린게 있음.
                            {
                                return true;
                            }
                            else return false;
                        }
                        , GetState(E_HunterState.HUNT_REWARD_EXPIRED), null
                        )
                        ,
                      new MyTransition(
                        () =>
                        {
                            bool rentalSth = true;//캐릭터 틀이든 뭐든 대여한 무기가 있는지, 없는지 검사함.
                            bool expired = true; //만기에 늦었는지.

                            HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
                            rentalSth = hunter.DidRentalWeapon();
                            expired = hunter.haveComeBeforeExpire;

                            if (!rentalSth && !expired) //빌리지 않았고, 만기에 늦지도 않았음.
                            {
                                return true;
                            }
                            else return false;
                        }
                        , GetState(E_HunterState.HUNT_REWARD_START), null
                        )
                    );

                break;

            case E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_NOT_BROKEN:
                state.AddTransitions(
                   new MyTransition(
                       () =>
                       {
                           bool expired = true;//만기 전에 왔었는지 어쟀는지.
                           HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
                           expired = hunter.haveComeBeforeExpire;
                           if (expired) //만지 전에 왔었다가 그냥 갔음.
                           {
                               return true;
                           }
                           else return false;
                       }
                       , GetState(E_HunterState.HUNT_REWARD_EXPIRED),
                       GetState(E_HunterState.HUNT_REWARD_START)
                       ));
                break;
            case E_HunterState.HUNT_REWARD_RETURN_OF_RENTAL_BROKEN:
                state.AddTransitions(
                  new MyTransition(
                      () =>
                      {
                          bool expired = true;//만기 전에 왔었는지 어쟀는지.
                          HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
                          expired = hunter.haveComeBeforeExpire;
                          if (expired) //만지 전에 왔었다가 그냥 갔음.
                           {
                              return true;
                          }
                          else return false;
                      }
                      , GetState(E_HunterState.HUNT_REWARD_EXPIRED),
                      GetState(E_HunterState.HUNT_REWARD_START)
                      ));
                break;
                
            case E_HunterState.HUNT_REWARD_EXPIRED:
                state.AddTransitions(new TriggerTransition(GetState(E_HunterState.HUNT_REWARD_START)));
                break;

            case E_HunterState.HUNT_REWARD_START:
                state.AddTransitions(
                    new MyTransition(
                        () =>
                        {
                            int pmc = EventParameterStorage.GetInstance().PlayerMultipleChoice;
                            if (pmc == 0)//완전지불이면
                                return true;
                            else return false;
                        }
                        , GetState(E_HunterState.HUNT_REWARD_ALL_PAYMENT), null
                        ),
                     new MyTransition(
                        () =>
                        {
                            int pmc = EventParameterStorage.GetInstance().PlayerMultipleChoice;
                            if (pmc == 2)//지불 거부, 거래쫑이면.
                                return true;
                            else return false;
                        }
                        , GetState(E_HunterState.HUNT_REWARD_DENIED), null
                        ),
                      new MyTransition(
                        () =>
                        {
                            int pmc = EventParameterStorage.GetInstance().PlayerMultipleChoice;
                            bool beSulk = true;

                            beSulk = CharactorFrame.GetInstance().hunterIdea.IsSulked();
                            if (pmc == 1)//부분지불이면
                            {
                                if (false == beSulk)//성격 등등을 통해 갬블하고
                                                    //삐쳤으면 거래 거부 해버림.
                                                    //안 삐쳤으면 부분 지불 받고 감.
                                    return true;
                                else return false;
                            }
                            else
                            {
                                Debug.LogError("EPS에러." + pmc);
                                return false;
                            }
                        }
                        , GetState(E_HunterState.HUNT_REWARD_PARTIAL_PAYMENT), GetState(E_HunterState.HUNT_REWARD_DENIED)
                        )
                    );
                break;

            case E_HunterState.HUNT_REWARD_ALL_PAYMENT:
                state.AddTransitions( new TriggerTransition(GetState(E_HunterState.LEAVE)));
                break;

            case E_HunterState.HUNT_REWARD_PARTIAL_PAYMENT:
                state.AddTransitions(new TriggerTransition(GetState(E_HunterState.LEAVE)));
                break;

            case E_HunterState.HUNT_REWARD_DENIED:
                state.AddTransitions(new TriggerTransition(GetState(E_HunterState.LEAVE)));
                break;
                
            case E_HunterState.LEAVE:
                break;
        }
    }
}
