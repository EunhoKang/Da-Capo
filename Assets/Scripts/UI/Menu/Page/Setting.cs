using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : Page
{
    [HideInInspector] public float BGMvolume=1;
	[HideInInspector] public float SFXvolume=1;
	[HideInInspector] public bool isVibOn=false;
    public Image[] sprites;
    public Text[] texts;
    public Image[] buttons;
    public Slider bgm;
    public Slider sfx;
    private bool isChanging=false;
    public override void Appear(int init,int after){
        if(isChanging)return;
        isChanging=true;
        JClass jc=DataManager.instance._data;
        BGMvolume=jc.BGMvolume;
        SFXvolume=jc.SFXvolume;
        isVibOn=jc.isVibOn;
        bgm.value=BGMvolume;
        sfx.value=SFXvolume;
        StartCoroutine(Appearing());
    }
    public override void Disappear(int init,int after){
        if(isChanging)return;
        isChanging=true;
        DataManager.instance.Setting(bgm.value,sfx.value,isVibOn);
        SoundManager.instance.VolumeSet();
        StartCoroutine(Disappearing());
    }
    IEnumerator Appearing(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=0;
        for(int i=0;i<sprites.Length;i++){
            c=sprites[i].color;
            c.a=0;
            sprites[i].color=c;
        }
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=0;
            texts[i].color=c;
        }
        if(isVibOn){
            c=buttons[0].color;
            c.a=0;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=0;
            buttons[1].color=c;
        }else{
            c=buttons[0].color;
            c.a=0;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=0;
            buttons[1].color=c;
        }
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount+=0.125f;
                for(int i=0;i<sprites.Length;i++){
                    c=sprites[i].color;
                    c.a=cCount;
                    sprites[i].color=c;
                }
                for(int i=0;i<texts.Length;i++){
                    c=texts[i].color;
                    c.a=cCount;
                    texts[i].color=c;
                }
                if(isVibOn){
                    c=buttons[0].color;
                    c.a=cCount;
                    buttons[0].color=c;
                    c=buttons[1].color;
                    c.a=0;
                    buttons[1].color=c;
                }else{
                    c=buttons[0].color;
                    c.a=0;
                    buttons[0].color=c;
                    c=buttons[1].color;
                    c.a=cCount;
                    buttons[1].color=c;
                }
                check+=dt;
            }
            yield return null;
        }
        for(int i=0;i<sprites.Length;i++){
            c=sprites[i].color;
            c.a=1;
            sprites[i].color=c;
        }
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=1;
            texts[i].color=c;
        }
        if(isVibOn){
            c=buttons[0].color;
            c.a=1;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=0;
            buttons[1].color=c;
        }else{
            c=buttons[0].color;
            c.a=0;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=1;
            buttons[1].color=c;
        }
        isChanging=false;
    }
    IEnumerator Disappearing(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=1;
        for(int i=0;i<sprites.Length;i++){
            c=sprites[i].color;
            c.a=1;
            sprites[i].color=c;
        }
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=1;
            texts[i].color=c;
        }
        if(isVibOn){
            c=buttons[0].color;
            c.a=1;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=0;
            buttons[1].color=c;
        }else{
            c=buttons[0].color;
            c.a=0;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=1;
            buttons[1].color=c;
        }
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount-=0.125f;
                for(int i=0;i<sprites.Length;i++){
                    c=sprites[i].color;
                    c.a=cCount;
                    sprites[i].color=c;
                }
                for(int i=0;i<texts.Length;i++){
                    c=texts[i].color;
                    c.a=cCount;
                    texts[i].color=c;
                }
                check+=dt;
            }
            yield return null;
        }
        isChanging=false;
        for(int i=0;i<sprites.Length;i++){
            c=sprites[i].color;
            c.a=0;
            sprites[i].color=c;
        }
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=0;
            texts[i].color=c;
        }
        if(isVibOn){
            c=buttons[0].color;
            c.a=0;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=0;
            buttons[1].color=c;
        }else{
            c=buttons[0].color;
            c.a=0;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=0;
            buttons[1].color=c;
        }
        gameObject.SetActive(false);
    }
    public void IsVibOn(bool isOn){
        if(isChanging)return;
        UIManager.instance.DefaultSound();
        isVibOn=isOn;
        if(isVibOn){
            Color c=buttons[0].color;
            c.a=1;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=0;
            buttons[1].color=c;
        }else{
            Color c=buttons[0].color;
            c.a=0;
            buttons[0].color=c;
            c=buttons[1].color;
            c.a=1;
            buttons[1].color=c;
        }
    }

    public void OffsetMode(){
        UIManager.instance.DefaultSound();
        UIManager.instance.ShowCanvas(3);
        StageManager.instance.mapName="Setting";
        UIManager.instance.ShowCanvas(1);
        StageManager.instance.Init();
        UIManager.instance.RemoveCanvas(0);
        UIManager.instance.SetFalseUICam();
    }
}
