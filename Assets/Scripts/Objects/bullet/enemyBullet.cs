using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : bullet
{
    public float bulletExistTime=5f;
    public Rigidbody2D rb;
    public bool isBulletSpin=false;
    Vector3 temp=new Vector3(0,0,180);
    protected void OnEnable(){
        StartCoroutine(TimeCheck());
        damage=StageManager.instance.stagefile.bulletDamage;
    }
    
    protected IEnumerator TimeCheck(){
        yield return new WaitForSeconds(bulletExistTime);
        gameObject.SetActive(false);
    }
    public void Shot(Vector3 vel){
        StartCoroutine(GetShot(vel));
    }
    public IEnumerator GetShot(Vector3 vel){
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
            if(isBulletSpin){
                transform.rotation=Quaternion.Euler(Vector3.Lerp(temp*rate*2f*(current-initial),
                temp*rate*2f*(current-initial+1),
                (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt)));
            }
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=initialPos+vel*rate*spb*(current-initial);
                second=initialPos+vel*rate*spb*(current-initial+1);
                if(isBulletSpin){
                    transform.rotation=Quaternion.Euler(temp*rate*2f*(current-initial));
                }
                rb.MovePosition(first);
            }
            yield return null;
        }
    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            CharacterManager.instance.PlayerGetDamage(damage);
        }
    }
    
}
