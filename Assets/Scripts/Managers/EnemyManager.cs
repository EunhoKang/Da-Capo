using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}
	public List<GameObject> enemyTypes;
	public enemyBullet bulletPrefab;
	[HideInInspector]public List<enemyBullet> bullets;
	[Range(0.01f,2f)]
	public float silhouetteTime=0.5f;
	[Range(0.01f,1f)]
	public float warnTime=0.5f;

	[HideInInspector]public List<BulletEnemy> bulletEnemys;
	[HideInInspector]public List<BulletEnemy> bulletEnemyYetAttack;
	[HideInInspector]public List<LazerEnemy> lazerEnemys;
	[HideInInspector]public List<LazerEnemy> lazerEnemyYetAttack;
	[HideInInspector]public float bulletSpeed;
	[HideInInspector]public float enemyLazerDuration;
	[HideInInspector]public float fixTime;
	[HideInInspector]public float rate;
	
	private Vector3 cameraDiff;
	
	public void Init(){
		rate=StageManager.instance.stagefile.metronomeRate;
		enemyTypes[0]=StageManager.instance.stagefile.bulletTypePrefab;
		enemyTypes[1]=StageManager.instance.stagefile.lazerTypePrefab;
		bulletPrefab=StageManager.instance.stagefile.bulletPrefab;
		bulletSpeed=StageManager.instance.stagefile.enemyBulletSpeed;
		enemyLazerDuration=StageManager.instance.stagefile.enemyLazerDuration*StageManager.instance.spb;
		fixTime=StageManager.instance.stagefile.fixTime*StageManager.instance.spb;
		cameraDiff=new Vector3(0,0,10);

		bullets=new List<enemyBullet>();
		bulletEnemys=new List<BulletEnemy>();
		lazerEnemys=new List<LazerEnemy>();
		lazerEnemyYetAttack=new List<LazerEnemy>();
		for(int i=0;i<16;i++){
			enemyBullet obj=Instantiate(bulletPrefab) as enemyBullet;
			obj.gameObject.SetActive(false);
			bullets.Add(obj);
		}
	}
	
	public BulletEnemy getBulletEnemy(){
		for(int i=0; i<bulletEnemys.Count;i++){
			if(!bulletEnemys[i].gameObject.activeInHierarchy){
				return bulletEnemys[i];
			}
		}	
		BulletEnemy obj=Instantiate(enemyTypes[0]).GetComponent<BulletEnemy>();
		obj.gameObject.SetActive(false);
		bulletEnemys.Add(obj);
		return obj;
	}
	
	public LazerEnemy getLazerEnemy(){
		for(int i=0; i<lazerEnemys.Count;i++){
			if(!lazerEnemys[i].gameObject.activeInHierarchy){
				return lazerEnemys[i];
			}
		}	
		LazerEnemy obj=Instantiate(enemyTypes[1]).GetComponent<LazerEnemy>();
		obj.gameObject.SetActive(false);
		lazerEnemys.Add(obj);
		return obj;
	}
	public enemyBullet getBullet(){
		for(int i=0;i<bullets.Count;i++){
			if(!bullets[i].gameObject.activeInHierarchy){
				return bullets[i];
			}
		}
		enemyBullet obj=Instantiate(bulletPrefab) as enemyBullet;
		obj.gameObject.SetActive(false);
		bullets.Add(obj);
		return obj;
	}
	public IEnumerator SpawnEnemyCoroutine(float time, int enemyType, Vector3 pos){ //Spawn after some seconds
		while(!StageManager.instance.isGameStart){
            yield return null;
        }
        yield return new WaitForSeconds(time-silhouetteTime*StageManager.instance.spb);
		switch(enemyType){
			case 1:
				BulletEnemy tp1=getBulletEnemy();
				tp1.gameObject.SetActive(true);
				tp1.SilhouetteMode();
				tp1.transform.SetParent(CameraManager.instance.cam.transform);
				tp1.transform.position=CameraManager.instance.cam.transform.position+pos+cameraDiff;
				break;
			case 2:
				LazerEnemy tp2=getLazerEnemy();
				tp2.gameObject.SetActive(true);
				tp2.SilhouetteMode();
				tp2.transform.SetParent(CameraManager.instance.cam.transform);
				tp2.transform.position=CameraManager.instance.cam.transform.position+pos+cameraDiff;
				break;
			default:
				break;
		}
    }

	public IEnumerator OrderAttackCoroutine(float time, int type){
		while(!StageManager.instance.isGameStart){
            yield return null;
        }
		switch(type){
			case 1 :
				BulletEnemy temp1=null;
				yield return new WaitForSeconds(time-warnTime*StageManager.instance.spb);
				for(int i=0;i<bulletEnemyYetAttack.Count;i++){
					if(bulletEnemyYetAttack[i].gameObject.activeSelf){
						temp1=bulletEnemyYetAttack[i];
						temp1.EnemyWait();
						bulletEnemyYetAttack.RemoveAt(i);
						break;
					}
				}
				yield return new WaitForSeconds(warnTime*StageManager.instance.spb-fixTime);
				if(temp1!=null){
					temp1.EnemyShot();
				}
				break;
			case 2 :
				LazerEnemy temp2=null;
				yield return new WaitForSeconds(time-warnTime*StageManager.instance.spb);
				for(int i=0;i<lazerEnemyYetAttack.Count;i++){
					if(lazerEnemyYetAttack[i].gameObject.activeSelf){
						temp2=lazerEnemyYetAttack[i];
						temp2.EnemyWait();
						temp2.ShowSilhouette();
						lazerEnemyYetAttack.RemoveAt(i);
						break;
					}
				}
				yield return new WaitForSeconds(warnTime*StageManager.instance.spb-fixTime);
				if(temp2!=null){
					temp2.EnemyShot();
				}	
				break;
			default :
				break;
		}
	}

	public void SpawnEnemy(float time, int enemyType, Vector3 pos){
		StartCoroutine(SpawnEnemyCoroutine(time,enemyType,pos));
	}
	public void OrderAttack(float time, int type){
		StartCoroutine(OrderAttackCoroutine(time,type));
	}

	public void EndEnemy(){
		StopAllCoroutines();
		bulletEnemyYetAttack.Clear();
		lazerEnemyYetAttack.Clear();
		for(int i=0;i<bullets.Count;i++){
			Destroy(bullets[i].gameObject);
		}
		for(int i=0; i<bulletEnemys.Count;i++){
			Destroy(bulletEnemys[i].gameObject);
		}
		for(int i=0; i<lazerEnemys.Count;i++){
			lazerEnemys[i].EndLazerEnemy();
			Destroy(lazerEnemys[i].gameObject);
		}
		bullets=null;
		bulletEnemys=null;
		lazerEnemys=null;
		lazerEnemyYetAttack=null;
		
	}
}
