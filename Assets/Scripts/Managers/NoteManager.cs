using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NoteManager : MonoBehaviour
{
    public static NoteManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}
    [HideInInspector]public Transform heartTransform;
    [HideInInspector]public Transform noteSpawnTransform;
    [HideInInspector]public Transform rayStartPoint;
    [HideInInspector]public float rate;
    [HideInInspector]public Line lineScript;
    public float noteSpeedMultiplier;
    public float judgeDistance;
    public GameObject linePrefab;
    public Note notePrefab;
    [Header("Setting")]
    public bool isVibrateOn=false;
    public bool isNoteSFXOn=false;
    public AudioClip touchSound;

    private GameObject line;
    private float noteDelay;
    private Vector3 noteSpeed;
    private List<Note> Notes;
    private float missDistanceReverse;
    public void Init()
    {
        rate=StageManager.instance.stagefile.metronomeRate;
        heartTransform=CameraManager.instance.camAction.heartTransform;
        noteSpawnTransform=CameraManager.instance.camAction.noteSpawnTransform;
        rayStartPoint=CameraManager.instance.camAction.rayStartPoint;
        noteSpeed=(rayStartPoint.position-heartTransform.position)*noteSpeedMultiplier;
        missDistanceReverse=1f/(float)(heartTransform.position.y-rayStartPoint.position.y);
        noteDelay=Vector3.Magnitude(heartTransform.position-noteSpawnTransform.position)/Vector3.Magnitude(noteSpeed);
        line=Instantiate(linePrefab,heartTransform.position,Quaternion.identity);
        line.transform.SetParent(CameraManager.instance.cam.transform);
        lineScript=line.GetComponent<Line>();
        StageManager.instance.SetStartDelay(noteDelay+0.01f);
        Notes=new List<Note>();
        for(int i=0;i<8;i++){
            Note obj=Instantiate(notePrefab) as Note;
            obj.gameObject.SetActive(false);
            Notes.Add(obj);
        }
        isVibrateOn=DataManager.instance._data.isVibOn;
    }

    public IEnumerator SpawnNoteCoroutine(float time){ //Spawn after some seconds
        if(time-noteDelay<0){
            Debug.Log("ERR : time has been negative");
            yield break;
        }
        while(!StageManager.instance.isGameStart){
            yield return null;
        }

        yield return new WaitForSeconds(time-noteDelay);
        Note temp=getNote();
        temp.transform.position=noteSpawnTransform.position;
        temp.transform.rotation=noteSpawnTransform.rotation;
        temp.transform.SetParent(CameraManager.instance.cam.transform);
        temp.gameObject.SetActive(true);
        temp.StartFall(noteSpeed);
    }

    public Note getNote(){
		for(int i=0; i<Notes.Count;i++){
			if(!Notes[i].gameObject.activeInHierarchy){
				return Notes[i];
			}
		}
		Note obj=Instantiate(notePrefab) as Note;
		obj.gameObject.SetActive(false);
		Notes.Add(obj);
		return obj;
	}

    public void JudgeNote(Vector3 direction){
        RaycastHit2D[] hit2D=Physics2D.RaycastAll(rayStartPoint.position,(heartTransform.position-rayStartPoint.position),
        judgeDistance,(1<<LayerMask.NameToLayer("Note")));
        if(hit2D.Length!=0){
            float diff=(hit2D[0].collider.transform.position.y-heartTransform.position.y)*missDistanceReverse;
                if(diff>-0.4f && diff<=0.4f){
                    JudgeSend(0);
                }else if(diff>-0.6f && diff<=0.6f){
                    JudgeSend(1);
                }else if(diff>-0.8f && diff<0.8f){
                    JudgeSend(2);
                }else{
                    JudgeSend(3);
                }
                hit2D[0].collider.gameObject.GetComponent<Note>().NoteHitted();
                CharacterManager.instance.MoveOrder(direction);
        }
    }

    public void JudgeSend(int judge){
        lineScript.NoteHitted(judge);
        if(judge<3){
            if(isNoteSFXOn){
                SoundManager.instance.SFXPlay(touchSound,2);
            }
            #if UNITY_ANDROID && !UNITY_EDITOR
            if(isVibrateOn){
                Vibration.Vibrate(60);
            }
            #endif
        }
        CountManager.instance.AddComboForHitNote(judge);
    }
    public void SpawnNote(float time){
        StartCoroutine(SpawnNoteCoroutine(time));
    }

    public void EndNote(){
        StopAllCoroutines();
        for(int i=0;i<Notes.Count;i++){
            Notes[i].EndNote();
            Destroy(Notes[i].gameObject);
        }
        Destroy(line);
        heartTransform=null;
        rayStartPoint=null;
        noteSpawnTransform=null;
        Notes=null;
        line=null;
    }
}
