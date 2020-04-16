﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class InGame : MonoBehaviour
{
    public Slider healthSlider;
    public Text score;
    public Text combo;
    public GameObject[] offsetSettingTarget;
    public GameObject pauseMenu;
    public GameObject min;
    public Text[] texts;
    public Image[] images;
    public Image glass;
    public GameObject pauseButton;
    private Vector3 joystickDirection=new Vector3(0,0,0);
    private StringBuilder scoreStringBuilder=new StringBuilder("00000000");
    private StringBuilder comboStringBuilder=new StringBuilder("0000");
    private bool isPaused;
    [HideInInspector]public float dashDistance;

    public void OnEnable(){//나중에 수정
        if(StageManager.instance.ingameUI!=this){
            StageManager.instance.GetIngameUI(this);
        }
        isPaused=false;
        pauseMenu.SetActive(false);
        score.text="00000000";
        combo.text="0000";
        healthSlider.value=healthSlider.maxValue;
        Time.timeScale=1;
        scoreStringBuilder=new StringBuilder("00000000");
        comboStringBuilder=new StringBuilder("0000");
        min.transform.rotation=Quaternion.Euler(Vector3.zero);
        Color c;
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=1;
            texts[i].color=c;
        }
        for(int i=0;i<images.Length;i++){
            c=images[i].color;
            c.a=1;
            images[i].color=c;
        }
        c=glass.color;
        c.a=0.4f;
        glass.color=c;
    }
    public void FadeOut(){
        StartCoroutine(Fading());
    }
    IEnumerator Fading(){
        float dt=0.05f;
        float maxTime=0.4f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=1;
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=1;
            texts[i].color=c;
        }
        for(int i=0;i<images.Length;i++){
            c=images[i].color;
            c.a=1;
            images[i].color=c;
        }
        c=glass.color;
        c.a=0.4f;
        glass.color=c;
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount-=0.125f;
                for(int i=0;i<texts.Length;i++){
                    c=texts[i].color;
                    c.a=cCount;
                    texts[i].color=c;
                }
                for(int i=0;i<images.Length;i++){
                    c=images[i].color;
                    c.a=cCount;
                    images[i].color=c;
                }
                c=glass.color;
                c.a=0.4f*cCount;
                glass.color=c;
                check+=dt;
            }
            yield return null;
        }
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=0;
            texts[i].color=c;
        }
        for(int i=0;i<images.Length;i++){
            c=images[i].color;
            c.a=0;
            images[i].color=c;
        }
        c=glass.color;
        c.a=0;
        glass.color=c;
    }
    public void SettingForOffset(){
        for(int i=0;i<offsetSettingTarget.Length;i++){
            offsetSettingTarget[i].SetActive(false);
        }
    }
    public void SettingForGame(){
        for(int i=0;i<offsetSettingTarget.Length;i++){
            offsetSettingTarget[i].SetActive(true);
        }
    }

    void Update(){
        #if UNITY_ANDROID && !UNITY_EDITOR
        if(!isPaused){ //silhouette and Touch
            if(CameraManager.instance.cam!=null && CharacterManager.instance!=null){
                if(Input.GetButtonDown("hit1")){
                    joystickDirection=CameraManager.instance.cam.ScreenToWorldPoint((Input.mousePosition))-CharacterManager.instance.FindPlayer();
                    joystickDirection.z=0;
                    if(joystickDirection.magnitude>dashDistance){
                        joystickDirection=Vector3.Normalize(joystickDirection);
                    }else{
                        joystickDirection/=dashDistance;
                    }
                    NoteManager.instance.JudgeNote(joystickDirection);
                }
            }
        }
        #else
        if(!isPaused){ //silhouette and Touch
            if(CameraManager.instance.cam!=null && CharacterManager.instance!=null){
                joystickDirection=CameraManager.instance.cam.ScreenToWorldPoint((Input.mousePosition))-CharacterManager.instance.FindPlayer();
		        joystickDirection.z=0;
                if(joystickDirection.magnitude>dashDistance){
                    joystickDirection=Vector3.Normalize(joystickDirection);
                }else{
                    joystickDirection/=dashDistance;
                }
                CharacterManager.instance.SetSilhouettePos(joystickDirection);
            }
            if(Input.GetButtonDown("hit1")){
                NoteManager.instance.JudgeNote(joystickDirection);
            }
        }
        if(Input.GetButtonDown("ESC")){
            PauseButton();
        }
        #endif
    }

    public void UpdateHealthSlider(float amount){
        healthSlider.maxValue=StageManager.instance.stagefile.playerHealth;
        StartCoroutine(Updating(amount));
    }

    public IEnumerator Updating(float amount){
        Vector3 tp=Vector3.zero;
        for(float i=0;i<=1;i+=0.1f){
            healthSlider.value=Mathf.Lerp(healthSlider.value,amount,i);
            tp.z=-1*Mathf.Lerp((100-healthSlider.value)*3.6f,(100-amount)*3.6f,i);
            min.transform.rotation=Quaternion.Euler(tp);
            yield return null;
        }
        healthSlider.value=amount;
    }

    public void UpdateCombo(float amount){
        comboStringBuilder.Remove(0, 4);
        string temp =amount.ToString();
        for (int i = 0; i < 4 - temp.Length; i++)
        {
            comboStringBuilder.Append('0');
        }
        if (temp.Length <= 4)
        {
            comboStringBuilder.Append(temp);
        }
        else
        {
            comboStringBuilder.Append("9999");
        }
        combo.text=comboStringBuilder.ToString();
    }

    public void UpdateScore(float amount){
        scoreStringBuilder.Remove(0, 8);
        string temp =amount.ToString();
        for (int i = 0; i < 8 - temp.Length; i++)
        {
            scoreStringBuilder.Append('0');
        }
        if (temp.Length <= 8)
        {
            scoreStringBuilder.Append(temp);
        }
        else
        {
            scoreStringBuilder.Append("99999999");
        }
        score.text=scoreStringBuilder.ToString();
    }
    public void ResetUI(){
        StopAllCoroutines();
        scoreStringBuilder=null;
        comboStringBuilder=null;
    }

    public void PauseButton(){
        if(!isPaused){
            Pause();
        }else{
            Resume();
        }
    }
    public void Pause(){
        UIManager.instance.DefaultSound();
        isPaused=true;
        pauseMenu.SetActive(true);
        Time.timeScale=0;
        SoundManager.instance.SoundPause();
    }
    public void Resume(){
        UIManager.instance.DefaultSound();
        isPaused=false;
        pauseMenu.SetActive(false);
        Time.timeScale=1;
        SoundManager.instance.SoundResume();
    }
    public void Restart(){
        UIManager.instance.SelectSound();
        StageManager.instance.EndStage();
        OnEnable();
        StageManager.instance.Init();
    }
    public void BackToMenu(){
        Time.timeScale=1;
        UIManager.instance.DefaultSound();
        StageManager.instance.EndStage();
        UIManager.instance.ShowCanvas(0);
        UIManager.instance.RemoveCanvas(1);
        UIManager.instance.SetTrueUICam();
    }

    public void CanPause(){
        pauseButton.SetActive(true);
    }
    public void CantPause(){
        pauseButton.SetActive(false);
    }
}
