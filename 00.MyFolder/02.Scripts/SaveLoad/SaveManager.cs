using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native;

public class SaveManager : Singleton<SaveManager> {
    SaveStructure st;
    bool savedGame;
    InGameTime savedDate;
    E_Language savedLang;

    public void Init() {
        if (false == PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success, string msg) =>
            {
                if (false == success)
                {
                    Debug.Log(" 실패 - 로그인 불가");
                    return;
                }
            });
        }
        st = null;
        savedGame = false;
        savedDate = null;
        savedLang = E_Language.ENGLISH;
        

        ISavedGameClient saveClient = PlayGamesPlatform.Instance.SavedGame;
        object n = new object();
        lock (n)
        {
            saveClient.OpenWithAutomaticConflictResolution(Constant.saveFileNameInGPGSCloud, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, (status, metaData) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    Debug.Log("세이브매니저 이닛 - 게임 오픈 성공");
                    saveClient.ReadBinaryData(metaData, (readStatus, savedByteArr) =>
                    {
                        if (readStatus == SavedGameRequestStatus.Success)
                        {
                            Debug.Log("세이브매니저 이닛 - 게임 읽어오기 성공");
                            if (savedByteArr.Length == 0)
                            {
                                Debug.Log("세이브매니저 이닛 - 게임 읽어오기 성공 - 바이트 데이타 길이가 0 이라서 빠져나감.");
                                savedGame = false;
                                savedDate = null;
                                savedLang = E_Language.ENGLISH;
                                GameManager.GetInstance().SendMessage("saveInitDone");
                                return;
                            }
                            BinaryFormatter bt = new BinaryFormatter();
                            MemoryStream ms = new MemoryStream(savedByteArr);
                            st = bt.Deserialize(ms) as SaveStructure;
                            if (st != null)
                            {
                                savedGame = true;
                                savedDate = InGameTime.DeepCopy( st.savedDate);
                                savedLang = st.lang;
                                Debug.Log("세이브매니저 이닛 - 게임 읽어오기 저장한 애 읽어오기 까지 성공 저장날짜=" + savedDate + "저장 언어" + savedLang);
                                GameManager.GetInstance().SendMessage("saveInitDone");
                               
                            }
                            else
                            {
                                Debug.Log("세이브매니저 이닛 - 게임 읽어오기 저장한 애 읽어오기에서 실패");
                                savedGame = false;
                                savedDate = null;
                                savedLang = E_Language.ENGLISH;
                                GameManager.GetInstance().SendMessage("saveInitDone");
                            }
                            ms.Dispose();
                            ms.Close();

                        }
                        else
                        {
                            savedGame = false;
                            savedDate = null;
                            savedLang = E_Language.ENGLISH;
                            Debug.Log("세이브매니저 이닛 - 게임 읽어오기자체가 실패");
                            GameManager.GetInstance().SendMessage("saveInitDone");
                        }
                    });
                }
            });
        }
    }


    public void SaveGame()
    {
        SaveStructure saveST = new SaveStructure();
        saveST.SavePrepare();
#if UNITY_ANDROID
        if (false == PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success, string msg) =>
            {
                if (false == success)
                {
                    Debug.Log("저장 실패 - 로그인 불가");
                    return;
                }
            });
        }
        Debug.Log("로그인 돼있음");
        BinaryFormatter bt = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bt.Serialize(ms, saveST);
        byte[] savingData = ms.ToArray();
        Debug.Log("시리얼라이즈 - 현재 세이빙 데이타 랭쓰"+savingData.Length );
        ms.Dispose();
        ms.Close();
        Debug.Log("시리얼라이즈 완료 , 스트림 클로즈 후 " + savingData.Length);
        ISavedGameClient saveClient =  PlayGamesPlatform.Instance.SavedGame;


        saveClient.OpenWithAutomaticConflictResolution(Constant.saveFileNameInGPGSCloud, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, (status, metaData) =>
        {
            if (status != SavedGameRequestStatus.Success)
            {
                Debug.Log("저장 실패 - 메타데이타 오픈 불가");
                return;
            }
            Debug.Log("메타데이타 오픈.");

            SavedGameMetadataUpdate updatedMetaData = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription(DateTime.Now + "at saved").Build();
            Debug.Log("저장 준비- 현재 세이빙데이타 렝쓰 " + savingData.Length);
            saveClient.CommitUpdate(metaData, updatedMetaData, savingData, (saveStatus, newMetaData) =>
            {

                if (status != SavedGameRequestStatus.Success)
                {
                    Debug.Log("저장 실패 - 저장이 불가");
                    //    return;
                }
                else
                {
                    st = saveST;
                    savedLang = saveST.lang;
                    savedGame = true;
                    savedDate = InGameTime.DeepCopy( saveST.savedDate);
                    Debug.Log("저장 성공");
                }
        });
        });
       

