using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("环境音效")]
    public AudioClip ambienceClip;//风声
    public AudioClip mainMusicClip;//背景音乐声
    public AudioClip doorClip;//开门的声音

    [Header("人物Audio音效")]
    public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip deathClip;
    public AudioClip deathReturnCoinClip;
    public AudioClip collectCoinClip;
    public AudioClip winClip;


    [Header("人物Voice音效")]
    public AudioClip jumpVoiceClip;
    public AudioClip deathVoiceClip;
    public AudioClip collectCoinVoiceClip;


    AudioSource ambienceAudioSource;
    AudioSource mainMusicAudioSource;
    AudioSource openDoorAudioSource;

    AudioSource walkAudioSource;
    AudioSource crouchAudioSource;
    AudioSource jumpAudioSource;
    AudioSource deathAudioSource;
    AudioSource deathReturnCoinAudioSource;
    AudioSource collectCoinAudioSource;
    AudioSource winAudioSource;




    AudioSource voiceAudioSource;

    private void Awake()
    {
        if (current != null)
        {
            Destroy(gameObject);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        ambienceAudioSource = gameObject.AddComponent<AudioSource>();
        mainMusicAudioSource = gameObject.AddComponent<AudioSource>();
        openDoorAudioSource = gameObject.AddComponent<AudioSource>();


        walkAudioSource = gameObject.AddComponent<AudioSource>();
        crouchAudioSource = gameObject.AddComponent<AudioSource>();
        jumpAudioSource = gameObject.AddComponent<AudioSource>();
        deathAudioSource = gameObject.AddComponent<AudioSource>();
        deathReturnCoinAudioSource = gameObject.AddComponent<AudioSource>();
        collectCoinAudioSource = gameObject.AddComponent<AudioSource>();
        winAudioSource = gameObject.AddComponent<AudioSource>();

        voiceAudioSource = gameObject.AddComponent<AudioSource>();

        MainMusicAudio();//背景主音乐播放
    }

    public void MainMusicAudio()
    {
        current.ambienceAudioSource.clip = ambienceClip;
        current.ambienceAudioSource.loop = true;
        current.ambienceAudioSource.Play(); //播放风声

        current.mainMusicAudioSource.clip = mainMusicClip;
        current.mainMusicAudioSource.loop = true;
        current.mainMusicAudioSource.Play();//播放背景音乐
    }

    public static void OpenDoorAudio()
    {
        current.openDoorAudioSource.clip = current.doorClip;
        current.openDoorAudioSource.PlayDelayed(1.0f);
    }

    public static void WalkStepAudio()
    {
        int index = Random.Range(0, current.walkStepClips.Length);
        current.walkAudioSource.clip = current.walkStepClips[index];
        current.walkAudioSource.Play();
    }//foot音效
    public static void CrouchStepAudio()
    {
        int index = Random.Range(0, current.crouchStepClips.Length);
        current.crouchAudioSource.clip = current.crouchStepClips[index];
        current.crouchAudioSource.Play();
    }//crouch音效
    public static void JumpAudio()
    {
        current.jumpAudioSource.clip = current.jumpClip;
        current.jumpAudioSource.Play();

        current.voiceAudioSource.clip = current.jumpVoiceClip;
        current.voiceAudioSource.Play();
    }//jump音效和jumpvoice音效

    public static void DeathAudio()
    {
        current.deathAudioSource.clip = current.deathClip;
        current.deathAudioSource.Play();

        current.deathReturnCoinAudioSource.clip = current.deathReturnCoinClip;
        current.deathReturnCoinAudioSource.Play();

        current.voiceAudioSource.clip = current.deathVoiceClip;
        current.voiceAudioSource.Play();
    }//death音效和人物死亡voice音效

    public static void CollectCoinAudio()
    {
        current.collectCoinAudioSource.clip = current.collectCoinClip;
        current.collectCoinAudioSource.Play();

        current.voiceAudioSource.clip = current.collectCoinVoiceClip; ;
        current.voiceAudioSource.Play();
    }//收集Coin音效

    public static void WinAudio()
    {
        current.winAudioSource.clip = current.winClip;
        current.winAudioSource.Play();

        current.mainMusicAudioSource.Stop();
        current.ambienceAudioSource.Stop();
    }//开启过关音效，关闭其他音效


}
