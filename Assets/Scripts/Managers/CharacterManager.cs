using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}

    public GameObject playerPrefab;
	public float initialSpeed=2f;
    public float standardSpb=0.5f;
	public ParticleSystem[] noteHitPrefab;
	List<List<ParticleSystem>> effectLists;
	[HideInInspector]public Vector3 silhouettePos;
	[HideInInspector]public float rate;
	[HideInInspector]public Vector3 lastPos;
    private GameObject player;
	private Player playerScript;
	[HideInInspector] public float dashDistance;
	private bool isplayerDead;

    public void Init()
    {
		rate=StageManager.instance.stagefile.metronomeRate;
		Vector3 temp=StageManager.instance.stagefile.initPos+CameraManager.instance.cam.gameObject.transform.position;
		temp.z=0;
        player=Instantiate(playerPrefab,temp,Quaternion.identity);
		lastPos=temp;
		playerScript=player.GetComponent<Player>();
		dashDistance=StageManager.instance.stagefile.dashLength;
		isplayerDead=false;
		effectLists=new List<List<ParticleSystem>>();
        for(int i=0;i<noteHitPrefab.Length;i++){
            List<ParticleSystem> tp=new List<ParticleSystem>();
            effectLists.Add(tp);
        }
        for(int i=0;i<noteHitPrefab.Length;i++){
            for(int j=0;j<4;j++){
                ParticleSystem obj=Instantiate(noteHitPrefab[i]) as ParticleSystem;
                obj.gameObject.SetActive(false);
                effectLists[i].Add(obj);
            }
        }
		playerScript.Setup();
		StartCoroutine(SetSilhouetteCoroutine());
    }

	public void UpdatePlayerPos(Vector3 pos){
		lastPos=pos;
	}

	public void MoveOrder(Vector3 direction){
		if(isplayerDead) return;
		playerScript.MovePlayer(direction,dashDistance);
	}
	public void SetSilhouettePos(Vector3 pos){
		silhouettePos=pos*dashDistance;
	}
	public IEnumerator SetSilhouetteCoroutine(){
        while(!StageManager.instance.isGameEnd && !isplayerDead){
            playerScript.SetSilhouette(silhouettePos);
            yield return null;
        }
    }
	public void SpinNiddles(){
		playerScript.SpinNiddles();
	}
	public void PlayerDead(){
		isplayerDead=true;
		StageManager.instance.isGameEnd=true;
		StageManager.instance.EndToShowResult(false);
	}
	public Vector3 FindPlayer(){
		if(isplayerDead) return Vector3.zero;
		return player.transform.position;
	}
	public Vector3 FindPlayerForEnemy(){
		if(isplayerDead) return Vector3.zero;
		return lastPos;
	}
	public void PlayerGetDamage(float amount){
		playerScript.PlayerDamaged(amount);
	}
	public void PlayerGotoCenter(){
		Vector3 temp=CameraManager.instance.cam.gameObject.transform.position;
		temp.z=0;
		player.transform.position=temp;
	}
	public void NoteHitted(int judge){
        if(judge>=noteHitPrefab.Length){
            return;
        }
        StartCoroutine(NoteHitEffect(judge,getNoteEffect(judge)));
    }

	public ParticleSystem getNoteEffect(int judge){
		for(int i=0; i<effectLists[judge].Count;i++){
			if(!effectLists[judge][i].gameObject.activeInHierarchy){
				return effectLists[judge][i];
			}
		}
		ParticleSystem obj=Instantiate(noteHitPrefab[judge]) as ParticleSystem;
		obj.gameObject.SetActive(false);
		effectLists[judge].Add(obj);
		return obj;
	}
	IEnumerator NoteHitEffect(int judge,ParticleSystem p){
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float spb=StageManager.instance.spb;
        float particleSpeedMultiplier=standardSpb/spb;

        p.gameObject.SetActive(true);
        p.transform.SetParent(player.transform);
        p.transform.localPosition=Vector3.zero;
        p.playbackSpeed=initialSpeed*particleSpeedMultiplier*TimeManager.instance.multiplier;
        p.Play();
        while(p.isPlaying){
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                p.Pause();
                p.playbackSpeed=initialSpeed*particleSpeedMultiplier*TimeManager.instance.multiplier;
                p.Play();
            }
            yield return null;
        }
        p.gameObject.SetActive(false);
        p.transform.SetParent(null);
    }

	public void EndCharacter(){
		StopAllCoroutines();
		Destroy(player);
		player=null;
		playerScript=null;
		for(int i=0;i<effectLists.Count;i++){
			for(int j=0;j<effectLists[i].Count;j++){
				Destroy(effectLists[i][j].gameObject);
			}
			effectLists[i].Clear();
		}
		effectLists.Clear();
	}
}
