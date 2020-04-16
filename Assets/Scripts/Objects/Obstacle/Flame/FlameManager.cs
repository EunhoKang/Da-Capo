using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameManager : MonoBehaviour
{
    private Flame flamePrefab;
    private WarnFlame warnPrefab;
    private List<Flame> flames;
    private List<WarnFlame> warnFlames;
    void Awake()
    {
        List<GameObject> temp=StageManager.instance.stagefile.usingObstacle;
        for(int i=0;i<temp.Count;i++){
            Flame flame=temp[i].GetComponent<Flame>();
            if(flame!=null){
                flamePrefab=flame;
                warnPrefab=flame.warnPrefab;
            }
        }
        flames=new List<Flame>();
        for(int i=0; i<40; i++){
            Flame obj=Instantiate(flamePrefab) as Flame;
            obj.gameObject.SetActive(false);
            flames.Add(obj);
        }
        warnFlames=new List<WarnFlame>();
        for(int i=0;i<2;i++){
            WarnFlame obj=Instantiate(warnPrefab) as WarnFlame;
            obj.gameObject.SetActive(false);
            warnFlames.Add(obj);
        }
    }
    public void Command_1(float[] array){
        StartCoroutine(SpawnFlame(array[0],new Vector3(array[1],array[2],0),array[3]));
    }
    Vector3 cameraAdd=new Vector3(0,0,10);
    Vector3[] moveVectors={new Vector3(3,3,0),new Vector3(-3,3,0),new Vector3(-3,-3,0),new Vector3(3,-3,0)};
    IEnumerator SpawnFlame(float waitTime,Vector3 pos,float speed){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        float warnTime=ObstacleManager.instance.warnTime*0.5f;
        yield return new WaitForSeconds(waitTime-warnTime);
        WarnFlame temp=getWarn();
        temp.transform.SetParent(CameraManager.instance.cam.transform);
        temp.transform.localPosition=cameraAdd+pos;
        temp.gameObject.SetActive(true);
        temp.ShowWarn(warnTime);
        yield return new WaitForSeconds(warnTime);
        Flame tp;
        for(int i=0;i<4;i++){
            tp=getFlame();
            tp.transform.position=temp.transform.position;
            tp.gameObject.SetActive(true);
            tp.ShowSpawn(moveVectors[i]*speed);
        }
    }
    public Flame getFlame(){
		for(int i=0;i<flames.Count;i++){
			if(!flames[i].gameObject.activeInHierarchy){
				return flames[i];
			}
		}
		Flame obj=Instantiate(flamePrefab) as Flame;
		obj.gameObject.SetActive(false);
		flames.Add(obj);
		return obj;
	}

    public WarnFlame getWarn(){
        for(int i=0;i<warnFlames.Count;i++){
			if(!warnFlames[i].gameObject.activeInHierarchy){
				return warnFlames[i];
			}
		}
		WarnFlame obj=Instantiate(warnPrefab) as WarnFlame ;
		obj.gameObject.SetActive(false);
		warnFlames.Add(obj);
		return obj;
    }

    public void EndObject(){
        StopAllCoroutines();
        for(int i=0;i<flames.Count;i++){
            Destroy(flames[i].gameObject);
        }
        for(int i=0;i<warnFlames.Count;i++){
            Destroy(warnFlames[i].gameObject);
        }
        flames.Clear();
        warnFlames.Clear();
        flamePrefab=null;
        warnPrefab=null;
    }
}
