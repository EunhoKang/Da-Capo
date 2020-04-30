using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Text loading;
    public Image[] loadings;
    public GameObject hour;
    public GameObject min;
    private string[] strings={"LOADING.","LOADING..","LOADING..."};
    private int c;
    private int current;
    public void BlackOut(){
        StartCoroutine(Blacken());
    }
    public void WhiteOut(){
        StartCoroutine(Whithen());
    }
    void OnEnable()
    {
        c=0;
        current=0;
    }
    void Update(){
        hour.transform.Rotate(0,0,-25*Time.deltaTime);
        min.transform.Rotate(0,0,-100*Time.deltaTime);
        c++;
        if(c>30){
            c=0;
            current++;
            loading.text=strings[current%3];
        }
    }
    IEnumerator Blacken(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=0;
        for(int i=0;i<loadings.Length;i++){
            c=loadings[i].color;
            c.a=0;
            loadings[i].color=c;
        }
        c=loading.color;
        c.a=0;
        loading.color=c;
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount+=0.125f;
                for(int i=0;i<loadings.Length;i++){
                    c=loadings[i].color;
                    c.a=cCount;
                    loadings[i].color=c;
                }
                c=loading.color;
                c.a=cCount;
                loading.color=c;
                check+=dt;
            }
            yield return null;
        }
        for(int i=0;i<loadings.Length;i++){
            c=loadings[i].color;
            c.a=1;
            loadings[i].color=c;
        }
        c=loading.color;
        c.a=1;
        loading.color=c;
    }
    IEnumerator Whithen(){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=1;
        for(int i=0;i<loadings.Length;i++){
            c=loadings[i].color;
            c.a=1;
            loadings[i].color=c;
        }
        c=loading.color;
        c.a=1;
        loading.color=c;
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount-=0.125f;
                for(int i=0;i<loadings.Length;i++){
                    c=loadings[i].color;
                    c.a=cCount;
                    loadings[i].color=c;
                }
                c=loading.color;
                c.a=cCount;
                loading.color=c;
                check+=dt;
            }
            yield return null;
        }
        for(int i=0;i<loadings.Length;i++){
            c=loadings[i].color;
            c.a=0;
            loadings[i].color=c;
        }
        c=loading.color;
        c.a=0;
        loading.color=c;
        gameObject.SetActive(false);
    }
    
}
