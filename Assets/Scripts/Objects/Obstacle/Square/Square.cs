using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public SpriteRenderer anim;
    public List<Sprite> warn;
    public Sprite appear;
    [Range(0.125f,2f)]
    public float duration=2f;
    [Range(0.125f,1f)]
    public float disappearAnim=0.25f;
    [Range(0.125f,2f)]
    public float warnTime=1f;
    private float damage;
    public CircleCollider2D col;
    IEnumerator looming;
    bool isHitPlayer;
    public void OnEnable()
    {
        isHitPlayer=false;
        transform.localScale=Vector3.one;
        damage=StageManager.instance.stagefile.obstacleDamage;
        col.enabled=false;
        Color c;
        c=anim.color;
        c.a=1;
        anim.color=c;
        anim.gameObject.transform.rotation=Quaternion.Euler(Vector3.zero);
    }
    public void OnDisable(){
        StopAllCoroutines();
    }

    public void Loom(){
        col.enabled=false;
        looming=Looming();
        StartCoroutine(looming);
    }

    IEnumerator Looming(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        int maxTick=(int)(warnTime/rate);
        int currentNum=0;
        int fixTm=(int)(EnemyManager.instance.fixTime/EnemyManager.instance.rate);
        anim.sprite=warn[0];
        while(true){
            transform.position=CharacterManager.instance.FindPlayer();
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                currentNum=(int)((current-initial))%warn.Count;
                if(currentNum>warn.Count-1){
                    currentNum=warn.Count-1;
                }
                anim.sprite=warn[currentNum];

            }
            yield return null;
        }
    }

    public void Appear(){
        StopCoroutine(looming);
        
        StartCoroutine(Appearing());
    }
    Vector3 tt= new Vector3(1.1f,1.1f,1f);
    Vector3 temp=new Vector3(0,0,180);
    IEnumerator Appearing(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        int maxTick=(int)(duration*0.25f/rate);
        float maxTickReverse=1f/maxTick;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        anim.sprite=appear;
        Vector3 fScale=tt*0;
        Vector3 sScale=tt;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            transform.localScale=Vector3.Lerp(fScale,sScale,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                fScale=tt*((current-initial)*maxTickReverse);
                sScale=tt*((current-initial+1)*maxTickReverse);
                transform.localScale=fScale;
            }
            yield return null;
        }
        col.enabled=true;
        transform.localScale=tt;

        current=TimeManager.instance.checkpoint;
        initial=current;
        maxTick=(int)(duration*0.825f/rate);
        maxTickReverse=1f/maxTick;
        delta=0;
        targetDelta=0;
        currentMt=1;
        anim.sprite=appear;
        Vector3 initv=anim.transform.rotation.eulerAngles;
        Vector3 firstv=initv;
        Vector3 secondv=firstv;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            anim.transform.rotation=Quaternion.Euler(Vector3.Lerp(firstv,secondv,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt)));
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                firstv=initv+temp*rate*0.2f*(current-initial);
                secondv=initv+temp*rate*0.2f*(current-initial+1);
                anim.transform.rotation=Quaternion.Euler(firstv);
            }
            yield return null;
        }
        col.enabled=false;
        current=TimeManager.instance.checkpoint;
        initial=current;
        maxTick=(int)(disappearAnim/rate);
        maxTickReverse=1f/maxTick;
        delta=0;
        targetDelta=0;
        currentMt=1;
        Color c=anim.color;
        c.a=1;
        anim.color=c;
        float first=1;
        float second=1;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            c=anim.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            anim.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=1f-(current-initial)*maxTickReverse;
                second=1f-(current-initial+1)*maxTickReverse;
                c=anim.color;
                c.a=first;
                anim.color=c;
            }
            yield return null;
        }
        gameObject.SetActive(false);
        c=anim.color;
        c.a=1;
        anim.color=c;
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            if(isHitPlayer){
                return;
            }
            CharacterManager.instance.PlayerGetDamage(damage);
            isHitPlayer=true;
        }
    }
}
