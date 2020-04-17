using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StagePage : Page
{
    [HideInInspector] public int stageNum;
    public Image[] sprites;
    public Text[] texts;
    public Image[] clocks;
    public GameObject noMiss;
    public GameObject fullCombo;
    public Text highScore;
    public GameObject stageButton;
    public GameObject lockToken;
    public GameObject blur;
    public Image clockForEffect;
    public Image bg;
    public Text songNum;
    public Text songName;
    public string[] songNums;
    public string[] songNames;
    public Sprite[] bgs;
    public AudioClip[] clips;
    public AudioClip bgm;
    public GameObject minute;
    [HideInInspector]public bool isOpened;
    [HideInInspector]public bool isCleared;
    [HideInInspector]public bool isNoMiss;
    [HideInInspector]public bool isFullCombo;
    [HideInInspector]public int highestScore;
    private Vector3 minTemp=Vector3.zero;
    private StringBuilder scoreStringBuilder=new StringBuilder("00000000");
    private bool isChanging=false;
    private int currentNum=2;
    public void OnEnable(){
        clockForEffect.gameObject.SetActive(false);
        JClass jc=DataManager.instance._data;
        if(currentNum<0){
            Debug.Log("ERR :");
            return;
        }
        if(jc.stages[currentNum].isOpened){
            stageButton.SetActive(true);
            blur.SetActive(false);
            lockToken.SetActive(false);
        }else{
            stageButton.SetActive(false);
            blur.SetActive(true);
            lockToken.SetActive(true);
        }
        if(jc.stages[currentNum].isFullCombo){
            fullCombo.SetActive(true);
        }else{
            fullCombo.SetActive(false);
        }
        if(jc.stages[currentNum].isNoMiss){
            noMiss.SetActive(true);
        }else{
            noMiss.SetActive(false);
        }
        scoreStringBuilder.Remove(0, 8);
        string temp=jc.stages[currentNum].highestScore.ToString();
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
        highScore.text=scoreStringBuilder.ToString();
        songName.text=songNames[currentNum];
        songNum.text=songNums[currentNum];
        stageNum=currentNum;
        SoundManager.instance.BGMStop();
        SoundManager.instance.BGMPlay(clips[currentNum]);
        bg.sprite=bgs[currentNum];
    }
    public override void Appear(int init,int after){
        if(isChanging)return;
        isChanging=true;
        clockForEffect.gameObject.SetActive(false);
        JClass jc=DataManager.instance._data;
        if(after-2<0){
            Debug.Log("ERR :");
            return;
        }
        if(jc.stages[after-2].isOpened){
            stageButton.SetActive(true);
            blur.SetActive(false);
            lockToken.SetActive(false);
        }else{
            stageButton.SetActive(false);
            blur.SetActive(true);
            lockToken.SetActive(true);
        }
        if(jc.stages[after-2].isFullCombo){
            fullCombo.SetActive(true);
        }else{
            fullCombo.SetActive(false);
        }
        if(jc.stages[after-2].isNoMiss){
            noMiss.SetActive(true);
        }else{
            noMiss.SetActive(false);
        }
        scoreStringBuilder.Remove(0, 8);
        string temp=jc.stages[after-2].highestScore.ToString();
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
        highScore.text=scoreStringBuilder.ToString();
        songName.text=songNames[after-2];
        songNum.text=songNums[after-2];
        stageNum=after-2;
        bg.sprite=bgs[after-2];
        currentNum=after-2;
        SoundManager.instance.BGMStop();
        SoundManager.instance.BGMPlay(clips[after-2]);
        StartCoroutine(Appearing(init,after));
    }
    public override void Disappear(int init,int after){
        if(isChanging)return;
        isChanging=true;
        SoundManager.instance.UIBGMFadeOut();
        StartCoroutine(Disappearing(init,after));
    }
    IEnumerator Appearing(int init,int after){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=0;
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=0;
            texts[i].color=c;
        }
        if(init<=1 || init>=13){
            for(int i=0;i<clocks.Length;i++){
                c=clocks[i].color;
                c.a=0;
                clocks[i].color=c;
            }
        }
        for(int i=0;i<sprites.Length;i++){
            c=sprites[i].color;
            c.a=0;
            sprites[i].color=c;
        }
        if(init<=1){
            minTemp.z=-30f;
            minute.transform.rotation=Quaternion.Euler(minTemp);
        }else if(init>=13){
            minTemp.z=-330f;
            minute.transform.rotation=Quaternion.Euler(minTemp);
        }else{
            minTemp.z=(after-1)*-30f;
            minute.transform.rotation=Quaternion.Euler(minTemp);
        }
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount+=0.125f;
                for(int i=0;i<sprites.Length;i++){
                    c=sprites[i].color;
                    c.a=cCount;
                    sprites[i].color=c;
                }
                if(init<=1 || init>=13){
                    for(int i=0;i<clocks.Length;i++){
                        c=clocks[i].color;
                        c.a=cCount;
                        clocks[i].color=c;
                    }
                }
                for(int i=0;i<texts.Length;i++){
                    c=texts[i].color;
                    c.a=cCount;
                    texts[i].color=c;
                }
                check+=dt;
            }
            yield return null;
        }
        for(int i=0;i<sprites.Length;i++){
            c=sprites[i].color;
            c.a=1;
            sprites[i].color=c;
        }
        if(init<=1 || init>=13){
            for(int i=0;i<clocks.Length;i++){
                c=clocks[i].color;
                c.a=1;
                clocks[i].color=c;
            }
        }
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=1;
            texts[i].color=c;
        }
        isChanging=false;
    }
    IEnumerator Disappearing(int init,int after){
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=1;
        int targetMin=after-1;
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=1;
            texts[i].color=c;
        }
        if(after<=1 || after>=13){
            SoundManager.instance.UIBGMFadeOut();
            for(int i=0;i<clocks.Length;i++){
                c=clocks[i].color;
                c.a=1;
                clocks[i].color=c;
            }
        }
        for(int i=0;i<sprites.Length;i++){
            c=sprites[i].color;
            c.a=1;
            sprites[i].color=c;
        }
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount-=0.125f;
                for(int i=0;i<sprites.Length;i++){
                    c=sprites[i].color;
                    c.a=cCount;
                    sprites[i].color=c;
                }
                if(after<=1 || after>=13){
                    for(int i=0;i<clocks.Length;i++){
                        c=clocks[i].color;
                        c.a=cCount;
                        clocks[i].color=c;
                    }
                }
                for(int i=0;i<texts.Length;i++){
                    c=texts[i].color;
                    c.a=cCount;
                    texts[i].color=c;
                }
                minTemp.z=Mathf.Lerp((init-1)*-30f,(after-1)*-30f,Mathf.Pow((1f-cCount),2));
                minute.transform.rotation=Quaternion.Euler(minTemp);
                check+=dt;
            }
            yield return null;
        }
        isChanging=false;
        
        for(int i=0;i<texts.Length;i++){
            c=texts[i].color;
            c.a=0;
            texts[i].color=c;
        }
        if(after<=1 || after>=13){
            SoundManager.instance.BGMStop();
            SoundManager.instance.BGMPlay(bgm);
            for(int i=0;i<clocks.Length;i++){
                c=clocks[i].color;
                c.a=0;
                clocks[i].color=c;
            }
        }
        for(int i=0;i<sprites.Length;i++){
            c=sprites[i].color;
            c.a=0;
            sprites[i].color=c;
        }
        
        if(after<=1 || after>=13){
            gameObject.SetActive(false);
        }
    }
    public void GameStart(){
        if(isChanging)return;
        isChanging=true;
        StartCoroutine(GameStartCoroutine());
    }
    IEnumerator GameStartCoroutine(){
        UIManager.instance.SelectSound();
        clockForEffect.gameObject.SetActive(true);
        float dt=0.025f;
        float maxTime=0.2f;
        float currentTime=0;
        float check=dt;
        Color c;
        Vector3 tp;
        float cCount=1;
        c=clockForEffect.color;
        c.a=cCount;
        clockForEffect.color=c;
        tp=Vector3.one*(1.5f-cCount*0.5f);
        clockForEffect.gameObject.transform.localScale=tp;
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount-=0.125f;
                c=clockForEffect.color;
                c.a=cCount;
                clockForEffect.color=c;
                tp=Vector3.one*(1.5f-cCount*0.5f);
                clockForEffect.gameObject.transform.localScale=tp;
                check+=dt;
            }
            yield return null;
        }
        c=clockForEffect.color;
        c.a=1;
        clockForEffect.color=c;
        tp=Vector3.one;
        clockForEffect.gameObject.transform.localScale=tp;
        clockForEffect.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        UIManager.instance.LoadingStart();
        SoundManager.instance.UIBGMFadeOut();
        yield return new WaitForSeconds(2.5f);
        StageManager.instance.mapName=songNames[stageNum];
        UIManager.instance.ShowCanvas(1);
        StageManager.instance.Init();
        isChanging=false;
        UIManager.instance.SetFalseUICam();
        UIManager.instance.RemoveCanvas(0);
    }
}
