using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountManager : MonoBehaviour
{
    public static CountManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}
    [Header("Positive")]
    public int ultimateScore;
    public int perfectScore;
    public int goodScore;
    [Header("Negative")]
    public int hitScore;
    [HideInInspector]public int combo;
    [HideInInspector]public int score;
    [HideInInspector]public int maxCombo;
    [HideInInspector]public int ultimate;
    [HideInInspector]public int perfect;
    [HideInInspector]public int good;
    [HideInInspector]public int miss;
    [HideInInspector]public int hit;
    [HideInInspector]public bool stageCleared;
    [HideInInspector]public float rate;


    public void Init(){
        rate=StageManager.instance.stagefile.metronomeRate;
        combo=0;
        score=0;
        maxCombo=0;
        ultimate=0;
        perfect=0;
        good=0;
        miss=0;
        hit=0;
        stageCleared=true;
        StageManager.instance.ingameUI.UpdateCombo(combo);
        StageManager.instance.ingameUI.UpdateScore(score);
    }

    public void AddComboForHitNote(int judge){
        if(judge==0){
            combo+=4;
            ultimate++;
        }else if(judge==1){
            combo+=3;
            perfect++;
        }else if(judge==2){
            combo+=1;
            good++;
        }else{
            BreakCombo();
            miss++;
        }
        StageManager.instance.ingameUI.UpdateCombo(combo);
    }

    public void BreakCombo(){
        if(combo>maxCombo){
            maxCombo=combo;
        }
        combo=0;
        StageManager.instance.ingameUI.UpdateCombo(combo);
    }

    public void DiscountScoreFromHit(float damage){
        hit++;
        score-=(int)(2000*damage);
        if(score<0){
            score=0;
        }
        StageManager.instance.ingameUI.UpdateScore(score);
    }

    public void StartScoreCount(){
        StartCoroutine(ScoreCount());
    }

    public IEnumerator ScoreCount(){
        int current=TimeManager.instance.checkpoint;
        while(true){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                    ScoreAdd();
                }
            }
            yield return null;
        }
    }
    public void ScoreAdd(){
        score+=(100+combo)*1;
        StageManager.instance.ingameUI.UpdateScore(score);
    }
    public void StopScoreCount(){
        StopAllCoroutines();
    }

    public void EndCount(){
        if(combo>maxCombo){
            maxCombo=combo;
        }
        StopAllCoroutines();
    }

}
