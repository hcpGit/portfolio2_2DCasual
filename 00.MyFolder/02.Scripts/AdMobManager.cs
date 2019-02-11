using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Advertisements;   //애드몹이 gpgs와 충돌 문제로 유니티 애드센스 사용.

//using GoogleMobileAds.Api;


/*
 http://minhyeokism.tistory.com/69
플랫폼 별로, banner_id 와 interstitial_id 변수 값에 앞서 얻은 id를 각각 입력해줍니다.



AdMob은 RequestAd() 로 광고를 요청하고 요청에 성공하면 ShowAd()로 광고를 송출하는 형식입니다.



배너광고는 이니셜라이즈 시 Request 하고 바로 Show 합니다.



전면광고는 미리 Request 로 캐싱해 두다가 원하는 상황에 Show 한 후 광고가 닫히면 재요청하는 구조입니다.



메모리 누수를 방지하기 위해



HandleOnInterstitialAdClosed 콜백에서 interstitialAd.Destroy(); 를 해주도록 합니다.

    https://developers.google.com/admob/unity/start

출처: http://minhyeokism.tistory.com/69 [programmer-dominic.kim]
     */
public class AdMobManager : SingletonMono<AdMobManager> //애드몹과 gpgs 충돌 문제로 유니티 애드센스로 변경.
{
    bool rewarded;
    public bool Rewarded
    {
        get {
            return rewarded;
        }
    }
    protected override void Awake()
    {
        base.Awake();

    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ShowDeffenseSaveRewardAd()
    {
        rewarded = false;
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                rewarded = true;
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
        DayOffManager dom = GameObject.Find("DayOffManager").GetComponent<DayOffManager>();
        if (dom != null)
        {
            dom.AdShowDone();
        }
    }


    /*
    private RewardBasedVideoAd rewardBasedVideo;

    bool rewarded;
    public bool Rewarded
    {
        get { return rewarded; }
    }
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        InitAd();
    }

    public void InitAd()
    {
#if UNITY_ANDROID
        string appID = "ca-app-pub-8308480569643032~7490666960";    //애드몹에서 앱 추가하면 얻는 아이디.
        MobileAds.Initialize(appID);

        this.rewardBasedVideo = RewardBasedVideoAd.Instance;    //싱글턴으로 구현되있음.
        RequestRewardBasedVideo();
        rewardBasedVideo.OnAdRewarded += OnDeffenseGoldSaveAdRewarded;
        rewardBasedVideo.OnAdClosed += OnDeffenseGoldSaveAdClosed;
#endif
    }

    private void RequestRewardBasedVideo()
    {
        bool isTest = true;
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-8308480569643032/6341236827"; //애드몹에서 광고 생성 시 얻는 광고 단위 아이디. 내 광고 단위 아이디임.

        if(isTest)
        adUnitId = "ca-app-pub-3940256099942544/5224354917";    //이건 테스트용 구글 애드몹의 테스트 광고 아이디.

        //절대 개발 도중 테스트 도중 실제 내 광고 단위 아이디를 쓰면 안됨.
        //정지 머금.

#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }
    void OnDeffenseGoldSaveAdRewarded(object sender ,  Reward reward)
    {
        string rewardType = reward.Type;    //보상설명
        double rewardAmount = reward.Amount;
        rewarded = true;

        Debug.Log("보상형광고 리워드 콜백 함수 호출" + rewardType + " 어마운트 = " + rewardAmount);
    }
    void OnDeffenseGoldSaveAdClosed(object sender, EventArgs args)
    {
        Debug.Log("OnDeffenseGoldSaveAdClosed");
        RequestRewardBasedVideo();  //광고를 다시 로드해둠.
        
        DayOffManager dom = GameObject.Find("DayOffManager").GetComponent<DayOffManager>();
        if (dom != null)
        {
            dom.AdShowDone();
        }
    }

    public void ShowDeffenseSaveRewardAd()
    {
        rewarded = false;
        if (! rewardBasedVideo.IsLoaded())
        {
            RequestRewardBasedVideo();
        }

        rewardBasedVideo.Show();
        
    }



    */
}

