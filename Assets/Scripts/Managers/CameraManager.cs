using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}
    public GameObject cameraPrefab;
    [HideInInspector]public Camera cam;
    [HideInInspector]public CameraMove camAction;
    [HideInInspector]public float rate;
    public void Init(){
        rate=StageManager.instance.stagefile.metronomeRate;
        GameObject temp=Instantiate(cameraPrefab,new Vector3(0,0,-10),Quaternion.identity);
        camAction=temp.GetComponent<CameraMove>();
        cam=camAction.realCam.GetComponent<Camera>();
    }
    public void SetFalseUICam(){
        cam.enabled=false;
    }
    public void SetTrueUICam(){
        cam.enabled=true;
    }
    Vector3 T;
    public void GetCameraAction(string[] tp,float time){
        if(tp[0]=="1"){
            T.x=float.Parse(tp[1]);
            T.y=float.Parse(tp[2]);
            T.z=0;
            if(tp.Length==3){
                MoveCameraToDirectionSmoothly(T,time);
            }
            else{
                MoveCameraToDirectionSmoothly(T,time,float.Parse(tp[3]));
            }
        }else if(tp[0]=="2"){
            T.x=float.Parse(tp[1]);
            T.y=float.Parse(tp[2]);
            T.z=0;
            MoveCameraSlowly(T,time);
            StopCamera(time+float.Parse(tp[3])*StageManager.instance.spb);
        }else if(tp[0]=="3"){
            SpinCameraSmootyhly(time,float.Parse(tp[1])*StageManager.instance.spb,float.Parse(tp[2]));
        }else if(tp[0]=="4"){
            SpinCameraVerrrrrrySmootyhly(time,float.Parse(tp[1])*StageManager.instance.spb*2f,float.Parse(tp[2]),int.Parse(tp[3]));
        }
        T=Vector3.zero;
    }
    
    public void MoveCameraToDirectionSmoothly(Vector3 target, float waitTime,float duration=1f){
        StartCoroutine(camAction.MoveCameraToDirectionSmoothlyCoroutine(target,waitTime,duration));
    }
    public void MoveCameraSlowly(Vector3 direction, float waitTime, float speed=0.05f){
        StartCoroutine(camAction.MoveCameraSlowlyCoroutine(direction,waitTime,speed));
    }
    public void StopCamera(float waitTime){
        StartCoroutine(camAction.StopCameraCoroutine(waitTime));
    }
    public void SpinCameraSmootyhly(float waitTime,float duration,float angle){
        StartCoroutine(camAction.SpinCameraSmoothlyCoroutine(waitTime,duration,angle));
    }
    public void SpinCameraVerrrrrrySmootyhly(float waitTime,float duration,float maxSpinSpeed,int isClockwise){
        StartCoroutine(camAction.SpinCameraVerySmoothly(waitTime,duration,maxSpinSpeed,isClockwise==1 ? true : false));
    }

    public void EndCamera(){
        StopAllCoroutines();
        Destroy(camAction.gameObject);
        cam=null;
        camAction=null;
    }
}
