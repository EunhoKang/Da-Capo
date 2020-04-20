using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerManager : MonoBehaviour
{
    private Lazer lazerPrefab;
    private List<Lazer> lazers;
    void Awake()
    {
        List<GameObject> temp=StageManager.instance.stagefile.usingObstacle;
        for(int i=0;i<temp.Count;i++){
            Lazer lazer=temp[i].GetComponent<Lazer>();
            if(lazer!=null){
                lazerPrefab=lazer;
            }
        }
        lazers=new List<Lazer>();
        for(int i=0; i<4; i++){
            Lazer obj=Instantiate(lazerPrefab) as Lazer;
            obj.gameObject.SetActive(false);
            lazers.Add(obj);
        }
    }

    public void Command_1(float[] array){
        StartCoroutine(SpawnLazer(array[0],(int)array[1],(int)array[2]));
    }
    Vector3 camPosPlus=new Vector3(0,2.55f,10);
    Vector3 camPosMinus=new Vector3(0,-2.55f,10);
    Vector3 trueRot=new Vector3(0,180,0);
    IEnumerator SpawnLazer(float waitTime, int isUpper, int isLeftStarted){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        float warnTime=lazerPrefab.warnTime;
        yield return new WaitForSeconds(waitTime-warnTime);
        Lazer temp=getLazer();
        temp.transform.position=CameraManager.instance.cam.transform.position
        +(isUpper==1?camPosPlus:camPosMinus);
        if(isLeftStarted==1){
            temp.transform.rotation=Quaternion.Euler(Vector3.zero);
        }else{
            temp.transform.rotation=Quaternion.Euler(trueRot);
        }
        temp.gameObject.SetActive(true);
        temp.Loom();
        yield return new WaitForSeconds(warnTime);
        temp.Appear();
    }

    public Lazer getLazer(){
		for(int i=0;i<lazers.Count;i++){
			if(!lazers[i].gameObject.activeInHierarchy){
				return lazers[i];
			}
		}
		Lazer obj=Instantiate(lazerPrefab) as Lazer;
		obj.gameObject.SetActive(false);
		lazers.Add(obj);
		return obj;
	}
    public void EndObject(){
        StopAllCoroutines();
        for(int i=0;i<lazers.Count;i++){
            Destroy(lazers[i].gameObject);
        }
        lazers.Clear();
        lazerPrefab=null;
    }
}
