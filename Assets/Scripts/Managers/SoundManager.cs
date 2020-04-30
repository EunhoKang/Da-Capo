using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}
    public AudioSource BGM;
    public AudioSource uiBGM;
    public AudioSource[] SFX;
    [HideInInspector]public float initialBGMVolume;
    [HideInInspector]public float initialSFXVolume;

    public void Init(){
        VolumeSet();
    }
    public void VolumeSet(){
        initialBGMVolume=DataManager.instance._data.BGMvolume;
        initialSFXVolume=DataManager.instance._data.SFXvolume;
        BGM.Pause();
        BGM.volume=initialBGMVolume;
        BGM.UnPause();
        uiBGM.Pause();
        uiBGM.volume=initialBGMVolume;
        uiBGM.UnPause();
        for(int i=0;i<SFX.Length;i++){
            SFX[i].Pause();
            SFX[i].volume=initialSFXVolume;
            SFX[i].UnPause();
        }
    }
    public void BGMPlay(AudioClip bgm){
        uiBGM.clip=bgm;
        uiBGM.volume=initialBGMVolume;
        uiBGM.Play();
    }

    public void BGMStop(){
        uiBGM.Stop();
    }
    public void UIBGMFadeOut(){
        StartCoroutine(UIFadeOutCoroutine());
    }
    public IEnumerator UIFadeOutCoroutine(){
        float temp=initialBGMVolume;
        for(int i=0;i<10;i++){
            uiBGM.volume-=temp*0.1f;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public IEnumerator SongPlayCoroutine(AudioClip c,float offset){
        BGM.clip=c;
        BGM.volume=initialBGMVolume;
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        yield return new WaitForSeconds(offset);
        BGM.Play();
    }
    public void SongPlay(AudioClip c,float offset){
        StartCoroutine(SongPlayCoroutine(c,offset));
    }
    public void SoundPause(){
        BGM.Pause();
    }
    public void SoundResume(){
        BGM.UnPause();
        BGM.volume=initialBGMVolume;
    }
    public void SFXPlay(AudioClip c,int num){
        SFX[num].clip=c;
        SFX[num].volume=initialSFXVolume;
        if(num==2){
            SFX[num].time=0.01f;
        }
        SFX[num].Play();
    }

    public void EndSound(){
        StopAllCoroutines();
        BGM.Stop();
        BGM.clip=null;
    }
    public void BGMFadeOut(){
        StartCoroutine(FadeOutCoroutine());
    }
    public IEnumerator FadeOutCoroutine(){
        float temp=initialBGMVolume;
        for(float i=1f;i>=0f;i-=0.1f){
            BGM.volume=temp*i;
            yield return new WaitForSeconds(0.05f);
        }
        BGM.volume=0;
    }
}
