using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : Page
{
    public Image letter;
    public GameObject letterInside;
    public Image locks;
    private bool isChanging=false;
    public void TouchLetter(){
        UIManager.instance.DefaultSound();
        JClass jClass=DataManager.instance._data;
        bool tp=false;
        for(int i=0;i<jClass.stages.Count;i++){
            if(!jClass.stages[i].isCleared){
                tp=true;
            }
        }
        if(!tp){
            StartCoroutine(LetterCoroutine());
        }
    }
    IEnumerator LetterCoroutine(){
        letterInside.SetActive(true);
        yield return null;
    }
    public void TouchLetterInside(){
        StartCoroutine(LetterInsideCoroutine());
    }
    IEnumerator LetterInsideCoroutine(){
        letterInside.SetActive(false);
        yield return null;
    }
    public override void Appear(int init,int after){
        if(isChanging)return;
        isChanging=true;
        JClass jClass=DataManager.instance._data;
        bool tp=false;
        for(int i=0;i<jClass.stages.Count;i++){
            if(!jClass.stages[i].isCleared){
                tp=true;
            }
        }
        locks.gameObject.SetActive(tp);
        StartCoroutine(Appearing());
    }
    public override void Disappear(int init,int after){
        if(isChanging)return;
        isChanging=true;
        StartCoroutine(Disappearing());
    }
    IEnumerator Appearing(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=0;
        c=letter.color;
        c.a=cCount;
        letter.color=c;
        c=locks.color;
        c.a=cCount;
        locks.color=c;
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount+=0.125f;
                c=letter.color;
                c.a=cCount;
                letter.color=c;
                c=locks.color;
                c.a=cCount;
                locks.color=c;
                check+=dt;
            }
            yield return null;
        }
        c=letter.color;
        c.a=1;
        letter.color=c;
        c=locks.color;
        c.a=1;
        locks.color=c;
        isChanging=false;
    }
    IEnumerator Disappearing(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=1;
        c=letter.color;
        c.a=cCount;
        letter.color=c;
        c=locks.color;
        c.a=cCount;
        locks.color=c;
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount-=0.125f;
                c=letter.color;
                c.a=cCount;
                c=locks.color;
                c.a=cCount;
                locks.color=c;
                letter.color=c;
                check+=dt;
            }
            yield return null;
        }
        c=letter.color;
        c.a=0;
        letter.color=c;
        c=locks.color;
        c.a=0;
        locks.color=c;
        isChanging=false;
        gameObject.SetActive(false);
    }
}
