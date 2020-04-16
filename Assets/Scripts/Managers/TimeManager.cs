using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}

    [HideInInspector]public int checkpoint;
    [HideInInspector]public float rate;
    private double nextTick;
    private double startTime;

    [HideInInspector]public bool isTimerPaused;
    [HideInInspector]public float multiplier;
    private IEnumerator multiplyCoroutine;
    public void Init(){
        rate=StageManager.instance.stagefile.metronomeRate;
        nextTick=0;
        startTime=0;
        multiplier=1;
        isTimerPaused=false;
    }

    public IEnumerator Timer(){
        float spb=StageManager.instance.spb;
        nextTick=spb*rate;
        startTime=Time.time;//AudioSetting.dsptime was used before Time.time
        checkpoint=0;
        while(true){
            if(Time.time-startTime>=nextTick){
                while(Time.time-startTime>=nextTick){
                    nextTick+=spb*rate;
                    checkpoint++;
                }
            }
            yield return null;
        }
    }

    public void HasteTimer(float time,float multiplier){
        StartCoroutine(HasteTimerCoroutine(time,multiplier));
    }

    public IEnumerator HasteTimerCoroutine(float time,float multiplier){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        yield return new WaitForSeconds(time);
        if(multiplyCoroutine!=null){
            StopCoroutine(multiplyCoroutine);
        }
        multiplyCoroutine=Multiplier(multiplier);
        StartCoroutine(multiplyCoroutine);
    }

    public IEnumerator Multiplier(float mp){
        startTime=Time.time;
        nextTick=StageManager.instance.spb*rate;
        multiplier=mp;
        while(true){
            startTime+=Time.deltaTime*(1-multiplier);
            yield return null;
        }
    }

    public void StartTimer(){
        StartCoroutine(Timer());
    }

    public void EndTime(){
        StopAllCoroutines();
        multiplyCoroutine=null;
    }
}
