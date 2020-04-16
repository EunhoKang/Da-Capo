using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerEnemy : enemy
{
    public GameObject lazer;
    public SpriteRenderer shotSilhouette;
    public SpriteRenderer shotSprite;
    public BoxCollider2D lazerCollider;
    public GameObject lazerCenter;
    [HideInInspector]public float enemyLazerDuration;
    private Vector3 initialPos;
    private Vector3 initialScale=new Vector3(3,0,1);
    WaitForSeconds lazerWait;
    WaitForSeconds fixtime;
    WaitForSeconds animationDelay;
    protected bool rotationFixed;
    public override void SetUp(){
        enemyLazerDuration=EnemyManager.instance.enemyLazerDuration;
        shotSilhouette.gameObject.SetActive(false);
        lazer.SetActive(false);
        lazerWait=new WaitForSeconds(enemyLazerDuration);
        fixtime=new WaitForSeconds(StageManager.instance.stagefile.fixTime*StageManager.instance.spb);
        animationDelay=new WaitForSeconds(StageManager.instance.spb*0.125f);
        initialPos=lazer.transform.localPosition;
        rotationFixed=false;
        base.SetUp();
    }
    protected Vector3 chaseRotation=Vector3.zero;
    protected override void Update()
    {
        base.Update();
        if(!rotationFixed){
            chaseRotation.z=GetAngle(transform.position,CharacterManager.instance.FindPlayer());
            lazerCenter.transform.rotation=Quaternion.Euler(chaseRotation);
        }
    }

    protected float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;
        return Mathf.Atan2(v.y, v.x) * 57.29578f+90;
    }

    public override void SilhouetteMode(){
        base.SilhouetteMode();
        StartCoroutine(Appear());
    }

    public IEnumerator Appear(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        int maxTick=(int)(EnemyManager.instance.silhouetteTime/rate);
        float maxTickReverse=1f/maxTick;
        int currentNum=0;
        bool isLoomed=false;
        appearSprite.sprite=appear[0];
        while(current<maxTick+initial){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                currentNum=(int)((appear.Length-1)*(current-initial)*maxTickReverse);
                if(currentNum>appear.Length-1){
                    currentNum=appear.Length-1;
                }
                appearSprite.sprite=appear[currentNum];
                if(currentNum>=appearLoomCount &&!isLoomed){
                    StartCoroutine(PlayerLoom());
                    if(animator!=null){
                        StopCoroutine(animator);
                    }
                    animator=Idle();
                    StartCoroutine(animator);
                    isLoomed=true;
                }
            }
            yield return null;
        }
        SetUp();
        EnemyManager.instance.lazerEnemyYetAttack.Add(this);
        transform.SetParent(null);
        yield return null;
    }
    public override void EnemyShot(){
        base.EnemyShot();
        animator=Shot();
        StartCoroutine(animator);
    }
    public IEnumerator Shot(){
        rotationFixed=true;
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        int maxTick=8;
        float maxTickReverse=1f/maxTick;
        int currentNum=0;
        bool isShot=false;
        int fixTm=(int)(EnemyManager.instance.fixTime/EnemyManager.instance.rate);
        animationSprite.sprite=attack[0];

        lazer.SetActive(false);
        rotationFixed=true;
        lazerCollider.enabled=false;
        while(current<maxTick+initial){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                currentNum=(int)((attack.Length-1)*(current-initial)*maxTickReverse);
                if(currentNum>attack.Length-1){
                    currentNum=attack.Length-1;
                }
                animationSprite.sprite=attack[currentNum];
            }
            if(currentNum>fixTm &&!isShot){
                Attack();
                isShot=true;
            }
            yield return null;
        }
    }
    
    public void Attack(){
    if(StageManager.instance.isGameEnd) return;
        StartCoroutine(LazerCoroutine());
    }
    IEnumerator LazerCoroutine(){
        lazer.SetActive(true);
        rotationFixed=true;
        chaseRotation.z=GetAngle(transform.position,CharacterManager.instance.FindPlayerForEnemy());
        lazerCenter.transform.rotation=Quaternion.Euler(chaseRotation);
        yield return animationDelay;
        lazerCollider.enabled=true;
        lazer.transform.localPosition*=1.001f;
        yield return lazerWait;
        lazerCollider.enabled=false;
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        Color c;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float rate=StageManager.instance.stagefile.metronomeRate;
        float first=1;
        float second=1;
        while(current<8+initial){
            delta+=Time.deltaTime;
            c = shotSprite.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            shotSprite.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(8-(current-initial))/8f;
                second=(8-(current-initial+1))/8f;
                c = shotSprite.color;
                c.a = first;
                shotSprite.color = c;
            }
            yield return null;
        }
        c = shotSprite.color;
        c.a = 1;
        shotSprite.color = c;
        lazer.SetActive(false);
        lazer.transform.localPosition/=1.001f;
        Dead();
    }

    public void ShowSilhouette(){
        StartCoroutine(LoomLazer());
    }
    IEnumerator LoomLazer(){
        shotSilhouette.gameObject.SetActive(true);
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        Color c;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float rate=StageManager.instance.stagefile.metronomeRate;
        float first=0.5f;
        float second=0.5f;
        c = shotSilhouette.color;
        c.a = 0.5f;
        shotSilhouette.color = c;
        while(current<8+initial){
            delta+=Time.deltaTime;
            c = shotSilhouette.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            shotSilhouette.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(8-(current-initial))/16f;
                second=(8-(current-initial+1))/16f;
                c = shotSilhouette.color;
                c.a = first;
                shotSilhouette.color = c;
            }
            yield return null;
        }
        shotSilhouette.gameObject.SetActive(false);
    }

    public void EndLazerEnemy(){
        StopAllCoroutines();
        rotationFixed=false;
    }
}