#else
        BinaryFormatter bt = new BinaryFormatter();
        FileStream file = File.Create(Constant.saveDataAllPath);
        bt.Serialize(file, saveST);
        file.Close();
#endif
    }

    public bool LoadGame()  //이닛 후에 불려져야함.
    {
#if UNITY_ANDROID

        if (st == null || ! savedGame)
        {
            Debug.Log("로드게임 - 세이브 데이타가 존재하지 않음 또는 "+ savedGame);
            return false;
        }

        
        if (false == PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success, string msg) =>
            {
                if (false == success)
                {
                    Debug.Log("로드 실패 - 로그인 불가");
                    return;
                }
            });
        }
        ISavedGameClient saveClient = PlayGamesPlatform.Instance.SavedGame;
        saveClient.OpenWithAutomaticConflictResolution(Constant.saveFileNameInGPGSCloud, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, (status, metaData) =>
        {
            if (status != SavedGameRequestStatus.Success)
            {
                Debug.Log("로드 실패 - 메타데이타 오픈 불가");
                return;
            }
            saveClient.ReadBinaryData(metaData, (readStatus, savedData) =>
            {

                if (readStatus == SavedGameRequestStatus.Success)
                {
                    Debug.Log("바이너리 데이타 리드 읽기 성공!");
                    byte[] savedDataByteArr = savedData;

                    BinaryFormatter bt = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream(savedDataByteArr);
                    st = bt.Deserialize(ms) as SaveStructure;
                    ms.Dispose();
                    ms.Close();

                    if (st == null) Debug.Log("리드는 했으나 스트럭쳐가 널임.");
                }
                else
                {
                    Debug.Log("바이너리 데이타 리드 읽기 실패!");
                    saveClient.ShowSelectSavedGameUI("리드가 안됨.", 5, false, false, (stq, md) => { });
                }
            });
        });
        if (st == null)
        {
            Debug.Log("리드 실패");
            return false;
        }

        

#else
        BinaryFormatter bt = new BinaryFormatter();
        FileStream file = File.Open(Constant.saveDataAllPath,FileMode.Open);

        if (file != null && file.Length > 0)
        {
            st = bt.Deserialize(file) as SaveStructure;
            file.Close();
            if (st == null)
            {
                Debug.Log("파일이 없음.");
                return false;
            }
        }
        else {
            file.Close();
            return false;
        }
