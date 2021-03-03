using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public AudioClip mergeClip;
    public AudioClip maxMergeClip;
    public AudioClip replayClip;
    public AudioClip dropFloorClip;
    public AudioClip boomClip;


    AudioSource asMerge;
    AudioSource asMaxMerge;
    AudioSource asReplay;
    AudioSource asDropFloor;
    AudioSource asBoom;

    private void Awake()
    {
        mergeClip = Resources.Load("Audios/coinmerge", typeof(AudioClip)) as AudioClip;
        maxMergeClip = Resources.Load("Audios/maxbigcoin", typeof(AudioClip)) as AudioClip;
        replayClip = Resources.Load("Audios/clickbutton", typeof(AudioClip)) as AudioClip;
        dropFloorClip = Resources.Load("Audios/coinbiubiubiu", typeof(AudioClip)) as AudioClip;
        boomClip = Resources.Load("Audios/coinboom", typeof(AudioClip)) as AudioClip;

        asMerge = this.gameObject.AddComponent<AudioSource>();
        asMaxMerge = this.gameObject.AddComponent<AudioSource>();
        asReplay = this.gameObject.AddComponent<AudioSource>();
        asDropFloor = this.gameObject.AddComponent<AudioSource>();
        asBoom = this.gameObject.AddComponent<AudioSource>();

        asMerge.playOnAwake = false;
        asMaxMerge.playOnAwake = false;
        asReplay.playOnAwake = false;
        asDropFloor.playOnAwake = false;
        asBoom.playOnAwake = false;

        asMerge.clip = mergeClip;
        asMaxMerge.clip = maxMergeClip;
        asReplay.clip = replayClip;
        asDropFloor.clip = dropFloorClip;
        asBoom.clip = boomClip;
    }
    public void PlayAudioMerge()
    {
        asMerge.Stop();
        asMerge.Play();
    }
    public void PlayAudioMaxMerge()
    {
        asMaxMerge.Stop();
        asMaxMerge.Play();
    }
    public void PlayReplay()
    {
        asReplay.Stop();
        asReplay.Play();
    }

    public void PlayDropFloor()
    {
        asDropFloor.Stop();
        asDropFloor.Play();
    }
    public void PlayBoom()
    {
        asBoom.Stop();
        asBoom.Play();
    }
}
