using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnFlame : MonoBehaviour
{
    public SpriteRenderer spr;
    void Update(){
        transform.rotation=CameraManager.instance.cam.transform.rotation;
    }

    public void ShowWarn(float warntime){
        StartCoroutine(ShowAnimation(warntime));
    }
    Vector3 temp=new Vector3(1,1,1);
    IEnumerator ShowAnimation(float warntime){
        float spb=StageManager.instance.spb;
        float rate=ObstacleManager.instance.rate;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        int maxTick=(int)(warntime*0.125f/rate);
        float maxTickReverse=1f/maxTick;
        Color c;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float first=0;
        float second=0;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            c = spr.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            spr.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(current-initial)*maxTickReverse*0.8f;
                second=(current-initial+1)*maxTickReverse*0.8f;
                c = spr.color;
                c.a = first;
                spr.color=c;
            }
            yield return null;
        }
        c = spr.color;
        c.a = 0.8f;
        spr.color=c;
        yield return new WaitForSeconds(warntime*0.875f);
        current=TimeManager.instance.checkpoint;
        initial=current;
        delta=0;
        targetDelta=0;
        currentMt=1;
        first=1;
        second=1;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            c = spr.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            spr.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(maxTick-(current-initial))*maxTickReverse;
                second=(maxTick-(current-initial+1))*maxTickReverse;
                c = spr.color;
                c.a = first;
                spr.color=c;
            }
            yield return null;
        }
        c = spr.color;
        c.a = 0;
        spr.color=c;
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
