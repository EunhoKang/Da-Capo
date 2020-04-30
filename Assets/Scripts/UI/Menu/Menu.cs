using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public int maxPage=13;
    [HideInInspector]public int pageNum=1;
    public Setting settingPage;
    public MainPage mainPage;
    public StagePage stages;
    public Letter letterPage;
    public AudioClip bgm;
    public Image[] bg;
    public GameObject[] buttons;
    private bool isChanging;
    public void OnEnable()
    {
        isChanging=false;
        SoundManager.instance.BGMPlay(bgm);
        BGPerClear();
    }
    public void OnDisable()
    {
        if(SoundManager.instance!=null){
            SoundManager.instance.BGMStop();
        }
    }

    public void BGPerClear(){
        float persent=1f;
        JClass temp=DataManager.instance._data;
        float amount=1f/((float)temp.stages.Count*3);
        for(int i=0;i<temp.stages.Count;i++){
            if(temp.stages[i].isCleared){
                persent-=amount;
            }
            if(temp.stages[i].isNoMiss){
                persent-=amount;
            }
            if(temp.stages[i].isFullCombo){
                persent-=amount;
            }
        }
        for(int i=0;i<bg.Length;i++){
            Color color=bg[i].color;
            color.a=persent;
            bg[i].color=color;
        }
    }
    public void ButtonPushed(bool isRight){
        if(isChanging || (isRight && pageNum>=maxPage) || (!isRight && pageNum<=0))return;
        isChanging=true;
        StartCoroutine(PageChanging(isRight));
    }
    IEnumerator PageChanging(bool isRight){
        if(isRight){
            PageRemove(pageNum,pageNum+1);
            UIManager.instance.DefaultSound();
            pageNum++;
            yield return new WaitForSeconds(0.25f);
            PageSet(pageNum-1,pageNum);
        }else{
            PageRemove(pageNum,pageNum-1);
            UIManager.instance.DefaultSound();
            pageNum--;
            yield return new WaitForSeconds(0.25f);
            PageSet(pageNum+1,pageNum);
        }
        buttons[0].SetActive(true);
        buttons[1].SetActive(true);
        if(pageNum==0){
            buttons[0].SetActive(false);
        }
        if(pageNum==maxPage){
            buttons[1].SetActive(false);
        }
        yield return new WaitForSeconds(0.25f);
        isChanging=false;
    }
    public void PageSet(int init,int after){
        if(after==0){
            settingPage.gameObject.SetActive(true);
            settingPage.Appear(init,after);
        }else if(after==1){
            mainPage.gameObject.SetActive(true);
            mainPage.Appear(init,after);
        }else if(after>=2 && after<maxPage){
            if(!stages.gameObject.activeInHierarchy){
                stages.gameObject.SetActive(true);
            }
            stages.Appear(init,after);
        }else if(after==maxPage){
            letterPage.gameObject.SetActive(true);
            letterPage.Appear(init,after);
        }

    }
    public void PageRemove(int init,int after){
        if(init==0){
            settingPage.Disappear(init,after);
        }else if(init==1){
            mainPage.Disappear(init,after);
        }else if(init>=2 && init<maxPage){
            stages.Disappear(init,after);
        }else if(init==maxPage){
            letterPage.Disappear(init,after);
        }
    }
}
