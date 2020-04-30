using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : Page
{
    public Text[] texts;
    public Image[] images;
    public GameObject min;
    public GameObject hour;
    private bool isChanging=false;
    public override void Appear(int init,int after){
        if(isChanging)return;
        isChanging=true;
        StartCoroutine(Appearing());
    }
    public override void Disappear(int init,int after){
        if(isChanging)return;
        isChanging=true;
        StartCoroutine(Disappearing());
    }
    void Update()
    {
        hour.transform.Rotate(0,0,-5*Time.deltaTime);
        min.transform.Rotate(0,0,-20*Time.deltaTime);
    }
    IEnumerator Appearing(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=0;
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=cCount;
            texts[i].color=c;
        }
        for(int i=0;i<images.Length;i++){
            c=images[i].color;
            c.a=cCount;
            images[i].color=c;
        }
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount+=0.125f;
                for(int i=0;i<texts.Length;i++){
                    c=texts[i].color;
                    c.a=cCount;
                    texts[i].color=c;
                }
                for(int i=0;i<images.Length;i++){
                    c=images[i].color;
                    c.a=cCount;
                    images[i].color=c;
                }
                check+=dt;
            }
            yield return null;
        }
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=1;
            texts[i].color=c;
        }
        for(int i=0;i<images.Length;i++){
            c=images[i].color;
            c.a=cCount;
            images[i].color=c;
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
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=cCount;
            texts[i].color=c;
        }
        for(int i=0;i<images.Length;i++){
            c=images[i].color;
            c.a=cCount;
            images[i].color=c;
        }
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount-=0.125f;
                for(int i=0;i<texts.Length;i++){
                    c=texts[i].color;
                    c.a=cCount;
                    texts[i].color=c;
                }
                for(int i=0;i<images.Length;i++){
                    c=images[i].color;
                    c.a=cCount;
                    images[i].color=c;
                }
                check+=dt;
            }
            yield return null;
        }
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=0;
            texts[i].color=c;
        }
        for(int i=0;i<images.Length;i++){
            c=images[i].color;
            c.a=cCount;
            images[i].color=c;
        }
        isChanging=false;
        gameObject.SetActive(false);
    }
}
