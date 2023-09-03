using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class enemy : MonoBehaviour
{
    protected float health;
    protected Rigidbody2D rb;
    public SpriteRenderer appearSprite;
    public SpriteRenderer animationSprite;
    public float idleAnimaionMultiplier=0.8f;
    protected IEnumerator animator;
    public Sprite[] appear;
    public Sprite[] idle;
    public Sprite[] attack;
    public Sprite[] wait;
    public int appearLoomCount;
    public virtual void SetUp(){ 
        rb=GetComponent<Rigidbody2D>();
        appearSprite.gameObject.SetActive(false);
    }
    protected virtual void Update(){}
    public IEnumerator PlayerLoom(){
        animationSprite.gameObject.SetActive(true);
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        Color c;
        c = animationSprite.color;
        c.a = 0;
        animationSprite.color = c;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float rate=StageManager.instance.stagefile.metronomeRate;
        float first=0;
        float second=0;
        while(current<10+initial){
            delta+=Time.deltaTime;
            c = animationSprite.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            animationSprite.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(current-initial)*0.1f;
                second=(current-initial+1)*0.1f;
                c = animationSprite.color;
                c.a = first;
                animationSprite.color = c;
            }
            yield return null;
        }
        c = animationSprite.color;
        c.a = 1;
        animationSprite.color = c;
    }
    public virtual void SilhouetteMode(){
        animator=null;
        appearSprite.gameObject.SetActive(true);
        animationSprite.gameObject.SetActive(false);
    }
    public IEnumerator Idle(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=StageManager.instance.stagefile.metronomeRate;
        int currentNum=0;
        animationSprite.sprite=idle[0];
        while(true){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                currentNum=((int)((current-initial)*idleAnimaionMultiplier))%idle.Length;
                if(currentNum>idle.Length-1){
                    currentNum=idle.Length-1;
                }
                animationSprite.sprite=idle[currentNum];
            }
            yield return null;
        }
    }
    public void EnemyWait(){
        if(animator!=null){
            StopCoroutine(animator);
        }
        animator=Wait();
        StartCoroutine(animator);
    }
    public IEnumerator Wait(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=StageManager.instance.stagefile.metronomeRate;
        int maxTick=(int)(EnemyManager.instance.warnTime/rate);
        float maxTickReverse=1f/maxTick;
        int currentNum=0;
        animationSprite.sprite=wait[0];
        while(current<maxTick+initial){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                currentNum=(int)((wait.Length-1)*(current-initial)*maxTickReverse);
                if(currentNum>wait.Length-1){
                    currentNum=wait.Length-1;
                }
                animationSprite.sprite=wait[currentNum];
            }
            yield return null;
        }
    }
    public virtual void EnemyShot(){
        if(animator!=null){
            StopCoroutine(animator);
        }
    }
    public virtual void Dead(){
        animator=null;
        StartCoroutine(DeadCoroutine());
    }

    public IEnumerator DeadCoroutine(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        Color c;
        c = animationSprite.color;
        c.a = 1;
        animationSprite.color = c;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float rate=StageManager.instance.stagefile.metronomeRate;
        float first=1;
        float second=1;
        while(current<8+initial){
            delta+=Time.deltaTime;
            c = animationSprite.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            animationSprite.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(8-(current-initial))*0.125f;
                second=(8-(current-initial+1))*0.125f;
                c = animationSprite.color;
                c.a = first;
                animationSprite.color = c;
            }
            yield return null;
        }
        c = animationSprite.color;
        c.a = 0;
        animationSprite.color = c;
        gameObject.SetActive(false);
    }
}
