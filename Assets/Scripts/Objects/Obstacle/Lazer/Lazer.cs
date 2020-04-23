using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public SpriteRenderer anim;
    public List<Sprite> warn;
    [Range(0.0625f,3f)]
    public float duration=0.25f;
    [Range(0.125f,2f)]
    public float warnTime=1f;
    public BoxCollider2D col;
    public LazerInside snake;
    bool isLooming;
    public void OnEnable()
    {
        isLooming=false;
        col.enabled=false;
        snake.damage=StageManager.instance.stagefile.obstacleDamage;
        snake.gameObject.SetActive(false);
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
                currentNum=(int)((current-initial)*1f)%warn.Count;
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
    Vector3 ff=new Vector3(23f,0,0);
    Vector3 ss=new Vector3(-23f,0,0);
    IEnumerator Appearing(){
        snake.gameObject.SetActive(true);
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        int maxTick=(int)(duration/rate);
        float maxTickReverse=1f/maxTick;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        Vector3 fPos=ff;
        Vector3 sPos=ff;
        snake.transform.localPosition=ff;
        col.enabled=true;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            snake.transform.localPosition=Vector3.Lerp(fPos,sPos,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                fPos=Vector3.Lerp(ff,ss,(current-initial)*maxTickReverse);
                sPos=Vector3.Lerp(ff,ss,(current-initial+1)*maxTickReverse);
                snake.transform.localPosition=fPos;
            }
            yield return null;
        }
        snake.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
