using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Offset : MonoBehaviour
{
    private float offset=0.1f;
    public Text offsetText;
    public void OnEnable(){
        JClass jClass=DataManager.instance._data;
        offset=jClass.offset;
        offset=Mathf.Round(offset*100f)/100;
        offsetText.text=offset.ToString();
    }
    public void BackToSetting(){
        Time.timeScale=1;
        UIManager.instance.DefaultSound();
        StageManager.instance.EndStage();
        UIManager.instance.ShowCanvas(0);
        UIManager.instance.RemoveCanvas(1);
        UIManager.instance.RemoveCanvas(3);
        UIManager.instance.SetTrueUICam();
    }
    public void OffsetChange(bool isPositive){
        UIManager.instance.DefaultSound();
        if(isPositive){
            offset+=0.01f;
        }else{
            if(offset>=0.01f){
                offset-=0.01f;
            }
        }
        offset=Mathf.Round(offset*100f)/100;
        offsetText.text=offset.ToString();
        DataManager.instance.Offsetting(offset);
    }
    public void ResetGame(){
        //
        UIManager.instance.DefaultSound();
        StageManager.instance.EndStage();
        UIManager.instance.RemoveCanvas(1);
        UIManager.instance.ShowCanvas(1);
        StageManager.instance.Init();
    }
}
