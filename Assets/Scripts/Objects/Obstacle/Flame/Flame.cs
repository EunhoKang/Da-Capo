using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public float bulletExistTime=8f;
    public SpriteRenderer anim;
    public Sprite[] idle;
    public WarnFlame warnPrefab;
    public GameObject spriteObject;
    [Range(0.125f,1f)]
    public float spawnTime=0.125f;
    public float idleAnimaionMultiplier=0.8f;
    public CircleCollider2D col;
    public Rigidbody2D rb;
    [HideInInspector] public bool isDisappearing;
    private float damage;
    public void OnEnable()
    {
        damage=StageManager.instance.stagefile.obstacleDamage;
        isDisappearing=false;
        col.enabled=true;
        Color c;
        c=anim.color;
        c.a=1;
        anim.color=c;
        StartCoroutine(IdleAnimation());
        StartCoroutine(TimeCheck());
    }
    IEnumerator IdleAnimation(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=StageManager.instance.stagefile.metronomeRate;
        int currentNum=0;
        anim.sprite=idle[0];
        while(!isDisappearing){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                currentNum=((int)((current-initial)*idleAnimaionMultiplier))%idle.Length;
                if(currentNum>idle.Length-1){
                    currentNum=idle.Length-1;
                }
                anim.sprite=idle[currentNum];
            }
            yield return null;
        }
    }
    IEnumerator TimeCheck(){
        yield return new WaitForSeconds(bulletExistTime);
        gameObject.SetActive(false);
    }
    public void OnDisable(){
        StopAllCoroutines();
    }
    public void ShowSpawn(Vector3 moveVector){
        StartCoroutine(ShowAnimation(spawnTime));
        StartCoroutine(Move(moveVector));
    }
    IEnumerator Move(Vector3 moveVector){
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        float spb=StageManager.instance.spb;
        Vector3 initialPos=transform.position;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        Vector3 first=initialPos;
        Vector3 second=initialPos;
        while(this.gameObject.activeInHierarchy){
            delta+=Time.deltaTime;
            rb.MovePosition(Vector3.Lerp(first,second,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt)));
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=initialPos+moveVector*rate*spb*(current-initial);
                second=initialPos+moveVector*rate*spb*(current-initial+1);
                rb.MovePosition(first);
            }
            yield return null;
        }
    }
    IEnumerator ShowAnimation(float spawnTime){
        float spb=StageManager.instance.spb;
        float rate=ObstacleManager.instance.rate;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        int maxTick=(int)(spawnTime/rate);
        float maxTickReverse=1f/maxTick;
        Color c;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float first=0;
        float second=0;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            c = anim.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            anim.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(current-initial)*maxTickReverse;
                second=(current-initial+1)*maxTickReverse;
                c = anim.color;
                c.a = first;
                anim.color=c;
            }
            yield return null;
        }
        c = anim.color;
        c.a = 1;
        anim.color=c;
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            CharacterManager.instance.PlayerGetDamage(damage*0.5f);
        }
    }
}
