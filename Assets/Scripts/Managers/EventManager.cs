using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class EventManager : MonoBehaviour
{
    public static EventManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}

	[HideInInspector]public List<SpriteRenderer> eventPos1;
	[HideInInspector]public List<SpriteRenderer> eventPos2;
	public List<Sprite> eventSprite;
	[HideInInspector]public Text centerText;
	[HideInInspector]public float rate;
	[Range(0.01f,2f)]
	public float eventTime=0.3f;
	private int countEvent=0;
	private float realEventTime;
	private List<List<string>> lyrics;
	private Outline centerShadow;
	private Color initialColor;
	private Vector3 initialSize;

	public void Init(){
		rate=StageManager.instance.stagefile.metronomeRate;
		eventPos1=CameraManager.instance.camAction.eventPos1;
		eventPos2=CameraManager.instance.camAction.eventPos2;
		centerText=CameraManager.instance.camAction.centerText;
		centerText.gameObject.SetActive(false);
		realEventTime=eventTime*StageManager.instance.spb;
		centerShadow=centerText.GetComponent<Outline>();
		lyrics=ParseLyrics(StageManager.instance.stagefile.lyrics);
		initialColor=eventPos1[0].color;
		initialSize=eventPos1[0].transform.localScale;
	}

	public List<List<string>> ParseLyrics(TextAsset text){
		List<List<string>> list = new List<List<string>>();
        List<string> temp= new List<string>();
		var lines = Regex.Split(text.text,StageManager.LINE_SPLIT_RE);
		for(int i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], ",");
            if (values.Length == 0 || values[0] == "") continue;
            for(int j=0;j<values.Length;j++){
				values[j]=values[j].Replace("^",",");
                temp.Add(values[j]);
            }
            list.Add(new List<string>(temp));
            temp.Clear();
        }
		return list;
	}

	public IEnumerator ShowLyricsCoroutine(int index,float time,bool isInitial){//animation included
		Color c;
		if(!isInitial){
			while(!StageManager.instance.isGameStart){
				yield return null;
			}
			yield return new WaitForSeconds(time);
		}
		centerText.gameObject.SetActive(true);
		centerText.text=lyrics[index][0]+"\n\n"+lyrics[index][1];
		for(float i=0.1f;i<=1;i+=0.1f){
			c=centerText.color;
			c.a=i;
			centerText.color=c;
			c=centerShadow.effectColor;
			c.a=i;
			centerShadow.effectColor=c;
			yield return null;
		}
		yield return new WaitForSeconds(float.Parse(lyrics[index][2])*StageManager.instance.spb);
		for(float i=0.9f;i>=0;i-=0.1f){
			c=centerText.color;
			c.a=i;
			centerText.color=c;
			c=centerShadow.effectColor;
			c.a=i;
			centerShadow.effectColor=c;
			yield return null;
		}
		centerText.text=null;
		centerText.gameObject.SetActive(false);
	}
	
	public IEnumerator PendEventCoroutine(string direction, float time){
		int eventCode=0;
		if(direction=="right"){
			eventCode=0;
		}else if(direction=="up"){
			eventCode=1;
		}else if(direction=="left"){
			eventCode=2;
		}else if(direction=="down"){
			eventCode=3;
		}else if(direction=="right-up"){
			eventCode=4;
		}else if(direction=="right-down"){
			eventCode=5;
		}else if(direction=="left-up"){
			eventCode=6;
		}else if(direction=="left-down"){
			eventCode=7;
		}else if(direction=="clockwise"){
			eventCode=8;
		}else if(direction=="reverseclockwise"){
			eventCode=9;
		}
		while(!StageManager.instance.isGameStart){
            yield return null;
        }
		yield return new WaitForSeconds(time);
		StartCoroutine(StartEventEffect(eventCode));
		countEvent++;
		yield return new WaitForSeconds(realEventTime);
		if(countEvent<=1){
			eventPos1[0].sprite=null;
			eventPos1[0].gameObject.SetActive(false);
			eventPos2[0].sprite=null;
			eventPos2[0].gameObject.SetActive(false);
			eventPos1[1].sprite=null;
			eventPos1[1].gameObject.SetActive(false);
			eventPos2[1].sprite=null;
			eventPos2[1].gameObject.SetActive(false);
		}
		countEvent--;
	}

	public IEnumerator StartEventEffect(int eventCode){
		Color c;
		Vector3 temp=new Vector3(1,1,1);
		eventPos1[0].gameObject.SetActive(true);
		eventPos1[0].sprite=eventSprite[eventCode];
		eventPos2[0].gameObject.SetActive(true);
		eventPos2[0].sprite=eventSprite[eventCode];
		eventPos1[1].gameObject.SetActive(true);
		eventPos1[1].sprite=eventSprite[eventCode];
		eventPos2[1].gameObject.SetActive(true);
		eventPos2[1].sprite=eventSprite[eventCode];
		for(int i=0;i<=10;i++){
			float t=i/10f;
			c=eventPos1[0].color;
			c.a=t*0.6f;
			eventPos1[0].color=c;
			c=eventPos2[0].color;
			c.a=t*0.6f;
			eventPos2[0].color=c;
			c=eventPos1[1].color;
			c.a=t*0.6f;
			eventPos1[1].color=c;
			c=eventPos2[1].color;
			c.a=t*0.6f;
			eventPos2[1].color=c;
			eventPos1[0].gameObject.transform.localScale=temp*(2-t);
			eventPos2[0].gameObject.transform.localScale=temp*(2-t);
			eventPos1[1].gameObject.transform.localScale=temp*(t);
			eventPos2[1].gameObject.transform.localScale=temp*(t);
			yield return null;
		}
		for(int i=0;i<=20;i++){
			float t=i/20f;
			c=eventPos1[1].color;
			c.a=(1-t)*0.6f;
			eventPos1[1].color=c;
			c=eventPos2[1].color;
			c.a=(1-t)*0.6f;
			eventPos2[1].color=c;
			eventPos1[1].gameObject.transform.localScale=temp*(1+2*t);
			eventPos2[1].gameObject.transform.localScale=temp*(1+2*t);
			yield return null;
		}
		
	}

	public void ShowLyrics(int index,float time,bool isInitial){
		StartCoroutine(ShowLyricsCoroutine(index,time,isInitial));
	}
	public void PendEvent(string direction, float time){
		StartCoroutine(PendEventCoroutine(direction,time));
	}

	public void EndEvent(){
		StopAllCoroutines();
		for(int i=0;i<eventPos1.Count;i++){
			eventPos1[i].sprite=null;
			eventPos1[i].color=initialColor;
			eventPos1[i].transform.localScale=initialSize;
			eventPos1[i].gameObject.SetActive(false);
		}
		for(int i=0;i<eventPos2.Count;i++){
			eventPos2[i].sprite=null;
			eventPos2[i].color=initialColor;
			eventPos2[i].transform.localScale=initialSize;
			eventPos2[i].gameObject.SetActive(false);
		}
		centerText.text="";
		eventPos1=null;
		eventPos2=null;
		centerText=null;
		lyrics=null;
		centerShadow=null;
	}
}