#endif
        Debug.Log("로드 게임");
        //  LanguageManager.GetInstance().SetLanguage(st.lang);   언어설정은 새로 할 수 있게.
        GoldManager.GetInstance().SetGold(st.golds);
        GameEndJudgeManager.GetInstance().Load(st);
        InGameTimeManager.GetInstance().Load(st);
        CharactorManager.GetInstance().Load(st);
        QuestManager.GetInstance().Load(st);
        Inventory.GetInstance().Load(st);
        WholeMonsterRiskManager.GetInstance().Load(st);
        PhaseManager.GetInstance().Load(st);
        TextManager.GetInstance().Load(st);
        
        return true;
    }

    public bool IsThereSavedGame()
    {
#if UNITY_ANDROID
        if (st == null || !savedGame)
        {
            Debug.Log("세이브 체크 - 세이브 데이타가 존재하지 않음 또는 " + savedGame);
            return false;
        }

        return true;


        /*

        if (false == PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success, string msg) =>
            {
                if (false == success)
                {
                    Debug.Log("세이브드 실패 - 로그인 불가");
                    return;
                }
            });
        }
        ISavedGameClient saveClient = PlayGamesPlatform.Instance.SavedGame;
        bool isThere = false;
        Debug.Log("세이브 있니? ");
        saveClient.FetchAllSavedGames(DataSource.ReadCacheOrNetwork, (status, metaList) =>
        {
            if (status == SavedGameRequestStatus.Success)
            {
                Debug.Log("세이브 있니? 팻치 성공");
                if (metaList != null && metaList.Count >= 1)
                {
                    Debug.Log("세이브 있니? 팻치 성공 - 세이브 있다야 메타데이타 수 = " + metaList.Count + ", 0번째 이름" + metaList[0].Filename + " 설명" + metaList[0].Description);
                    isThere = true;
                    Debug.Log("세이브 있니? 정상 판정 =" + isThere);
                    // return;
                }
            }
            else
            {
                Debug.Log("세이브 있니? 팻치 실패");
                isThere = false;
            }
        });

        Debug.Log("세이브 있니? 콜백 나와서 판정 =" + isThere);
        if (isThere)
        {
            Debug.Log("세이브 있어. true 반환.");
            return true;
        }

        return false;
        */
#else

        //나중에 구글플레이 저장 이랑 쓰까서 생각해보기.
        if (File.Exists(Constant.saveDataAllPath))
        {
            return true;
        }
        return false;
#endif
    }



    public InGameTime GetSavedDate()//저장된 게임의 세이브 데이터 날짜를 가져오기.
    {
#if UNITY_ANDROID
        if (st == null || !savedGame)
        {
            Debug.Log("겟세이브드데이트 - 세이브 데이타가 존재하지 않음 또는 " + savedGame);
            return null;
        }
        Debug.Log("겟세이브드데이트 반환" + savedDate);
        return savedDate;

        /*
        if (!IsThereSavedGame()) return null;

        if (false == PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success, string msg) =>
            {
                if (false == success)
                {
                    Debug.Log("겟데이트 실패 - 로그인 불가");
                    return;
                }
            });
        }
        ISavedGameClient saveClient = PlayGamesPlatform.Instance.SavedGame;
        InGameTime savedDate = null;
        Debug.Log("세이브 있고, 지금 날짜 받으러 왔어.");

        saveClient.OpenWithAutomaticConflictResolution(Constant.saveFileNameInGPGSCloud, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, (status, metaData) =>
        {
            if (status != SavedGameRequestStatus.Success)
            {
                Debug.Log("날짜 로드 실패 - 메타데이타 오픈 불가");
                return;
            }
            saveClient.ReadBinaryData(metaData, (readStatus, savedData) =>
            {
                if (readStatus == SavedGameRequestStatus.Success)
                {
                    byte[] savedDataByteArr = savedData;
                    Debug.Log("날짜 로드 스테이터스는 성공, 저장된 바이트 렝쓰 = "+ savedDataByteArr.Length);

                    SaveStructure st;
                    BinaryFormatter bt = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream(savedDataByteArr);
                    st = bt.Deserialize(ms) as SaveStructure;

                    savedDate = st.savedDate;
                    Debug.Log("날짜 뽑기 위해 디시리얼라이즈도 했고,  그 날짜는" + savedDate);
                    ms.Dispose();
                    ms.Close();
                }
            });
        });
        Debug.Log("최종 반환 날짜는" + savedDate);
        return savedDate;

    */
#else
        

        if (IsThereSavedGame())
        {
            SaveStructure st = null;
            BinaryFormatter bt = new BinaryFormatter();
            FileStream file = File.Open(Constant.saveDataAllPath, FileMode.Open);

            if (file != null && file.Length > 0)
            {
                st = bt.Deserialize(file) as SaveStructure;
                file.Close();
                if (st == null)
                {
                    Debug.Log("겟 세이브드 데이트 - 저장 파일이 없음.");
                    return null;
                }
                return st.savedDate;
            }
            else
            {
                file.Close();
                Debug.Log("겟 세이브드 데이트 - 저장 파일이 없음.");
                return null;
            }
        }
        
        else return null;
#endif
    }
    public void DeleteSavedGame()   //저장된 게임 지워버리기.
    {
#if UNITY_ANDROID
        st = null;
        savedDate = null;
        savedGame = false;
        savedLang = E_Language.KOREAN;
       // if (!IsThereSavedGame()) return;

        if (false == PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success, string msg) =>
            {
                if (false == success)
                {
                    Debug.Log("딜리트 실패 - 로그인 불가");
                 //   return;
                }
            });
        }

        ISavedGameClient saveClient = PlayGamesPlatform.Instance.SavedGame;
        saveClient.OpenWithAutomaticConflictResolution(Constant.saveFileNameInGPGSCloud, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, (status, metaData) =>
        {
            if (status != SavedGameRequestStatus.Success)
            {
                Debug.Log("삭제 실패 - 메타데이타 오픈 불가");
               // return;
            }
            else saveClient.Delete(metaData);
        });
        return;
        
