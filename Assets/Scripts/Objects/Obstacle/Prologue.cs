using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    public Text content;
    public Image image;
    public Image bg;
    public List<Sprite> exImages;
    public Text downText;
    [TextArea]
    public List<string> exTexts;
    private bool isAnim;
    private IEnumerator loom;
    private int resumeCount;
    void Awake()
    {
        isAnim=false;
        resumeCount=0;
    }

    void OnEnable()
    {
        content.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
    }
    public void Command_0(float[] array){
        int num=(int)array[1];
        if(DataManager.instance._data.prologues[num])return;
        float waitTime=array[0];
        content.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
        downText.gameObject.SetActive(false);
        StartCoroutine(ShowCanvas(waitTime,num));
    }
    Vector3 add=new Vector3(0,0,10);
    IEnumerator ShowCanvas(float waitTime,int num){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        Color c;
        yield return new WaitForSeconds(waitTime);
        content.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        bg.gameObject.SetActive(true);
        downText.gameObject.SetActive(true);
        isAnim=true;
        transform.position=CameraManager.instance.cam.transform.position+add;
        content.text=exTexts[num];
        image.sprite=exImages[num];
        DataManager.instance.PrologueCount(num);
        for(float i=0.1f;i<=1;i+=0.1f){
			c=content.color;
			c.a=i;
			content.color=c;
			c=image.color;
			c.a=i;
			image.color=c;
            c=bg.color;
			c.a=i;
			bg.color=c;
			yield return null;
		}
        isAnim=false;
        Time.timeScale=0;
        StageManager.instance.ingameUI.CantPause();
        SoundManager.instance.SoundPause();
        loom=Looming();
        yield return StartCoroutine(loom);
    }

    IEnumerator Looming(){
        Color c;
        int i=0;
        while(true){
            c=downText.color;
            if(i>25){
                c.a=(50-i)*0.04f;
            }else{
                c.a=i*0.04f;
            }
            downText.color=c;
            i++;
            if(i>50){
                i=0;
            }
            yield return null;
        }
    }

    public void EndTimeScaling(){
        if(isAnim)return;
        if(resumeCount==0){
            resumeCount++;
        }else{
            StartCoroutine(RemoveCanvas());
            resumeCount=0;
        }
    }

    IEnumerator RemoveCanvas(){
        Time.timeScale=1;
        SoundManager.instance.SoundResume();
        Color c;
        isAnim=true;
        if(loom!=null){
            StopCoroutine(loom);
            loom=null;
        }
        for(float i=0.9f;i>=0;i-=0.1f){
			c=content.color;
			c.a=i;
			content.color=c;
			c=image.color;
			c.a=i;
			image.color=c;
            c=bg.color;
			c.a=i;
			bg.color=c;
			yield return null;
		}
        isAnim=false;
        downText.gameObject.SetActive(false);
        content.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
        StageManager.instance.ingameUI.CanPause();
    }

    public void EndObject(){
        StopAllCoroutines();
    }
}
