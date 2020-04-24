using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public GameObject other;
    public Vector3 otherPos;
    Vector3 noteSpeed;
    List<Material> material;
    BoxCollider2D col;
    float rate;
    public SpriteRenderer[] matObject;
    public ParticleSystem[] particles;
    public float initialSpeed=2f;
    public float standardSpb=0.5f;
    
    public float maxFade = 0f;
    public float deltaFade = 0.065f;
    public float startFade=0.88f;
    
    IEnumerator fallCoroutine;
    public void Awake(){
        material=new List<Material>();
        for(int i=0;i<matObject.Length;i++){
            material.Add(matObject[i].material);
        }
        col=GetComponent<BoxCollider2D>();
    }
    public void OnEnable(){
        other.transform.localPosition=otherPos;
    }
    public void StartFall(Vector3 notespeed){
        for(int i=0;i<material.Count;i++){
            material[i].SetFloat("_Fade", 1.1f);
        }
        col.enabled=true;
        noteSpeed=notespeed;
        rate=NoteManager.instance.rate;
        fallCoroutine=Fall();
        StartCoroutine(fallCoroutine);
    }
    public IEnumerator Fall(){
        while(this.gameObject.activeInHierarchy){
            transform.localPosition+=noteSpeed*Time.deltaTime;
            other.transform.localPosition+=noteSpeed*Time.deltaTime*-2f;
            yield return null;
        }
    }

    public void NoteHitted(int judge){
        col.enabled=false;
        StopCoroutine(fallCoroutine);
        StartCoroutine(NoteHitEffect());
        if(judge<=1){
            StartCoroutine(NoteHitParticle());
        }
    }
    
    IEnumerator NoteHitParticle(){
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float spb=StageManager.instance.spb;
        float particleSpeedMultiplier=standardSpb/spb;

        for(int i=0;i<particles.Length;i++){
            particles[i].Stop();
            particles[i].gameObject.SetActive(true);
            particles[i].playbackSpeed=initialSpeed*particleSpeedMultiplier*TimeManager.instance.multiplier;
            particles[i].Play();
        }
        while(particles[0].isPlaying){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                for(int i=0;i<particles.Length;i++){
                    particles[i].Pause();
                    particles[i].playbackSpeed=initialSpeed*particleSpeedMultiplier*TimeManager.instance.multiplier;
                    particles[i].Play();
                }
            }
            yield return null;
        }
        for(int i=0;i<particles.Length;i++){
            particles[i].Stop();
            particles[i].gameObject.SetActive(false);
        }
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
        while(fade>=maxFade){
            delta+=Time.deltaTime;
            fade=Mathf.Lerp(first,second,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            for(int i=0;i<material.Count;i++){
                material[i].SetFloat("_Fade", fade);
            }
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=startFade-(current-initial)*deltaFade;
                second=startFade-(current-initial+1)*deltaFade;
                fade=first;
                for(int i=0;i<material.Count;i++){
                    material[i].SetFloat("_Fade", fade);
                }
            }
            yield return null;
        }
        gameObject.SetActive(false);
        yield return null;
    }

    public void EndNote(){
        StopAllCoroutines();
    }

}