#else
        File.Delete(Constant.saveDataAllPath);
#endif
    }

    public E_Language GetSavedGameLang()//저장된 게임의 세이브 데이터 날짜를 가져오기.
    {
#if UNITY_ANDROID
        if (!savedGame || st == null)
        {
            Debug.Log("랭귀지를 읽어올 수 없음");
            return E_Language.KOREAN;
        }
        else {
            return savedLang;
        }
            

        /*
        if (!IsThereSavedGame()) return E_Language.KOREAN;

        if (false == PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success, string msg) =>
            {
                if (false == success)
                {
                    Debug.Log("겟 랭귀지 실패 - 로그인 불가");
                    return;
                }
            });
        }
        ISavedGameClient saveClient = PlayGamesPlatform.Instance.SavedGame;
        E_Language savedLang = E_Language.KOREAN;

        saveClient.OpenWithAutomaticConflictResolution(Constant.saveFileNameInGPGSCloud, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, (status, metaData) =>
        {
            if (status != SavedGameRequestStatus.Success)
            {
                Debug.Log("로드 실패 - 메타데이타 오픈 불가");
                //   return;
            }
            else
            {
                saveClient.ReadBinaryData(metaData, (readStatus, savedData) =>
                {
                    if (readStatus == SavedGameRequestStatus.Success)
                    {
                        byte[] savedDataByteArr = savedData;
                        SaveStructure st;
                        BinaryFormatter bt = new BinaryFormatter();
                        MemoryStream ms = new MemoryStream(savedDataByteArr);
                        st = bt.Deserialize(ms) as SaveStructure;
                        ms.Dispose();
                        ms.Close();
                        savedLang = st.lang;
                    }
                });
            }
        });

        return savedLang;
        */
#else
        if (IsThereSavedGame())
        {
            SaveStructure st = null;
            BinaryFormatter bt = new BinaryFormatter();
            FileStream file = File.Open(Constant.saveDataAllPath, FileMode.Open);

            if (file != null && file.Length > 0)
            {
                st = bt.Deserialize(file) as SaveStructure;
                file.Close();
                if (st == null)
                {
                    Debug.Log("겟 세이브드 데이트 - 저장 파일이 없음.");
                    return E_Language.KOREAN;
                }
                return st.lang;
            }
            else
            {
                file.Close();
                Debug.Log("겟 세이브드 데이트 - 저장 파일이 없음.");
                return E_Language.KOREAN;
            }
           
        }

        else return E_Language.KOREAN;
#endif
    }
}
