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


	public void EndCharacter(){
		StopAllCoroutines();
		Destroy(player);
		player=null;
		playerScript=null;
	}
}
