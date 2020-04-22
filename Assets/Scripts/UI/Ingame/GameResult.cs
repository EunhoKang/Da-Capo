using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class GameResult : MonoBehaviour
{
    public Text score;
    public Text combo;
    public Text ultimate;
    public Text perfect;
    public Text good;
    public Text miss;
    public Text hit;
    public Image bg;
    public Image bgBack;
    public Text[] texts;
    public Image[] images;
    private StringBuilder scoreStringBuilder=new StringBuilder("00000000");
    private StringBuilder comboStringBuilder=new StringBuilder("0000");
    public void OnEnable(){
        if(StageManager.instance.stagefile!=null){
            bgBack.sprite=StageManager.instance.stagefile.backgroundIllust;
        }
        Color c;
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
        FadeIn();
        ShowScore();
    }
    public void FadeIn(){
        StartCoroutine(Fading());
    }
    IEnumerator Fading(){
        float dt=0.05f;
        float maxTime=0.4f;
        float currentTime=0;
        float check=dt;
        Color c;
        float cCount=0;
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
        while(currentTime<=maxTime){
            currentTime+=Time.deltaTime;
            if(currentTime>=check){
                cCount+=0.125f;
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
                check+=dt;
            }
            yield return null;
        }
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
    }
    public void ShowScore(){
        scoreStringBuilder.Remove(0, 8);
        string temp=CountManager.instance.score.ToString();
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

        comboStringBuilder.Remove(0, 4);
        temp=CountManager.instance.maxCombo.ToString();
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

        comboStringBuilder.Remove(0, 4);
        temp=CountManager.instance.ultimate.ToString();
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
        ultimate.text=comboStringBuilder.ToString();

        comboStringBuilder.Remove(0, 4);
        temp=CountManager.instance.perfect.ToString();
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
        perfect.text=comboStringBuilder.ToString();

        comboStringBuilder.Remove(0, 4);
        temp=CountManager.instance.good.ToString();
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
        good.text=comboStringBuilder.ToString();

        comboStringBuilder.Remove(0, 4);
        temp=CountManager.instance.miss.ToString();
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
        miss.text=comboStringBuilder.ToString();

        comboStringBuilder.Remove(0, 4);
        temp=CountManager.instance.hit.ToString();
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
        hit.text=comboStringBuilder.ToString();
    }

    public void ReStart(){
        UIManager.instance.SelectSound();
        UIManager.instance.SetFalseUICam();
        UIManager.instance.ShowCanvas(1);
        StageManager.instance.Init();
        UIManager.instance.RemoveCanvas(2);
    }

    public void BackToMenu(){
        UIManager.instance.DefaultSound();
        UIManager.instance.ShowCanvas(0);
        UIManager.instance.RemoveCanvas(2);
    }
}
