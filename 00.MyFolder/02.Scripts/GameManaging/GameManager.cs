using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GameManager : SingletonMono<GameManager> {

    [SerializeField]
    Image screenImage;

    bool isDayDone;
  //  WaitForSeconds wsForGameTimeGo = new WaitForSeconds(10f);   //현실시간 10초에 게임시간 15분이라는 느낌임 지금.

    float time15Min = 4f;  //4초 마다 게임 시각 15분이 흐름.
    float wineDineCheck = 1f;   //1초 마다 반문자들을 체크함.

    bool dayOnGoing;
    bool dayDone;
    float timeCursor;
    float wineDineCursor;

    bool pauseSaveDayOnGoing;
    bool pauseSaveDayDone;
    float pauseSaveTimeCursor;
    float pauseSaveWineDineCursor;

    bool firstLogin;
    
    protected override void Awake()
    {
        base.Awake();

        Debug.Log("게임매니저 어웨이크 1");
      
    }
  
    IEnumerator Start()
    {
        Debug.Log("게임매니저 스타트");

        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        
        //Screen.SetResolution(1920, 1080, false);

        Screen.SetResolution(1080, 1920, true);


        Screen.orientation = ScreenOrientation.Landscape;
        firstLogin = false;
        object n = new object();
        lock (n)
        {
            LogInGooglePlay();
        }


        DontDestroyOnLoad(this.gameObject);
        screenImage.gameObject.SetActive(false);

        dayOnGoing = false;
        dayDone = false;
        timeCursor = 0f;
        wineDineCursor = 0f;

        TextManager.GetInstance().Init();
        LanguageManager.GetInstance().Init();
        MobEviInfoManager.GetInstance().Init();
        WeaponInfoManager.GetInstance().Init();
        UIGeneralTextsManager.GetInstance().Init(); //이닛단에서 랭귀지 애드 리스너도 함.
        

        
        StartSceneUIManager ssUI = GameObject.Find("StartCanvas").GetComponent<StartSceneUIManager>();
        LanguageManager.GetInstance().AddListnerLanguageChange(ssUI.langChanged);
        ssUI.gameObject.SetActive(false);
        while (Social.localUser.authenticated == false &&  false == firstLogin) //인증 되는 거 기다림., 세이브 매니저 이닛도 기다림.
        {
            Debug.Log("인증 기다리는중");
            yield return new WaitForSeconds(2f);
        }

        Debug.Log("게임매니저 - 인증 끝 확인");
        yield return new WaitForSeconds(2f);

      
        bool saved = SaveManager.GetInstance().IsThereSavedGame();

        if (saved)
        {
            Debug.Log("게임매니저 - 세이브 있음 확인.");
            LanguageManager.GetInstance().SetLanguage(SaveManager.GetInstance().GetSavedGameLang());
        }
        ssUI.gameObject.SetActive(true);
        
        ssUI.InitStartUI();
    }

    void saveInitDone()
    {
        Debug.Log("세이브 이닛던 메세지 받음");
        firstLogin = true;
    }

    public void GoToStartScene()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartScene"))
        {
            return;
        }
        dayOnGoing = false;
        dayDone = false;
        timeCursor = 0f;
        wineDineCursor = 0f;
        //하루는 정리해줌

        Debug.Log("스타트씬으로");
        StartCoroutine(LoadingScene("StartScene", ()=> {

        StartSceneUIManager ssUI = GameObject.Find("StartCanvas").GetComponent<StartSceneUIManager>();
        LanguageManager.GetInstance().AddListnerLanguageChange(ssUI.langChanged);

            ssUI.InitStartUI();

        }));
    }


    public void NewStartGame()  //완전 새로운 게임 시작.
    {
            SaveManager.GetInstance().DeleteSavedGame();

        SettingUIManager.GetInstance().PauseBtn.interactable = true;
        Debug.Log("게임매니져 - 뉴스타트게임-");

        InGameTimeManager.GetInstance().SetMainTime(1, 1, 1, 1, 1);
        WholeMonsterRiskManager.GetInstance().Init();
        PhaseManager.GetInstance().InitForNewStart();
        GoldManager.GetInstance().SetGold(Constant.newStartGolds);
        QuestManager.GetInstance().QuestDic.Clear();
        Inventory.GetInstance().MobEvidenceInven.Clear();
        Inventory.GetInstance().WeaponInventory.Clear();
        GameEndJudgeManager.GetInstance().InitForNewStart();
        CharactorManager.GetInstance().InitForNewStart();
        TextManager.GetInstance().DistributedNames.Clear();

        //모킹/
        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            for (int j = 0; j < (int)E_Evidence.MAX; j++)
            {
                Inventory.GetInstance().AddMobEvi(new QuestPerMob((E_Monster)i, (E_Evidence)j, 3));
            }
        }
        List<Weapon> temp = new List<Weapon>();
        Weapon w = WeaponInfoManager.GetInstance().CreateWeapon(E_Weapon.AXE);
        temp.Add(w);
        Inventory.GetInstance().AddWeaponsToInven(temp);
        
        //모킹종료


        StartCoroutine( LoadingScene("QuestRoom_1", StartNewMorning));
    }
    public void LoadGame()  //저장했던 게임 시작.   저장 자료ㅗ구조 만들기.
    {
        SettingUIManager.GetInstance().PauseBtn.interactable = true;
        //저장을, 하루의 완전 종료 일때!
        //11일을 다하고 12일날 하다가 종료면
        //11일 마지막까지 저장이 된 후
        //이 로드를 불러올때는 
        //이렇게 하루 시작만 하면 되게/.
        

        bool loadSuccess = SaveManager.GetInstance().LoadGame();

        if (loadSuccess == false)
        {
            Debug.LogError("저장 데이터가 존재하지 않음!");
            return;
        }
        
        StartCoroutine(LoadingScene("QuestRoom_1", StartNewMorning));
    }




    /*
     퀘스트룸씬으로 가는 거 성공.
         */

    public bool IsDayOnGoing()
    {
        return !dayDone && dayOnGoing;
    }

    public void StartNewMorning()
    {
        InGameTimeManager.GetInstance().NewMorningStart();  //하루 올라가고 9시 00분.
        Debug.Log(InGameTimeManager.GetInstance().GetNowTime());
        string dateTxt = LanguageManager.GetInstance().GetDateString(InGameTimeManager.GetInstance().GetNowTime());

        MainUIManager.GetInstance().AdjustYMD(dateTxt);
         MainUIManager.GetInstance().AdjustTime(InGameTimeManager.GetInstance().Hour, InGameTimeManager.GetInstance().Minute);

        dayOnGoing = true;
        dayDone = false;
        timeCursor = 0f;
        wineDineCursor = 0f;

#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            if (InGameTimeManager.GetInstance().GetNowTime().GetDaysGap(Constant.GameStartTime) >= 10)
            {
                Social.ReportProgress(GPGSIds.achievement_hold_10_days, 100.0, (sucess) =>
                {
                });
            }
            if (InGameTimeManager.GetInstance().GetNowTime().GetDaysGap(Constant.GameStartTime) >= 20)
            {
                Social.ReportProgress(GPGSIds.achievement_hold_20_days, 100.0, (sucess) =>
                {
                });
            }

            Social.ReportScore(GoldManager.GetInstance().Gold, GPGSIds.leaderboard_gold_leaderboard, (s) =>
            {
            });

        }
