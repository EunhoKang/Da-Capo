using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    Vector3 noteSpeed;
    Material material;
    BoxCollider2D col;
    float rate;
    public GameObject matObject;
    public float maxFade = 0.7f;
    public float deltaFade = 0.07f;
    public float startFade=0.21f;
    IEnumerator fallCoroutine;
    public void Awake(){
        material=matObject.GetComponent<SpriteRenderer>().material;
        col=GetComponent<BoxCollider2D>();
    }
    public void StartFall(Vector3 notespeed){
        material.SetFloat("_Fade", 0);
        col.enabled=true;
        noteSpeed=notespeed;
        rate=NoteManager.instance.rate;
        fallCoroutine=Fall();
        StartCoroutine(fallCoroutine);
    }
    public IEnumerator Fall(){
        while(this.gameObject.activeInHierarchy){
            transform.Translate(noteSpeed*Time.deltaTime);
            yield return null;
        }
    }

    public void NoteHitted(){
        col.enabled=false;
        StopCoroutine(fallCoroutine);
        StartCoroutine(NoteHitEffect());
    }

    IEnumerator NoteHitEffect(){
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float rate=EnemyManager.instance.rate;
        float spb=StageManager.instance.spb;

        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float fade = startFade;
        float first=fade;
        float second=fade;
        while(fade<=maxFade){
            delta+=Time.deltaTime;
            fade=Mathf.Lerp(first,second,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            material.SetFloat("_Fade", fade);
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=startFade+(current-initial)*deltaFade;
                second=startFade+(current-initial+1)*deltaFade;
                fade=first;
                material.SetFloat("_Fade", fade);
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void EndNote(){
        StopAllCoroutines();
    }

}
