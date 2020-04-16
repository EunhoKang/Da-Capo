using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : enemy
{
    [HideInInspector]public float bulletSpeed;
    //private Vector3 playerPos;
    public override void SetUp(){
        bulletSpeed=EnemyManager.instance.bulletSpeed; 
        base.SetUp();
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
                if(currentNum>appearLoomCount &&!isLoomed){
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
        EnemyManager.instance.bulletEnemyYetAttack.Add(this);
        transform.SetParent(null);
        yield return null;
    }
    public override void EnemyShot(){
        base.EnemyShot();
        animator=Shot();
        //playerPos=CharacterManager.instance.FindPlayer();
        StartCoroutine(animator);
    }
    public IEnumerator Shot(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        int maxTick=8;
        float maxTickReverse=1f/maxTick;
        int currentNum=0;
        int fixTm=(int)(EnemyManager.instance.fixTime/EnemyManager.instance.rate);
        bool isShot=false;
        animationSprite.sprite=attack[0];
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
        enemyBullet temp=EnemyManager.instance.getBullet();
        temp.gameObject.SetActive(true);
        temp.gameObject.transform.position=transform.position;
        temp.gameObject.transform.rotation=transform.rotation;
        temp.Shot(Vector3.Normalize(CharacterManager.instance.FindPlayerForEnemy()-transform.position)*bulletSpeed);
        Dead();
    }
    
}
