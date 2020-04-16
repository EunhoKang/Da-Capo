using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public float initialSpeed=2f;
    public float standardSpb=0.5f;
    public ParticleSystem[] noteHitPrefab;

    List<List<ParticleSystem>> effectLists;
    
    public void Awake(){
        effectLists=new List<List<ParticleSystem>>();
        for(int i=0;i<noteHitPrefab.Length;i++){
            List<ParticleSystem> temp=new List<ParticleSystem>();
            effectLists.Add(temp);
        }
        for(int i=0;i<noteHitPrefab.Length;i++){
            for(int j=0;j<4;j++){
                ParticleSystem obj=Instantiate(noteHitPrefab[i]) as ParticleSystem;
                obj.gameObject.SetActive(false);
                effectLists[i].Add(obj);
            }
        }
    }
    public void NoteHitted(int judge){
        if(judge>=noteHitPrefab.Length){
            return;
        }
        StartCoroutine(NoteHitEffect(judge,getNoteEffect(judge)));
    }

    public ParticleSystem getNoteEffect(int judge){
		for(int i=0; i<effectLists[judge].Count;i++){
			if(!effectLists[judge][i].gameObject.activeInHierarchy){
				return effectLists[judge][i];
			}
		}
		ParticleSystem obj=Instantiate(noteHitPrefab[judge]) as ParticleSystem;
		obj.gameObject.SetActive(false);
		effectLists[judge].Add(obj);
		return obj;
	}

    IEnumerator NoteHitEffect(int judge,ParticleSystem p){
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float spb=StageManager.instance.spb;
        float particleSpeedMultiplier=standardSpb/spb;

        p.gameObject.SetActive(true);
        p.transform.SetParent(transform);
        p.transform.localPosition=Vector3.zero;
        p.playbackSpeed=initialSpeed*particleSpeedMultiplier*TimeManager.instance.multiplier;
        p.Play();
        while(p.isPlaying){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                p.Pause();
                p.playbackSpeed=initialSpeed*particleSpeedMultiplier*TimeManager.instance.multiplier;
                p.Play();
            }
            yield return null;
        }
        p.gameObject.SetActive(false);
        p.transform.SetParent(null);
    }
}
