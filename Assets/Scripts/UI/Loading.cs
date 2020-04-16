using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image black;
    public void BlackOut(){
        StartCoroutine(Blacken());
    }
    public void WhiteOut(){
        StartCoroutine(Whithen());
    }
    IEnumerator Blacken(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=0;
        c=black.color;
        c.a=0;
        black.color=c;
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount+=0.125f;
                c=black.color;
                c.a=cCount;
                black.color=c;
                check+=dt;
            }
            yield return null;
        }
        c=black.color;
        c.a=1;
        black.color=c;
    }
    IEnumerator Whithen(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=1;
        c=black.color;
        c.a=1;
        black.color=c;
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount-=0.125f;
                c=black.color;
                c.a=cCount;
                black.color=c;
                check+=dt;
            }
            yield return null;
        }
        c=black.color;
        c.a=0;
        black.color=c;
        gameObject.SetActive(false);
    }
    
}
