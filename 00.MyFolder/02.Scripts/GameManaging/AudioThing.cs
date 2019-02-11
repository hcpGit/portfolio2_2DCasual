using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioThing : SingletonMono<AudioThing> {
    public enum E_SFX {
        CLICK,
        SLIDE,
        GROWL,
        FOOTSTEP,
        GOLD,
        INVENOPEN,
        UNDERTAKE,
        MAX
    }

    [SerializeField]
    AudioSource BGMAudio;
    [SerializeField]
    AudioSource SFXAudio;


    public AudioClip BGMClip;
    public AudioClip[] SFXClick;
    public AudioClip[] SFXSlide;
    public AudioClip[] SFXGrowl;
    public AudioClip[] SFXFootStep;
    public AudioClip[] SFXGold;
    public AudioClip[] SFXInvenOpen;
    public AudioClip[] SFXUndertake;
    [Header("For Reference")]
    public AnimationClip comeAnim;
    public AnimationClip leaveAnim;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        AudioSource[] ases = GetComponents<AudioSource>();
        BGMAudio = ases[0];
        BGMAudio.clip = BGMClip;
        BGMAudio.Play();
        SFXAudio = ases[1];
    }

    public void PlaySFX(E_SFX sfx)
    {
        AudioClip clip = null;
        AudioClip[] cliplist = null;
        switch (sfx)
        {
            case E_SFX.CLICK:
                cliplist = SFXClick;
                break;
            case E_SFX.SLIDE:
                cliplist = SFXSlide;
                break;
            case E_SFX.GROWL:
                cliplist = SFXGrowl;
                break;
            case E_SFX.FOOTSTEP:
                cliplist = SFXFootStep;
                break;
            case E_SFX.GOLD:
                cliplist = SFXGold;
                break;
            case E_SFX.INVENOPEN:
                cliplist = SFXInvenOpen;
                break;
            case E_SFX.UNDERTAKE:
                cliplist = SFXUndertake;
                break;
        }
        clip = cliplist[Random.Range(0, cliplist.Length )];

        SFXAudio.PlayOneShot(clip, SettingUIManager.GetInstance().GetSFXVolume());
    }

    public void SetBGMVolume(float vol)
    {
        if (vol < 0) vol = 0;
        if (vol > 1) vol = 1f;
        BGMAudio.volume = vol;
    }
    public void StartFootSteps(bool come)
    {
        if(come)
        StartCoroutine(WalkingSound(comeAnim.length));
        else
            StartCoroutine(WalkingSound(leaveAnim.length));
    }
    /*
    public void StopFootSteps()
    {
        Debug.Log("스탑워킹");
        StopCoroutine(WalkingSound());
    }
    */
    IEnumerator WalkingSound(float length)
    {
        Debug.Log("스타트워킹");
       
        AudioClip clip = SFXFootStep[Random.Range(0, SFXFootStep.Length)];
        WaitForSeconds ws = new WaitForSeconds(clip.length);
        float time =0f;

        while (time< length)
        {
            time += clip.length;
            SFXAudio.PlayOneShot(clip, SettingUIManager.GetInstance().GetSFXVolume());
            yield return ws;
        }
    }
}
