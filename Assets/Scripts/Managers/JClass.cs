using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JClass 
{
    [System.Serializable]
    public class Stage{
        public bool isOpened;
        public bool isCleared;
        public bool isNoMiss;
        public bool isFullCombo;
        public int highestScore;
        public Stage(bool isSet, bool isFirst){
            if(isSet){
                if(isFirst){
                    isOpened=true;
                }else{
                    isOpened=false;
                }
                isCleared=false;
                isNoMiss=false;
                isFullCombo=false;
                highestScore=0;
            }
        }
    }
	public float BGMvolume;
	public float SFXvolume;
	public bool isVibOn;
	public float offset;
    public List<bool> prologues;
    public List<Stage> stages;
	public JClass() { }
	public JClass(bool isSet,int stageCount)
	{
		if (isSet)
		{
			BGMvolume=1;
			SFXvolume=1;
			isVibOn=false;
			offset=0.10f;
            stages=new List<Stage>();
            prologues=new List<bool>();
            for(int i=0;i<11;i++){
                prologues.Add(false);
            }
            for(int i=0;i<stageCount;i++){
                if(i==0 || i==1){
                    stages.Add(new Stage(true,true));
                }else{
                    stages.Add(new Stage(true,false));
                }
            }
		}
	}
    public void Print()
    {
        Debug.Log("BGMvolume = " + BGMvolume);
        Debug.Log("SFXvolume = " + SFXvolume);
        Debug.Log("isVibOn = " + isVibOn);
        Debug.Log("offset = " + offset);
        for(int i=0;i<stages.Count;i++){
            Debug.Log("isOpened = " + stages[i].isOpened);
            Debug.Log("isCleared = " + stages[i].isCleared);
            Debug.Log("isNoMiss = " + stages[i].isNoMiss);
            Debug.Log("isFullCombo = " + stages[i].isFullCombo);
            Debug.Log("highestScore = " + stages[i].highestScore);
        }
    }
}