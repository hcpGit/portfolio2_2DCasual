using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingUIManager : SingletonMono<SettingUIManager> {
    public Button PauseBtn;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public GameObject pauseShow;
    [SerializeField]
    bool paused=false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        pauseShow.SetActive(false);
        gameObject.SetActive(false);
        PauseBtn.interactable = false;
    }
    public void PauseGame()
    {
        paused = !paused;
        if (paused)
        {
            pauseShow.SetActive(true);
            GameManager.GetInstance().GamePause();
        }
        else
        {
            pauseShow.SetActive(false);
            GameManager.GetInstance().GameContinue();
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);//나중에 트윈을 쓰든지.
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
    }
    
    public void OnValueChanged_BGMSlider(Slider slider)
    {
        AudioThing.GetInstance().SetBGMVolume(slider.value);
    }
    public void OnValueChanged_SFXSlider(Slider slider)
    {

    }
    public float GetSFXVolume()
    {
        return SFXSlider.value;
    }

    public void OnClickExit()
    {
        if (paused == true) return;
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        this.gameObject.SetActive(false);
    }
    public void OnClick_GoToStartScene()
    {
        Debug.Log("스타트씬으로");
        GameManager.GetInstance().GoToStartScene();
        this.gameObject.SetActive(false);
    }
}
