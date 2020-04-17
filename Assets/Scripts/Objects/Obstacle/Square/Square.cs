using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public SpriteRenderer anim;
    public List<Sprite> warn;
    public List<Sprite> appear;
    public List<Sprite> disappear;
    public GameObject spriteObject;
    [Range(0.125f,2f)]
    public float duration=2f;
    [Range(0.125f,1f)]
    public float disappearAnim=0.25f;
    [Range(0.125f,2f)]
    public float warnTime=1f;
    private float damage;
    public BoxCollider2D col;
    IEnumerator looming;
    public void OnEnable()
    {
        transform.localScale=Vector3.one;
        damage=StageManager.instance.stagefile.obstacleDamage;
        col.enabled=false;
        Color c;
        c=anim.color;
        c.a=1;
        anim.color=c;
    }
    public void OnDisable(){
        StopAllCoroutines();
    }
    void Update()
    {
        //spriteObject.transform.rotation=CameraManager.instance.cam.transform.rotation;
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
        StopCoroutine(looming);
        StartCoroutine(Appearing());
    }
    Vector3 tt= new Vector3(1.1f,1.1f,1f);
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
        int currentNum=0;
        int fixTm=(int)(EnemyManager.instance.fixTime/EnemyManager.instance.rate);
        anim.sprite=appear[0];
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
                currentNum=((int)((appear.Count-1)*(current-initial)*maxTickReverse));
                if(currentNum>appear.Count-1){
                    currentNum=appear.Count-1;
                }
                anim.sprite=appear[currentNum];
            }
            yield return null;
        }
        yield return new WaitForSeconds(duration*0.0625f);
        /*
        current=TimeManager.instance.checkpoint;
        initial=current;
        maxTick=(int)(duration*0.0625f/rate);
        maxTickReverse=1f/maxTick;
        delta=0;
        targetDelta=0;
        currentMt=1;
        currentNum=0;
        fixTm=(int)(EnemyManager.instance.fixTime/EnemyManager.instance.rate);
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
            }
            yield return null;
        }
        */

        col.enabled=true;
        transform.localScale=tt;
        yield return new WaitForSeconds(duration*0.8125f);
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
        gameObject.SetActive(false);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            CharacterManager.instance.PlayerGetDamage(damage);
        }
    }
}
