using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
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
	}

	public void SetIllust(Sprite spr){ //change this to animation after
		IllustPos.sprite=spr;
	}

	IEnumerator SettingIllust(){
		Color c;
		float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float rate=StageManager.instance.stagefile.metronomeRate;
        float first=0;
        float second=0;
        while(current<4+initial){
            delta+=Time.deltaTime;
            c = IllustPos.color;
            c.a=Mathf.Lerp(first,second,(delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            IllustPos.color=c;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=(current-initial)*0.25f;
                second=(current-initial+1)*0.25f;
                c = IllustPos.color;
                c.a = first;
                IllustPos.color = c;
            }
            yield return null;
        }
		c = IllustPos.color;
        c.a = 1;
        IllustPos.color = c;
	}
}
