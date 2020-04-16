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
    public Image bg;
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
        //
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
