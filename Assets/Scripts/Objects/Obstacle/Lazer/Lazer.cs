using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public SpriteRenderer anim;
    public List<Sprite> warn;
    public Sprite appear;
    public List<Sprite> disappear;
    [Range(0.125f,2f)]
    public float duration=2f;
    [Range(0.125f,1f)]
    public float disappearAnim=0.25f;
    [Range(0.125f,2f)]
    public float warnTime=1f;
    public BoxCollider2D col;
    public LazerInside scr;
    bool isLooming;
    public void OnEnable()
    {
        anim.transform.localScale=tt;
        isLooming=false;
        col.enabled=false;
        Color c;
        c=anim.color;
        c.a=1;
        anim.color=c;
        scr.damage=StageManager.instance.stagefile.obstacleDamage;
    }
    public void OnDisable(){
        StopAllCoroutines();
    }
    public void Loom(){
        col.enabled=false;
        StartCoroutine(Looming());
    }

    IEnumerator Looming(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        int maxTick=(int)(warnTime/rate);
        float maxTickReverse=1f/maxTick;
        int currentNum=0;
        int fixTm=(int)(EnemyManager.instance.fixTime/EnemyManager.instance.rate);
        anim.sprite=warn[0];
        isLooming=true;
        while(isLooming){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                currentNum=(int)((current-initial)*0.6f)%warn.Count;
                if(currentNum>warn.Count-1){
                    currentNum=warn.Count-1;
                }
                anim.sprite=warn[currentNum];
            }
            yield return null;
        }
    }

    public void Appear(){
        isLooming=false;
        StartCoroutine(Appearing());
    }
    Vector3 tt= new Vector3(1.35f,1.35f,1f);
    Vector3 ff=new Vector3(-9,0,0);
    Vector3 sf=new Vector3(1.35f,0,1);
    IEnumerator Appearing(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        int maxTick=(int)(duration*0.5f/rate);
        float maxTickReverse=1f/maxTick;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        int currentNum=0;
        int fixTm=(int)(EnemyManager.instance.fixTime/EnemyManager.instance.rate);
        anim.sprite=appear;
        Vector3 fScale=tt*0;
        Vector3 sScale=tt;
        Vector3 fPos=ff;
        Vector3 sPos=Vector3.zero;
        anim.transform.localPosition=ff;
        anim.transform.localScale=sf;
        col.enabled=true;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            anim.transform.localScale=Vector3.Lerp(fScale,sScale,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            anim.transform.localPosition=Vector3.Lerp(fPos,sPos,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                fScale=tt*((current-initial)*maxTickReverse);
                fScale.x=tt.x;
                fScale.z=tt.z;
                sScale=tt*((current-initial+1)*maxTickReverse);
                sScale.x=tt.x;
                sScale.z=tt.z;
                fPos=ff*(maxTick-(current-initial))*maxTickReverse;
                sPos=ff*(maxTick-(current-initial+1))*maxTickReverse;
                anim.transform.localScale=fScale;
                anim.transform.localPosition=fPos;
            }
            yield return null;
        }

        yield return new WaitForSeconds(duration*0.5f);
        col.enabled=false;
        current=TimeManager.instance.checkpoint;
        initial=current;
        maxTick=(int)(disappearAnim/rate);
        maxTickReverse=1f/maxTick;
        currentNum=0;
        fixTm=(int)(EnemyManager.instance.fixTime/EnemyManager.instance.rate);
        anim.sprite=disappear[0];
        while(current<maxTick+initial){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                currentNum=((int)((disappear.Count-1)*(current-initial)*maxTickReverse));
                if(currentNum>disappear.Count-1){
                    currentNum=disappear.Count-1;
                }
                anim.sprite=disappear[currentNum];
            }
            yield return null;
        }
        anim.transform.localScale=tt;
        anim.transform.localPosition=Vector3.zero;
        gameObject.SetActive(false);
    }
}
