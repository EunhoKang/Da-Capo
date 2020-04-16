using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class StageManager : MonoBehaviour
{
    public static StageManager instance=null;
    public static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}
    [HideInInspector]public string mapName;
    public GameObject grid;
    [HideInInspector]public float offset=0.2f;
    private Grid gridScript;
    private float startDelay;
    [HideInInspector]public int stageNum;
    [HideInInspector]public StageFile stagefile;
    [HideInInspector]public bool isGameStart;
    [HideInInspector]public bool isGameEnd;
    [HideInInspector]public InGame ingameUI;
    [HideInInspector]public float spb;

    public void Init(){
        stagefile=(Resources.Load(mapName) as GameObject).GetComponent<StageFile>();
        isGameEnd=false;
        isGameStart=false;
        spb=60/stagefile.BPM;
        stageNum=stagefile.stageNum;
        offset=DataManager.instance._data.offset;
        gridScript=Instantiate(grid,stagefile.initPos,Quaternion.identity).GetComponent<Grid>();
        if(ingameUI!=null){
            ingameUI.dashDistance=stagefile.dashLength;
            ingameUI.healthSlider.maxValue=stagefile.playerHealth;
        }
        CameraManager.instance.Init();
        BackgroundManager.instance.Init();
        CountManager.instance.Init();
        EventManager.instance.Init();
        NoteManager.instance.Init();
        CharacterManager.instance.Init();
        EnemyManager.instance.Init();
        ObstacleManager.instance.Init();
        TimeManager.instance.Init();
        StartCoroutine(GameStart(ReadData(stagefile.noteFile)));
    }

    public void SetStartDelay(float delay){
        startDelay=delay;
    }

    List<List<string>> ReadData(TextAsset file){
        List<List<string>> list = new List<List<string>>();
        List<string> temp= new List<string>();

        var lines = Regex.Split(file.text, LINE_SPLIT_RE);
        for(int i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], ",");
            if (values.Length == 0 || values[0] == "") continue;
            for(int j=0;j<values.Length;j++){
                string tp=values[j].Replace("(","").Replace(")","").Replace("/","");
                temp.Add(tp);
            }
            list.Add(new List<string>(temp));
            temp.Clear();
        }
        return list;
    }
    
    public IEnumerator GameStart(List<List<string>> notes){
        while(startDelay<=0){
            yield return null;
        }
        if(stageNum!=-1){
            ingameUI.SettingForGame();
        }else{
            ingameUI.SettingForOffset();
        }
        yield return new WaitForSeconds(0.2f);
        UIManager.instance.LoadingEnd();
        if(stageNum!=-1){
            EventManager.instance.ShowLyrics(0,0,true);
            gridScript.InitAnimation();
            StartCoroutine(MoveGrid());
        }
        BackgroundManager.instance.SetIllust(stagefile.backgroundIllust);
        float OS=offset+stagefile.offset;
        yield return new WaitForSeconds(0.2f);
        StageManager.instance.ingameUI.CanPause();
        //Start Effect
        Vector3 T;
        for(int i=0;i<notes.Count;i++){
            float time=float.Parse(notes[i][0])*spb+startDelay+OS;
            if(notes[i][1]!=""){//spawn notes
                NoteManager.instance.SpawnNote(time);
            }
            if(notes[i][3]!=""){//order enemy attack
                EnemyManager.instance.OrderAttack(time,int.Parse(notes[i][3]));
            }
            if(notes[i][4]!=""){//order camera action, pending event
                string[] tp=notes[i][4].Split('^');
                CameraManager.instance.GetCameraAction(tp,time);
            }
            if(notes[i][5]!=""){
                string action=notes[i][5];
                EventManager.instance.PendEvent(action,time);
            }
            if(notes[i][6]!=""){
                EventManager.instance.ShowLyrics(int.Parse(notes[i][6]),time,false);
            }
            if(notes[i][7]!=""){
                TimeManager.instance.HasteTimer(time,float.Parse(notes[i][7]));

            }
            if(notes[i][8]!=""){
                ObstacleManager.instance.GetAction(notes[i][8],time);
            }
            if(notes[i][2]!=""){//spawn enemy
                string[] tp=notes[i][2].Split('^');
                if(tp[1]=="rand"){
                    T.x=Random.Range(-6.5f,6.5f);
                }else{
                    T.x=float.Parse(tp[1]);
                }
                if(tp[2]=="rand"){
                    T.y=Random.Range(-3.5f,3.5f);
                }
                else{
                    T.y=float.Parse(tp[2]);
                }
                T.z=0;
                EnemyManager.instance.SpawnEnemy(time,int.Parse(tp[0]),T);
                
                T=Vector3.zero;
            }
        }
        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.SongPlay(stagefile.songFile,startDelay);//Song start.
        isGameStart=true;//Loading Ended.
        TimeManager.instance.StartTimer();
        yield return new WaitForSeconds(startDelay);
        CharacterManager.instance.SpinNiddles();
        CountManager.instance.StartScoreCount();
        yield return new WaitForSeconds(stagefile.runTime-startDelay);
        EndToShowResult(true);
    }

    public void EndToShowResult(bool isCleared){
        if(stageNum==-1){
            return;
        }
        if(isCleared){
            StartCoroutine(ResultingCoroutine());
        }else{
            StartCoroutine(GameOver());
        }
    }
    public IEnumerator GameOver(){
        Time.timeScale=1;
        SoundManager.instance.BGMFadeOut();
        ingameUI.FadeOut();
        yield return new WaitForSeconds(1f);
        UIManager.instance.LoadingStart();
        yield return new WaitForSeconds(0.5f);
        UIManager.instance.LoadingEnd();
        UIManager.instance.ShowCanvas(0);
        StageManager.instance.EndStage();
        UIManager.instance.RemoveCanvas(1);
        UIManager.instance.SetTrueUICam();
    }
    public IEnumerator ResultingCoroutine(){
        SoundManager.instance.BGMFadeOut();
        DataManager.instance.StageCleared(CountManager.instance.stageCleared,
        StageManager.instance.stagefile.stageNum,
        CountManager.instance.score,
        CountManager.instance.hit==0 ? true : false,
        CountManager.instance.miss==0 ? true : false);
        yield return new WaitForSeconds(0.5f);
        ingameUI.FadeOut();
        yield return new WaitForSeconds(0.5f);
        UIManager.instance.ShowCanvas(2);
        yield return new WaitForSeconds(0.5f);
        UIManager.instance.RemoveCanvas(1);
        UIManager.instance.SetTrueUICam();
        EndStage();
    }

    public void GetIngameUI(InGame script){
        ingameUI=script;
    }

    public IEnumerator MoveGrid(){
        WaitForSeconds tp=new WaitForSeconds(0.5f);
        while(!isGameEnd){
            gridScript.MoveToTargetDirection(CharacterManager.instance.FindPlayer());
            yield return tp;
        }
    }

    public void EndStage(){
        CameraManager.instance.EndCamera();
        CharacterManager.instance.EndCharacter();
        CountManager.instance.EndCount();
        EnemyManager.instance.EndEnemy();
        EventManager.instance.EndEvent();
        NoteManager.instance.EndNote();
        ObstacleManager.instance.EndObstacle();
        SoundManager.instance.EndSound();
        TimeManager.instance.EndTime();
        StopAllCoroutines();
        ingameUI.ResetUI();
        Destroy(gridScript.gameObject);
        ingameUI=null;
        stagefile=null;
        gridScript=null;
    }
}
