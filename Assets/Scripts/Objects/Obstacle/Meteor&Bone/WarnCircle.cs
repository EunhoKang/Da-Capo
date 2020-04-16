using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnCircle : MonoBehaviour
{
    public SpriteRenderer warnPos;
    public SpriteRenderer circlePoint;
    public SpriteRenderer edge;

    public void ShowWarn(float warntime,Sprite warnSprite){
        warnPos.sprite=warnSprite;
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
            c = warnPos.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            warnPos.color=c;
            circlePoint.color=c;
            edge.color=c;
            circlePoint.transform.localScale=temp*c.a;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(current-initial)*maxTickReverse;
                second=(current-initial+1)*maxTickReverse;
                c = warnPos.color;
                c.a = first;
                warnPos.color=c;
                circlePoint.color=c;
                edge.color=c;
                circlePoint.transform.localScale=temp*c.a;
            }
            yield return null;
        }
        c = warnPos.color;
        c.a = 1;
        warnPos.color=c;
        circlePoint.color=c;
        edge.color=c;
        circlePoint.transform.localScale=temp*c.a;
        yield return new WaitForSeconds(warntime*0.75f);
        current=TimeManager.instance.checkpoint;
        initial=current;
        delta=0;
        targetDelta=0;
        currentMt=1;
        first=1;
        second=1;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            c = warnPos.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            warnPos.color=c;
            circlePoint.color=c;
            edge.color=c;
            circlePoint.transform.localScale=temp*(2f-c.a);
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(maxTick-(current-initial))*maxTickReverse;
                second=(maxTick-(current-initial+1))*maxTickReverse;
                c = warnPos.color;
                c.a = first;
                warnPos.color=c;
                circlePoint.color=c;
                edge.color=c;
                circlePoint.transform.localScale=temp*(2-c.a);
            }
            yield return null;
        }
        c = warnPos.color;
        c.a = 0;
        warnPos.color=c;
        circlePoint.color=c;
        edge.color=c;
        circlePoint.transform.localScale=temp*(2.5f-1.5f*c.a);
        warnPos.sprite=null;
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