#endif
    }

    public void Update()
    {
        if (!dayDone && !dayOnGoing)    //데이 오프.
        {
            return;
        }

        if (dayDone)    //하루가 끝났으면.
        {
            dayOnGoing = false;
            wineDineCursor += Time.deltaTime;

            if (wineDineCursor >= wineDineCheck)
            {
                wineDineCursor = 0f;
                if (false == RoomEventManager.GetInstance().IsNowWineAndDine)   //접대 중이 아니라면
                {
                    EndOfDay();
                }
            }
            return;
        }

        else if (dayOnGoing) //하루의 시작
        {
            timeCursor += Time.deltaTime;
            wineDineCursor += Time.deltaTime;

            if (timeCursor >= time15Min)
            {
                timeCursor = 0f;
                InGameTimeManager.GetInstance().Update15Minute(out dayDone); //15분 지나게 처리.
                MainUIManager.GetInstance().AdjustTime(InGameTimeManager.GetInstance().Hour, InGameTimeManager.GetInstance().Minute);
            }

            if (wineDineCursor >= wineDineCheck)
            {
                wineDineCursor = 0f;
                if (false == RoomEventManager.GetInstance().IsNowWineAndDine)   //접대 중이 아니라면
                {
                    VisitNextOne();
                }
                else {
                    //접대 중이면. 만기 방문자 부름.
                    CharactorManager.GetInstance(). SelectExpireVisit();
                }
            }
        }
    }

    public void GamePause()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartScene"))
        {
            return;
        }
        pauseSaveDayOnGoing = dayOnGoing;
         pauseSaveDayDone = dayDone;
         pauseSaveTimeCursor = timeCursor;
         pauseSaveWineDineCursor = wineDineCursor;

        dayOnGoing = false;
        dayDone = false;
    }
    public void GameContinue()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartScene"))
        {
            return;
        }
        dayOnGoing = pauseSaveDayOnGoing;
        dayDone = pauseSaveDayDone;
        timeCursor = pauseSaveTimeCursor;
        wineDineCursor = pauseSaveWineDineCursor;
    }
    
    void VisitNextOne() //다음 놈 부르기.
    {
        if (RoomEventManager.GetInstance().IsNowWineAndDine) return;    //접대중이면 패쓰함.

        EventParameterStorage.GetInstance().ParametersReset();

        CharactorIdea nextOne = CharactorManager.GetInstance().GetNextCharactor();
        if (nextOne == null) return;
        
        nextOne.AnamnesisToCharactorFrame();
        CharactorFrame.GetInstance().ComeAnim();
    }

    void EndOfDay() //하루가 끝났을 떄 호출.
    {
        Debug.Log("게임매니져 = 엔드 오브 데이");
        dayOnGoing = false;
        dayDone = false;
        timeCursor = 0f;
        wineDineCursor = 0f;

        CharactorManager.GetInstance().DayOff();    //캐릭터들 취합
        QuestManager.GetInstance().DayOff();        //연관된 캐릭터가 없는 퀘스트는 삭제.
        
        StartCoroutine(LoadingScene("DayOff", () => { }));

    }

    public void ContinueDay()
    {
        SaveManager.GetInstance().SaveGame();   //게임 저장. (새 하루가 시작하기 전에 )
        StartCoroutine(LoadingScene("QuestRoom_1", StartNewMorning));
    }

    IEnumerator LoadingScene(string sceneName , System.Action loadedAction)
    {
        AsyncOperation qrs = SceneManager.LoadSceneAsync(sceneName);
        // qrs.allowSceneActivation = false; 이걸 펄스로 해두면 프로그레스 (0~1) 은 0.9 에서 멈추고 즉, 얘가 트루가 되야 씬 로드 되고 하는것.
        //  Debug.Log(sceneName + "로딩중");
        screenImage.gameObject.SetActive(true);
        screenImage.color = Color.black;
        
        while (!qrs.isDone)
        {
        //    Debug.Log(sceneName + "로딩중"+ qrs.progress);
            screenImage.color = new Color(0, 0, 0, qrs.progress / 1f);
            //로딩 프로그레스 바 같은 거 해주기.

            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        screenImage.gameObject.SetActive(false);
        
        loadedAction();
    }

    public void LogInGooglePlay()
    {
#if UNITY_ANDROID

        Debug.Log("구글 로그인 로직 시작");
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        

        Social.localUser.Authenticate((bool success, string msg) =>
        {
            Debug.Log("구글플레이 어센티케이트 메세지 인자 = "+msg);
            if (success)
            {
                Debug.Log("구글 플레이 로그인 성공"+ Social.localUser.authenticated);
               
                //  Social.ShowAchievementsUI();
                // Social.ShowLeaderboardUI();

                Debug.Log("세이브매니저 이닛 시작");
                SaveManager.GetInstance().Init();
            }
            else {
                Debug.Log("구글 플레이 로그인 실패!!!"+ Social.localUser.authenticated);
              
            }

        });

#endif


    }

}
