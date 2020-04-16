using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    private Meteor thrownPrefab;
    private WarnCircle warnPrefab;
    private Sprite warnSprite;
    private List<WarnCircle> circles;
    private List<Meteor> throwns;
    public int instantNum;

    void Awake()
    {
        List<GameObject> temp=StageManager.instance.stagefile.usingObstacle;
        for(int i=0;i<temp.Count;i++){
            Meteor meteor=temp[i].GetComponent<Meteor>();
            if(meteor!=null && meteor.instantNum==instantNum){
                thrownPrefab=meteor;
                warnPrefab=meteor.warnPrefab;
                warnSprite=meteor.warnSprite;
            }
        }
        circles=new List<WarnCircle>();
        for(int i=0;i<6;i++){
            WarnCircle obj=Instantiate(warnPrefab) as WarnCircle;
            obj.gameObject.SetActive(false);
            circles.Add(obj);
        }
        throwns=new List<Meteor>();
        for(int i=0;i<6;i++){
            Meteor obj=Instantiate(thrownPrefab) as Meteor;
            obj.gameObject.SetActive(false);
            throwns.Add(obj);
        }
    }
    public void Command_1(float[] array){
        StartCoroutine(MeteorShot(array[0],
        new Vector3(array[1],array[2],0),
        new Vector3(array[3],array[4],0)));
    }
    Vector3 cameraAdd=new Vector3(0,0,10);
    IEnumerator MeteorShot(float waitTime,Vector3 direction,Vector3 initPos){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        Vector3 chaseRotation=Vector3.zero;
        float warnTime=ObstacleManager.instance.warnTime;
        yield return new WaitForSeconds(waitTime-warnTime);
        WarnCircle temp=getCircle();
        temp.transform.SetParent(CameraManager.instance.cam.transform);
        Vector3 node=FindNode(direction,initPos);
        temp.transform.localPosition=node+cameraAdd;
        temp.gameObject.SetActive(true);
        if(node!=Vector3.zero){
            chaseRotation.z=GetAngle(initPos,temp.transform.localPosition);
            temp.transform.localRotation=Quaternion.Euler(chaseRotation);
            temp.ShowWarn(warnTime,warnSprite);
            yield return new WaitForSeconds(warnTime);
            Meteor tp=getMeteor();
            tp.transform.rotation=Quaternion.Euler(chaseRotation);
            //tp.transform.rotation=temp.transform.rotation;
            Vector3 tpp=initPos+CameraManager.instance.cam.transform.position;
            tpp.z=0;
            tp.transform.position=tpp;
            //tp.transform.SetParent(CameraManager.instance.cam.transform);
            //tp.transform.localPosition=initPos;
            //tp.transform.SetParent(null);
            tp.gameObject.SetActive(true);
            tp.Shot(direction);
        }
    }

    float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;
        return Mathf.Atan2(v.y, v.x) * 57.29578f+90f;
    }

    Vector3 FindNode(Vector3 direction,Vector3 initPos){
        List<Vector3> array=new List<Vector3>();
        float arc=direction.y/direction.x;
        float temp=arc*(7f-initPos.x)+initPos.y;
        if(temp>=-3.5f && temp<=3.5f){
            array.Add(new Vector3(7,temp,0));
        }
        temp=arc*(-7f-initPos.x)+initPos.y;
        if(temp>=-3.5f && temp<=3.5f){
            array.Add(new Vector3(-7,temp,0));
        }
        arc=direction.x/direction.y;
        temp=arc*(-3.5f-initPos.y)+initPos.x;
        if(temp>-7f && temp<7f){
            array.Add(new Vector3(temp,-3.5f,0));
        }
        temp=arc*(3.5f-initPos.y)+initPos.x;
        if(temp>-7f && temp<7f){
            array.Add(new Vector3(temp,3.5f,0));
        }
        if(array.Count>2){
            Debug.Log("error:too many nodes");
            return Vector3.zero;
        }else if(array.Count==2){
            if(Vector3.SqrMagnitude(array[0]-initPos)>=Vector3.SqrMagnitude(array[1]-initPos)){
                return array[1];
            }else{
                return array[0];
            }
        }else if(array.Count==1){
            return array[0];
        }else{
            return Vector3.zero;
        }
    }
    
    public WarnCircle getCircle(){
		for(int i=0;i<circles.Count;i++){
			if(!circles[i].gameObject.activeInHierarchy){
				return circles[i];
			}
		}
		WarnCircle obj=Instantiate(warnPrefab) as WarnCircle;
		obj.gameObject.SetActive(false);
		circles.Add(obj);
		return obj;
	}

    public Meteor getMeteor(){
		for(int i=0;i<throwns.Count;i++){
			if(!throwns[i].gameObject.activeInHierarchy){
				return throwns[i];
			}
		}
		Meteor obj=Instantiate(thrownPrefab) as Meteor;
		obj.gameObject.SetActive(false);
		throwns.Add(obj);
		return obj;
	}

    public void EndObject(){
        StopAllCoroutines();
        for(int i=0;i<circles.Count;i++){
            Destroy(circles[i].gameObject);
        }
        for(int i=0;i<throwns.Count;i++){
            Destroy(throwns[i].gameObject);
        }
        throwns.Clear();
        circles.Clear();
        thrownPrefab=null;
        warnPrefab=null;
        warnSprite=null;
    }
}
