using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    static float pi=Mathf.PI;
    private Image damagedBg;
    private float damageTime;
    public static BackgroundManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}

	[HideInInspector]public float rate;
	private Image IllustPos;
    public void Init(){
		rate=StageManager.instance.stagefile.metronomeRate;
		IllustPos=CameraManager.instance.camAction.IllustPos;
        damagedBg=CameraManager.instance.camAction.hitImage;
        damageTime=1.5f*StageManager.instance.spb;
	}

	public void SetIllust(Sprite spr){ //change this to animation after
		IllustPos.sprite=spr;
	}

    public void GetHitted(){
        StartCoroutine(HitEffect());
    }
    IEnumerator HitEffect(){
        damagedBg.gameObject.SetActive(true);
        Color c;
		float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        int maxTick=(int)(damageTime/rate);
        float maxTickReverse=1f/maxTick;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float first=0;
        float second=0;
        c = damagedBg.color;
        c.a = 0;
        damagedBg.color= c;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            
            c = damagedBg.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            damagedBg.color=c;
            
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                
                first=(1-Mathf.Cos((2*pi*(current-initial)*maxTickReverse)))*0.4f;
                second=(1-Mathf.Cos((2*pi*(current-initial+1)*maxTickReverse)))*0.4f;
                c = damagedBg.color;
                c.a = first;
                damagedBg.color = c;
                
            }
            yield return null;
        }
        
		c = damagedBg.color;
        c.a = 0;
        damagedBg.color= c;
        
        damagedBg.gameObject.SetActive(false);
	}

}
